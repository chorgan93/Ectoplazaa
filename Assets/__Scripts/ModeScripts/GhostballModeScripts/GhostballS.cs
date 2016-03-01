using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostballS : MonoBehaviour {

	public ParticleSystem respawnParticles;
	private ParticleSystem currentRespawnParticles;
	private SpriteRenderer myRenderer;

	private int playerOwner;
	private int teamOwner;
	private Color startColor;

	private Color redTeamColor = Color.red;
	private Color blueTeamColor = Color.blue;

	
	public GameObject damageEffectObj;
	public GameObject damageEffectObjNoScreen;
	private float damageEffectStartRange = 100f;
	public GameObject hitEffectFastObj;

	private float attackForce = 7500f;
	private float lv1Mult = 1.75f;
	private float lv2Mult = 3f;
	private float lv3Mult = 4.5f;

	// Use this for initialization
	void Start () {

		myRenderer = GetComponent<SpriteRenderer>();
		startColor = myRenderer.color;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (currentRespawnParticles){
			if (!currentRespawnParticles.isPlaying){
				Destroy(currentRespawnParticles);
			}
		}
	
	}

	void OnCollisionEnter(Collision other){

		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();

			GetComponent<Rigidbody>().velocity = Vector3.zero;
			
			float attackAdd = attackForce*Time.deltaTime;
			// add extra force if attacking
			if (playerRef.attacking){
			if (playerRef.GetChompState() || playerRef.attackToPerform == 0){
				//Debug.Log("LV1 HIT " + attackAdd);
				attackAdd*=lv1Mult;
				//Debug.Log("LV1 HITAFTER " + attackAdd);
				}
				else if (playerRef.attackToPerform == 1){
				//Debug.Log("LV2 HIT " + attackAdd);
				attackAdd*=lv2Mult;
				//Debug.Log("LV2 HITAFTER " + attackAdd);
				}
			else if (playerRef.attackToPerform == 2){
				//Debug.Log("LV3 HIT " + attackAdd);
				attackAdd*=lv3Mult;
				//Debug.Log("LV3 HITAFTER " + attackAdd);
				}
				else{}
			}

				Vector3 addAttack = (transform.position-other.contacts[0].point).normalized;
				addAttack*=attackAdd;
				GetComponent<Rigidbody>().AddForce(addAttack, ForceMode.Impulse);
				//Debug.Log(addAttack);


			// team mode
			if (CurrentModeS.isTeamMode){
				if (teamOwner != GlobalVars.teamNumber[playerRef.playerNum-1]){
					teamOwner = GlobalVars.teamNumber[playerRef.playerNum-1];
					
					Color newCol = myRenderer.color;

					if (teamOwner == 1){
						newCol = redTeamColor;
					}
					if (teamOwner == 2){
						newCol = blueTeamColor; 

					}
					newCol.a = 1;
					myRenderer.color = newCol;
					
					MakeSlashEffectNoScreen(other.gameObject.transform.position);
					CameraShakeS.C.PunchIn();
					CameraShakeS.C.MicroShake();
					CameraShakeS.C.TimeSleep(0.08f);
				}
				else{
					CameraShakeS.C.MicroShake();
				}
			}
			// not team mode
			else{
			if (playerOwner != playerRef.playerNum){
				playerOwner = playerRef.playerNum;
				Color newCol = playerRef.GetComponentInChildren<TrailRenderer>().materials[0].color;
				newCol.a = 1;
				myRenderer.color = newCol;
	
					MakeSlashEffectNoScreen(other.gameObject.transform.position);
					CameraShakeS.C.PunchIn();
					CameraShakeS.C.MicroShake();
					CameraShakeS.C.TimeSleep(0.08f);
			}
			else{
				CameraShakeS.C.MicroShake();
			}
			}
		}

	}

	public void ResetBall(Vector3 respawnPos){

		currentRespawnParticles = Instantiate(respawnParticles, transform.position, Quaternion.identity)
			as ParticleSystem;
		myRenderer.color = startColor;
		transform.position = respawnPos;
		GetComponent<Rigidbody>().velocity = Vector3.zero;

		playerOwner = -1;
		teamOwner = -1;

	}

	public int GetCurrentPlayer(){

		return playerOwner;

	}

	public int GetCurrentTeam(){
		
		return teamOwner;
		
	}


	public void MakeSlashEffect(Vector3 otherPos){
		
		// makes the cool slash thing when a player is hit
		
	
		
		Vector3 effectDir = (otherPos-transform.position).normalized;

		Vector3 spawnPos = transform.position-effectDir*damageEffectStartRange;
		spawnPos.z-=1;
		
		GameObject slashEffect = Instantiate(damageEffectObj,spawnPos,Quaternion.identity)
			as GameObject;
		
		slashEffect.GetComponent<SlashEffectS>().moveDir = effectDir;
		slashEffect.GetComponent<SlashEffectS>().attachedLightning.GetComponent<Renderer>().material.color 
			= myRenderer.color;

		slashEffect.GetComponent<TrailRenderer>().materials[0].color = 
			myRenderer.color;
		
		spawnPos = (transform.position+otherPos)/2;
		spawnPos.z = transform.position.z +1;
		
		GameObject hitEffect = Instantiate(hitEffectFastObj,spawnPos,Quaternion.identity) as GameObject;
		hitEffect.GetComponent<SpriteRenderer>().color = myRenderer.color;
		
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

		CameraShakeS.C.LargeShake();
		CameraShakeS.C.TimeSleep(0.2f);
		
	}

	public void MakeSlashEffectNoScreen(Vector3 otherPos){
		
		// makes the cool slash thing when a player is hit
		
		
		
		Vector3 effectDir = (transform.position-otherPos).normalized;
		
		Vector3 spawnPos = transform.position+effectDir*damageEffectStartRange;
		spawnPos.z-=1;
		
		GameObject slashEffect = Instantiate(damageEffectObj,spawnPos,Quaternion.identity)
			as GameObject;
		
		slashEffect.GetComponent<SlashEffectS>().moveDir = effectDir;

		Color halfCol = myRenderer.color;
		halfCol.a = 0.5f;
		slashEffect.GetComponent<SlashEffectS>().attachedLightning.GetComponent<Renderer>().material.color 
			= halfCol;
		
		slashEffect.GetComponent<TrailRenderer>().materials[0].color = 
			myRenderer.color;
		
		spawnPos = (transform.position+otherPos)/2;
		spawnPos.z = transform.position.z +1;
		
		GameObject hitEffect = Instantiate(hitEffectFastObj,spawnPos,Quaternion.identity) as GameObject;
		hitEffect.GetComponent<SpriteRenderer>().color = myRenderer.color;
		
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

		CameraShakeS.C.SmallShake();
		CameraShakeS.C.TimeSleep(0.2f);
		
	}
}
