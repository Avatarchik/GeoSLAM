using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointCloudSceneRoot : MonoBehaviour {
	
	public bool hideChildrenIfNoTracking = true;
	public static bool rotationSet = false;


	public static float up; 

		
	void Awake()
	{
		PointCloudBehaviour.AddSceneRoot(this);
		up = Mathf.Atan2( Input.gyro.attitude.y, Input.gyro.attitude.x )*Mathf.Rad2Deg;
	}
	
	public static void EnableRenderingRecursively(Transform go, bool enable)
	{
		Renderer rendererComponent = go.GetComponent<Renderer>();
		if (rendererComponent != null)
		{
			rendererComponent.enabled = enable;
		}
			
		foreach(Transform child in go)
		{
			EnableRenderingRecursively(child, enable);
		}
//		if( !rotationSet ){
//			rotationSet = true;
//			up = Mathf.Atan2( Input.acceleration.y, Input.acceleration.x )*Mathf.Rad2Deg;
//			Vector3 newRotation = new Vector3( go.rotation.eulerAngles.x, go.rotation.eulerAngles.y, up );
//			go.transform.localEulerAngles = newRotation;
//		}
//		up = Mathf.Atan2( Input.gyro.attitude.y, Input.gyro.attitude.x )*Mathf.Rad2Deg;
//		Debug.Log("Rotation: " + go.transform.name + " : " + go.transform.localRotation );
//		Debug.Log("Up: " + up );
////		Debug.Log("Cloud Orientation: " + PointCloudAdapter.pointcloud_get_points() );
	}
	
	public virtual void OnPointCloudStateChanged()
	{
		if(hideChildrenIfNoTracking) {
			bool showChildren = PointCloudBehaviour.HasTracking();
			EnableRenderingRecursively(transform, showChildren);
		}

//		float length = Mathf.Sqrt( Mathf.Pow( Input.gyro.gravity.x, 2 ) + Mathf.Pow( Input.gyro.gravity.y, 2 ) + Mathf.Pow( Input.gyro.gravity.z, 2 ) );
//
//		if( length > 0.1 ){
//			Matrix4x4 gravity = new Matrix4x4();
//			gravity[3,3] = 1.0f;
//			gravity[0,0] = Input.gyro.gravity.x/length;
//			gravity[0,1] = Input.gyro.gravity.y/length;
//			gravity[0,2] = Input.gyro.gravity.z/length;
//
//			gravity[1,0] = 0.0f;
//			gravity[1,1] = 1.0f;
//			gravity[1,2] = -1*Input.gyro.gravity.y / Input.gyro.gravity.z;
//			length = Mathf.Sqrt ( Mathf.Pow( gravity[1,0], 2 ) + Mathf.Pow( gravity[1,1], 2 )  + Mathf.Pow( gravity[1,2], 2 ) );
//			gravity[1,0] = gravity[1,0]/length;
//			gravity[1,1] = gravity[1,0]/length;
//			gravity[1,2] = gravity[1,0]/length;
//
//			gravity[2,0] = gravity[0,1] * gravity[1,2] - gravity[0,2] * gravity[1,1];
//			gravity[2,1] = gravity[1,0] * gravity[0,2] - gravity[1,2] * gravity[0,0];
//			gravity[2,2] = gravity[0,0] * gravity[1,1] - gravity[0,1] * gravity[1,0];
//
//			transform.rotation = GetRotation(gravity);
//		}
//		Quaternion converted = ConvertRotation( Input.gyro.attitude );
//		Vector3 newRotationVector = new Vector3( Input.gyro.attitude.eulerAngles.x - Camera.main.transform.localEulerAngles.x, Input.gyro.attitude.eulerAngles.y - Camera.main.transform.localEulerAngles.y, Input.gyro.attitude.eulerAngles.z - Camera.main.transform.localEulerAngles.z );
////		Vector3 newRotationVector = new Vector3( Camera.main.transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, Camera.main.transform.localEulerAngles.z );
//		Vector3 testRotationVector = new Vector3(  Input.gyro.attitude.x * 180/Mathf.PI - Camera.main.transform.eulerAngles.x, Input.gyro.attitude.y * 180/Mathf.PI - Camera.main.transform.eulerAngles.y, Input.gyro.attitude.z * 180/Mathf.PI - Camera.main.transform.eulerAngles.z ); 
//		Debug.Log( "T: " + transform.eulerAngles );
//		Debug.Log( "C: " + camera.transform.eulerAngles );
//		Debug.Log( "G: " + Input.gyro.attitude.eulerAngles );
//		Debug.Log( "TEST: " + testRotationVector );
//		Debug.Log( "NEW: " + newRotationVector );
//		transform.LookAt( Camera.main.transform );
//		Vector3 newRotationVector = new Vector3( 0, transform.eulerAngles.y, 0 ); 
//		transform.eulerAngles = newRotationVector;
////		transform.eulerAngles = camera.transform.eulerAngles;
//		transform.eulerAngles = Input.gyro.attitude.eulerAngles - transform.rotation.eulerAngles;
//		if( Input.deviceOrientation == DeviceOrientation.Portrait ){
//			Vector3 newAngle = new Vector3( transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - Input.gyro.attitude.eulerAngles.x);
//			transform.eulerAngles = newAngle;
//		}
//		Debug.Log( "T: " + transform.eulerAngles );
//		if( transform.childCount > 0 ){
//			Debug.Log( "Tardis" );
//			Transform child =transform.FindChild("tardisTest").transform;
//			Debug.Log( "Tardis" + child.eulerAngles );
//			Debug.Log( "Tardis" + child.localEulerAngles );
//			child.localEulerAngles = converted.eulerAngles - child.transform.localEulerAngles;
//		}
//		transform.LookAt( Input.gyro.gravity );
//		up = Mathf.Atan2( Input.gyro.attitude.y, Input.gyro.attitude.x )*Mathf.Rad2Deg;
//		Debug.Log( "R: " + transform.rotation );
//		transform.rotation = Quaternion.LookRotation( Input.gyro.attitude.eulerAngles );
//		Debug.Log( "R: " + transform.rotation );
	}

	void Update(){

//		Vector3 newRotationVector = new Vector3( Input.gyro.attitude.x - Camera.main.transform.eulerAngles.x, Input.gyro.attitude.y - Camera.main.transform.eulerAngles.y, Input.gyro.attitude.z - Camera.main.transform.eulerAngles.z ); 
//		this.transform.eulerAngles = newRotationVector;
//		this.transform.eulerAngles = newRotationVector;

//		Quaternion converted = ConvertRotation( Input.gyro.attitude );
//		transform.eulerAngles = Input.gyro.gravity;
//		transform.LookAt( Camera.main.transform );
//		Vector3 newRotationVector = new Vector3( 0, transform.eulerAngles.y, 0 );
//		transform.eulerAngles = newRotationVector;
//		transform.localRotation = Quaternion.Inverse(ConvertRotation(Input.gyro.attitude));
	}

	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);
	}

	public static Quaternion GetRotation(Matrix4x4 matrix)
	{
		var qw = Mathf.Sqrt(1f + matrix.m00 + matrix.m11 + matrix.m22) / 2;
		var w = 4 * qw;
		var qx = (matrix.m21 - matrix.m12) / w;
		var qy = (matrix.m02 - matrix.m20) / w;
		var qz = (matrix.m10 - matrix.m01) / w;
		
		return new Quaternion(qx, qy, qz, qw);
	}
	
}
