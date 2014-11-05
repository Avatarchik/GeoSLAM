using UnityEngine;
using System.Collections;

[System.Serializable]
public class GeoObject {

	private GeoLocation location;

	[SerializeField]
	private string latitude;

	[SerializeField]
	private string longitude;

	[SerializeField]
	private string altitude;
	
	protected double objectLatitude;
	protected double objectLongitude;
	protected double objectAltitude;

	public GeoLocation Location {
		get {
			if (location == null) {
				location = GameObject.FindObjectOfType<GeoLocation>();

				double.TryParse(latitude, out objectLatitude);
				double.TryParse(longitude, out objectLongitude);
				double.TryParse(altitude, out objectAltitude);
			}

			return location;
		}
	}

	public Vector3 RelativePosition {
		get {
			Vector3 proj = Location.GetGeoPlaneProjectionRelative(objectLatitude, objectLongitude, objectAltitude);
			return proj;
		}
	}

	public float RelativeDistance {
		get {
			return Vector3.Distance(RelativePosition, Vector3.zero);
		}
	}
}
