using UnityEngine;
using System.Collections;

public class MrWrapsSpecialAttackS : MonoBehaviour {

	// put this on a collider that is spawned by ghostmask when special attack triggers
	public PlayerS playerRef;

	public float lifeTime = 2f;


	void Start(){

		// do spawn effects (muzzle flare) here and rotate accordingly
		FaceTarget(GetComponent<Rigidbody>().velocity);
		CameraShakeS.C.SmallShake();
	}


	void FixedUpdate () {

		if (playerRef){
			GetComponent<DamageS>().MakeSpecial(playerRef);

		}

		lifeTime -= Time.deltaTime*TimeManagerS.timeMult;
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

		if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground"){

			//Destroy and leave behind explosion
			lifeTime = 0;

		}

	}
}
