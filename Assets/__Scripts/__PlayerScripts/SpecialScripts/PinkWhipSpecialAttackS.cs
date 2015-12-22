using UnityEngine;
using System.Collections;

public class PinkWhipSpecialAttackS : MonoBehaviour {

	// put this on a collider that is spawned by ghostmask when special attack triggers
	public PlayerS playerRef;



	void Start(){

	}


	void FixedUpdate () {

		if (playerRef){
			GetComponent<DamageS>().MakeSpecial(playerRef);
			GetComponent<DamageS>().EnablePinkSpecial();

			transform.position = playerRef.transform.position;
			transform.rotation = playerRef.transform.rotation;

			if (!playerRef.GetSpecialState()){
				Destroy(gameObject);
			}
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

}
