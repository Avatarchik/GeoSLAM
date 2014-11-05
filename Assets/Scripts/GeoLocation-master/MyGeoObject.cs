using UnityEngine;
using System.Collections;

public class MyGeoObject : MonoBehaviour {

	public GeoObject obj;
	private static int offset = 10;

	private int thisOffset = 0;

	// Use this for initialization
	void Start () {
		thisOffset = offset;
		offset += 20;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = obj.RelativePosition;

		// distance from user to obj is pos.magnitude
	}

	void OnGUI() {
		GUI.Label (new Rect (10, thisOffset, 400, 50), gameObject.name);
		GUI.Label (new Rect (100, thisOffset, 400, 50), ""+obj.RelativeDistance);
	}
}
