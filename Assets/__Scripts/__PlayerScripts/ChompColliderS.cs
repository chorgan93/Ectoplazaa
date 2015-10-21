using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChompColliderS : MonoBehaviour {

	public float inactiveTime; // time before collider turns on
	public float lifeTime; // time before destruction

	private Vector3 attackDir;

	//private Rigidbody ownRigid;
	private Collider ownCollider;

	public SpriteRenderer ownRender;
	public List<Sprite> animFrames;
	private int currentSprite;
	private float animRateMax;
	private float animCountdown;


	private float pauseTime = 0.03f;

	public float attackSpeed; // force to apply at set direction

	public GameObject damageEffectObj;
	public GameObject damageEffectObjNoFlash;
	private float damageEffectStartRange = 100f;
	public GameObject hitEffectObj;
	public GameObject hitEffectFastObj;

	private float chompRad = 3f;
	
	private float knockbackMult = 1.5f;

	private PlayerS playerRef; // the player that created me

	public GameObject sfxChargeUpObj;
	
	public GameObject sfxAttackObj;

	void Start () {

		//ownRender = GetComponent<SpriteRenderer>();
		ownCollider = GetComponent<Collider>();
		ownCollider.enabled = false;


		animCountdown = animRateMax = lifeTime/animFrames.Count;
		ownRender.sprite = animFrames[currentSprite];

		
		Instantiate(sfxChargeUpObj,transform.position,Quaternion.identity);


	}


	// Update is called once per frame
	void FixedUpdate () {
	
		if (!TimeManagerS.paused){

			// destroy if player not active or dodging
			if (!playerRef || playerRef.respawning || playerRef.numLives == 0 || playerRef.dodgeTimeCountdown > 0){
				Destroy(gameObject);
			}

			animCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			if (animCountdown <= 0){
				currentSprite++;
				if (currentSprite > animFrames.Count-1){
					currentSprite = animFrames.Count-1;
				}
				animCountdown = animRateMax;

				ownRender.sprite = animFrames[currentSprite];
			}

			inactiveTime -= Time.deltaTime*TimeManagerS.timeMult;
			if (inactiveTime <= 0){
				if (!ownCollider.enabled){
					ownCollider.enabled = true;
					Instantiate(sfxAttackObj,transform.position,Quaternion.identity);
					CameraShakeS.C.MicroShake();
				}
			}

			lifeTime -= Time.deltaTime*TimeManagerS.timeMult;
			if (lifeTime <= 0){
				Destroy(gameObject);
			}
		}

	}

	public void SetDirection(Vector3 newDir, PlayerS newPlayer){

		//ownRigid = GetComponent<Rigidbody>();

		playerRef = newPlayer;

		attackDir = newDir;

		float rotateZ = 0;
		
		if(attackDir.x == 0){
			if (attackDir.y > 0){
				rotateZ = 90;
			}
			else{
				rotateZ = -90;
			}
		}
		else{
			rotateZ = Mathf.Rad2Deg*Mathf.Atan((attackDir.y/attackDir.x));
		}	
		
		
		if (attackDir.x < 0){
			rotateZ += 180;
			if (transform.rotation.z > 270 || transform.rotation.z < 90){
				Vector3 flipSize = transform.localScale;
				flipSize.y *= -1;
				transform.localScale = flipSize;
			}
		}
		
		transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateZ));



		//ownRigid.AddForce(newDir*attackSpeed*Time.deltaTime, ForceMode.Impulse);

		// parent to player
		transform.parent = playerRef.transform;
		transform.localPosition = attackDir.normalized*chompRad;
	}

	void OnTriggerEnter(Collider other){
		// this code is taken from the damageS code
		if (other.gameObject.tag == "Player" ) {
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
						otherPlayer.TakeDamage(otherPlayer.health);
						GlobalVars.totalDeaths[otherPlayer.playerNum-1] ++;
						GlobalVars.totalKills[playerRef.playerNum-1] ++; 
					}
					else
					{
						int damageTaken = (int)otherPlayer.health+1;  //Mathf.RoundToInt((otherPlayer.health/2f));
						
						otherPlayer.GetComponent<TrailHandlerRedubS>().DestroyPlayerDotsRange(damageTaken);
						otherPlayer.TakeDamage (damageTaken);
						GlobalVars.totalDeaths[otherPlayer.playerNum-1] ++;
						GlobalVars.totalKills[playerRef.playerNum-1] ++; 
					}
					
					//print ("KILL!"); 
					
					MakeExplosion(otherPlayer.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 
					
					CameraShakeS.C.LargeShake ();
					
					
					// spawn slash effect
					MakeSlashEffect(other.transform.position);
					
				} else {
					// apply knockback to both players and end attacks if priority is same
					if (otherPlayer.attacking && otherPlayer.attackPriority == playerRef.attackPriority) {
						// apply vel to both players equal to current vel x something
						MakeExplosion(otherPlayer.gameObject, playerRef.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 
						
						Vector3 spawnPos = (otherPlayer.transform.position + transform.position)/2;
						
						GameObject hitEffect = Instantiate(hitEffectFastObj,spawnPos,Quaternion.identity) as GameObject;
						//	MakeExplosion(otherPlayer.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 
						playerRef.InstantiateDeathParticles();
						CameraShakeS.C.TimeSleep(0.12f);
						//print ("Tie!");
					}
				}
			}
			
			playerRef.DisableAttacks ();
			otherPlayer.DisableAttacks();
			
		}
		
		if (other.gameObject.tag == "PlayerTrail"){
			
			// auto kill player tail is attached to
			
			
			//print ("yeah");
			
			PlayerS otherPlayer = other.GetComponent<DotColliderS>().whoCreatedMe;
			
			if (otherPlayer != playerRef && otherPlayer.health > 0 && otherPlayer.respawnInvulnTime <= 0){
				
				//print ("DAMAGING PLAYER " + otherPlayer.playerNum); 
				
				otherPlayer.SleepTime (pauseTime);
				playerRef.SleepTime (pauseTime);
				
				
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
					otherPlayer.TakeDamage(otherPlayer.health);
					GlobalVars.totalDeaths[otherPlayer.playerNum-1] ++;
					GlobalVars.totalKills[playerRef.playerNum-1] ++; 
				}
				else
				{
					int damageTaken = (int)otherPlayer.health+1;  //Mathf.RoundToInt((otherPlayer.health/2f));
					
					otherPlayer.GetComponent<TrailHandlerRedubS>().DestroyPlayerDotsRange(damageTaken);
					otherPlayer.TakeDamage (damageTaken);
					GlobalVars.totalDeaths[otherPlayer.playerNum-1] ++;
					GlobalVars.totalKills[playerRef.playerNum-1] ++; 
				}
				
				//print ("KILL!"); 
				
				MakeExplosion(otherPlayer.gameObject, Vector3.Lerp(otherPlayer.transform.position,playerRef.transform.position, 0.5f)); 
				
				CameraShakeS.C.LargeShake ();
				
				
				// spawn slash effect
				MakeSlashEffect(other.transform.position);
				
			}
			
		}
	}

	// actually just copied the rest of damageS here 
	void MakeSlashEffect(Vector3 otherPos){
		
		// makes the cool slash thing when a player is hit
		
		Vector3 spawnPos = transform.position-playerRef.GetComponent<Rigidbody>().velocity.normalized*damageEffectStartRange;
		spawnPos.z-=1;
		
		Vector3 effectDir = playerRef.GetComponent<Rigidbody>().velocity.normalized;
		
		GameObject slashEffect = Instantiate(damageEffectObj,spawnPos,Quaternion.identity)
			as GameObject;
		
		slashEffect.GetComponent<SlashEffectS>().moveDir = effectDir;
		slashEffect.GetComponent<SlashEffectS>().attachedLightning.GetComponent<Renderer>().material.color = playerRef.playerParticleMats
			[playerRef.characterNum-1].GetColor("_TintColor");
		
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
