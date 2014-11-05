using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallGun : MonoBehaviour {
	
	public PointCloudSceneRoot pointCloudSceneRoot;
	public int maxProjectiles = 30;
	public float fireDelay = 0.5f;
	public float projectileScale = 0.05f;
	
	private List<Rigidbody> activeProjectile;
	private float lastShot = 0f;
	
	// Use this for initialization
	void Start ()
	{
		activeProjectile = new List<Rigidbody>();
	}
		
	void OnPointCloudStateChanged() {
		if (!PointCloudBehaviour.HasInitialized()) 
		{
			DestroyProjectiles();
		}
	}
	
	void DestroyProjectiles()
	{
		if (activeProjectile != null)
		{
			foreach(Rigidbody projectile in activeProjectile)
			{
				GameObject.Destroy(projectile.gameObject);
			}
			activeProjectile.Clear();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		lastShot -= Time.deltaTime;
		
		bool touch = Input.touchCount > 0;
		
		float buttonHeight = 0.1f * Screen.height;
		
		if ( lastShot < 0f && (
			(touch && Input.touches[0].position.y > buttonHeight) || 
			(Input.GetMouseButton(0) && Input.mousePosition.y > buttonHeight)) &&
			PointCloudBehaviour.HasTracking())
		{
			FireProjectile(5 * transform.forward);
			lastShot = fireDelay;
		}
	}
	
	void FireProjectile(Vector3 direction)
	{
		Vector3 position = transform.position + new Vector3(0, -0.08f, 0.04f);
		NewProjectile(position, direction);
	}
	
	Rigidbody NewProjectile(Vector3 position, Vector3 velocity)
	{
		Rigidbody projectile = null;
		if (activeProjectile.Count > maxProjectiles)
		{
			projectile = activeProjectile[0];
			activeProjectile.RemoveAt(0);
			projectile.position = position;
		}
		else
		{
			GameObject newProjectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			newProjectile.transform.position = position;
			newProjectile.transform.parent = pointCloudSceneRoot.transform;
			newProjectile.transform.localScale = Vector3.one * projectileScale;
			
			projectile = newProjectile.AddComponent<Rigidbody>();
			projectile.freezeRotation = true;
			projectile.drag = 1.8f;
		}
		projectile.velocity = velocity;
		
		activeProjectile.Add(projectile);
		
		return projectile;
	}
}
