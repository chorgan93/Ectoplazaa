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

	public void ManageCollision(GameObject other){
		
		
		if (other.gameObject.tag == "Player") {
			//print (other.name); 
			
			PlayerS otherPlayer = other.gameObject.GetComponent<PlayerS> ();
			
			if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0) {
				// only deal damage if higher priority or other player isnt attacking
				if ((!otherPlayer.attacking) || (otherPlayer.attacking && otherPlayer.attackPriority < playerRef.attackPriority)) {
					otherPlayer.TakeDamage (50f);
					otherPlayer.SleepTime (pauseTime);
					playerRef.SleepTime (pauseTime);
					
					MakeExplosion(otherPlayer.gameObject, playerRef.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 
					
					CameraShakeS.C.LargeShake ();
					
					// add to score
					//playerRef.score++;
				} else {
					// apply knockback to both players and end attacks if priority is same
					if (otherPlayer.attacking && otherPlayer.attackPriority == playerRef.attackPriority) {
						// apply vel to both players equal to current vel x something
						MakeExplosion(otherPlayer.gameObject, playerRef.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 

						print ("Tie!");
					}
				}
			}
		}

		 if (other.gameObject.tag == "PlayerTrail"){


			//print ("yeah");

			PlayerS otherPlayer = other.GetComponent<DotColliderS>().whoCreatedMe;

			if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0){

				otherPlayer.GetComponent<TrailHandlerRedubS>().ChopTail(other.gameObject);
				if(playerRef.playerNum ==1)
				{
					print("P1 TAIL HIT" );
				}

				//print (other.GetComponent<DotColliderS>().whoCreatedMe); 

				//print (playerRef);
			
				otherPlayer.SleepTime(pauseTime);
				playerRef.SleepTime(pauseTime/4);

				CameraShakeS.C.LargeShake();


				
				// add to score
				//playerRef.score++;

			}
		}

	}

	/*
	void OnTriggerEnter(Collider other){


		if (other.gameObject.tag == "Player") {
			print (other.name); 

			PlayerS otherPlayer = other.gameObject.GetComponent<PlayerS> ();
			
			if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0) {
				// only deal damage if higher priority or other player isnt attacking
				if ((!otherPlayer.attacking) || (otherPlayer.attacking && otherPlayer.attackPriority < playerRef.attackPriority)) {
					otherPlayer.TakeDamage (100f);
					otherPlayer.SleepTime (pauseTime);
					playerRef.SleepTime (pauseTime);

					MakeExplosion(otherPlayer.gameObject, playerRef.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 

					CameraShakeS.C.LargeShake ();
					
					// add to score
					//playerRef.score++;
				} else {
					// apply knockback to both players and end attacks if priority is same
					if (otherPlayer.attacking && otherPlayer.attackPriority == playerRef.attackPriority) {
						// apply vel to both players equal to current vel x something
						otherPlayer.GetComponent<Rigidbody> ().AddForce (otherPlayer.GetComponent<Rigidbody> ().velocity * -knockbackMult);
						playerRef.GetComponent<Rigidbody> ().AddForce (playerRef.GetComponent<Rigidbody> ().velocity * -knockbackMult);
						
						print ("Tie!");
					}
				}
			}
		}
		/*
		 if (other.gameObject.tag == "PlayerTrail"){


			//print ("yeah");

			PlayerS otherPlayer = other.GetComponent<DotColliderS>().whoCreatedMe;

			if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0){

				print (other.GetComponent<DotColliderS>().whoCreatedMe); 

				//print (playerRef);
			
				otherPlayer.TakeDamage(10f);
				otherPlayer.SleepTime(pauseTime);
				playerRef.SleepTime(pauseTime/4);

				CameraShakeS.C.LargeShake();

				
				// add to score
				//playerRef.score++;

			}
		}

	}
	*/

	void MakeExplosion(GameObject object1, GameObject object2, Vector3 exploPos)
	{

		object1.GetComponent<Rigidbody> ().AddExplosionForce (2500f, exploPos, 5f); 
		object2.GetComponent<Rigidbody> ().AddExplosionForce (2500f, exploPos, 5f); 

		//object1.GetComponent<Rigidbody> ().AddForce (object1.GetComponent<Rigidbody> ().velocity * -knockbackMult);
		//object2.GetComponent<Rigidbody> ().AddForce (object2.GetComponent<Rigidbody> ().velocity * -knockbackMult);



	}
}
