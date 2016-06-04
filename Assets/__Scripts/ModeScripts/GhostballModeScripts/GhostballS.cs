using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostballS : MonoBehaviour {

	public ParticleSystem respawnParticles;
	private ParticleSystem currentRespawnParticles;
	private SpriteRenderer myRenderer;
	private Sprite defaultSprite;

	private int playerOwner;
	private PlayerS _playerRef;
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

	public GameObject[] hitSounds;

	public Sprite[] blinkSprites;
	public float blinkFrameRate = 0.08f;
	private float blinkFrameCountdown;
	private int currentBlinkFrame = 0;
	private bool blinking = false;
	public float blinkTimeMin = 2f;
	public float blinkTimeMax = 8f;
	private float blinkCountdown;

	
	public Sprite[] hitSprites;
	public float hitFrameRate = 0.08f;
	private float hitFrameCountdown;
	private int currentHitFrame = 0;
	private bool gettingHit = false;

	public GameObject hitWhiteFlash;
	private float hitFlashTimeMax = 0.08f;
	private float hitFlashCountdown;

	// Use this for initialization
	void Start () {

		myRenderer = GetComponent<SpriteRenderer>();
		defaultSprite = myRenderer.sprite;
		startColor = myRenderer.color;

		blinkCountdown = Random.Range(blinkTimeMin, blinkTimeMax);
	
	}
	
	// Update is called once per frame
	void Update () {

		if (currentRespawnParticles){
			if (!currentRespawnParticles.isPlaying){
				hitWhiteFlash.SetActive(false);
				Destroy(currentRespawnParticles);
			}
		}
		else{
			HandleBlinking();
		}
	
	}

	public void PlayHitSound(){

		int hitSoundToPlay = Mathf.RoundToInt(Random.Range(0, hitSounds.Length-1));

		Instantiate(hitSounds[hitSoundToPlay]);

	}

	private void PlayHitAnimation(){

		blinking = false;
		currentBlinkFrame = 0;
		hitFlashCountdown = hitFlashTimeMax;
		hitWhiteFlash.transform.localPosition = new Vector3(0,0,-1f);
		hitWhiteFlash.SetActive(true);

		gettingHit = true;
		currentHitFrame = 0;
		myRenderer.sprite = hitSprites[currentHitFrame];
		hitFrameCountdown= hitFrameRate+hitFlashTimeMax;

	}

	private void HandleBlinking(){

		if (gettingHit){
			hitFrameCountdown -= Time.deltaTime;
			if (hitFrameCountdown <= 0){
				currentHitFrame++;
				if (currentHitFrame >= hitSprites.Length){
					gettingHit = false;
					blinkCountdown = Random.Range(blinkTimeMin, blinkTimeMax);
					myRenderer.sprite = defaultSprite;
				}
				else{
					myRenderer.sprite = hitSprites[currentHitFrame];
					hitFrameCountdown = hitFrameRate;
				}
			}

			if (hitWhiteFlash.activeSelf){
				hitFlashCountdown -= Time.deltaTime;
				if (hitFlashCountdown <=0){
					hitWhiteFlash.SetActive(false);
				}
			}
		}
		else if (blinking){
			blinkFrameCountdown -= Time.deltaTime;
			if (blinkFrameCountdown <= 0){
				currentBlinkFrame++;
				if (currentBlinkFrame >= blinkSprites.Length){
					blinking = false;
					blinkCountdown = Random.Range(blinkTimeMin, blinkTimeMax);
					myRenderer.sprite = defaultSprite;
				}
				else{
					myRenderer.sprite = blinkSprites[currentBlinkFrame];
					blinkFrameCountdown = blinkFrameRate;
				}
			}

		}
		else{
			blinkCountdown -= Time.deltaTime;
			if (blinkCountdown <= 0){
				blinking = true;
				currentBlinkFrame = 0;
				blinkFrameCountdown = blinkFrameRate;
				myRenderer.sprite = blinkSprites[currentBlinkFrame];
			}
		}

	}

	void OnCollisionEnter(Collision other){

		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();

			GetComponent<Rigidbody>().velocity = Vector3.zero;

			PlayHitSound();
			PlayHitAnimation();
			
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
				_playerRef = playerRef;
				playerOwner = playerRef.playerNum;
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
					_playerRef = playerRef;
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

		_playerRef = null;
		playerOwner = -1;
		teamOwner = -1;

	}

	public int GetCurrentPlayer(){

		return playerOwner;

	}

	public PlayerS GetCurrentPlayerRef(){
		
		return _playerRef;
		
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
		
		GameObject slashEffect = Instantiate(damageEffectObjNoScreen,spawnPos,Quaternion.identity)
			as GameObject;
		
		slashEffect.GetComponent<SlashEffectS>().moveDir = effectDir;

		
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
