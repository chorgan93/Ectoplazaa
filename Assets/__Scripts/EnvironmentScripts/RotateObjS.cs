using UnityEngine;
using System.Collections;

public class RotateObjS : MonoBehaviour {

	public float rotateRate = 1f;

	public bool chanceToReverse = true;

	// Use this for initialization
	void Start () {

		if (chanceToReverse){
			float chance = Random.Range(0,100);
			if (chance < 50){
				rotateRate *= -1f;
			}
		}
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		Vector3 rotateAmt = new Vector3(0,0,rotateRate*Time.deltaTime);
		transform.Rotate(rotateAmt);

	}
}
