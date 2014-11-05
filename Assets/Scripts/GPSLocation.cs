using UnityEngine;
using System.Collections;

public class GPSLocation : MonoBehaviour {

	public GeoObject[] waypoints = new GeoObject[10];
	public GUISkin skin;
	
	public bool waypoint1Found = false;
	public bool waypoint2Found = false;
	private float distance = 5000f;
	
	public GameObject model01;
	public GameObject model02;

	void Update () {

		int found = 0;
		foreach ( GeoObject waypoint in waypoints ){
			if( waypoint.RelativeDistance < distance )
			{
				found += 1;
			}
		}
		if( found > 0 ){
			model01.SetActive( true );
			waypoint1Found = true;
		}else{ 
			model01.SetActive( false );
			waypoint1Found = false;
		}


	}

	void OnGUI () {
		GUI.skin = skin;
		for( int i = 0; i < waypoints.Length; i++ ){
			GUI.Label( new Rect( 0, 0, Screen.width, i * ( Screen.height / 12 ) ), "Waypoint: " + i + ": " + waypoints[ i ].RelativeDistance );
		}

		if( waypoint1Found )
		{
			GUI.Label( new Rect( 0, Screen.height/4, Screen.width, Screen.height ), "Scan for Aliens");
		}
	}
}
