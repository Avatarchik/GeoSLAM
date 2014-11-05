using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class PointCloudKeyWizard : EditorWindow {
    
    string appKey;
	
    public static void CreateWindow()
	{
		PointCloudKeyWizard window = (PointCloudKeyWizard)EditorWindow.GetWindow (typeof (PointCloudKeyWizard), true, "PointCloud App Key");
		window.appKey = PointCloudAppKey.AppKey;
    }
	
    void OnGUI () {
		
		GUILayout.Label ("PointCloud Application Key:", EditorStyles.boldLabel);
            appKey = EditorGUILayout.TextField ("", appKey);
        
		if (GUILayout.Button("Request Key", GUILayout.MinHeight(40)))
		{
			PointCloudMenu.OpenUrl( "http://developer.pointcloud.io/sdk/applicationkeys" );
		}
		
		GUILayout.FlexibleSpace();
		
		if (GUILayout.Button("Save", GUILayout.MinHeight(40)) )
		{
			WriteAppKey(appKey);
			Close();
		}
		GUILayout.FlexibleSpace();
    }
	
	static void WriteAppKey(string key)
	{
		string path = "Assets/Plugins/PointCloud/PointCloudAppKey.cs";

		string content = string.Format("public class PointCloudAppKey {{ \n  public static string AppKey = \"{0}\"; \n}}", key);
		
		File.WriteAllText(path, content);
		
		AssetDatabase.ImportAsset(path);
	}
	
}