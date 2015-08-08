using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerS : MonoBehaviour {

	private string platformType;
	private Rigidbody ownRigid;


	public GameObject spriteObject;
	private SpriteRenderer spriteObjRender;

	//public ButtObjS buttObj;
	//private Rigidbody buttObjRigid;

	public int playerNum; //controller, playernumber for UI 
	public int characterNum; //skin, character chosen;

	public int score;


	public float walkSpeed;
	public float maxSpeed;
	public float airControlMult;
	private bool canAirStrafe = true; 

 	//private bool isBouncy = false; 
	public PhysicMaterial bouncyPhysics, normalPhysics; 

	public float jumpSpeed;
	private bool jumped = false;
	private bool jumpButtonDown = false;
	public float addJumpForce;
	public float addJumpForceMaxTime;
	private float addingJumpTime;
	private bool stopAddingJump;

	public GroundDetectS groundDetect;

	private bool canStretch = true;
	private bool stretching = false;
	private bool stretchButtonDown = false;
	private float triggerSensitivity = 0.5f;

	private bool doFling = false;

	public GameObject placeDotPrefab;
	
	public List<Vector3> movePositions;
	public List<GameObject> placedDots;

	private float moveToNextPosCountdown = 0;
	private int currentTarget = 0;

	private float placeDotCountdownMax = 0.1f;
	private float placeDotCountdown;

	public float stretchSpeed;
	public float snapSpeedMult;
	private Vector3 snapVel;

	//private float minFlingForce = 500f;
	int flingsLeft = 2; 
	public float flingForceMult = 3f;
	public float flingForceMultLv2;
	public float flingForceMultLv3;
	public int flingLv1Cap;
	public int flingLv2Cap;
	//[HideInInspector]
	//public bool dontCorrectSpeed = false;

	public bool isDangerous = false;
	public float maxHealth = 50;
	public float initialHealth = 10;
	public float health = 10;

	public bool facingRight = false;

	private bool groundPounded = false;
	public float groundPoundForce;

	private float pauseDelay;
	//private bool prevGravState;
	private Vector3 prevVel;
	private Vector3 prevButtVel;
	[HideInInspector]
	public bool effectPause = false;

	//private float groundPoundPauseTime = 0.3f;
	private float flingPauseTime = 0.45f;


	public GameObject dangerObj;
	public bool respawning = false;
	public float respawnTimeMax = 1f;
	private float respawnTimeCountdown;

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
	private float medChargeTime = 0.45f;
	private float maxChargeTime = 1f;

	[HideInInspector]
	public int attackToPerform = 0;
	[HideInInspector]
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

	//private float lv1AttackForce = 60000f;
	//private float lv1AttackTargetRange = 12f;
	private float lv1OutRate = 3350f;
	
	private float lv2OutRate = 12000f; //original 8000
	private float lv1OutTimeMax = 0.2f;
	public float lv1OutCountdown;
	private float lv1ReturnRate = 1f;
	private bool snapReturning = false;

	[HideInInspector]
	private float lv1ButtDelay = 1f;
	public float lv2FlingForce = 100;
	private float lv3BulletSpeed = 12500; //was 12500
	private bool lockInPlace = false;
	private Vector3 bulletVel;

	private float lv2AttackPauseTimeMax = 0.06f;
	private float lv2AttackPauseCountdown;
	private bool startedLv2Pause = false;
	private float lv2AttackTimeMax = 0.1f;
	private float lv2AttackTimeCountdown;

	// physics layer experimentation for attack 0 and 1
	//private int physicsLayerDefault;
	//private string physicsLayerNoWalls = "IgnoreWallCollider";
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
	GameObject chargingParticles;

	public GameObject initialSpawnParticlesPrefab; 
	GameObject initialSpawnParticles; 
	bool isSpawning; 

	private AudioSource chargeSource;
	private float currentChargeVolume;
	private float maxChargeVolume = 1;
	private float chargeFadeRate = 2f;

	// Use this for initialization
	void Start () {



	

		platformType = PlatformS.GetPlatform();

		ownRigid = GetComponent<Rigidbody>();
		spriteObjRender = spriteObject.GetComponent<SpriteRenderer>();


		allSpawnPts = GameObject.FindGameObjectsWithTag("Spawn");

		//physicsLayerDefault = gameObject.layer;

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

	}


	void FixedUpdate () {

		ManageCharge();


		if (!ScoreKeeperS.gameEnd && ScoreKeeperS.gameStarted) {

			// trying to stop a stop moving bug
			transform.rotation = Quaternion.identity;

			if (!TimeManagerS.paused) {

				respawnInvulnTime -= Time.deltaTime * TimeManagerS.timeMult;
	
				ManageDelay ();
				//print (leftCheck);
	
				if (!effectPause && !respawning) {
	
					
					CheckWallCast ();
	
					Walk ();
					Jump ();

					//WallJump ();

	
	
					ChargeAttack ();
					AttackRelease ();
	
				}

				MiscAction (); //TRAIL RENDERER UPDATE, OTHER THINGS
	
				Respawn ();
	
				/*
				if (isDangerous && !respawning){
					if (!dangerObj.activeSelf){
						dangerObj.SetActive(true);
					}
				}
				else{
					dangerObj.SetActive(false);
				}
				*/
			}
	
		} else {
			ownRigid.velocity = Vector3.zero;
		}


		if(Input.GetKeyDown(KeyCode.K))
		{
				//TakeDamage(100000f);
		}
	
	}

	void CheckWallCast(){

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
			    rightHit.collider.gameObject.tag != "Glob"){
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
			    leftHit.collider.gameObject.tag != "Glob"){
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
		if (effectPause){

			ownRigid.velocity = Vector3.zero;
		//	buttObjRigid.velocity = Vector3.zero;

			pauseDelay -= Time.deltaTime*TimeManagerS.timeMult;

			ownRigid.useGravity = false;

			if (pauseDelay <= 0){
				effectPause = false;
				ownRigid.velocity = prevVel;
				//ownRigid.useGravity = prevGravState;
				//buttObjRigid.velocity = prevButtVel;
			}
		}
	}

	/*
	_________________________________________________________________________________________________________________________________________________________________________________________________________________________________

	ATTACKING-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	_____________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________

	*/

	void ChargeAttack () {

		// turn stretch button bool on/off
		if (Input.GetAxis("RightTriggerPlayer" + playerNum + platformType) > triggerSensitivity){
			stretchButtonDown = true;
		}
		else{
			stretchButtonDown = false;
			playedLV1ChargeSound = false;
			playedLV2ChargeSound = false;
			playedLV3ChargeSound = false;
		}

		if (stretchButtonDown && !charging && canCharge && !isDangerous){
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
			}

			groundLeeway = 0.5f;

			//print ("charging!!");

			ownRigid.useGravity = false;
			ownRigid.velocity = Vector3.zero;

			chargeTime+=Time.deltaTime*TimeManagerS.timeMult;

			// play sounds when you hit each threshold
			if (chargeTime > 0 && !playedLV1ChargeSound){
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



			if (!stretchButtonDown){
				GameObject.Destroy( chargingParticles.gameObject); 

				groundLeeway = attackGroundLeewayMaxTime;
				attacking = true;
				charging = false;
				performedAttack = false;

				// allow for ground pound
				jumped = true;

				// butt should not follow
				//buttObj.isFollowing = false;

				if (chargeTime >= maxChargeTime){
					// fully charged attack
					attackToPerform = 2;
					attackPriority = 2;
				}
				else if (chargeTime >= medChargeTime){
					attackToPerform = 1;
					attackPriority = 1;
					//print ("do attack 1");
				}
				else{
					attackToPerform = 0;
					attackPriority = 0;
				}
			}
		}

	}
	void AttackRelease () {

		if (attacking) {

			groundLeeway -= Time.deltaTime * TimeManagerS.timeMult;

			if (!performedAttack) 
			{
				isDangerous = true;
					
				snapReturning = false;

				//buttDelayCountdown = lv1ButtDelay;



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

			} else if (attackToPerform == 0) {
				//JabAttack();
				FlingMiniAttack (false);

			}
			
			performedAttack = true;

		} 
		else 
		{
			attackPriority = 0; 
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

			bulletVel = attackDir.normalized*Time.deltaTime*lv1OutRate ;
			ownRigid.AddForce(bulletVel* (1+chargeTime),ForceMode.VelocityChange);

			//print(chargeTime); 

			//dontCorrectSpeed = true;
			ownRigid.useGravity = true;
			//buttObj.isFollowing = true;
			//print ("Fling Mini Attack");

			//print (bulletVel); 
		}
		else
		{

		}

		if(endAttack)
		{
			// trigger return early
			isDangerous = false;
			attacking = false;
			//snapReturning = true;
		}

	}
	
	void JabAttack(bool endAttack)
	{
		if(endAttack)
		{
			
		}
		else
		{
			if(!performedAttack)
			{
				ownRigid.useGravity = false;


				
				// set start snap vel
				bulletVel = attackDir.normalized*Time.deltaTime*lv1OutRate;
				
				print (bulletVel); 
				
				lv1OutCountdown = lv1OutTimeMax;
				
			}
			else
			{
				print ("JAB ATTACK!");
				// lerp out to attack target and then back to butt poss
				if (lv1OutCountdown > 0){
					
					lv1OutCountdown -= Time.deltaTime*TimeManagerS.timeMult;
					//print (lv1OutCountdown);
					
					if (!snapReturning){
						
						// vel decrease over time (linear, can be made exponential)
						
						// vel decrease over time (linear, can be made exponential)
						ownRigid.velocity = bulletVel*TimeManagerS.timeMult*(lv1OutCountdown/lv1OutTimeMax);
					}
					else{
						ownRigid.velocity = -bulletVel*TimeManagerS.timeMult*(lv1OutCountdown/
						                                                      (lv1OutTimeMax/lv1ReturnRate))
							*lv1ReturnRate;
					}
				}
				else{
					
					// reverse direction & accel
					if (!snapReturning){
						snapReturning = true;
						lv1OutCountdown = lv1OutTimeMax/lv1ReturnRate;
					}
					// else end attack 1
					else{
						//print ("USE GRAV");
						ownRigid.useGravity = true;
						isDangerous = false;
						attacking = false;
						TurnOffIgnoreWalls();
						//buttObj.isFollowing = true;
						
						// stop vel
						ownRigid.velocity = Vector3.zero;
					}
				}
			}
		}



	
	}

	void FlingSlowAttack(bool endAttack)
	{

		// this is lv 2 attack
		if(endAttack)
		{

			//print ("I ended attack");
			
			//attackToPerform = 0;
			lv1OutCountdown = 0;
			snapReturning = true;
			didLv2Fling = true;
			//attacking = false;
			//isDangerous = false;
			ownRigid.useGravity = true;
			//buttObj.isFollowing = true;
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
			chargeTime = medChargeTime*1.35f;
			attackToPerform = 0;
			FlingMiniAttack(false);

			soundSource.PlayChargeLv1();
			
			//print ("part 2!");
			
		}
		else
		{
			if(!performedAttack)
			{

				//print ("Start slow fling");

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

						//print ("YEAYAEYEYYA");

						//canAirStrafe = true; 
						startedLv2Pause = true;
						ownRigid.velocity = Vector3.zero;
						
						// allows time for tail to catch up
						lv2AttackPauseCountdown = lv2AttackPauseTimeMax;
						//ownRigid.velocity = Vector3.zero;
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

		if(endAttack)
		{
			// have butt go to head
			//buttObj.isFollowing = true;
			// lock in place so no sliding

			//attacking = false;
			//isDangerous = false;
			ownRigid.useGravity = true;
			//buttObj.isFollowing = true;
			canAirStrafe = true; 

			this.GetComponent<SphereCollider>().material = normalPhysics; 

		}
		else
		{
			if(!performedAttack)
			{
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
					ownRigid.AddForce(bulletVel,ForceMode.VelocityChange);
					
					ownRigid.useGravity = true;
				}
				else{
					
					//print ("DONT ATTACK THRU WALL");
					

					// set attack to 2 so butt behaves properly
					attackToPerform = 2;

				}
				
				// add kinesthetic effects
				//CameraShakeS.C.MicroShake();
				//	SleepTime(0.3f);
				
				// SEEMS TO BE WORKING CORRECTLY
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

		if (!groundDetect.Grounded()){
			groundLeeway = 0;
		}
		else{
			canAirStrafe = true;
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
		
					// only add force if not flinging/hit
					
					//THIS WAS DISABLING AIR STRAFE WHILE FLINGING
					/*if (!dontCorrectSpeed){
						ownRigid.AddForce(new Vector3(xForce,0,0));
					}*/
			
					bool applyForce = true; 
	
					if((ownRigid.velocity.x > maxSpeed) && (xForce > 0))
					{
							applyForce = false; 
					//print ("Dont apply force!");
					}
					else if((ownRigid.velocity.x < -maxSpeed) && (xForce < 0))
					{
					applyForce = false; 
					//print ("Dont apply force!");
					}
	
					if(applyForce)
						ownRigid.AddForce(new Vector3(xForce,0,0));
	
	
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
			//isDangerous = false;
			canCharge = true;
		}

		// turn jump ability on/off depending on grounded status

		if (jumped){
			if (groundDetect.Grounded()){
				jumped = false;
				if (groundPounded){
					groundPounded = false;
					isDangerous = false;
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
		}



		if (Input.GetButton("AButtonPlayer" + playerNum + platformType) && !jumpButtonDown)
		{

	
				//print ("Jump!");
	
			jumpButtonDown = true;
			
			if (!jumped)
			{

				// don't do regular jump while attacking or charging
				if (!attacking && !charging)
				{
						Vector3 jumpForce = Vector3.zero;
					
						jumpForce.y = jumpSpeed*Time.deltaTime*TimeManagerS.timeMult;
			
						ownRigid.AddForce(jumpForce);
			
						Instantiate(jumpParticles, this.transform.position, Quaternion.identity); 

					// play jump sound
					soundSource.PlayJumpSound();
	
						addingJumpTime = 0;
						stopAddingJump = false;
						jumped = true;
				}
			}
		
			// do ground pound if not charging
			else if (!groundPounded && !charging && !clingingToWall){
				Vector3 groundPoundVel = Vector3.zero;
				groundPoundVel.y = -groundPoundForce*Time.deltaTime*TimeManagerS.timeMult;
				ownRigid.velocity = groundPoundVel;
				isDangerous = true;
				groundPounded = true;
				attackPriority = 0;

				//print ("Groundpound!");
				// play an attack sound
				//soundSource.PlayChargeLv2();
				soundSource.PlayGroundPoundReleaseSound();

				// allow for air control
				//dontCorrectSpeed = false;

				//SleepTime(groundPoundPauseTime);
			}
			else{
				// don't do anything
			}
		}
		
	

		// add addtl jump force
		if (jumped){
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

	void WallJump () {

		// allows player to slow descent and jump when "clinging" to a wall

		if (!groundDetect.Grounded()){

		// first, check if player is touching a wall on either side
		if (leftWallDetect.WallTouching()){

			// trigger "cling" if player is tilting stick towards wall
			if (Input.GetAxis("HorizontalPlayer" + playerNum + platformType) < 0){

				clingingToWall = true;


			}

		}

		if (rightWallDetect.WallTouching()){

			// trigger "cling" if player is tilting stick towards wall
			if (Input.GetAxis("HorizontalPlayer" + playerNum + platformType) > 0){
				
				clingingToWall = true;
				
				
			}
			
		}

		if (clingingToWall){

			// start applying cling force against gravity
			Vector3 clingForce = Vector3.zero;
			clingForce.y = wallDragForce*Time.deltaTime*TimeManagerS.timeMult;
			ownRigid.AddForce(clingForce);
			
			// check for button press to trigger wall jump
			if (Input.GetButton("AButtonPlayer"+playerNum+platformType)){
				Vector3 jumpForce = Vector3.zero;

				// apply x force in opposite direction of wall
				if (leftWallDetect.WallTouching()){
					jumpForce.x = wallJumpXForce;
				}
				if (rightWallDetect.WallTouching()){
					jumpForce.x = -wallJumpXForce;
				}

				// apply normal y jump force
				jumpForce.y = jumpSpeed;

				jumpForce *= Time.deltaTime;

				ownRigid.AddForce(jumpForce);

				clingingToWall = false;
			}
		}
		}
		else{
			clingingToWall = false;
		}


	}

	void MiscAction()
	{
		//this.GetComponent<LineRenderer>().SetPosition(0,this.transform.position);
		//this.GetComponent<LineRenderer>().SetPosition(1,buttObj.transform.position);

		if(isDangerous)
			dangerousSprite.GetComponent<SpriteRenderer>().enabled = true; 
		else
			dangerousSprite.GetComponent<SpriteRenderer>().enabled = false; 

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
			groundLeeway = 0;
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
		spriteObject.GetComponent<PlayerAnimS> ().SetCurrentSprites (characterNum);
		GetComponent<TrailHandlerRedubS> ().SetDotMaterial ();


	}

	void Respawn () {

		if (respawning){
			if(respawnTimeCountdown == respawnTimeMax)
			{
				if(chargingParticles!= null)
				{
					GameObject.Destroy(chargingParticles.gameObject); 
				}
				respawnParticles = Instantiate(respawnParticlePrefab, this.transform.position, Quaternion.identity) as GameObject;
				respawnParticles.GetComponent<ParticleSystem>().startColor = playerParticleMats[characterNum - 1].GetColor("_TintColor");
				respawnParticles.GetComponent<ParticleSystem>().startLifetime = respawnTimeCountdown;

				deathPos = this.transform.position; 

				//move player to new respawn position right away so trail renderer has time to update.
				int randomSpawn = Random.Range(0, allSpawnPts.Length-1);
				GameObject newSpawn = allSpawnPts[randomSpawn];
				transform.position = newSpawn.transform.position;

				trailRendererGO.GetComponent<TrailRenderer>().enabled = false ;
				trailRendererGO2.GetComponent<TrailRenderer>().enabled = false ;
			}


			respawnParticles.transform.position = Vector3.Lerp(deathPos, this.transform.position, respawnParticles.GetComponent<ParticleSystem>().time/respawnTimeMax); 


			respawnTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			ownRigid.useGravity = false;
			ownRigid.velocity = Vector3.zero;
			dangerObj.SetActive(false);

			GetComponent<Collider>().enabled = false;

			stretching = false;
			isDangerous = false;
			jumpButtonDown = false;
			stretchButtonDown = false;
			charging = false;
			attacking = false;
			chargeTime = 0;

			canAirStrafe = true;

			//if (placedDots

			if (!effectPause){
				spriteObjRender.enabled = false;
			}
			if (respawnTimeCountdown <= 0){
				Instantiate(spawnParticlePrefab,this.transform.position, Quaternion.identity); 

				GetComponent<Collider>().enabled = true;
				spriteObjRender.enabled = true;
				respawning = false;

				respawnInvulnTime = 1.5f;

				ownRigid.useGravity = true;
				trailRendererGO.GetComponent<TrailRenderer>().enabled = true ;
				trailRendererGO2.GetComponent<TrailRenderer>().enabled = true ;
				health = initialHealth;

				DisableAttacks(); 

				soundSource.PlayCharIntroSound();
			}
		}

	}

	public void TakeDamage(float dmg){
		health -= dmg;
		hurtCounter = hurtTimer; 

		if (health <= 0){
			GetComponent<TrailHandlerRedubS>().DestroyPlayerDots(); 

			respawning = true;
			respawnTimeCountdown = respawnTimeMax;

			Instantiate(deathParticles, this.transform.position, Quaternion.identity); 
			soundSource.PlayDeathSounds();


		}
	}

	public void SleepTime(float delayTime){
		//pauseDelay = delayTime;
		//prevGravState = ownRigid.useGravity;
		//prevVel = ownRigid.velocity;
		//prevButtVel = buttObjRigid.velocity;
		//effectPause = true;

		//print ("PAUSE");
	}

	void OnCollisionEnter(Collision other){
		// end bullet attack

		if(isDangerous)
			dangerObj.GetComponent<DamageS> ().ManageCollision (other.gameObject); 


		Vector3 hitParticleSpawn = this.transform.position; 
		//hitParticleSpawn.z +=.5f;


		GameObject newParticles =  Instantiate(hitParticles,hitParticleSpawn,Quaternion.identity) as GameObject;
		newParticles.GetComponent<ParticleSystem>().startColor = playerParticleMats[characterNum - 1].GetColor("_TintColor");



		if (attacking &&groundLeeway <= 0 ){
			//attacking = false;
			//print ("STOP!!");

			//DISABLED FOR BOUNCINESS ------------------------------------------------------------------------------------------
			//ownRigid.velocity = Vector3.zero;


			//buttDelayCountdown = 0;
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


			//CameraShakeS.C.SmallShake();

		}

		// for hit sounds
		if ((other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground")){
			
			// play bounce sound if not groundpounding
			if (!groundPounded){
				if (soundSource != null){
					if (other.gameObject.GetComponent<PlatformSoundS>() != null){
						other.gameObject.GetComponent<PlatformSoundS>().PlayPlatformSounds();
						print ("Played platform sound");
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

			//print ("played wall sound");
			}
			else{
				soundSource.PlayGroundPoundHit();
				if (other.gameObject.GetComponent<PlatformSoundS>() != null){
					other.gameObject.GetComponent<PlatformSoundS>().PlayPlatformSounds();
				}
				//print ("played ground sound");
				// bigger shake
				CameraShakeS.C.SmallShake();
			}


		}

		if (other.gameObject.tag == "Ground"){
			if (groundDetect.Grounded()){
				attacking = false; 
				isDangerous = false; 
			}
		}

		//DisableAttacks (); 


	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag != "Debris") {
			if (isDangerous) {
				dangerObj.GetComponent<DamageS> ().ManageCollision (other.gameObject); 
				//DisableAttacks(); 

			}
		}

		//attacking = false; 
		//isDangerous = false; 
		//this.GetComponent<SphereCollider>().material = normalPhysics; 

	}

	public void DisableAttacks()
	{
		attacking = false; 
		isDangerous = false; 

		ownRigid.useGravity = true;
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


	public void UnlockVel () {
		lockInPlace = false;
	}

	public void TurnOnIgnoreWalls(){
		//gameObject.layer = LayerMask.NameToLayer(physicsLayerNoWalls);
	}

	public void TurnOffIgnoreWalls(){
		//gameObject.layer = physicsLayerDefault;
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
}
