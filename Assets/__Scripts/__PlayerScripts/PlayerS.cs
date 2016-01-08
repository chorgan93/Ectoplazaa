using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerS : MonoBehaviour {

	//___________________________________CLASS PROPERTIES

	//___________________________________INSTANCE PROPERTIES

	//___________________________________GETTERS AND SETTERS
	
	private string platformType;
	private Rigidbody ownRigid;


	
	public GameObject spriteObject;
	private SpriteRenderer spriteObjRender;
	
	//public ButtObjS buttObj;
	//private Rigidbody buttObjRigid;
	
	public int playerNum; //controller, playernumber for UI 
	public int characterNum; //skin, character chosen;
	
	public int score;

	public bool attackTriggerDown;
	
	public float walkSpeed;
	public float maxSpeed;
	public float airControlMult;
	private bool canAirStrafe = true; 
	
	//private bool isBouncy = false; 
	public PhysicMaterial bouncyPhysics, normalPhysics; 
	
	public float jumpSpeed;
	private bool jumped = false;
	[HideInInspector]
	public bool hasDoubleJumped = false;
	private bool jumpButtonDown = false;
	public float addJumpForce;
	public float addJumpForceMaxTime;
	private float addingJumpTime;
	private bool stopAddingJump;
	
	public GroundDetectS groundDetect;

	private float triggerSensitivity = 0.5f;

	public bool isDangerous = false;
	public float maxHealth = 50;
	public float initialHealth = 10;
	[HideInInspector]
	public float startEctoNum;
	public float health = 10;
	public float ectoScore;
	
	public bool facingRight = false;
	
	[HideInInspector]
	public bool groundPounded = false;
	public float groundPoundForce;
	private float groundPoundPauseCountdown;
	private float groundPoundPauseMax = 0.3f;
	private bool chargingGroundPound = false;
	
	//private float pauseDelay;
	private bool resetFromPause = false;
	private bool prevGravState;
	private Vector3 prevVel;
	private Vector3 prevButtVel;

	// following is for pausing during special attacks/pause menu
	[HideInInspector]
	public bool effectPause = false;
	private bool capturedDanger;
	private bool capturedGrav;
	private Vector3 capturedVel;

	
	
	public GameObject dangerObj;
	public bool canRespawn = true;
	public bool respawning = false;
	private float respawnTimeMax = 2f;
	private float respawnTimeCountdown;
	private bool isDead = false;
	
	[HideInInspector]//gets value from mode.
	public int numLives = -1;
	
	private Vector3 spawnPos;
	public GameObject spawnPt;
	private GameObject [] allSpawnPts;
	
	// raycasting to prevent wall sticking
	private bool rightCheck;
	private bool leftCheck;
	private RaycastHit hit;
	
	// new (as of 2 July) attack stuff
	private bool canCharge = true;
	[HideInInspector]
	public bool charging = false;
	private float chargeTime = 0;
	private float medChargeTime = 0.65f;
	private float maxChargeTime = 1.3f;
	
	[HideInInspector]
	public int attackToPerform = 0;
	//[HideInInspector]
	public bool attacking = false;
	
	[HideInInspector]
	public int attackPriority = 0;
	
	//private float buttDelayCountdown;
	
	[HideInInspector]
	public Vector3 attackDir;
	
	private float attackGroundLeewayMaxTime = 0.5f;
	private float groundLeeway;
	
	[HideInInspector]
	public bool performedAttack = false;

	
	private float lv1Force = 4500f; // locked fling speed (NEW)
	
	private float lv2OutRate = 16000f; //original 8000
	private float lv1OutTimeMax = 0.125f;
	public float lv1OutCountdown;

	public float lv2FlingForce = 100;
	private float lv3BulletSpeed = 15500; //was 12500
	private Vector3 bulletVel;
	
	private float lv2AttackPauseTimeMax = 0.2f;
	private float lv2AttackPauseCountdown;
	private bool startedLv2Pause = false;
	private float lv2AttackTimeCountdown;
	
	// physics layer experimentation for bullet fling
	private int physicsLayerDefault;
	private string physicsLayerNoWalls = "IgnoreWallCollider";
	private RaycastHit newHit;
	
	//[HideInInspector]
	public bool didLv2Fling = false;
	
	//VISUAL VARS------------------------------------------------
	public GameObject deathParticles, jumpParticles, hitParticles, groundedParticles; 
	public GameObject dangerousSprite; 
	public GameObject trailRendererGO;
	public GameObject trailRendererGO2;  
	
	public Material [] playerMats;
	public Material [] playerParticleMats; 
	public Material [] playerHurtMats; 
	public Material	 hurtMat; 
	Color hurtTint = new Color(255f/255f,130f/255f,130f/255f); 
	
	// sound stuff
	[HideInInspector]
	public PlayerSoundS soundSource;
	private bool playedLV1ChargeSound = false;
	private bool playedLV2ChargeSound = false;
	private bool playedLV3ChargeSound = false;
	
	// wall jump detects
	public WallDetectS leftWallDetect;
	public WallDetectS rightWallDetect;
	public float 		wallJumpXForce;
	public float 		wallDragForce;
	private bool 		clingingToWall = false;
	
	int hurtCounter, hurtTimer = 12; 
	
	public float respawnInvulnTime = 1.5f;
	
	private FlashObjS lv2Flash;
	private FlashObjS lv3Flash;
	
	public GameObject respawnParticlePrefab, spawnParticlePrefab;
	GameObject respawnParticles; 
	Vector3 deathPos; 
	
	public GameObject chargingParticlePrefab;
	public GameObject chargingSpecialPrefab;
	GameObject chargingParticles;
	GameObject specialParticles;
	
	public GameObject initialSpawnParticlesPrefab; 
	GameObject initialSpawnParticles; 
	bool isSpawning; 
	
	private AudioSource chargeSource;
	private float currentChargeVolume;
	private float maxChargeVolume = 1;
	private float chargeFadeRate = 2f;
	
	// dodge variables
	private bool dodgeButtonDown = false;
	[HideInInspector]
	public bool dodging = false; // true when dodging
	private bool canDodge = true; // allow infinite dodges on ground, one in air
	public float dodgeTimeMax; // length of time dodge is active
	public float dodgeInvulnTimeMax; // time to be invuln while dodging

	[HideInInspector]
	public float dodgeTimeCountdown;
	public float dodgeForce; // force to apply to character at start of dodge
	
	// NEW Lv0 "Chomp" attack vars
	private float lv0MaxChargeTime = 0.3f; // if charge time is under this time, trigger lv0
	private bool doingChomp = false;
	public float chompMaxTime;
	private float chompTimeCountdown;
	public GameObject chompHitObj; // obj to spawn on chomp
	public float chompForce; // force to add to vel on chomp attack
	private float startDrag; // reg drag
	private float chompDrag = 14; // drag for chomp attack
	private float dodgeDrag = 16;
	//private float chompRad = 10f;

	private bool inCharSelect = false; // when in char select, never take damage
	
	//[HideInInspector]
	public int colorNum; // for alternate colors
	
	//[HideInInspector]
	public bool nonActive = false; // total disable of character, just show -- for character select screen

	//special attack stuff
	[Header ("Special Attacks")]
	public int numKOsInRow = 0;
	private bool doingSpecial = false;
	private float specialCooldown;
	private float specialCooldownMax = 1f;

	// ghostMask Special
	public GameObject ghostMaskSpecialPrefab;

	// pinkwhip special
	public GameObject pinkWhipSpecialPrefab;
	private Vector3 pinkWhipSpecialVel;
	private float pinkWhipSpecialSpeed = 12500f;

	// mr wraps special
	public GameObject mrWrapsSpecialProjectile;
	private float wrapsSpecialProjSpeed = 5000f;
	private float timeBetweenProjsMax = 0.6f;
	private float timeBettwenProjCountdown = 0;
	private int numProjsMax = 3;
	private int currentProj;
	private float currentLerpTarget;
	private float rotateAmt = 30f;

	// acid special
	public GameObject acidSpecialCollider;
	private GameObject acidSpecialReference;
	private float acidSpecialTimeMax = 1.25f;
	private float acidSpecialStartRotateRate = 1f;
	private float acidSpecialRotateAccel = 200f;
	private float acidSpecialCurrentRotateRate;
	
	public GameObject char5SpecialHandler;

	public GameObject char6SpecialCollider;

	//slowed vars
	private bool isSlowed;
	private float slowMult = 0.5f;

	
	
	// Use this for initialization
	void Start () {
		
		
		
		
		
		platformType = PlatformS.GetPlatform();
		
		ownRigid = GetComponent<Rigidbody>();
		spriteObjRender = spriteObject.GetComponent<SpriteRenderer>();
		
		dangerousSprite.GetComponent<SpriteRenderer>().enabled = false;
		
		
		allSpawnPts = GameObject.FindGameObjectsWithTag("Spawn");
		
		physicsLayerDefault = gameObject.layer;
		
		if (GlobalVars.characterSelected && GlobalVars.launchingFromScene) { //assign character numbers from global vars when spawning only in game, not while in character select screen
			characterNum = GlobalVars.characterNumber [playerNum - 1]; 
		} 
		
		
		SetSkin ();
		
		soundSource = GetComponent<PlayerSoundS>();
		
		if (GameObject.Find("BlueFlash")){
			lv2Flash = GameObject.Find("BlueFlash").GetComponent<FlashObjS>();
		}
		if (GameObject.Find("RedFlash")){
			lv3Flash = GameObject.Find("RedFlash").GetComponent<FlashObjS>();
		}
		
		
		initialSpawnParticles =  Instantiate (initialSpawnParticlesPrefab, this.transform.position, Quaternion.identity) as GameObject; 
		initialSpawnParticles.GetComponent<ParticleSystem>().startColor = playerParticleMats[characterNum - 1].GetColor("_TintColor");
		
		
		chargeSource = GetComponent<AudioSource>();
		
		startDrag = ownRigid.drag;
		
		startEctoNum = initialHealth; // for ecto mode tail generation

		
		//Get number of lives (mode stuff)
		
		
	}

	void Update () {

		/*
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			characterNum = 1;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			characterNum = 2;
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)){
			characterNum = 3;
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)){
			characterNum = 4;
		}*/

		if (Input.GetKeyDown(KeyCode.Alpha5)){
			characterNum = 5;
		}

		if (Input.GetKeyDown(KeyCode.Alpha6)){
			characterNum = 6;
		}



		/*if (Input.GetKeyDown(KeyCode.P)){
			if (effectPause){
				UnpauseCharacter();
			}
			else{
				PauseCharacter();
			}
		}*/

	}
	
	
	void FixedUpdate () {
		
		if (!nonActive){
			
			ManageCharge();
			BackToMenu();
			
			if (!ScoreKeeperS.gameEnd && ScoreKeeperS.gameStarted) {
				
				// trying to stop a stop moving bug
				transform.rotation = Quaternion.identity;
				
				if (!TimeManagerS.paused) {
					
					respawnInvulnTime -= Time.deltaTime * TimeManagerS.timeMult;
					
					
					// if game is active 
					if (!effectPause && !isDead) {
						
						// movement methods
						CheckWallCast ();
						Walk ();
						Jump ();
						
						Dodge ();
						
						// attack methods


						
						ChargeAttack ();
						AttackRelease ();
						
					}

					DoSpecial();
					
					MiscAction (); //TRAIL RENDERER UPDATE, OTHER THINGS
					
					if(canRespawn)
					{
						Respawn (); // handles respawn during death
					}
					
				}
				
			} else {
				if (ScoreKeeperS.gameStarted){
					
					// don't allow movement once game is over
					
					ownRigid.velocity = Vector3.zero;
				}
			}

			
		}
		
	}
	
	void CheckWallCast(){
		
		// makes sure player goes not try to apply velocity into wall, to avoid getting stuck
		
		// uses raycasts
		RaycastHit rightHit;
		RaycastHit leftHit;
		
		Vector3 castFrom = transform.position;
		castFrom.y += 1f;
		
		Physics.Raycast(castFrom,Vector3.right, out rightHit, 2.5f);
		Physics.Raycast(castFrom,-Vector3.right, out leftHit, 2.5f);
		
		if (rightHit.collider != null){
			
			//print (rightHit.collider.name);
			
			if (rightHit.collider.gameObject.tag != "Player" &&
			    rightHit.collider.gameObject.tag != "PlayerTrail" &&
			    rightHit.collider.gameObject.tag != "Butt" &&
			    rightHit.collider.gameObject.tag != "Glob" &&
			    rightHit.collider.gameObject.tag != "Spawn"&&
			    rightHit.collider.gameObject.tag != "Goal"){
				rightCheck = true;
				//print ("HIT!!");
			}
			else{
				rightCheck = false;
			}
			
		}
		else{
			rightCheck = false;
		}
		
		//print (leftHit.collider);
		
		if (leftHit.collider != null){
			
			if (leftHit.collider.gameObject.tag != "Player" &&
			    leftHit.collider.gameObject.tag != "PlayerTrail"&&
			    leftHit.collider.gameObject.tag != "Butt" &&
			    leftHit.collider.gameObject.tag != "Glob" &&
			    leftHit.collider.gameObject.tag != "Spawn" &&
			    leftHit.collider.gameObject.tag != "Goal"){
				leftCheck = true;
				//print (leftHit.collider.name);
			}
			else{
				leftCheck = false;
			}
			
		}
		else{
			leftCheck = false;
		}
		
	}
	
	void ManageDelay(){
		
		// was used for early kinesthetic purposes, is now outdated
		
		if (TimeManagerS.timeMult != 1){
			
			resetFromPause = false;
			
			ownRigid.velocity = Vector3.zero;
			//	buttObjRigid.velocity = Vector3.zero;
			
			//pauseDelay -= Time.deltaTime*TimeManagerS.timeMult;
			
			ownRigid.useGravity = false;
			
			
		}
		else{
			if (!resetFromPause){
				
				ownRigid.velocity = prevVel;
				ownRigid.useGravity = prevGravState;
			}
			prevVel = ownRigid.velocity;
			prevGravState = ownRigid.useGravity;
			resetFromPause = true;
		}
	}
	
	/*
	_________________________________________________________________________________________________________________________________________________________________________________________________________________________________

	ATTACKING-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	_____________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________

	*/

	void DoSpecial(){

		if (doingSpecial){

			// execute attack according to character num




			// ninja pauses while the slash does its thing
			if (characterNum == 1){
				specialCooldown -= Time.deltaTime;
				ownRigid.velocity = Vector3.zero;
			}

			// acidMouth does a DEATH LASER
			if (characterNum == 2){
				specialCooldown -= Time.deltaTime;
				ownRigid.velocity = Vector3.zero;

				acidSpecialCurrentRotateRate += acidSpecialRotateAccel*Time.deltaTime;
				spriteObject.transform.Rotate(new Vector3(0,0,acidSpecialCurrentRotateRate*Time.deltaTime));

				Debug.Log(acidSpecialCurrentRotateRate);
				Debug.Log(spriteObject.transform.rotation.z);

				Vector3 laserDir = spriteObject.transform.right;
				if (spriteObject.transform.localScale.x > 0){
					laserDir *= -1;
				}

				RaycastHit laserHit;


				Ray laserRay = new Ray(transform.position+laserDir*transform.localScale.x, laserDir);

				if(Physics.Raycast(laserRay, out laserHit, 10000, LayerMask.NameToLayer("LaserRaycase"),
				                   QueryTriggerInteraction.Ignore)){

				acidSpecialReference.transform.position = laserHit.point;
				}
				else{
					Debug.Log("FAKE LASER");
					acidSpecialReference.transform.position = transform.position+laserDir*10000f;
				}
			}

			if (characterNum == 3){
				timeBettwenProjCountdown -= Time.deltaTime;
				ownRigid.velocity = Vector3.zero;

				// rotate head after each shot
				Vector3 targetHeadRot = new Vector3(currentLerpTarget, 0, 0);
				spriteObject.transform.rotation = Quaternion.Euler(targetHeadRot);


				if (timeBettwenProjCountdown <= 0){
					currentProj++;
					if (currentProj > numProjsMax){
						specialCooldown = 0;
					}
					else{
						// shoot projectile using head transform
						GameObject newProj = Instantiate(mrWrapsSpecialProjectile, transform.position,
						                                 Quaternion.identity) as GameObject;

						Vector3 projVel = spriteObject.transform.right;
						if (spriteObject.transform.localScale.x > 0){
							projVel *= -1f;
						}
						projVel *= wrapsSpecialProjSpeed*Time.deltaTime;
						newProj.GetComponent<Rigidbody>().velocity = projVel;
						newProj.GetComponent<MrWrapsSpecialAttackS>().playerRef = this;

						// rotate head
						if ( currentProj != numProjsMax ){
							currentLerpTarget += rotateAmt;
						}

						timeBettwenProjCountdown = timeBetweenProjsMax;

					}
				}
			}

			// pinkwhip does a modified lv3 with no grav
			if (characterNum == 4){
				
				ownRigid.useGravity = false;
				ownRigid.velocity = pinkWhipSpecialVel*Time.deltaTime;
				
			}

			// character 5 pauses while chomp does its thing
			if (characterNum == 5){
				ownRigid.velocity = Vector3.zero;
			}


			// end when period is over

			if (specialCooldown <= 0){
				doingSpecial = false;
				if (acidSpecialReference){
					Destroy(acidSpecialReference);
				}
				UnpauseCharacter();
			}

		}
		else{
			if (numKOsInRow >= 3 && Input.GetButton("BButtonPlayer" + playerNum + platformType) && !attacking && !charging){

				dangerObj.GetComponent<DamageS>().MakeSlashEffect(transform.position);

				if (characterNum != 6){
					doingSpecial = true;
					specialCooldown = specialCooldownMax;
					PauseCharacter();
				}


				CameraShakeS.C.TimeSleep(0.2f);
				numKOsInRow = 0;

				if (specialParticles != null){
					Destroy(specialParticles.gameObject);
				}

				// if ghostMask, execute attack immediately
				if (characterNum == 1){

					GameObject specialAttack = Instantiate(ghostMaskSpecialPrefab, transform.position, Quaternion.identity)
						as GameObject;
					specialAttack.GetComponent<GhostMaskSpecialAttackS>().playerRef = this;

				}

				// if acid, prep for DEATH LASER
				if (characterNum == 2){
					specialCooldown = acidSpecialTimeMax;

					acidSpecialReference = Instantiate(acidSpecialCollider, transform.position, Quaternion.identity)
						as GameObject;
					acidSpecialReference.GetComponent<DamageS>().MakeSpecial(this);

					acidSpecialCurrentRotateRate = acidSpecialStartRotateRate;
					
					// face input dir
					Vector3 inputDir = Vector3.zero;
					inputDir.x = Input.GetAxis("HorizontalPlayer" + playerNum + platformType);
					inputDir.y = Input.GetAxis("VerticalPlayer" + playerNum + platformType);
					spriteObject.GetComponent<PlayerAnimS>().FaceTargetInstant(inputDir);
					currentLerpTarget = spriteObject.transform.rotation.eulerAngles.z;
				}

				// if mummy, prep for shots
				if (characterNum == 3){
					timeBettwenProjCountdown = 0;
					currentProj = 0;
					specialCooldown = 1;
					
					// face input dir
					Vector3 inputDir = Vector3.zero;
					inputDir.x = Input.GetAxis("HorizontalPlayer" + playerNum + platformType);
					inputDir.y = Input.GetAxis("VerticalPlayer" + playerNum + platformType);
					spriteObject.GetComponent<PlayerAnimS>().FaceTargetInstant(inputDir);
					currentLerpTarget = spriteObject.transform.rotation.eulerAngles.z;
				}

				if (characterNum == 4){
					specialCooldown = 1;
					GameObject specialAttack = Instantiate(pinkWhipSpecialPrefab, transform.position, transform.rotation)
						as GameObject;
					specialAttack.transform.parent = transform;
					specialAttack.GetComponent<PinkWhipSpecialAttackS>().playerRef = this;

 					// shoot off in dir
					Vector3 inputDir = Vector3.zero;
					inputDir.x = Input.GetAxis("HorizontalPlayer" + playerNum + platformType);
					inputDir.y = Input.GetAxis("VerticalPlayer" + playerNum + platformType);
					pinkWhipSpecialVel = inputDir.normalized*pinkWhipSpecialSpeed;

					TurnOnIgnoreWalls();

				}

				// character 5 does vertical chomp
				if (characterNum == 5){

					GameObject SpecialAttackChar5 = 
						Instantiate(char5SpecialHandler, transform.position, Quaternion.identity)
							as GameObject;

					SpecialAttackChar5.GetComponent<MegaChompHandlerS>().playerRef = this;
					specialCooldown = 1f;

				}

				// character 6 (unnamed for now) destroys self with explosion
				if (characterNum == 6){
					GameObject SpecialAttackChar6 = 
						Instantiate(char6SpecialCollider, transform.position, Quaternion.identity)
							as GameObject;
					SpecialAttackChar6.GetComponent<MrWrapsSpecialAttackS>().playerRef = this;
					SelfDestruct();
				}



			}
		}

	}
	
	void ChargeAttack () {
		
		// method for handling attack charge
		
		// turn stretch button bool on/off
		if ((Input.GetAxis("RightTriggerPlayer" + playerNum + platformType) > triggerSensitivity) 
		    || Input.GetButton("RightBumperPlayer" + playerNum + platformType) 
		    || (Input.GetButton("BButtonPlayer" + playerNum + platformType) && numKOsInRow < 3)){
			attackTriggerDown = true;
		}
		else{
			attackTriggerDown = false;
			playedLV1ChargeSound = false;
			playedLV2ChargeSound = false;
			playedLV3ChargeSound = false;
		}
		
		// don't allow charge if already attacking, charging, or dodging
		
		if (!doingChomp){
			if (attackTriggerDown && !charging && canCharge && !isDangerous && !doingSpecial && dodgeTimeCountdown <= 0){
				charging = true;
				canCharge = false;
				
				TriggerCharge();
				
				chargeTime = 0;
				
				groundLeeway = 0.5f;
			}
			
			if (charging){
				if(chargingParticles == null)
				{
					chargingParticles = Instantiate( chargingParticlePrefab, this.transform.position, Quaternion.identity) as GameObject; 
					chargingParticles.transform.parent = this.transform; 
					chargingParticles.GetComponent<ParticleSystem>().startColor = playerParticleMats[characterNum-1].GetColor("_TintColor");
				}
				
				groundLeeway = 0.5f;
				
				//print ("charging!!");
				
				ownRigid.useGravity = false;
				ownRigid.velocity = Vector3.zero;
				hasDoubleJumped = true;

				// apply character charge mult
				float chargeMult = 1;
				if (characterNum == 1){
					chargeMult = PlayerCharStatsS.ninja_ChargeMult;
				}
				if (characterNum == 2){
					chargeMult = PlayerCharStatsS.acidMouth_ChargeMult;
				}
				if (characterNum == 3){
					chargeMult = PlayerCharStatsS.mummy_ChargeMult;
				}
				if (characterNum == 4){
					chargeMult = PlayerCharStatsS.pinkWhip_ChargeMult;
				}
				if (characterNum == 5){
					chargeMult = PlayerCharStatsS.char5_ChargeMult;
				}
				if (characterNum == 6){
					chargeMult = PlayerCharStatsS.char6_ChargeMult;
				}
				if (characterNum == 7){
					chargeMult = PlayerCharStatsS.char7_ChargeMult;
				}
				
				chargeTime+=chargeMult*Time.deltaTime*TimeManagerS.timeMult;
				
				// play sounds when you hit each threshold
				if (chargeTime > lv0MaxChargeTime && !playedLV1ChargeSound){
					//soundSource.PlayChargeLv1();
					playedLV1ChargeSound = true;
				}
				if (chargeTime > medChargeTime && !playedLV2ChargeSound){
					//soundSource.PlayChargeLv2();
					playedLV2ChargeSound = true;
				}
				if (chargeTime > maxChargeTime && !playedLV3ChargeSound){
					//soundSource.PlayChargeLv3();
					playedLV3ChargeSound = true;
				}
				
				
				
				if (!attackTriggerDown || !chargeSource.isPlaying){
					GameObject.Destroy( chargingParticles.gameObject); 
					
					groundLeeway = attackGroundLeewayMaxTime;
					attacking = true;
					charging = false;
					performedAttack = false;
					
					// allow for ground pound
					if (!jumped){
						jumped = true;
					}
					else{
						hasDoubleJumped = true;
					}
					
					// butt should not follow
					//buttObj.isFollowing = false;
					
					
					if (chargeTime >= maxChargeTime){
						// fully charged attack
						attackToPerform = 2;
						attackPriority = 2;
					}
					// do redirect attack
					else if (chargeTime >= medChargeTime){
						attackToPerform = 1;
						attackPriority = 1;
						//print ("do attack 1");
					}
					// do fling
					// TRYING OUT TAKING OUT FLING LV1
					/*
					else if (chargeTime > lv0MaxChargeTime){
						attackToPerform = 0;
						attackPriority = 0;
					}*/
					// do chomp
					else{
						// activate chomp vars
						doingChomp = true;
						ownRigid.useGravity = false;
						canAirStrafe = false;
						attacking = true;
						
						// attack priority of 1 to defeat ground pound
						attackPriority = 1;
						attackToPerform = -1;
						ownRigid.velocity = Vector3.zero;
						ownRigid.drag = chompDrag;
						
						Vector3 attackDir = Vector3.zero;
						attackDir.x = Input.GetAxis ("HorizontalPlayer" + playerNum + platformType);
						attackDir.y = Input.GetAxis ("VerticalPlayer" + playerNum + platformType);
						
						if (attackDir.x == 0 && attackDir.y == 0) {
							attackDir.x = 1; 
						}

						if (!isSlowed){
							ownRigid.AddForce(attackDir.normalized*chompForce*Time.deltaTime, ForceMode.Impulse);
						}
						else{
							ownRigid.AddForce(attackDir.normalized*chompForce*slowMult*Time.deltaTime, ForceMode.Impulse);
						}


						GameObject chompObj = Instantiate(chompHitObj, transform.position, Quaternion.identity)
							as GameObject;
						chompObj.GetComponent<ChompColliderS>().SetDirection(attackDir.normalized, this);
						
						chompTimeCountdown = chompMaxTime;
					}
				}
				
				
			}
		}
		
		// get out of chomp state
		if (doingChomp){
			//print (ownRigid.useGravity);
			//print (chompTimeCountdown);
			//print (canAirStrafe);
			chompTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			if (chompTimeCountdown <= 0){
				//print ("CHOMP OVER");
				doingChomp = false;
				ownRigid.useGravity = true;
				canAirStrafe = true;
				ownRigid.velocity = Vector3.zero;
				attacking = false;
				chargeTime = 0;
				ownRigid.drag = startDrag;
			}
		}
		
	}
	void AttackRelease () {
		
		// method for triggering appropriate attack once charge button is released, based on charge time
		
		if (!doingChomp){
			if (attacking) {
				
				// allow for short attacks on the ground
				if (groundDetect.Grounded()){
					groundLeeway -= Time.deltaTime * TimeManagerS.timeMult;
				}
				
				if (!performedAttack) 
				{
					isDangerous = true;

					
					GlobalVars.totalFlings[playerNum-1]++; 
					
					
					attackDir = Vector3.zero;
					attackDir.x = Input.GetAxis ("HorizontalPlayer" + playerNum + platformType);
					attackDir.y = Input.GetAxis ("VerticalPlayer" + playerNum + platformType);
					
					if (attackDir.x == 0 && attackDir.y == 0) {
						attackDir.x = 1; 
					}
					
					// play attack release sound
					soundSource.PlayReleaseSound ();
					if (attackToPerform == 2){
						lv3Flash.ResetFade();
						soundSource.PlayChargeLv3();
					}
					if (attackToPerform == 1){
						lv2Flash.ResetFade();
						soundSource.PlayChargeLv2();
					}
					if (attackToPerform == 0){
						
						soundSource.PlayChargeLv1();
					}
				}
				
				if (attackToPerform == 2) {
					FlingFastAttack (false);
				}
				if (attackToPerform == 1) {
					FlingSlowAttack (false);
					
					//FlingMiniAttack (false); //replacing for now
					
				}  if (attackToPerform == 0) {
					//JabAttack();
					FlingMiniAttack (false);
					
					//print ("IM DOING THE FLING");
					
				}
				
				performedAttack = true;
				
			} 
			else 
			{
				attackPriority = 0; 
			}
		}
		
		
	}
	
	
	void FlingMiniAttack(bool endAttack)
	{
		/*
		Just a small fling in a direction
		*/
		
		if(!performedAttack)
		{
			if (attackDir.x == 0 && attackDir.y == 0){
				attackDir.x = 1;
			}
			
			// attack priority of 2 (beat ground pound and chomp)
			attackPriority = 2;
			
			bulletVel = attackDir.normalized*Time.deltaTime*lv1Force ;
			if (isSlowed){
				bulletVel*=slowMult;
			}
			ownRigid.AddForce(bulletVel,ForceMode.VelocityChange);
			
			//print(chargeTime); 

			ownRigid.useGravity = true;
			
			//print (bulletVel); 
		}
		else
		{
			
		}
		
		if(endAttack)
		{
			// trigger return early
			//isDangerous = false;
			attacking = false;
			//snapReturning = true;
		}
		
	}
	
	void FlingSlowAttack(bool endAttack)
	{
		
		// this is beginning of lv 2 attack (before the pikachu up-b direction change)
		if(endAttack)
		{
			
			//print ("I ended attack");
			

			lv1OutCountdown = 0;
			didLv2Fling = true;
			groundLeeway = 0;
			ownRigid.useGravity = true;
			canAirStrafe = true; 
			
			// trigger second half (mini fling)
			
			//lv2AttackPauseCountdown = lv2AttackPauseTimeMax;
			
			performedAttack = false;
			attackDir = ownRigid.velocity;
			
			float inputX = Input.GetAxis("HorizontalPlayer"+playerNum+platformType);
			float inputY = Input.GetAxis("VerticalPlayer"+playerNum+platformType);
			
			// trigger lv 2
			if (inputY != 0 || inputX != 0){
				attackDir.x = inputX;
				attackDir.y = inputY;
			}
			chargeTime = medChargeTime*2f;
			attackToPerform = 0;
			
			// trigger lv 1 fling at end of attack
			FlingMiniAttack(false);
			
			soundSource.PlayChargeLv1();
			
			//print ("part 2!");
			
		}
		else
		{
			if(!performedAttack)
			{
				
				// start of first fling
				
				//print ("Start slow fling");
				
				// attack priority of 3 to defeat everything below mini fling
				attackPriority = 3;
				
				canAirStrafe = false; 
				ownRigid.useGravity = false;
				
				// set start snap vel
				bulletVel = attackDir.normalized*Time.deltaTime*lv2OutRate;
				
				//print (bulletVel); 
				
				lv1OutCountdown = lv1OutTimeMax;
				
				startedLv2Pause = false;
			}
			else
			{
				// this will act the same as weak attack up until attack time is over 
				// in which case butt does its thing
				// lerp out to attack target
				if (lv1OutCountdown > 0)
				{
					
					//print ("I am trying to end attack");
					
					//print (lv1OutCountdown);
					
					// old way without physics
					/*
							// set vel to 0
							ownRigid.velocity = Vector3.zero;
							
							lv1OutCountdown -= Time.deltaTime*TimeManagerS.timeMult;
							
							transform.position = Vector3.Lerp(transform.position,bulletVel,lv1OutRate*Time.deltaTime
							                                  *TimeManagerS.timeMult);
							                                  */
					
					lv1OutCountdown -= Time.deltaTime*TimeManagerS.timeMult;
					
					// vel decrease over time (linear, can be made exponential)
					ownRigid.velocity = bulletVel*TimeManagerS.timeMult*(lv1OutCountdown/lv1OutTimeMax)*1.2f;
				}
				else
				{
					//butt should return
					//buttObj.isFollowing = true;
					
					if (!startedLv2Pause){

						
						//canAirStrafe = true; 
						startedLv2Pause = true;
						ownRigid.velocity = Vector3.zero;
						
						// allows time for tail to catch up
						lv2AttackPauseCountdown = lv2AttackPauseTimeMax;
					}
					else{
						// count down pause time
						lv2AttackPauseCountdown -= TimeManagerS.timeMult*Time.deltaTime;
						//print (lv2AttackPauseCountdown);
						ownRigid.velocity = Vector3.zero;
						// once this reaches zero, end the attack
						if (lv2AttackPauseCountdown <= 0){
							canAirStrafe = true;
							FlingSlowAttack(true);
							//print ("part 2!");
						}
						
					}
				}
			}
		}
		
		
	}
	
	void FlingFastAttack(bool endAttack)
	{
		
		// lv 3 attack
		
		if(endAttack)
		{
			// have butt go to head
			//buttObj.isFollowing = true;
			// lock in place so no sliding
			
			attacking = false;
			ownRigid.useGravity = true;
			canAirStrafe = true; 
			
			this.GetComponent<SphereCollider>().material = normalPhysics; 
			
		}
		else
		{
			if(!performedAttack)
			{
				
				// attack priority of 4 to beat everything 
				attackPriority = 4;
				
				// turn on platform ghosting
				TurnOnIgnoreWalls();
				
				
				if(!groundDetect.Grounded())
					this.GetComponent<SphereCollider>().material = bouncyPhysics; 
				
				// bullet snap
				bulletVel = attackDir.normalized*Time.deltaTime*lv3BulletSpeed;
				//dontCorrectSpeed = true;
				
				//print ("LV3!!");
				
				bool wallHit = false;
				
				Physics.Raycast(transform.position,bulletVel.normalized, out newHit, 2f);
				
				if (newHit.collider != null)
				{
					if (newHit.collider.tag == "Ground" || newHit.collider.tag == "Wall")
					{
						wallHit = true;
					}
				}
				
				if (!wallHit){
					//set attack time
					lv1OutCountdown = lv1OutTimeMax;
					
					// add bullet force
					ownRigid.velocity = Vector3.zero;
					if (isSlowed){
						bulletVel*=slowMult;
					}
					ownRigid.AddForce(bulletVel,ForceMode.VelocityChange);
					
					ownRigid.useGravity = true;
					
					// kinesthetics
					CameraShakeS.C.TimeSleep(0.2f);
					dangerObj.GetComponent<DamageS>().MakeSlashEffect(transform.position+bulletVel.normalized);
				}
				else{

					
					// set attack to 2 so butt behaves properly
					attackToPerform = 2;
					
				}
			}
		}
		
		
		
	}
	
	
	
	/*
	_________________________________________________________________________________________________________________________________________________________________________________________________________________________________

	MOVEMENT-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	_____________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________

	*/
	void Walk () 
	{
		
		// turn variables related to being on the ground on/off
		if (!groundDetect.Grounded()){ // if in the air
			groundLeeway = 0;
		}
		else{ // if not the air
			canAirStrafe = true;
			canDodge = true;
		}
		
		
		if (!charging && canAirStrafe)
		{
			float xForce = Input.GetAxis("HorizontalPlayer" + playerNum + platformType);
			//float xForce = Input.GetAxis("Horizontal");
			xForce *= walkSpeed*TimeManagerS.timeMult*Time.deltaTime;
			
			if (!groundDetect.Grounded())
			{
				xForce*=airControlMult;
			}
			
			// make sure not to do force stuff when up against wall
			if ((xForce > 0 && !rightCheck) || xForce < 0 && !leftCheck){
				
				bool applyForce = true; 

				// apply character run mult
				float runMult = 1;

				if (characterNum == 1){
					runMult *= PlayerCharStatsS.ninja_SpeedMult;
				}
				if (characterNum == 2){
					runMult *= PlayerCharStatsS.acidMouth_SpeedMult;
				}
				if (characterNum == 3){
					runMult *= PlayerCharStatsS.mummy_SpeedMult;
				}
				if (characterNum == 4){
					runMult *= PlayerCharStatsS.pinkWhip_SpeedMult;
				}
				if (characterNum == 5){
					runMult *= PlayerCharStatsS.char5_SpeedMult;
				}
				if (characterNum == 6){
					runMult *= PlayerCharStatsS.char6_SpeedMult;
				}
				if (characterNum == 7){
					runMult *= PlayerCharStatsS.char7_SpeedMult;
				}
				
				if((ownRigid.velocity.x > maxSpeed*runMult) && (xForce > 0))
				{
					applyForce = false; 
					//print ("Dont apply force right!");
				}
				else if((ownRigid.velocity.x < -maxSpeed*runMult) && (xForce < 0))
				{
					applyForce = false; 
					//print ("Dont apply force left!");
				}
				
				//print (applyForce);


				
				if(applyForce){
					ownRigid.AddForce(new Vector3(xForce*runMult,0,0));
				}

				if (isSlowed){
					ownRigid.AddForce(new Vector3(-xForce*0.5f*runMult,0,0));
				}
				
				
				Vector3 fixVel = ownRigid.velocity;
				
				/*
					July 13th: Changing the way max speed in handled, player will not decelerate when over max speed.
					However the player cannot add force in the direction that the player is going max speed, but can always decelerate. 
					
					if (!dontCorrectSpeed){	
	
						if (fixVel.x < -maxSpeed){
							fixVel.x = -maxSpeed;
						}
						if (fixVel.x > maxSpeed){
							fixVel.x = maxSpeed;
						}
					}
					*/
				
				if (fixVel.x > 0){
					facingRight = true;
				}
				if (fixVel.x < 0){
					facingRight = false;
				}
				
				//ownRigid.velocity = fixVel;
			}
			
			
			//print (Input.GetAxis("Horizontal"));
			
		}
		else{
			// if not allowed to apply force, say so for debugging purposes
			//print("I can't move!");
		}
		
		
	}
	
	
	
	void Jump () {
		
		//print (groundPounded);
		
		// turn danger off
		
		// allow charge attack
		
		// don't do during lv 3
		if (attacking && attackToPerform == 2){
		}
		else{
			
			if (groundDetect.Grounded()){
				// end a fling attack on ground hit
				//if (attackToPerform == 1 && attacking){
				if (groundLeeway <= 0){
					isDangerous = false;
					attacking = false;
					didLv2Fling = false;
					ownRigid.useGravity = true;

					
					groundPounded = false;
					
					charging = false;
				}
				//}
				canCharge = true;
			}
		}

		// turn off gravity and pause in place for a bit before ground pound
		if (chargingGroundPound){

			if(chargingParticles == null)
			{
				chargingParticles = 
					Instantiate( chargingParticlePrefab, this.transform.position, Quaternion.identity) as GameObject; 
				chargingParticles.transform.parent = this.transform; 

			}

			groundPoundPauseCountdown += Time.deltaTime*TimeManagerS.timeMult;
			if (groundPoundPauseCountdown >= groundPoundPauseMax){
				Vector3 groundPoundVel = Vector3.zero;
				groundPoundVel.y = -groundPoundForce*Time.deltaTime*TimeManagerS.timeMult;
				if (isSlowed){
					groundPoundVel *= slowMult;
				}
				ownRigid.velocity = groundPoundVel;
				isDangerous = true;
				ownRigid.useGravity = true;

				hasDoubleJumped = true;
				
				canAirStrafe = true;

				chargingGroundPound = false;
				groundPounded = true;
				
				// lowest attack priority
				attackPriority = 0;
				
				//print ("Groundpound!");
				// play an attack sound
				//soundSource.PlayChargeLv2();
				soundSource.PlayGroundPoundReleaseSound();
				Instantiate(spawnParticlePrefab,this.transform.position, Quaternion.identity);

				Destroy(chargingParticles);
				
				// allow for air control
				//dontCorrectSpeed = false;
				
				//SleepTime(groundPoundPauseTime);
			}

		}
		
		// turn jump ability on/off depending on grounded status
		
		if (jumped){
			if (groundDetect.Grounded()){
				jumped = false;
				hasDoubleJumped = false;
				if (groundPounded){
					groundPounded = false;
					//isDangerous = false;
				}
			}
		}
		else{
			if (!groundDetect.Grounded()){
				jumped = true;
			}
		}
		
		
		// detect button up
		if (!Input.GetButton("AButtonPlayer" + playerNum + platformType))
		{


			jumpButtonDown = false;
			chargingGroundPound = false;
			groundPoundPauseCountdown = 0;

			if (chargingParticles){
				Destroy(chargingParticles);
			}


		}
		
		
		
		if (Input.GetButton("AButtonPlayer" + playerNum + platformType) && !jumpButtonDown)
		{
			
			
			//print ("Jump!");
			
			jumpButtonDown = true;
			
			if (!jumped)
			{
				
				// don't do regular jump while attacking or charging or dodging
				if (!attacking && !charging && !dodging)
				{
					Vector3 jumpForce = Vector3.zero;
					
					jumpForce.y = jumpSpeed*Time.deltaTime*TimeManagerS.timeMult;
					if (isSlowed){
						jumpForce.y *= slowMult;
					}


					// apply character jump mult
					if (characterNum == 1){
						jumpForce *= PlayerCharStatsS.ninja_JumpMult;
					}
					if (characterNum == 2){
						jumpForce *= PlayerCharStatsS.acidMouth_JumpMult;
					}
					if (characterNum == 3){
						jumpForce *= PlayerCharStatsS.mummy_JumpMult;
					}
					if (characterNum == 4){
						jumpForce *= PlayerCharStatsS.pinkWhip_JumpMult;
					}
					if (characterNum == 5){
						jumpForce *= PlayerCharStatsS.char5_JumpMult;
					}
					if (characterNum == 6){
						jumpForce *= PlayerCharStatsS.char6_JumpMult;
					}
					if (characterNum == 7){
						jumpForce *= PlayerCharStatsS.char7_JumpMult;
					}

					Vector3 fixVel = ownRigid.velocity;
					fixVel.y = 0;
					ownRigid.velocity = fixVel;

					ownRigid.AddForce(jumpForce);
					
					Instantiate(jumpParticles, this.transform.position, Quaternion.identity); 
					
					// play jump sound
					soundSource.PlayJumpSound();
					
					addingJumpTime = 0;
					stopAddingJump = false;

					jumped = true;

				}
			}
			
			// do ground pound charge if not charging or dodging
			else if (!groundPounded && !charging && !clingingToWall && !dodging){
		
				chargingGroundPound = true;
				groundPoundPauseCountdown = 0;

				if (!hasDoubleJumped){
					Vector3 jumpForce = Vector3.zero;
					
					// add to jump speed for double jump bc of lack of air boost
					jumpForce.y = jumpSpeed*1.7f*Time.deltaTime*TimeManagerS.timeMult;
					
					
					// apply character jump mult
					if (characterNum == 1){
						jumpForce *= PlayerCharStatsS.ninja_JumpMult;
					}
					if (characterNum == 2){
						jumpForce *= PlayerCharStatsS.acidMouth_JumpMult;
					}
					if (characterNum == 3){
						jumpForce *= PlayerCharStatsS.mummy_JumpMult;
					}
					if (characterNum == 4){
						jumpForce *= PlayerCharStatsS.pinkWhip_JumpMult;
					}
					
					Vector3 fixVel = ownRigid.velocity;
					fixVel.y = 0;
					ownRigid.velocity = fixVel;
					
					ownRigid.AddForce(jumpForce);
					
					Instantiate(jumpParticles, this.transform.position, Quaternion.identity); 
					
					// play jump sound
					soundSource.PlayJumpSound();
					
					addingJumpTime = 0;
					stopAddingJump = false;
					
					ownRigid.useGravity = true;
					hasDoubleJumped = true;
					
					
					addingJumpTime = 0;
					stopAddingJump = true;
					canAirStrafe = true;

					
					Destroy(chargingParticles);
				}



			}
			else{
				// don't do anything
			}
		}
		
		
		// add addtl jump force
		if ((jumped || hasDoubleJumped) && !chargingGroundPound){
			if (!jumpButtonDown){
				stopAddingJump = true;
			}
			else{
				if (!stopAddingJump){
					Vector3 jumpForce = Vector3.zero;
					
					jumpForce.y = addJumpForce*Time.deltaTime*TimeManagerS.timeMult;
					
					ownRigid.AddForce(jumpForce);
					
					addingJumpTime += Time.deltaTime*TimeManagerS.timeMult;
					if (addingJumpTime > addJumpForceMaxTime){
						stopAddingJump = true;
					}
				}
			}
		}
	}
	
	void Dodge () {
		
		if (!TimeManagerS.paused){
			dodgeTimeCountdown -= Time.deltaTime;
			if (dodging && dodgeTimeCountdown <= 0){
				dodging = false;
				canAirStrafe = true;
				ownRigid.velocity = Vector3.zero;
				ownRigid.useGravity = true;
				ownRigid.drag = startDrag;
			}
		}
		
		if (!Input.GetButton("XButtonPlayer"+playerNum+platformType) && dodgeButtonDown){
			dodgeButtonDown = false;
		}
		
		// read for dodge input and do dodge
		if (!dodging && canDodge && dodgeTimeCountdown <= 0){
			if (Input.GetButton("XButtonPlayer"+playerNum+platformType) && !dodgeButtonDown){
				
				//print ("DODGED");
				
				// reset dodge time, add invuln, and add dodge force
				
				// read direction from left stick
				Vector3 dodgeDir = Vector3.zero;
				dodgeDir.x = Input.GetAxis("HorizontalPlayer" + playerNum + platformType);
				dodgeDir.y = Input.GetAxis("VerticalPlayer" + playerNum + platformType);
				
				// add force
				ownRigid.velocity = Vector3.zero;
				if (!isSlowed){
					ownRigid.AddForce(dodgeDir*dodgeForce*Time.deltaTime, ForceMode.Impulse);
				}
				else{
					ownRigid.AddForce(dodgeDir*dodgeForce*slowMult*Time.deltaTime, ForceMode.Impulse);
				}

				// stop current attack

				attacking = false;
				isDangerous = false;
				
				
				
				//print (dodgeDir*dodgeForce*Time.deltaTime);
				//print (ownRigid.velocity);
				
				// reset times
				dodgeTimeCountdown = dodgeTimeMax;
				respawnInvulnTime = dodgeInvulnTimeMax;
				
				// only allow one dodge in air; should reset if on ground
				canDodge = false;
				
				// turn off grav for dodge
				ownRigid.useGravity = false;
				dodging = true;
				canAirStrafe = false;
				
				ownRigid.drag = dodgeDrag;

				// play sound
				soundSource.PlayJumpSound();

				dodgeButtonDown = true;
				
			}
		}
		
	}
	
	void MiscAction()
	{
		
		// handles various things including dangerous states and trail renderers
		
		//this.GetComponent<LineRenderer>().SetPosition(0,this.transform.position);
		//this.GetComponent<LineRenderer>().SetPosition(1,buttObj.transform.position);
		
		if(isDangerous && !isSlowed){
			dangerousSprite.GetComponent<SpriteRenderer>().enabled = true; 
			
		}
		else{
			dangerousSprite.GetComponent<SpriteRenderer>().enabled = false; 
		}
		
		hurtCounter -= 1;
		
		if (hurtCounter < 0) {
			
			spriteObjRender.color = Color.white;
			trailRendererGO.GetComponent<TrailRenderer>().material = playerMats [characterNum -1];
			
			
		} else {
			spriteObjRender.color = hurtTint; 
			trailRendererGO.GetComponent<TrailRenderer>().material = playerHurtMats[characterNum -1];
		}
		
		// trying to fix something with lv 2
		if (!groundDetect.Grounded()){
			if (attackToPerform == 1){
				groundLeeway = 0;
			}
		}
		else{
			
			this.GetComponent<SphereCollider>().material = normalPhysics; 
		}
		
		if (isSpawning) {
			if (initialSpawnParticles != null) {
				
				spriteObject.GetComponent<Renderer> ().enabled = false; 
				trailRendererGO.GetComponent<TrailRenderer> ().enabled = false; 
				trailRendererGO2.GetComponent<TrailRenderer> ().enabled = false; 
				
			} else {
				if(!isSpawning)
				{
					Instantiate(spawnParticlePrefab,this.transform.position, Quaternion.identity); 
					isSpawning = true; 
					spriteObject.GetComponent<Renderer> ().enabled = true; 
					trailRendererGO.GetComponent<TrailRenderer> ().enabled = true; 
					trailRendererGO2.GetComponent<TrailRenderer> ().enabled = true; 
				}
			}
		}
		
	}
	
	
	public void SetSkin() //Ninja, Acid, Mummy, Pink
	{
		this.GetComponent<LineRenderer> ().material = playerMats [characterNum-1];
		trailRendererGO.GetComponent<TrailRenderer> ().material = playerMats [characterNum - 1]; 
		spriteObject.GetComponent<PlayerAnimS> ().SetCurrentSprites (characterNum, colorNum);
		GetComponent<TrailHandlerRedubS> ().SetDotMaterial ();
		
		
		
	}
	
	
	
	void Respawn () {
		
		
		if (respawning && !effectPause){
			
			//Disable player actions and physics
			ownRigid.useGravity = false;
			ownRigid.velocity = Vector3.zero;
			//dangerObj.SetActive(false);

			//Halt collisions
			GetComponent<Collider>().enabled = false;
			
			//Take Away Control
			isDangerous = false;
			jumpButtonDown = false;
			attackTriggerDown = false;
			charging = false;
			attacking = false;
			chargeTime = 0;
			canAirStrafe = true;
			
			if(respawnTimeCountdown == respawnTimeMax)
			{
				
				
				
				//VISUAL EFFECTS
				//Eliminate any current 
				if(chargingParticles!= null)
				{
					GameObject.Destroy(chargingParticles.gameObject); 
				}
				if(specialParticles!= null)
				{
					GameObject.Destroy(specialParticles.gameObject); 
				}
				
				
				deathPos = this.transform.position;

				// reset death streak
				numKOsInRow = 0;
				
				//move player to new respawn position right away so trail renderer has time to update.
				
				// look for spawn positions that don't have a player attached
				List<GameObject> newSpawnPts = new List<GameObject>(0);
				for (int i = 0; i < allSpawnPts.Length; i++){
					if (!allSpawnPts[i].GetComponent<SpawnPtS>().playerInRange()){
						newSpawnPts.Add (allSpawnPts[i]);
					}
				}
				
				int randomSpawn = Mathf.FloorToInt( Random.Range(0, newSpawnPts.Count));
				GameObject newSpawn = newSpawnPts[randomSpawn];
				transform.position = newSpawn.transform.position;
				
				trailRendererGO.GetComponent<TrailRenderer>().enabled = false ;
				trailRendererGO2.GetComponent<TrailRenderer>().enabled = false ;
			}
			
			
			if(respawnParticles != null && numLives != 0){
				ParticleSystem spawnParticles = respawnParticles.GetComponent<ParticleSystem>();
				//spawnParticles.startLifetime = 2;
				respawnParticles.transform.position = 
					Vector3.Lerp(deathPos, this.transform.position, spawnParticles.time/(respawnTimeMax*0.5f)); 

				if (spawnParticles.isPaused){
					spawnParticles.Play();
				}


			}

			spriteObjRender.enabled = false;

			
			//Give back control - should probably be converted to alternate thread -- ienumerator?
			if (respawnTimeCountdown <= 0 && numLives != 0){
				Instantiate(spawnParticlePrefab,this.transform.position, Quaternion.identity); 
				
				GetComponent<Collider>().enabled = true;
				spriteObjRender.enabled = true;
				respawning = false;
				
				respawnInvulnTime = 1.5f;
				
				ownRigid.useGravity = true;;
				trailRendererGO.GetComponent<TrailRenderer>().enabled = true ;
				trailRendererGO2.GetComponent<TrailRenderer>().enabled = true ;


				health = initialHealth;
				
				//DisableAttacks(); 
				
				soundSource.PlayCharIntroSound();
				
				//no longer dead
				isDead = false;
			}
			
			
			
			//Countdown
			respawnTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			
			
			
			
			
		}

		if (effectPause){
			if(respawnParticles != null){
				
				respawnParticles.GetComponent<ParticleSystem>().Pause();
				
				
			}
		}
		
	}
	
	public void TakeDamage(float dmg){

		if (!inCharSelect){
		health -= dmg;
		hurtCounter = hurtTimer; 
		
		if (health <= 0){

				ownRigid.velocity = Vector3.zero;

				doingSpecial = false;
				specialCooldown = 0;
			
			
			numLives --; 			//Decrement Counter
			
			GetComponent<TrailHandlerRedubS>().DestroyPlayerDots(); 
			
			UpdateScorekeeper(); 	//Have scorekeeper update (and update UI)
			
			respawning = true;
			respawnTimeCountdown = respawnTimeMax;

			
			
			if(numLives == 0)
				this.gameObject.SetActive(false);
			else{
				respawnParticles = Instantiate(respawnParticlePrefab, this.transform.position, Quaternion.identity) as GameObject;
				respawnParticles.GetComponent<ParticleSystem>().startColor = playerParticleMats[characterNum - 1].GetColor("_TintColor");
				respawnParticles.GetComponent<ParticleSystem>().startLifetime = respawnTimeCountdown;
			}
			
			Instantiate(deathParticles, this.transform.position, Quaternion.identity); 
			soundSource.PlayDeathSounds();
			}
			
			
		}
	}

	public void TurnOnCharSelect(){
		inCharSelect = true;
	}
	void UpdateScorekeeper()
	{
		
		
		bool hasMoreLives = (numLives != 0);
		ScoreKeeperS scoreKeeper = FindObjectOfType<Camera>().GetComponent<ScoreKeeperS>() as ScoreKeeperS;
		scoreKeeper.PlayerDied(this, hasMoreLives);
		
		
	}
	public void InstantiateDeathParticles(){
		
		Instantiate(deathParticles, this.transform.position, Quaternion.identity); 
	}
	
	public void SleepTime(float delayTime){
		//pauseDelay = delayTime;
		//prevGravState = ownRigid.useGravity;
		//prevVel = ownRigid.velocity;
		//prevButtVel = buttObjRigid.velocity;
		//effectPause = true;
		
		//print ("PAUSE");
	}

	public void PauseCharacter(){

		effectPause = true;
		capturedGrav = ownRigid.useGravity;
		capturedVel = ownRigid.velocity;
		capturedDanger = isDangerous;

		ownRigid.useGravity = false;
		ownRigid.velocity = Vector3.zero;
		isDangerous = capturedDanger;

	}
	public void UnpauseCharacter(){

		effectPause = false;
		ownRigid.useGravity = capturedGrav;
		ownRigid.velocity = capturedVel;
		isDangerous = capturedDanger;

	}
	
	void OnCollisionEnter(Collision other){
		
		
		Vector3 hitParticleSpawn = this.transform.position; 
		
		
		GameObject newParticles =  Instantiate(hitParticles,hitParticleSpawn,Quaternion.identity) as GameObject;
		newParticles.GetComponent<ParticleSystem>().startColor = playerParticleMats[characterNum - 1].GetColor("_TintColor");
		
		if (other.gameObject.tag == "Ground"){

			if (doingSpecial){
				if (characterNum == 4){
					
					// destroy character
					SelfDestruct();
					TurnOffIgnoreWalls();
					
					
				}
			}
			
			if (attacking){
				

				//TURN OFF THE ATTACKS
				if (attackToPerform == 0)
				{
					//JabAttack();
					FlingMiniAttack(true);
				}
				if (attackToPerform == 1)
				{
					//FlingMiniAttack(true);
					//print ("TURN OFF");
					FlingMiniAttack(true);
					FlingSlowAttack(true); 
				}
				if (attackToPerform == 2)
				{
					FlingFastAttack(true); 
				}
				
			}
			
			// turn off bounciness
			this.GetComponent<SphereCollider>().material = normalPhysics; 
			
			
			CameraShakeS.C.SmallShake();
			
		}
		
		// for hit sounds
		if ((other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground")){

			if (doingSpecial){
				if (characterNum == 4){

					// destroy character
					SelfDestruct();

					
				}
			}
			
			// turn off lv3 ability
			TurnOffIgnoreWalls();
			
			// play bounce sound if not groundpounding
			if (!groundPounded){
				if (soundSource != null){
					if (other.gameObject.GetComponent<PlatformSoundS>() != null){
						other.gameObject.GetComponent<PlatformSoundS>().PlayPlatformSounds();
					}
					else{
						soundSource.PlayWallHit();
					}
					
					
				}
				// tiny shake if not attacking, bigger one if you are
				if (attacking){
					CameraShakeS.C.SmallShake();
				}
				else{
					CameraShakeS.C.MicroShake();
				}

			}
			else{
				soundSource.PlayGroundPoundHit();
				if (other.gameObject.GetComponent<PlatformSoundS>() != null){
					other.gameObject.GetComponent<PlatformSoundS>().PlayPlatformSounds();
				}

				// bigger shake
				CameraShakeS.C.SmallShake();
			}
			
			
		}
		
		if (other.gameObject.tag == "Ground"){
			//print ("HIT GROUND!!");
			if (groundDetect.Grounded()){
				attacking = false; 
				isDangerous = false; 
			}
		}

		
		
	}
	
	public void SelfDestruct(){

		UnpauseCharacter();
		doingSpecial = false;
		specialCooldown = 0;
		ownRigid.useGravity = true;
		TakeDamage(10000);

	}

	public void EndSpecialCooldown(){

		specialCooldown = 0;

	}
	
	public void DisableAttacks()
	{
		attacking = false; 
		canAirStrafe = true; 
		
		this.GetComponent<SphereCollider>().material = normalPhysics; 
		
	}
	
	public void TriggerCharge(){
		chargeSource.volume = maxChargeVolume;
		chargeSource.Play ();
	}
	
	void ManageCharge () {
		if (chargeSource.volume > 0){
			if (!charging){
				chargeSource.volume -= chargeFadeRate*Time.deltaTime*TimeManagerS.timeMult;
			}
			else{
				chargeSource.volume = maxChargeVolume;
			}
		}
		else{
			chargeSource.volume = 0;
			chargeSource.Stop();
		}
	}
	

	public void TurnOnIgnoreWalls(){
		ownRigid.useGravity = false;
		gameObject.layer = LayerMask.NameToLayer(physicsLayerNoWalls);
	}
	
	public void TurnOffIgnoreWalls(){
		//print ("USE GRAV!!");
		ownRigid.useGravity = true;
		gameObject.layer = physicsLayerDefault;
	}
	
	// for aim obj
	public float GetChargeTime(){
		return(chargeTime);
	}
	
	public float GetChargeLv2Min(){
		return (medChargeTime);
	}
	
	public float GetChargeLv3Min(){
		return (maxChargeTime);
	}

	public bool GetSpecialState(){
		return doingSpecial;
	}

	public bool GetChompState(){
		return doingChomp;
	}


	public void AddKO(){

		numKOsInRow ++;
		if (numKOsInRow >= 3 && characterNum < 7){
			if (!specialParticles){
				GameObject newParticles = Instantiate(chargingSpecialPrefab, transform.position,Quaternion.identity)
					as GameObject;
				specialParticles = newParticles;
				newParticles.transform.parent = transform;
			}
		}

	}

	public void TriggerSlow(){



		isSlowed = true;
		if (!doingSpecial){
			ownRigid.velocity *= slowMult;
			ownRigid.drag /= slowMult;
		}



	}

	public void DisableSlow(){

		isSlowed = false;
		if (!doingSpecial){
			ownRigid.velocity /= slowMult;
			ownRigid.drag *= slowMult;
		}

	}

	public bool GetSlowedState(){

		return isSlowed;

	}

	void BackToMenu(){
		// reset for festival demo purposes
		if (Input.GetButton("StartButtonPlayer"+playerNum+platformType) &&
		    Input.GetButton("AButtonPlayer"+playerNum+platformType) &&
		    Input.GetButton("BButtonPlayer"+playerNum+platformType) &&
		    Input.GetButton("XButtonPlayer"+playerNum+platformType) &&
		    Input.GetButton("YButtonPlayer"+playerNum+platformType)){
			Application.LoadLevel("1StartMenu");
		}
	}
}
