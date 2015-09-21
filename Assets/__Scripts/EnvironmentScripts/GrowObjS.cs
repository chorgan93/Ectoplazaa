using UnityEngine;
using System.Collections;

public class GrowObjS : MonoBehaviour {

	public float growRate = 1f;

	// Update is called once per frame
	void Update () {

		Vector3 newSize = transform.localScale;
		newSize.x += growRate*Time.deltaTime*TimeManagerS.timeMult;
		newSize.y = newSize.x;
		transform.localScale = newSize;
	
	}
}
