using UnityEngine;
using System.Collections;

public class BandageObjS : MonoBehaviour {
	
	public float rotateRate;

	// Update is called once per frame
	void FixedUpdate () {

		transform.Rotate(new Vector3(0,0,rotateRate*Time.deltaTime));
	
	}
}
