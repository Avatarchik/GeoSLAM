using UnityEngine;
using System.Collections;

public class Domino : MonoBehaviour {
	
	public int dotTop = 0;
	public int dotBottom = 0;
	const int maxDots = 6;
	
	string dotConfiguration;
	public string DotConfiguration {
		get {
			return dotConfiguration;
		}
	}

	void Awake () {
		AddDots();
	}

	void AddDots()
	{
		GameObject dot = transform.GetChild(0).gameObject;
		GameObject[] bottom = DotArray(dotBottom);
		GameObject[] top = DotArray(dotTop);
		
		bottom[0] = dot; // place existing object
		
		PlaceDots( FillArray(dot, bottom), 1f/4f );
		PlaceDots( FillArray(dot, top), -1f/4f );
		
		dotConfiguration = Mathf.Min(bottom.Length, top.Length).ToString() + Mathf.Max(bottom.Length, top.Length).ToString();
	}

	GameObject[] DotArray(int count)
	{
		count = count > 0 ? count : Random.Range(1,maxDots+1);
		count = Mathf.Min(count, maxDots);
		return new GameObject[count];
	}
	
	GameObject[] FillArray(GameObject source, GameObject[] array)
	{
		for(int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				GameObject newDot = (GameObject)GameObject.Instantiate(source);
				newDot.transform.parent = source.transform.parent;
				newDot.transform.localRotation = source.transform.localRotation;
				newDot.transform.localPosition = source.transform.localPosition;
				newDot.transform.localScale = source.transform.localScale;
				array[i] = newDot;
			}
		}
		return array;
	}
	
	void DotPosition(GameObject dot, float x, float y)
	{
		dot.transform.localPosition = new Vector3(x, y, dot.transform.localPosition.z);
	}
	
	void PlaceDots(GameObject[] array, float centerOffsetY)
	{
		const float dx = 1f/4f;
		const float dy = 1f/8f;
		
		int len = array.Length;
		if (len % 2 != 0)
		{
			DotPosition(array[0], 0, centerOffsetY);
		}

		if (len > 1)
		{
			DotPosition(array[len-1], -dx, centerOffsetY + dy);
			DotPosition(array[len-2],  dx, centerOffsetY - dy);
		}

		if (len > 3)
		{
			DotPosition(array[len-3], -dx, centerOffsetY - dy);
			DotPosition(array[len-4],  dx, centerOffsetY + dy);
		}

		if (len > 5)
		{
			DotPosition(array[len-5], -dx, centerOffsetY);
			DotPosition(array[len-6],  dx, centerOffsetY);
		}
	}	
}
