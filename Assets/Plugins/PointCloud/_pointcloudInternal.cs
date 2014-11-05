using System;
using System.Runtime.InteropServices;


class _pointcloudInternal {

#if UNITY_IPHONE
	
	[DllImport("__Internal")]
	public static extern void pointcloud_unity_init(int viewport_width, int viewport_height, string appkey);

	[DllImport("__Internal")]
	public static extern int pointcloud_unity_update(bool load_camera_texture, float nearClipPlane, float farClipPlane, bool draw_points, ref IntPtr cameraMatrixArray, ref IntPtr projectionMatrixArray);

	[DllImport("__Internal")]
	public static extern void pointcloud_unity_render_frame();

	[DllImport("__Internal")]
	public static extern void pointcloud_unity_get_points(out int size, out System.IntPtr outArray);

	[DllImport("__Internal")]
	public static extern void pointcloud_unity_destroy();
		
	[DllImport("__Internal")]
	public static extern void pointcloud_unity_start_slam();
	
	[DllImport("__Internal")]
	public static extern void pointcloud_unity_reset();
	
	[DllImport("__Internal")]
	public static extern int pointcloud_unity_enable_map_expansion();
	
	[DllImport("__Internal")]
	public static extern int pointcloud_unity_disable_map_expansion();
	
	[DllImport("__Internal")]
	public static extern int pointcloud_unity_get_state();
	
	[DllImport("__Internal")]
	public static extern int pointcloud_unity_get_video_width();
	
	[DllImport("__Internal")]
	public static extern int pointcloud_unity_get_video_height();
	
	[DllImport("__Internal")]
	public static extern int pointcloud_unity_get_video_crop_x();
	
	[DllImport("__Internal")]
	public static extern int pointcloud_unity_get_video_crop_y();
		
	[DllImport("__Internal")]
	public static extern void pointcloud_unity_add_image_target(string image_id, byte[] data, int data_size, float physical_width, float physical_height);
    
	[DllImport("__Internal")]
    public static extern void pointcloud_unity_remove_image_target(string image_id);
    
	[DllImport("__Internal")]
    public static extern void pointcloud_unity_activate_image_target(string image_id);
    
	[DllImport("__Internal")]
    public static extern void pointcloud_unity_deactivate_image_target(string image_id);

	[DllImport("__Internal")]
	public static extern void pointcloud_save_current_map(string file_path);

	[DllImport("__Internal")]
	public static extern void pointcloud_load_map(string file_path);

#endif

}
