using UnityEngine;
using System.Collections;

public class MapSave : MonoBehaviour {
		
	const float buttonHeightNormalized = 0.1f;
	Rect guiArea;
	public float ButtonHeight {get; set;}
	private string CloudPath;
	
	
	void Start()
	{	
		
		guiArea = new Rect(0, Screen.height * (1f - buttonHeightNormalized), Screen.width, Screen.height * buttonHeightNormalized);
		ButtonHeight = Screen.height * buttonHeightNormalized;
		CloudPath = Application.dataPath + "/../../Documents/CloudMap.txt";
	}
	
	void Update() {
		
		guiArea = new Rect(0, Screen.height * (1f - buttonHeightNormalized), Screen.width, Screen.height * buttonHeightNormalized);
		ButtonHeight = Screen.height * buttonHeightNormalized;
		
	}
	
	void OnPointCloudStateChange() {
		if(PointCloudBehaviour.State == pointcloud_state.POINTCLOUD_IDLE) {
			PointCloudAdapter.pointcloud_enable_map_expansion();
			PointCloudBehaviour.ActivateAllImageTargets();
		}
	}	
	
	void OnGUI()
	{	
		GUILayout.BeginArea(guiArea);
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Reset", GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/4)))
		{
			PointCloudBehaviour.Instance.Reset();
			PointCloudAdapter.pointcloud_reset();
			
		}
		else if (GUILayout.Button("Start Mapping", GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/4)))
		{
			PointCloudAdapter.pointcloud_reset();
			PointCloudBehaviour.Instance.Reset();
			OnPointCloudStateChange();	
		}
		else if (GUILayout.Button("Save Map", GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/4)))
		{
			PointCloudAdapter.pointcloud_unity_save_current_map(CloudPath);
		}	
		else if (GUILayout.Button("Load Map", GUILayout.MinHeight(ButtonHeight), GUILayout.MaxWidth(Screen.width/4)))
		{
			PointCloudAdapter.pointcloud_unity_load_map(CloudPath);
		}
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

	}
}
