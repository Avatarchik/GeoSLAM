using UnityEngine;
using System.Collections;

public class GeoLocation : MonoBehaviour {
	private static readonly double R_MAJOR = 63781370;
	private static readonly double R_MINOR = 63567523.142;
	private static readonly double RATIO = R_MINOR / R_MAJOR;
	private static readonly double ECCENT = System.Math.Sqrt(1.0 - (RATIO * RATIO));

	private static readonly double DEG_TO_RAD = System.Math.PI / 180.0;
	private static readonly double RAD_TO_DEG = 180.0 / System.Math.PI;

	public float locationPollRate;

	public int maxTimeOut;

	private float deviceLatitude;
	private float deviceLongitude;
	private float deviceAltitude;

	private double deviceGeoX;
	private double deviceGeoY;
	private double deviceGeoZ;

	private float gyroX;
	private float gyroY;
	private float gyroZ;

	// allows fixing for consistant results between I-Phone and Android
	private Quaternion rotFix = Quaternion.identity;
	private Gyroscope gyro;
	private GameObject camParent;
	private GameObject camGrandParent;

	private IEnumerator StartInternalService() {
		if (Input.location.isEnabledByUser) {
			Input.location.Start(0.1f,0.1f);
			Input.compass.enabled = true;
			
			int maxWait = maxTimeOut;
			
			while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
				yield return new WaitForSeconds(1);
				maxWait--;
			}
		}
	}

	public float DeviceLatitude {
		get { return this.deviceLatitude; }
	}

	public float DeviceLongitude {
		get { return this.deviceLongitude; }
	}

	public float DeviceAltitude {
		get { return this.deviceAltitude; }
	}

	public float DeviceGyroX {
		get { return this.gyroX; }
	}

	public float DeviceGyroY {
		get { return this.gyroY; }
	}

	public float DeviceGyroZ {
		get { return this.gyroZ; }
	}

	public GameObject Parent {
		get {
			return camGrandParent;
		}
	}

	public Vector3 DeviceGyroEular {
		get {
			return new Vector3(gyroX, gyroY, gyroZ);
		}
	}

	public float CompassDegrees {
		get {
			return Input.compass.magneticHeading;
		}
	}

	public Vector3 DeviceGeoToCartesian {
		get {
			float lat = DeviceLatitude * Mathf.Deg2Rad;
			float lon = DeviceLongitude * Mathf.Deg2Rad;

			float x = 6371.0f * Mathf.Cos(lat) * Mathf.Cos(lon);
			float y = 6371.0f * Mathf.Cos(lat) * Mathf.Sin(lon);
			float z = 6371.0f * Mathf.Sin(lat);
			
			return new Vector3(x,y,z);
		}
	}

	public Vector3 DeviceGeoToCartesianM {
		get {
			double lat = DeviceLatitude * DEG_TO_RAD;
			double lon = DeviceLongitude * DEG_TO_RAD;
			
			double x = (6371.0 * 1000.0) * System.Math.Cos(lat) * System.Math.Cos(lon);
			double y = (6371.0 * 1000.0) * System.Math.Cos(lat) * System.Math.Sin(lon);
			double z = (6371.0 * 1000.0) * System.Math.Sin(lat);
			
			return new Vector3((float)y,(float)z,(float)x);
		}
	}

	/**
	 * Implementation of Elliptical Mercator
	 * 
	 */
	public Vector3 DeviceGeoPlaneProjection {
		get {
			//return GetGeoPlaneProjection(DeviceLatitude, DeviceLongitude, DeviceAltitude);
			//return GetGeoProjection(DeviceLatitude, DeviceLongitude, DeviceAltitude);
			return new Vector3((float) deviceGeoX, (float) deviceGeoY, (float) deviceGeoZ);
		}
	}

	/*public Vector2 GetGeoPlaneProjection(float lat, float lon) {
		float x = (Mathf.Deg2Rad * lon) * R_MAJOR;
		
		lat = Mathf.Min(89.5f, Mathf.Max(lat, -89.5f));
		float phi = Mathf.Deg2Rad * lat;
		float sinphi = Mathf.Sin(phi);
		float con = ECCENT * sinphi;
		
		con = Mathf.Pow(((1.0f - con) / (1.0f + con)), (0.5f * ECCENT));
		float ts = Mathf.Tan(0.5f * ((Mathf.PI * 0.5f) - phi)) / con;
		
		float y = 0.0f - R_MAJOR * Mathf.Log(ts);
		
		return new Vector2(x * FINAL_SCALE, y * FINAL_SCALE);
	}*/

	public Vector3 GetGeoPlaneProjection(double lat, double lon, double alt) {
		double x = (lon * DEG_TO_RAD) * R_MAJOR;

		lat = System.Math.Min(89.5, System.Math.Max(lat, -89.5));

		double phi = lat * DEG_TO_RAD;
		double sinphi = System.Math.Sin(phi);
		double con = ECCENT * sinphi;

		con = System.Math.Pow(((1.0 - con) / (1.0 + con)), (0.5 * ECCENT));

		double ts = System.Math.Tan(0.5 * ((System.Math.PI * 0.5) - phi)) / con;

		double y = 0.0 - R_MAJOR * System.Math.Log(ts);

		//return new Vector3((float)x, (float)alt, (float)y);
		return new Vector3((float)x, (float)alt, (float)y);
	}

	public Vector3 GetGeoPlaneProjectionRelative(double lat, double lon, double alt) {
		double x = (lon * DEG_TO_RAD) * R_MAJOR;
		
		lat = System.Math.Min(89.5, System.Math.Max(lat, -89.5));
		
		double phi = lat * DEG_TO_RAD;
		double sinphi = System.Math.Sin(phi);
		double con = ECCENT * sinphi;
		
		con = System.Math.Pow(((1.0 - con) / (1.0 + con)), (0.5 * ECCENT));
		
		double ts = System.Math.Tan(0.5 * ((System.Math.PI * 0.5) - phi)) / con;
		double y = 0.0 - R_MAJOR * System.Math.Log(ts);

		double relX = x - deviceGeoX;
		double relY = alt - deviceGeoY;
		double relZ = y - deviceGeoZ;

		// relative coordinates are smaller, minimal precision loss here
		return new Vector3((float) relX, (float) relY, (float) relZ);
	}
	
	private void SetDeviceGeoPlaneProjection() {
		double x = (((double)deviceLongitude) * DEG_TO_RAD) * R_MAJOR;
		
		double lat = System.Math.Min(89.5, System.Math.Max((double) deviceLatitude, -89.5));
		
		double phi = lat * DEG_TO_RAD;
		double sinphi = System.Math.Sin(phi);
		double con = ECCENT * sinphi;
		
		con = System.Math.Pow(((1.0 - con) / (1.0 + con)), (0.5 * ECCENT));
		
		double ts = System.Math.Tan(0.5 * ((System.Math.PI * 0.5) - phi)) / con;
		double y = 0.0 - R_MAJOR * System.Math.Log(ts);
		
		deviceGeoX = x;
		deviceGeoY = deviceAltitude;
		deviceGeoZ = y;
	}

	public Vector3 GetGeoProjectionRelative(double lat, double lon, double alt) {
		double Re = 637813.7;
		double Rp = 635675.231424518;
		
		double latrad = lat / 180.0 * System.Math.PI;
		double lonrad = lon / 180.0 * System.Math.PI;

		double coslat = System.Math.Cos(latrad);
		double sinlat = System.Math.Sin(latrad);
		double coslon = System.Math.Cos(lonrad);
		double sinlon = System.Math.Sin(lonrad);
		
		double term1 = (Re * Re * coslat) /
			System.Math.Sqrt(Re * Re * coslat * coslat + Rp * Rp * sinlat * sinlat);
		
		double term2 = alt * coslat + term1;
		
		double x = coslon * term2;
		double y = sinlon * term2;
		double z = alt * sinlat + (Rp * Rp * sinlat) /
			System.Math.Sqrt(Re * Re * coslat * coslat + Rp * Rp * sinlat * sinlat);

		double relX = x - deviceGeoX;
		double relY = z - deviceGeoZ;
		double relZ = y - deviceGeoY;

		return new Vector3((float) relX, (float) relY, (float) relZ);
	}

	private void SetDeviceGeoProjection() {
		double Re = 637813.7;
		double Rp = 635675.231424518;
		
		double latrad = ((double) deviceLatitude) / 180.0 * System.Math.PI;
		double lonrad = ((double) deviceLongitude) / 180.0 * System.Math.PI;
		
		double coslat = System.Math.Cos(latrad);
		double sinlat = System.Math.Sin(latrad);
		double coslon = System.Math.Cos(lonrad);
		double sinlon = System.Math.Sin(lonrad);
		
		double term1 = (Re * Re * coslat) /
			System.Math.Sqrt(Re * Re * coslat * coslat + Rp * Rp * sinlat * sinlat);
		
		double term2 = ((double) deviceAltitude) * coslat + term1;
		
		double x = coslon * term2;
		double y = sinlon * term2;
		double z = ((double) deviceAltitude) * sinlat + (Rp * Rp * sinlat) /
			System.Math.Sqrt(Re * Re * coslat * coslat + Rp * Rp * sinlat * sinlat);

		deviceGeoX = x;
		deviceGeoY = z;
		deviceGeoZ = y;
	}

	/*public Vector3 GetGeoPlaneProjection(float lat, float lon, float alt) {
		return GetGeoPlaneProjection((double)lat, (double)lon, (double) alt);
	}*/

	/*public Vector3 GetGeoProjection(float lat, float lon, float alt) {
		return GetGeoProjection((double)lat, (double) lon, (double) alt);
	}*/

	public bool IsWithinRegion(float latitude, float longitude, float distance) {
		float dlon = longitude - deviceLongitude;
		float dlat = latitude - deviceLatitude;

		float dlatSQ = Mathf.Sin(dlat / 2.0f) * Mathf.Sin(dlat / 2.0f);
		float dlonSQ = Mathf.Sin(dlon / 2.0f) * Mathf.Sin(dlon / 2.0f);

		float a = dlatSQ + Mathf.Cos(deviceLatitude) * Mathf.Cos(latitude) * dlonSQ;
		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1.0f - a));

		float dist = 6373.0f * c;

		if (dist <= distance) {
			return true;
		}

		return false;
	}

	public void StopService() {
		if (Input.location.status == LocationServiceStatus.Running) {
			Input.location.Stop();
			gyro.enabled = false;
		}
	}

	public void StartService() {
		if (SystemInfo.supportsGyroscope) {
			gyro = Input.gyro;
			gyro.enabled = true;

			// Setup Gyro Fixes
			if (camParent == null) {
				Transform currentParent = transform.parent;
				
				camParent = new GameObject("GYRO_PARENT");

				camParent.transform.position = transform.position;
				transform.parent = camParent.transform;

				camGrandParent = new GameObject("GYRO_GRAND_PARENT");

				camGrandParent.transform.position = transform.position;
				camParent.transform.parent = camGrandParent.transform;
				camGrandParent.transform.parent = currentParent;
			}

			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
				camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
			} 
			else if (Screen.orientation == ScreenOrientation.Portrait) {
				camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
			} 
			else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
				camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
			} 
			else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
				camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
			} 
			else {
				camParent.transform.eulerAngles = new Vector3 (90, 180, 0);
			}
			
			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
				rotFix = new Quaternion (0, 0, 1, 0);
			} 
			else if (Screen.orientation == ScreenOrientation.Portrait) {
				rotFix = new Quaternion (0, 0, 1, 0);
			} 
			else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
				rotFix = new Quaternion (0, 0, 1, 0);
			} 
			else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
				rotFix = new Quaternion (0, 0, 1, 0);
			} 
			else {
				rotFix = new Quaternion (0, 0, 1, 0);
			}

			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}

		if (Input.location.status == LocationServiceStatus.Stopped) {
			StartCoroutine(StartInternalService());
		}
	}

	private Quaternion currentRot;
	private Quaternion gCurrentRot;
	private Quaternion newRot;
	private Quaternion gNewRot;

	void Update() {
		if (Input.location.status == LocationServiceStatus.Running) {
			deviceLatitude = Input.location.lastData.latitude;
			deviceLongitude = Input.location.lastData.longitude;
			deviceAltitude = Input.location.lastData.altitude;

			SetDeviceGeoPlaneProjection();

			// compute Cartesian Coordinates and store as double precision

			/*double x = (deviceLongitude * DEG_TO_RAD) * R_MAJOR;
			
			double lat = System.Math.Min(89.5, System.Math.Max(deviceLatitude, -89.5));
			
			double phi = lat * DEG_TO_RAD;
			double sinphi = System.Math.Sin(phi);
			double con = ECCENT * sinphi;
			
			con = System.Math.Pow(((1.0 - con) / (1.0 + con)), (0.5 * ECCENT));
			
			double ts = System.Math.Tan(0.5 * ((System.Math.PI * 0.5) - phi)) / con;
			double y = 0.0 - R_MAJOR * System.Math.Log(ts);

			deviceGeoX = x;
			deviceGeoY = (double) deviceAltitude;
			deviceGeoZ = y;*/
		}

		if (SystemInfo.supportsGyroscope && gyro.enabled == true) {
			currentRot = transform.localRotation;
			gCurrentRot = camGrandParent.transform.localRotation;

			Quaternion quatMap;

			#if UNITY_IPHONE
			quatMap = gyro.attitude;
			#elif UNITY_ANDROID
			quatMap = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
			#endif

			/*float mag = Input.compass.magneticHeading;

			gNewRot = Quaternion.Euler(camGrandParent.transform.localEulerAngles.x,
			                           mag,
			                           camGrandParent.transform.localEulerAngles.z);

			camGrandParent.transform.localRotation = Quaternion.Lerp(gCurrentRot, gNewRot, Time.smoothDeltaTime * 10);*/
			//transform.localRotation = Quaternion.Lerp(transform.localRotation, quatMap * rotFix, 0.1f);
			newRot = quatMap * rotFix;
			transform.localRotation = Quaternion.Lerp(currentRot, newRot, Time.smoothDeltaTime * 10);
			Vector3 rotEular = transform.localRotation.eulerAngles;
			
			gyroX = rotEular.x;
			gyroY = rotEular.y;
			gyroZ = rotEular.z;
		}
	}

	public Vector3 GeoToCartesian(float lat, float lon) {
		lat = lat * Mathf.Deg2Rad;
		lon = lon * Mathf.Deg2Rad;

		float x = 6371.0f * Mathf.Cos(lat) * Mathf.Cos(lon);
		float y = 6371.0f * Mathf.Cos(lat) * Mathf.Sin(lon);
		float z = 6371.0f * Mathf.Sin(lat);

		return new Vector3(x,y,z);
	}

	public Vector3 GeoToCartesianM(double lat, double lon) {
		lat = lat * DEG_TO_RAD;
		lon = lon * DEG_TO_RAD;
		
		double x = (6371.0 * 1000.0) * System.Math.Cos(lat) * System.Math.Cos(lon);
		double y = (6371.0 * 1000.0) * System.Math.Cos(lat) * System.Math.Sin(lon);
		double z = (6371.0 * 1000.0) * System.Math.Sin(lat);

		return new Vector3((float)y,(float)z,(float)x);
	}

	public LocationServiceStatus GetStatus() {
		return Input.location.status;
	}
}
