using UnityEngine;
using System.Collections;

public class MyGeoLocation : MonoBehaviour {

	public GeoLocation location;

	// Use this for initialization
	void Start () {
		location.StartService();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
