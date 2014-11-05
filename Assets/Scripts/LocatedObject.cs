using UnityEngine;
using System.Collections;

public class LocatedObject : MonoBehaviour {

	public GeoObject obj;
	private Vector3 test;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		test = obj.RelativePosition;
	}
}
