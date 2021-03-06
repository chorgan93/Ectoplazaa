﻿using UnityEngine;
using System.Collections;

public class DamageS : MonoBehaviour {

	// for damage collider attached to all players
	// turns on/off according to attacking state of player
	// deals damage to other players when hits their collider or tail collider
	// also spawns hit effects

	private float pauseTime = 0.8f;
	public PlayerS playerRef;

	public GameObject damageEffectObj;
	public GameObject damageEffectObjNoFlash;
	private float damageEffectStartRange = 100f;
	public GameObject hitEffectObj;
	public GameObject hitEffectFastObj;

	private float collSizeMult = 1.5f;
	private Vector3 startSize;

	private int startPhysicsLayer;

	private Vector3 groundPoundSize;

	public bool specialAttackDmg = false;

	private Collider ownColl;

	// for pink
	private bool isPinkSpecialCollider = false;
	public GameObject pinkExplosionPrefab;

	// for acid
	private LineRenderer myLaserRender;
	public GameObject laserRenderBegin;
	public GameObject laserRenderEnd;
	public GameObject laserDrool;
	private float laserAnimRate = 0.08f;
	private float laserAnimCountdown;
	private int currentLaserFrame = 0;


	public Sprite[] laserEndSprites;
	public Sprite[] laserBeginSprites;
	public Texture[] laserMidTextures;


	// for megachomp
	private MegaChompHandlerS megaRef;

	void Start () {
		if (transform.parent){
			playerRef = transform.parent.GetComponent<PlayerS>();
		}
		else{
			specialAttackDmg = true;
		}

		startPhysicsLayer = gameObject.layer;

		startSize = transform.localScale;

		ownColl = GetComponent<Collider>();

		groundPoundSize = new Vector3(0.6f, 1.2f, 0.6f);

		laserAnimCountdown = laserAnimRate;

	}

	void FixedUpdate () {

		// this is to fix a bug with damage obj not finding its own reference
		if (!specialAttackDmg){
		if (playerRef == null){
			playerRef = transform.parent.GetComponent<PlayerS>();
		}
		else{
			// increase in size for lv3, reset otherwise
			if (playerRef.attacking && playerRef.attackToPerform == 2){
				transform.localScale = startSize*collSizeMult;
			}
			else if (playerRef.groundPounded){
				transform.localScale = groundPoundSize;
			}
			else{
				transform.localScale = startSize;
			}

			if (playerRef.isDangerous){
				ownColl.enabled = true;
			}
			else{
				ownColl.enabled = false;
			}
		}

		if (gameObject.layer != startPhysicsLayer){
			gameObject.layer = startPhysicsLayer;
		}
		}
		else{
			if (myLaserRender){
				LaserHandler();
			}
		}



	}

	//public void ManageCollision(GameObject other){
	void OnTriggerEnter (Collider other){	

		if (playerRef){
		if (!playerRef.GetSlowedState() || (playerRef.GetSlowedState() && playerRef.GetSpecialState())){
		
		if (other.gameObject.tag == "Player") {
			//print (other.name); 
			// check to make sure they have a player script, and then do stuff if its not our player
			if (other.gameObject.GetComponent<PlayerS>()){
				
				PlayerS otherPlayer = other.gameObject.GetComponent<PlayerS> ();

				if (otherPlayer != playerRef && 
						    (!CurrentModeS.isTeamMode 
						 || (CurrentModeS.isTeamMode && !GlobalVars.OnSameTeam(playerRef, otherPlayer)))){
					
					if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0) {
						// only deal damage if higher priority or other player isnt attacking

						if (!otherPlayer.attacking || 
						    (otherPlayer.attacking && otherPlayer.attackPriority < playerRef.attackPriority)
						 || specialAttackDmg){
							
							//print ("DAMAGING PLAYER " + otherPlayer.playerNum); 

							
							CameraShakeS.C.TimeSleep(0.2f);
							
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
										if (!isPinkSpecialCollider){
								otherPlayer.TakeDamage(otherPlayer.health, specialAttackDmg);
										}
								GlobalVars.totalDeaths[otherPlayer.playerNum-1] ++;
								GlobalVars.totalKills[playerRef.playerNum-1] ++; 


							}
							else
							{
								int damageTaken = (int)otherPlayer.health+1;  //Mathf.RoundToInt((otherPlayer.health/2f));
								
								otherPlayer.GetComponent<TrailHandlerRedubS>().DestroyPlayerDotsRange(damageTaken);
										if (!isPinkSpecialCollider){
											otherPlayer.TakeDamage (damageTaken, specialAttackDmg);
										}
								GlobalVars.totalDeaths[otherPlayer.playerNum-1] ++;
								GlobalVars.totalKills[playerRef.playerNum-1] ++; 
							}
							
							//print ("KILL!"); 

							if (isPinkSpecialCollider){
										playerRef.SetPinkHold(otherPlayer);
										if (pinkExplosionPrefab){
											Instantiate(pinkExplosionPrefab, transform.position, Quaternion.identity);
										}
							}

							
							MakeExplosion(otherPlayer.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 
							
							CameraShakeS.C.LargeShake ();

							// add special ko count
							if (specialAttackDmg){
								otherPlayer.numLives = 0;
							}
							else{
								
								playerRef.AddKO();
							}
							
							// spawn slash effect
							MakeSlashEffect(other.transform.position);
							
						} else {
							// apply knockback to both players and end attacks if priority is same
							// apply vel to both players equal to current vel x something
							MakeExplosion(otherPlayer.gameObject, playerRef.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 

							Vector3 spawnPos = (otherPlayer.transform.position + transform.position)/2;
						
							Instantiate(hitEffectFastObj,spawnPos,Quaternion.identity);
							//	MakeExplosion(otherPlayer.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 
							playerRef.InstantiateDeathParticles();
							CameraShakeS.C.TimeSleep(0.12f);
							print ("Tie!");

						}
					}
					
					//playerRef.DisableAttacks ();
					//otherPlayer.DisableAttacks();
				}
			}
			



		}

		if (other.gameObject.tag == "PlayerTrail"){

			// auto kill player tail is attached to


			//print ("yeah");

			PlayerS otherPlayer = other.GetComponent<DotColliderS>().whoCreatedMe;

					if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0
					   && (!CurrentModeS.isTeamMode 
					 || (CurrentModeS.isTeamMode && !GlobalVars.OnSameTeam(playerRef, otherPlayer)))){

				//print ("DAMAGING PLAYER " + otherPlayer.playerNum); 
				
				otherPlayer.SleepTime (pauseTime);
				playerRef.SleepTime (pauseTime);
				
				playerRef.numKOsInRow ++;
				
				CameraShakeS.C.TimeSleep(0.2f);

				// add special ko count
				if (!specialAttackDmg){
					playerRef.AddKO();
				}
				
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
							if (!isPinkSpecialCollider){
								otherPlayer.TakeDamage(otherPlayer.health, specialAttackDmg);
							}
					GlobalVars.totalDeaths[otherPlayer.playerNum-1] ++;
					GlobalVars.totalKills[playerRef.playerNum-1] ++; 


				}
				else
				{
					int damageTaken = (int)otherPlayer.health+1;  //Mathf.RoundToInt((otherPlayer.health/2f));
					
					otherPlayer.GetComponent<TrailHandlerRedubS>().DestroyPlayerDotsRange(damageTaken);
							if (!isPinkSpecialCollider){
								otherPlayer.TakeDamage (damageTaken, specialAttackDmg);
							}
					GlobalVars.totalDeaths[otherPlayer.playerNum-1] ++;
					GlobalVars.totalKills[playerRef.playerNum-1] ++; 
				}

						if (isPinkSpecialCollider){
							playerRef.SetPinkHold(otherPlayer);
							if (pinkExplosionPrefab){
								Instantiate(pinkExplosionPrefab, transform.position, Quaternion.identity);
							}
						}

				if (specialAttackDmg){
					otherPlayer.numLives = 0;
				}
				//print ("KILL!"); 
				
				MakeExplosion(otherPlayer.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 
				
				CameraShakeS.C.LargeShake ();
				
				
				// spawn slash effect
				MakeSlashEffect(other.transform.position);

			}

		}

		if (megaRef){
			if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground"){
				megaRef.PauseMega();
				CameraShakeS.C.TimeSleep(0.1f);
				CameraShakeS.C.SmallShake();
			}

			if (other.gameObject.GetComponent<DamageS>() == megaRef.bottomChomp ||
			    other.gameObject.GetComponent<DamageS>() == megaRef.topChomp){

				megaRef.EndMega();

			}
		}

		}

		}

	}

	public void MakeSlashEffect(Vector3 otherPos){

		// makes the cool slash thing when a player is hit

		Vector3 spawnPos = transform.position-playerRef.GetComponent<Rigidbody>().velocity.normalized*damageEffectStartRange;
		spawnPos.z-=1;

		Vector3 effectDir = playerRef.GetComponent<Rigidbody>().velocity.normalized;

		GameObject slashEffect = Instantiate(damageEffectObj,spawnPos,Quaternion.identity)
			as GameObject;

		slashEffect.GetComponent<SlashEffectS>().moveDir = effectDir;
		slashEffect.GetComponent<SlashEffectS>().attachedLightning.GetComponent<Renderer>().material.color 
			= playerRef.playerParticleMats
			[playerRef.characterNum-1].GetColor("_TintColor");

		slashEffect.GetComponent<TrailRenderer>().materials[0].color = 
			playerRef.trailRendererGO.GetComponent<TrailRenderer>().materials[0].color;

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

		// for a non-lethal hit (probably aren't many of these now due to redesigns)
		
		Vector3 spawnPos = transform.position-playerRef.GetComponent<Rigidbody>().velocity.normalized*damageEffectStartRange;
		spawnPos.z-=1;
		
		Vector3 effectDir = playerRef.GetComponent<Rigidbody>().velocity.normalized;
		
		GameObject slashEffect = Instantiate(damageEffectObjNoFlash,spawnPos,Quaternion.identity)
			as GameObject;
		
		slashEffect.GetComponent<SlashEffectS>().moveDir = effectDir;

		slashEffect.GetComponent<TrailRenderer>().materials[0].color = 
			playerRef.trailRendererGO.GetComponent<TrailRenderer>().materials[0].color;
		
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

	private void LaserHandler(){

		if (myLaserRender){

			Vector3 renderStart =playerRef.transform.position;
			Vector3 renderEnd = transform.position;
			renderEnd.z = renderStart.z;

			myLaserRender.SetPosition(0, renderStart);
			myLaserRender.SetPosition(1, renderEnd);

			Vector3 laserBeginPos = playerRef.transform.position;
			laserBeginPos.z -= 3f;
			laserRenderBegin.transform.position = laserDrool.transform.position = laserBeginPos;

			Vector3 laserEndPos = transform.position;
			laserEndPos.z -= 3f;
			laserRenderEnd.transform.position = laserEndPos;
			Vector3 laserPieceRotation = playerRef.spriteObject.transform.rotation.eulerAngles;
			laserPieceRotation.z += 90f;
			if (playerRef.spriteObject.transform.localScale.x > 0){
				laserPieceRotation.z += 180f;
			}
			laserRenderBegin.transform.rotation =
				laserRenderEnd.transform.rotation = Quaternion.Euler(laserPieceRotation);

			// animate
			laserAnimCountdown -= Time.deltaTime;
			if (laserAnimCountdown <= 0){
				laserAnimCountdown =laserAnimRate;
				currentLaserFrame++;
				if (currentLaserFrame > laserMidTextures.Length-1){
					currentLaserFrame = 0;
				}

				myLaserRender.materials[0].SetTexture("_MainTex", laserMidTextures[currentLaserFrame]);
				
				laserRenderBegin.GetComponent<SpriteRenderer>().sprite = laserBeginSprites[currentLaserFrame];
				laserRenderEnd.GetComponent<SpriteRenderer>().sprite = laserEndSprites[currentLaserFrame];
			}

		}

	}

	void MakeExplosion(GameObject object1, Vector3 exploPos)
	{
		
		object1.GetComponent<Rigidbody> ().AddExplosionForce (5000f, exploPos, 5f); 
		
		
		
	}

	void MakeExplosion(GameObject object1, GameObject object2, Vector3 exploPos)
	{

		object1.GetComponent<Rigidbody> ().AddExplosionForce (5000f, exploPos, 5f); 
		object2.GetComponent<Rigidbody> ().AddExplosionForce (5000f, exploPos, 5f); 



	}

	public void MakeSpecial(PlayerS newRef){
		specialAttackDmg = true;
		playerRef = newRef;

		if (playerRef.characterNum == 2){

			// activate laser visual
			myLaserRender = GetComponent<LineRenderer>();
			myLaserRender.enabled = true;

		}

		if (!ownColl){
			ownColl = GetComponent<Collider>();
		}
		ownColl.enabled = true;
	}

	public void MakeSpecial(PlayerS newRef, MegaChompHandlerS mega){

		specialAttackDmg = true;
		playerRef = newRef;
		
		if (playerRef.characterNum == 2){
			
			// activate laser visual
			myLaserRender = GetComponent<LineRenderer>();
			myLaserRender.enabled = true;


		}
		
		if (!ownColl){
			ownColl = GetComponent<Collider>();
		}
		ownColl.enabled = true;

		megaRef = mega;


	}

	public void EnablePinkSpecial(){
		isPinkSpecialCollider = true;
	}
}
