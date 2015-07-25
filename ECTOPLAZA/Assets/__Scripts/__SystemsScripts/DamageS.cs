using UnityEngine;
using System.Collections;

public class DamageS : MonoBehaviour {

	private float pauseTime = 0.8f;
	public PlayerS playerRef;

	private float knockbackMult = 1.5f;

	void Start () {
		playerRef = transform.parent.GetComponent<PlayerS>();
	}

	void FixedUpdate () {

		// this is to fix a bug with damage obj not finding its own reference
		if (playerRef == null){
			playerRef = transform.parent.GetComponent<PlayerS>();
		}

	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "Player"){
			PlayerS otherPlayer = other.GetComponent<PlayerS>();

			if (otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0){
				// only deal damage if higher priority or other player isnt attacking
				if ((!otherPlayer.attacking) || (otherPlayer.attacking &&
					otherPlayer.attackPriority < playerRef.attackPriority)){
					otherPlayer.TakeDamage(playerRef.maxHealth);
					otherPlayer.SleepTime(pauseTime);
					playerRef.SleepTime(pauseTime);
		
					CameraShakeS.C.LargeShake();
	
					// add to score
					playerRef.score++;
				}
				else{
					// apply knockback to both players and end attacks if priority is same
					if (otherPlayer.attacking && otherPlayer.attackPriority == playerRef.attackPriority){
						// apply vel to both players equal to current vel x something
						otherPlayer.GetComponent<Rigidbody>().AddForce(otherPlayer.GetComponent<Rigidbody>().velocity
						                                               *-knockbackMult);
						playerRef.GetComponent<Rigidbody>().AddForce(playerRef.GetComponent<Rigidbody>().velocity
						                                             *-knockbackMult);

						print ("Tie!");
					}
				}
			}
		}

		if (other.gameObject.tag == "PlayerTrail"){

			//print ("yeah");

			PlayerS otherPlayer = other.GetComponent<DotColliderS>().whoCreatedMe;

			if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0){

				print (playerRef);
			
				otherPlayer.TakeDamage(playerRef.maxHealth);
				otherPlayer.SleepTime(pauseTime);
				playerRef.SleepTime(pauseTime/4);

				CameraShakeS.C.LargeShake();

				
				// add to score
				playerRef.score++;

			}
		}
	}
}
