using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;

public class PointCloudMenu : ScriptableObject {

	[MenuItem("PointCloud/Set Application Key")]
	static void SetAppKey()
	{
		PointCloudKeyWizard.CreateWindow();
	}

	[MenuItem("PointCloud/iOS SDK Web Page")]
	static void SdkWebSite()
	{
		OpenUrl( "http://developer.pointcloud.io/sdk/" );
	}
	
	public static void OpenUrl(string url)
	{
		ProcessStartInfo processStartInfo = new ProcessStartInfo( "/usr/bin/open", url );
		Process.Start( processStartInfo );
	}
	
	[MenuItem("PointCloud/Documentation")]
	static void Documentation()
	{
		OpenUrl( "Assets/Plugins/PointCloudPlugin.html" );
	}
	
}
