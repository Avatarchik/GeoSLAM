using UnityEngine;
using System.Collections;

[System.Serializable]
public class PointCloudImageTarget{
	public TextAsset imageTarget;
	public float physicalWidth;
	public float physicalHeight;
	
	public void Activate() {
		PointCloudAdapter.pointcloud_activate_image_target(imageTarget.name);	
	}
	
	public void Deactivate() {
		PointCloudAdapter.pointcloud_deactivate_image_target(imageTarget.name);	
	}
}
