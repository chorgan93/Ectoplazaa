using UnityEngine;
using System.Collections;

public class ButtObjS : MonoBehaviour {

	public bool isFollowing = false;
	public float xDiff = 0.5f;
	public float lerpRate = 1f;

	public PlayerS parentObj;

	// Use this for initialization
	void Start () {

		parentObj = transform.parent.gameObject.GetComponent<PlayerS>();
		transform.parent = null;
		parentObj.buttObj = this;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (isFollowing){

			Vector3 followPos = parentObj.transform.position;
			if (parentObj.facingRight){
				followPos.x -= xDiff;
			}
			else{
				followPos.x += xDiff;
			}

			transform.position = Vector3.Lerp
				(transform.position,followPos,lerpRate*Time.deltaTime*TimeManagerS.timeMult);
		}

	}
}
