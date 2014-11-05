using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PointCloudSceneRoot))]
public class DominoPlacement : MonoBehaviour {
	
	public GameObject dominoPrefab;
	public int maxDominos = 30;
	
	private float dd = 0f, dr = 0f;
	public float initD = 0.5f;
	public float initR = 0.4f;
	public float deltaD = 0.2f;
	public float deltaR = 0.01f;

	void OnPointCloudStateChanged() {
		if(PointCloudBehaviour.State == pointcloud_state.POINTCLOUD_NOT_CREATED) 
		{
			DestroyDominos();
		} 
		else if (transform.GetChildCount() == 0) 
		{
			PlaceDominos();
		}
	}
	
	void DestroyDominos()
	{
		foreach(Transform child in transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}
	
	void PlaceDominos()
	{
		dd = initD;
		dr = initR;
		
		Vector3 position = GetNewPosition();
		Vector3 previousPosition = position;
		
		int dominoIndex = 0;
		PlaceDomino( CreateNewDomino(dominoIndex), previousPosition, previousPosition);
		
		while (++dominoIndex < maxDominos)
		{
			position = GetNewPosition();
			PlaceDomino( CreateNewDomino(dominoIndex), position, previousPosition);
			previousPosition = position;
		}
	}
	
	Vector3 GetNewPosition()
	{
		dd += deltaD;
		dr += deltaR;
		return new Vector3(Mathf.Sin(dd) * dr, 0, Mathf.Sin(dd + Mathf.PI * 0.5f ) * dr);
	}
	
	GameObject CreateNewDomino(int index)
	{
		GameObject domino = (GameObject)GameObject.Instantiate(dominoPrefab);
		domino.transform.parent = transform;
		
		domino.name = "Domino" + index + "_" + domino.GetComponent<Domino>().DotConfiguration;

		return domino;
	}
	
	void PlaceDomino(GameObject domino, Vector3 position, Vector3 previousPosition)
	{
		Quaternion medianRotation = Quaternion.Slerp(Quaternion.LookRotation(position) , Quaternion.LookRotation(previousPosition), 0.5f);

		domino.transform.position = position;
		domino.transform.rotation = medianRotation * Quaternion.AngleAxis(90, Vector3.up);
	}
	
}
