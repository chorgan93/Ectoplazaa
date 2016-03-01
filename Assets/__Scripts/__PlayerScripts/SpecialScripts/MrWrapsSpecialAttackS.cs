using UnityEngine;
using System.Collections;

public class MrWrapsSpecialAttackS : MonoBehaviour {

	// put this on a collider that is spawned by ghostmask when special attack triggers
	public PlayerS playerRef;

	public float lifeTime = 2f;

	public float turnOffColliderTime = 0f;

	public MrWrapsSpecialAttackS subExplosion;

	private Vector3 startSize;
	private Rigidbody myRigid;


	void Start(){

		// do spawn effects (muzzle flare) here and rotate accordingly
		FaceTarget(GetComponent<Rigidbody>().velocity);
		CameraShakeS.C.SmallShake();

		startSize = transform.localScale;
		myRigid = GetComponent<Rigidbody>();
	}


	void FixedUpdate () {

		if (playerRef){
			GetComponent<DamageS>().MakeSpecial(playerRef);

		}

		if(myRigid.velocity != Vector3.zero){
			if (myRigid.velocity.x > 0){
				Vector3 flipSize = startSize;
				flipSize.x *= -1f;
				transform.localScale = flipSize;
			}
			else{
				transform.localScale = startSize;
			}
		}

		lifeTime -= Time.deltaTime*TimeManagerS.timeMult;

		if (lifeTime <= turnOffColliderTime){
			GetComponent<Collider>().enabled = false;
		}

		if (lifeTime <= 0){
			Destroy(gameObject);
		}

	}

	void FaceTarget (Vector3 targetDir) {
		// for head
		
		float rotateZ = 0;
		
		if(targetDir.x == 0){
			if (targetDir.y > 0){
				rotateZ = 90;
			}
			else{
				rotateZ = -90;
			}
		}
		else{
			rotateZ = Mathf.Rad2Deg*Mathf.Atan((targetDir.y/targetDir.x));
		}	
		
		
		if (targetDir.x < 0){
			//rotateZ += 180;
		}
		
		transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateZ));
	}

	void OnTriggerEnter (Collider other){

		if ((other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground") && turnOffColliderTime <= 0){

			Debug.Log("HIT WALL " + other.gameObject.name + " " + transform.position);

			//Destroy and leave behind explosion
			if (subExplosion){
				MrWrapsSpecialAttackS newExplo = Instantiate(subExplosion, transform.position, Quaternion.identity)
					as MrWrapsSpecialAttackS;
				newExplo.GetComponent<Rigidbody>().velocity = Vector3.zero;
				newExplo.playerRef = playerRef;
			}
			lifeTime = 0;

		}

	}
}
