﻿using UnityEngine;
using System.Collections;

public class DamageS : MonoBehaviour {

	private float pauseTime = 0.8f;
	public PlayerS playerRef;

	public GameObject damageEffectObj;
	public GameObject damageEffectObjNoFlash;
	private float damageEffectStartRange = 100f;
	public GameObject hitEffectObj;

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

			//print("HIT PLAYER " +  otherPlayer.playerNum); 

			if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0) {
				// only deal damage if higher priority or other player isnt attacking
				//if ((!otherPlayer.attacking) || (otherPlayer.attacking && otherPlayer.attackPriority < playerRef.attackPriority)) {
				if (!otherPlayer.attacking){
				
					//print ("DAMAGING PLAYER " + otherPlayer.playerNum); 

					otherPlayer.SleepTime (pauseTime);
					playerRef.SleepTime (pauseTime);

					//chop all of tail off
					// make sure there are dots to destroy first
					//otherPlayer.TakeDamage (otherPlayer.health);
					/*
					if (Mathf.RoundToInt((otherPlayer.health/2)) < 5){
						otherPlayer.initialHealth = 5;
					}
					else{
						otherPlayer.initialHealth = Mathf.RoundToInt((otherPlayer.health/2));
					}
					*/

					if(otherPlayer.health < 5)
					{
						otherPlayer.GetComponent<TrailHandlerRedubS>().SpawnGlobs(otherPlayer.transform.position,2); 
						otherPlayer.TakeDamage(otherPlayer.health);
					}
					else
					{
						int damageTaken = (int)otherPlayer.health+1;  //Mathf.RoundToInt((otherPlayer.health/2f));

						otherPlayer.GetComponent<TrailHandlerRedubS>().DestroyPlayerDotsRange(damageTaken);
						otherPlayer.TakeDamage (damageTaken);
					}

					MakeExplosion(otherPlayer.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 

					CameraShakeS.C.LargeShake ();

					
					// spawn slash effect
					MakeSlashEffect(other.transform.position);
					
				} else {
					// apply knockback to both players and end attacks if priority is same
					if (otherPlayer.attacking && otherPlayer.attackPriority == playerRef.attackPriority) {
						// apply vel to both players equal to current vel x something
						MakeExplosion(otherPlayer.gameObject, playerRef.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 

						print ("Tie!");
					}
				}
			}

			playerRef.DisableAttacks ();
			otherPlayer.DisableAttacks();

		}

		if (other.gameObject.tag == "PlayerTrail"){


			//print ("yeah");

			PlayerS otherPlayer = other.GetComponent<DotColliderS>().whoCreatedMe;

			if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0){

				otherPlayer.GetComponent<TrailHandlerRedubS>().ChopTail(other.gameObject);
				if(playerRef.playerNum ==1)
				{
					//print("P1 TAIL HIT" );
				}

				//print (other.GetComponent<DotColliderS>().whoCreatedMe); 

				//print (playerRef);
			
				otherPlayer.SleepTime(pauseTime);
				playerRef.SleepTime(pauseTime/4);

				CameraShakeS.C.LargeShake();


				
				// add to score
				//playerRef.score++;

				// spawn tail slash effect
				MakeSlashEffectNoFlash(other.transform.position);

			}

		}



	}

	void MakeSlashEffect(Vector3 otherPos){

		Vector3 spawnPos = transform.position-playerRef.GetComponent<Rigidbody>().velocity.normalized*damageEffectStartRange;
		spawnPos.z-=1;

		Vector3 effectDir = playerRef.GetComponent<Rigidbody>().velocity.normalized;

		GameObject slashEffect = Instantiate(damageEffectObj,spawnPos,Quaternion.identity)
			as GameObject;

		slashEffect.GetComponent<SlashEffectS>().moveDir = effectDir;

		spawnPos = (transform.position+otherPos)/2;
		spawnPos.z = transform.position.z +1;

		GameObject hitEffect = Instantiate(hitEffectObj,spawnPos,Quaternion.identity) as GameObject;

		// rotate hit effect to match slash
		float rotateZ = 0;
		
		if(effectDir.x == 0){
			rotateZ = 90;
		}
		else{
			rotateZ = Mathf.Rad2Deg*Mathf.Atan((effectDir.y/effectDir.x));
		}	
		
		//print (rotateZ);
		
		if (effectDir.x < 0){
			rotateZ += 180;
		}
		
		hitEffect.transform.Rotate(new Vector3(0,0,rotateZ+90));

	}

	void MakeSlashEffectNoFlash(Vector3 otherPos){
		
		Vector3 spawnPos = transform.position-playerRef.GetComponent<Rigidbody>().velocity.normalized*damageEffectStartRange;
		spawnPos.z-=1;
		
		Vector3 effectDir = playerRef.GetComponent<Rigidbody>().velocity.normalized;
		
		GameObject slashEffect = Instantiate(damageEffectObjNoFlash,spawnPos,Quaternion.identity)
			as GameObject;
		
		slashEffect.GetComponent<SlashEffectS>().moveDir = effectDir;
		
		spawnPos = (transform.position+otherPos)/2;
		spawnPos.z = transform.position.z +1;
		
		GameObject hitEffect = Instantiate(hitEffectObj,spawnPos,Quaternion.identity) as GameObject;
		
		// rotate hit effect to match slash
		float rotateZ = 0;
		
		if(effectDir.x == 0){
			rotateZ = 90;
		}
		else{
			rotateZ = Mathf.Rad2Deg*Mathf.Atan((effectDir.y/effectDir.x));
		}	
		
		//print (rotateZ);
		
		if (effectDir.x < 0){
			rotateZ += 180;
		}
		
		hitEffect.transform.Rotate(new Vector3(0,0,rotateZ+90));
		
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

	void MakeExplosion(GameObject object1, Vector3 exploPos)
	{
		
		object1.GetComponent<Rigidbody> ().AddExplosionForce (5000f, exploPos, 5f); 

		//object1.GetComponent<Rigidbody> ().AddForce (object1.GetComponent<Rigidbody> ().velocity * -knockbackMult);
		//object2.GetComponent<Rigidbody> ().AddForce (object2.GetComponent<Rigidbody> ().velocity * -knockbackMult);
		
		
		
	}

	void MakeExplosion(GameObject object1, GameObject object2, Vector3 exploPos)
	{

		object1.GetComponent<Rigidbody> ().AddExplosionForce (5000f, exploPos, 5f); 
		object2.GetComponent<Rigidbody> ().AddExplosionForce (5000f, exploPos, 5f); 

		//object1.GetComponent<Rigidbody> ().AddForce (object1.GetComponent<Rigidbody> ().velocity * -knockbackMult);
		//object2.GetComponent<Rigidbody> ().AddForce (object2.GetComponent<Rigidbody> ().velocity * -knockbackMult);



	}
}
