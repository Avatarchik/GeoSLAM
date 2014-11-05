using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointCloudBehaviour : MonoBehaviour {
	
	static public PointCloudBehaviour Instance; // singleton instance

	public bool drawPoints = true;
	public float sceneScale = 1f;
	public Matrix4x4 camera_matrix;
	
	private bool _drawCamera = true;
	public bool DrawCamera {
		get { return _drawCamera; }
		set {
			_drawCamera = value;
			camera.clearFlags = value ? CameraClearFlags.Nothing : CameraClearFlags.Skybox;
		}
	}

	public List<PointCloudImageTarget> imageTargets = new List<PointCloudImageTarget>();
	
	// Cooridnate transform
	private Matrix4x4 convert = new Matrix4x4();

	private Matrix4x4 frustum, cam;
	
	static public pointcloud_state PreviousState { get; private set; }
	static public pointcloud_state State { get; private set; }
	static List<PointCloudSceneRoot> sceneRoots = new List<PointCloudSceneRoot>();

	public static bool HasTracking() {
		return State == pointcloud_state.POINTCLOUD_TRACKING_IMAGES ||
			   State == pointcloud_state.POINTCLOUD_TRACKING_SLAM_MAP;
	}
	
	public static bool HasInitialized() {
		return State == pointcloud_state.POINTCLOUD_RELOCALIZING ||
			   HasTracking();
	}

	void Awake() {
		if (Instance) {
			LogError("Only one instance of PointCloudBehaviour allowed!");
		}
		Instance = this;
	}
	
	void Start()
	{	
		if (PointCloudAppKey.AppKey == "")
		{
			LogError("No PointCloud Application Key provided!");
		}
		
		convert.SetRow(0, new Vector4(0,-1,0,0));
		convert.SetRow(1, new Vector4(-1,0,0,0));
		convert.SetRow(2, new Vector4(0,0,-1,0));
		convert.SetRow(3, new Vector4(0,0,0,1));

		Initialize();
	}

	void Initialize()
	{	
		int bigDim = Mathf.Max(Screen.width, Screen.height);
		int smallDim = Mathf.Min(Screen.width, Screen.height);
		PointCloudAdapter.init(smallDim, bigDim, PointCloudAppKey.AppKey);
		
		if (Application.isEditor)
		{
			// When running from editor, initialize scene imediatly
			State = pointcloud_state.POINTCLOUD_TRACKING_SLAM_MAP;	
		} else
		{
			AddImageTargets();
			if (_drawCamera) {
				camera.clearFlags = CameraClearFlags.Nothing;
			}
			else {
				camera.clearFlags = CameraClearFlags.Skybox;
			}
		}
		NotifyStateChange();
	}

	void AddImageTargets()
	{
		foreach(PointCloudImageTarget imageTarget in imageTargets) {
			Debug.Log( "Add Image target: " + imageTarget );
			PointCloudAdapter.pointcloud_add_image_target(imageTarget);
		}
	}
	
	public void Reset()
	{
		if (Application.isEditor)
		{
			// From editor, simulate reset of point cloud
			State = pointcloud_state.POINTCLOUD_IDLE;
			NotifyStateChange();
		}
		
		PointCloudAdapter.pointcloud_reset();
		
		Initialize();
	}

	// Update is called once per frame
	void Update () 
	{	
		if (Application.isEditor) { return; }
		
		int flags = PointCloudAdapter.update(_drawCamera, camera.nearClipPlane, camera.farClipPlane, drawPoints, ref cam, ref frustum);

		bool updated = (flags & 1) > 0 && (flags & 2) > 0;

		if (updated) {
			camera_matrix = convert * cam;
			bool isIdentity = Mathf.Abs(camera_matrix[0,0] - 1) < 1e-5;

			if (!isIdentity) {
				Matrix4x4 camera_pose = camera_matrix.inverse;
			
				camera.transform.position = camera_pose.GetColumn(3) / sceneScale;
				camera.transform.rotation = QuaternionFromMatrixColumns(camera_pose);
			
				camera.projectionMatrix = frustum * convert;
				
				switch(Screen.orientation)
				{
					default:
					case ScreenOrientation.LandscapeLeft:
						RotateProjectionMatrix(90);
						break;
					case ScreenOrientation.LandscapeRight:
						RotateProjectionMatrix(-90);
						break;
					case ScreenOrientation.Portrait:
						break;
					case ScreenOrientation.PortraitUpsideDown:
						RotateProjectionMatrix(180);
						break;
				}
			}
		}

		MonitorStateChanges();
	}

	void OnPreRender()
	{
		if (_drawCamera) {
			GL.Clear(true, false, Color.black);
			PointCloudAdapter.renderFrame();
		}
	}

	static Quaternion QuaternionFromMatrixColumns(Matrix4x4 m) {
		return Quaternion.LookRotation(-m.GetColumn(2), m.GetColumn(1)); // forward, up
	}
	
	void MonitorStateChanges()
	{
		pointcloud_state newState = PointCloudAdapter.pointcloud_get_state();
		if (newState != State)
		{
			Debug.Log( "New State: " + newState );
			PreviousState = State;
			State = newState;
			NotifyStateChange();
		}
	}
	
	void NotifyStateChange()
	{
		foreach(PointCloudSceneRoot sceneRoot in sceneRoots)
		{
			sceneRoot.gameObject.SendMessage("OnPointCloudStateChanged", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnDestroy()
	{
		PointCloudAdapter.pointcloud_destroy();
		sceneRoots.Clear();

		// force local update of state from libpointcloud
		MonitorStateChanges();

		// post-destroy internal state should always be POINTCLOUD_NOT_CREATED
		if (State != pointcloud_state.POINTCLOUD_NOT_CREATED) {
			LogError ("State did not properly transition, internal state is: " + State);
		}
	}
	
	public static void AddSceneRoot(PointCloudSceneRoot sceneRoot)
	{
		sceneRoots.Add(sceneRoot);
	}

	public static void RemoveSceneRoot(PointCloudSceneRoot sceneRoot) {
		sceneRoots.Remove (sceneRoot);
	}
	
	private void LogError(string msg) {
		Debug.LogError(msg);
		Application.Quit();
	}
	
	private void RotateProjectionMatrix(float angleDegrees) {
		Quaternion correction_q = Quaternion.AngleAxis(angleDegrees, new Vector3(0, 0, 1));
		Matrix4x4 correction_rot = Matrix4x4.TRS(Vector3.zero, correction_q, new Vector3(1, 1, 1));
		camera.projectionMatrix = correction_rot * camera.projectionMatrix;
	}
	
	public static void ActivateAllImageTargets() {
		foreach(PointCloudImageTarget target in Instance.imageTargets) {
			Debug.Log( "Activate Image Target: " + target );
			target.Activate();
		}
	}
	
	public static void DeactivateAllImageTargets() {
		foreach(PointCloudImageTarget target in Instance.imageTargets) {
			target.Deactivate();
		}
	}
}