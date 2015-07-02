using UnityEngine;
using System.Collections;

public class ButtObjS : MonoBehaviour {

	public bool isFollowing = false;
	public float xDiff = 0.5f;
	public float yDiff = 0.5f;
	public float lerpRate = 1f;

	private SpriteRenderer headRender;
	private SpriteRenderer ownRender;

	public PlayerS parentObj;

	// Use this for initialization
	void Start () {

		parentObj = transform.parent.gameObject.GetComponent<PlayerS>();
		transform.parent = null;
		parentObj.buttObj = this;

		ownRender = GetComponentInChildren<SpriteRenderer>();
		headRender = parentObj.spriteObject.GetComponent<SpriteRenderer>();
	
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

			followPos.y += yDiff;

			transform.position = Vector3.Lerp
				(transform.position,followPos,lerpRate*Time.deltaTime*TimeManagerS.timeMult);
		}

		ownRender.enabled = headRender.enabled;

	}

	void OnTriggerEnter(Collider other){
	//	print (other.name);
		if (other.gameObject.tag == "Player"){
			//print ("YEAH");
			if (other.gameObject.GetComponent<PlayerS>() == parentObj){
				//print("YEAH");
				if (parentObj.attacking){
					if (parentObj.attackToPerform == 2){
						print ("END BULLET ATTACK");
						parentObj.isDangerous = false;
						parentObj.attackToPerform = 0;
						parentObj.attacking = false;
						parentObj.GetComponent<Rigidbody>().useGravity = true;
					}
					else if (parentObj.attackToPerform == 1){
						// add fling force
						print ("YEAH FLING");
						if (!parentObj.didLv2Fling){

							// THIS IS TRIGGERING TWICE, NULLING
							// FIX THIS IMMEDIATELY

							parentObj.didLv2Fling = true;
							parentObj.GetComponent<Rigidbody>().useGravity = true;
							parentObj.GetComponent<Rigidbody>().AddForce((parentObj.transform.position-
						                                              transform.position).normalized
						                                             *parentObj.lv2FlingForce*Time.deltaTime,
						                                             ForceMode.VelocityChange);
							// add kinesthetic effects
							CameraShakeS.C.MicroShake();
							//parentObj.SleepTime(0.15f);
						}

					}
					else{
						// ahhhh
						parentObj.GetComponent<Rigidbody>().useGravity = true;
						parentObj.isDangerous = false;
						parentObj.attacking = false;
						isFollowing = true;
					}
				}
			}
		}
	}
}
