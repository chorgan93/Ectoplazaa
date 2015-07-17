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

	public int playerNum;

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
	private bool isSnapping = false;

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

	public float flingForceMult = 3f;
	public float flingForceMultLv2;
	public float flingForceMultLv3;
	public int flingLv1Cap;
	public int flingLv2Cap;
	//[HideInInspector]
	//public bool dontCorrectSpeed = false;

	public bool isDangerous = false;
	public float maxHealth = 3;
	public float health = 3;

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
	public float respawnTimeMax = 2f;
	private float respawnTimeCountdown;

	private Vector3 spawnPos;
	public GameObject spawnPt;

	
	public List<GameObject> allSpawnPts;

	// raycasting to prevent wall sticking
	private bool rightCheck;
	private bool leftCheck;
	private RaycastHit hit;

	// new (as of 2 July) attack stuff
	private bool canCharge = true;
	[HideInInspector]
	public bool charging = false;
	private float chargeTime = 0;
	private float medChargeTime = 0.4f;
	private float maxChargeTime = 1.14f;

	[HideInInspector]
	public int attackToPerform = 0;
	[HideInInspector]
	public bool attacking = false;

	//private float buttDelayCountdown;

	[HideInInspector]
	public Vector3 attackDir;

	private float attackGroundLeewayMaxTime = 0.5f;
	private float groundLeeway;

	[HideInInspector]
	public bool performedAttack = false;

	//private float lv1AttackForce = 60000f;
	//private float lv1AttackTargetRange = 12f;
	private float lv1OutRate = 5500f;
	
	private float lv2OutRate = 8000f;
	private float lv1OutTimeMax = 0.3f;
	public float lv1OutCountdown;
	private float lv1ReturnRate = 1f;
	private bool snapReturning = false;

	[HideInInspector]
	public bool dropDown = false;
	private float lv1ButtDelay = 1f;
	public float lv2FlingForce = 100;
	private float lv3BulletSpeed = 12500;
	private bool lockInPlace = false;
	private Vector3 bulletVel;

	// physics layer experimentation for attack 0 and 1
	//private int physicsLayerDefault;
	//private string physicsLayerNoWalls = "IgnoreWallCollider";
	private RaycastHit newHit;

	//[HideInInspector]
	public bool didLv2Fling = false;

	//VISUAL VARS------------------------------------------------
	public GameObject deathParticles, jumpParticles, hitParticles, groundedParticles; 
	public GameObject dangerousSprite; 

	private TrailRenderer playerTrailRenderer; 
	public GameObject tempHeadSphere,tempButtSphere; 
	public Material [] playerMats;
	public Material [] playerParticleMats; 
	public Material	 hurtMat; 

	// Use this for initialization
	void Start () {

		platformType = PlatformS.GetPlatform();

		ownRigid = GetComponent<Rigidbody>();
		spriteObjRender = spriteObject.GetComponent<SpriteRenderer>();

		playerTrailRenderer = this.GetComponent<TrailRenderer>(); 

		transform.position = spawnPos = spawnPt.transform.position;
		GetComponent<TrailHandlerS>().buttObj.transform.position = spawnPos;

		tempButtSphere.GetComponent<Renderer>().material = playerMats[playerNum-1]; 
		tempHeadSphere.GetComponent<Renderer>().material = playerMats[playerNum-1]; 

		playerTrailRenderer.material = playerMats[playerNum-1];

		//physicsLayerDefault = gameObject.layer;
	
	}


	void FixedUpdate () {



		if (!ScoreKeeperS.gameEnd){

			if (!TimeManagerS.paused){
	
				ManageDelay();
				//print (leftCheck);
	
				if (!effectPause && !respawning){
	
					
					CheckWallCast();
	
					Walk();
					Jump ();
					MiscAction(); //TRAIL RENDERER UPDATE, OTHER THINGS
	
					//Stretch();
					//Snap();
	
					ChargeAttack();
					AttackRelease();
	
				}
	
				Respawn();
	
	
				if (lockInPlace){
					//DISABLED FOR BOUNCINESS ------------------------------------------------------------------------------------------
					//ownRigid.velocity = Vector3.zero;
				}

				if (dropDown){
					Vector3 dropVel = ownRigid.velocity;
					dropVel.x = 0;
					ownRigid.velocity = dropVel;
					//print (ownRigid.velocity);
				}
	
				if (isDangerous && !respawning){
					if (!dangerObj.activeSelf){
						dangerObj.SetActive(true);
					}
				}
				else{
					dangerObj.SetActive(false);
				}
			}
	
		}
		else{
			ownRigid.velocity = Vector3.zero;
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
			    rightHit.collider.gameObject.tag != "Butt"){
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

		if (leftHit.collider != null){
			
			if (leftHit.collider.gameObject.tag != "Player" &&
			    leftHit.collider.gameObject.tag != "PlayerTrail"&&
			    leftHit.collider.gameObject.tag != "Butt"){
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
		}

		if (stretchButtonDown && !charging && canCharge){
			charging = true;
			canCharge = false;
			chargeTime = 0;

		}

		if (charging){

			//print ("charging!!");

			ownRigid.useGravity = false;
			ownRigid.velocity = Vector3.zero;

			chargeTime+=Time.deltaTime*TimeManagerS.timeMult;

			if (!stretchButtonDown){
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
					GetComponent<TrailHandlerS>().SetButtDelay(0.2f);
				}
				else if (chargeTime >= medChargeTime){
					attackToPerform = 1;
					//print ("do attack 1");
				}
				else{
					attackToPerform = 0;
					GetComponent<TrailHandlerS>().SetButtDelay(0.1f);
				}
			}
		}

	}
	void AttackRelease () {

		if (attacking){

			groundLeeway -= Time.deltaTime*TimeManagerS.timeMult;

			if (!performedAttack)
			{
				isDangerous = true;
					
				snapReturning = false;

				//buttDelayCountdown = lv1ButtDelay;

				attackDir = Vector3.zero;
				attackDir.x = Input.GetAxis("HorizontalPlayer"+playerNum+platformType);
				attackDir.y = Input.GetAxis("VerticalPlayer"+playerNum+platformType);

			}

			if (attackToPerform == 2)
			{
				FlingFastAttack(false);
			}
			if (attackToPerform == 1)
			{
				FlingSlowAttack(false);
				//print ("started slow fling");
			}
			else if( attackToPerform == 0)
			{
				//JabAttack();
				FlingMiniAttack(false);
			}
			
			performedAttack = true;

		}


	}


	void FlingMiniAttack(bool endAttack)
	{
		/*
		Just a small fling in a direction
		*/

		if(!performedAttack)
		{
			ownRigid.useGravity = false;
			
			bulletVel = attackDir.normalized*Time.deltaTime*lv1OutRate;
			ownRigid.AddForce(bulletVel*.75f,ForceMode.VelocityChange);

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
						dropDown = true;
					}
				}
			}
		}



	
	}

	void FlingSlowAttack(bool endAttack)
	{
		if(endAttack)
		{

			//print ("I ended attack");
			
			//attackToPerform = 0;
			lv1OutCountdown = 0;
			snapReturning = true;
			didLv2Fling = false;
			attacking = false;
			isDangerous = false;
			ownRigid.useGravity = true;
			//buttObj.isFollowing = true;
			canAirStrafe = true; 
			
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
					ownRigid.velocity = bulletVel*TimeManagerS.timeMult*(lv1OutCountdown/lv1OutTimeMax);
				}
				else
				{
					//butt should return
					//buttObj.isFollowing = true;
					canAirStrafe = true; 
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
			lockInPlace = true;

			attacking = false;
			isDangerous = false;
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
					
					// have butt go to head
					//buttObj.isFollowing = true;
					// lock in place so no sliding
					lockInPlace = true;
					//GetComponent<TrailHandlerS>().SetButtDelay(0.8f);
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

			//print (dontCorrectSpeed);

		// JULY 15: Trying to remove dontCorrectSpeed

			//if (dontCorrectSpeed && groundDetect.Grounded())
			//{
			//	dontCorrectSpeed = false;
			//}

			//if (!isSnapping && !stretching && !groundPounded){
			if (!charging && canAirStrafe) //&& !attacking)
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

		// turn danger off

		// allow charge attack

		if (groundDetect.Grounded()){
			// end a fling attack on ground hit
			//if (attackToPerform == 1 && attacking){
			if ((attacking || groundPounded) && groundLeeway <= 0){
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
			dropDown = false;
		}

		// turn jump ability on/off depending on grounded status

		if (jumped){
			if (groundDetect.Grounded()){
				jumped = false;
				//groundPounded = false;
			}
		}
		else{
			if (!groundDetect.Grounded()){
				jumped = true;
			}
		}

		
		// detect button up
		if (!Input.GetButton("AButtonPlayer" + playerNum + platformType)){
			jumpButtonDown = false;
		}


		// detect button press, do jump
		//if (!isSnapping && !stretching){
		//if (!attacking && !charging){
			if (Input.GetButton("AButtonPlayer" + playerNum + platformType) && !jumpButtonDown){
	
				//print ("Jump!");
	
				jumpButtonDown = true;
			
				if (!jumped){

				// don't do regular jump while attacking or charging
				if (!attacking && !charging){
						Vector3 jumpForce = Vector3.zero;
					
						jumpForce.y = jumpSpeed*Time.deltaTime*TimeManagerS.timeMult;
			
						ownRigid.AddForce(jumpForce);
			
						Instantiate(jumpParticles, this.transform.position, Quaternion.identity); 
	
						addingJumpTime = 0;
						stopAddingJump = false;
						jumped = true;
					}
				}
				else{
					// do ground pound if not charging
					if (!groundPounded && !charging){
						Vector3 groundPoundVel = Vector3.zero;
						groundPoundVel.y = -groundPoundForce*Time.deltaTime*TimeManagerS.timeMult;
						ownRigid.velocity = groundPoundVel;
						isDangerous = true;
						groundPounded = true;

						// allow for air control
						//dontCorrectSpeed = false;

						//SleepTime(groundPoundPauseTime);
					}
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
		//}
	}

	void MiscAction()
	{
		//this.GetComponent<LineRenderer>().SetPosition(0,this.transform.position);
		//this.GetComponent<LineRenderer>().SetPosition(1,buttObj.transform.position);

		if(isDangerous)
			dangerousSprite.GetComponent<SpriteRenderer>().enabled = true; 
		else
			dangerousSprite.GetComponent<SpriteRenderer>().enabled = false; 

	}

	void Stretch () {

		if (!isSnapping){

			// turn stretch button bool on/off
			if (Input.GetAxis("RightTriggerPlayer" + playerNum + platformType) > triggerSensitivity){
				stretchButtonDown = true;
			}
			else{
				stretchButtonDown = false;
			}
	
			// when not stretching, check for stretch
			if (!stretching){
	
				if (groundDetect.Grounded()){
					canStretch = true;
					isSnapping = false;
				}
	
				// start stretch
				if (stretchButtonDown && canStretch && groundDetect.Grounded()){
					stretching = true;
					canStretch = false;

					//buttObj.transform.position = transform.position;

					placeDotCountdown = placeDotCountdownMax;
				}
			}
			else{
				if (!stretchButtonDown){
					stretching = false;
					if (movePositions.Count > 0){
						isSnapping = true;
					}
					currentTarget = 0;
				}
	
				Vector3 stretchVel = Vector3.zero;
				stretchVel.x = Input.GetAxis("HorizontalPlayer" + playerNum + platformType);
				stretchVel.y = Input.GetAxis("VerticalPlayer" + playerNum + platformType);
				stretchVel = stretchVel.normalized;
				stretchVel *= stretchSpeed*TimeManagerS.timeMult*Time.deltaTime;

				ownRigid.velocity = stretchVel;

				if (stretchVel != Vector3.zero){
					placeDotCountdown -= Time.deltaTime*TimeManagerS.timeMult;
				}

				// place movement positions
				if (placeDotCountdown <= 0){
					placeDotCountdown = placeDotCountdownMax;

					GameObject placeDot = Instantiate(placeDotPrefab, transform.position, Quaternion.identity)
						as GameObject;

					// change size of dot for now to indicate future power of fling
					if (placedDots.Count-1 > flingLv2Cap){
						placeDot.transform.localScale *= 1.25f;
					}
					// if inbetween, keep same size
					if (placedDots.Count-1 < flingLv1Cap){
						placeDot.transform.localScale *= 0.75f;
					}
					
					placeDot.GetComponent<DotColliderS>().whoCreatedMe = this;
					
					placedDots.Add(placeDot);
					movePositions.Add(transform.position);
				}
	
			}
		}


	}

	void Snap () {

		if (isSnapping){

			ownRigid.velocity = Vector3.zero;


			// check for fling
			if (Input.GetAxis("RightTriggerPlayer"+playerNum+platformType) >= 1){
				doFling = true;
			}
			
			
			
			moveToNextPosCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			
			if (moveToNextPosCountdown <= 0){
				//print (currentTarget);
				if (placedDots.Count >= currentTarget-1){
					Destroy(placedDots[currentTarget]);
				}
				currentTarget++;
				
				if (currentTarget > movePositions.Count-1){
					isSnapping = false;
					isDangerous = true;
					//buttObjRigid.velocity = Vector3.zero;

					int numDots = movePositions.Count;

					movePositions.Clear();
					placedDots.Clear();
					currentTarget = 0;

					//rigidbody.useGravity = true;
					if (doFling){
						
						snapVel.z = 0;

						//snapVel.x = Input.GetAxis("HorizontalPlayer"+playerNum+platformType);
						//snapVel.y = Input.GetAxis("VerticalPlayer"+playerNum+platformType);

						snapVel = snapVel.normalized;

						if (snapVel != Vector3.zero){
						
							if (numDots > flingLv2Cap){
								snapVel*=flingForceMultLv3*Time.deltaTime;
							}
							else if (numDots > flingLv1Cap){
								snapVel*=flingForceMultLv2*Time.deltaTime;
							}
							else{
								snapVel*=flingForceMult*Time.deltaTime;
							}

							
						}
						else{
							// CLEAN UP after confirmation of working code
							snapVel= GetComponent<TrailHandlerS>().buttObj.
								GetComponent<Rigidbody>().velocity.normalized*flingForceMult*Time.deltaTime;
						}
						
						//CameraShakeS.C.TimeSleep(0.1f);
						//capturedVel = snapVel;
						//capturedGravity = true;
						//attackDelay = attackDelayMax;


					}
					else{
						snapVel= GetComponent<TrailHandlerS>().buttObj.
							GetComponent<Rigidbody>().velocity;
					}
					prevVel = ownRigid.velocity = snapVel;
					//buttObjRigid.velocity = Vector3.zero;

					//dontCorrectSpeed = true;

					if (doFling){
						if (numDots > flingLv2Cap){
							SleepTime(flingPauseTime*1.25f);
						}
						else if (numDots > flingLv1Cap){
							SleepTime(flingPauseTime);
						}
						else{
							SleepTime(flingPauseTime*0.75f);
						}
						
						doFling = false;
					}

				}
				else{
					
					Vector3 snapDir = (movePositions[currentTarget]-GetComponent<TrailHandlerS>().buttObj.transform.position);
					
					snapVel = snapDir/(placeDotCountdownMax/snapSpeedMult);
					
				}
				
				moveToNextPosCountdown = placeDotCountdownMax/snapSpeedMult;
			}
			
			
			//buttObjRigid.velocity = snapVel*TimeManagerS.timeMult;
			
			
		
		}

	}

	void Respawn () {

		if (respawning){
			respawnTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			ownRigid.useGravity = false;
			ownRigid.velocity = Vector3.zero;
			dangerObj.SetActive(false);

			GetComponent<Collider>().enabled = false;

			stretching = false;
			isSnapping = false;
			isDangerous = false;
			jumpButtonDown = false;
			stretchButtonDown = false;
			charging = false;
			attacking = false;
			chargeTime = 0;

			//if (placedDots

			if (!effectPause){
				spriteObjRender.enabled = false;
			}
			if (respawnTimeCountdown <= 0){
				GetComponent<Collider>().enabled = true;
				spriteObjRender.enabled = true;
				respawning = false;
				transform.position = spawnPos;
				GetComponent<TrailHandlerS>().buttObj.transform.position = spawnPos;
				GetComponent<TrailHandlerS>().ClearTrail();
				ownRigid.useGravity = true;
				
				health = maxHealth;
			}
		}

	}

	public void TakeDamage(float dmg){
		health -= dmg;
		if (health <= 0){
			respawning = true;
			respawnTimeCountdown = respawnTimeMax;

			Instantiate(deathParticles, this.transform.position, Quaternion.identity); 
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

		Vector3 hitParticleSpawn = this.transform.position; 
		hitParticleSpawn.z +=.5f;
		Instantiate(hitParticles,hitParticleSpawn,Quaternion.identity);

		if (attacking &&
		    (other.gameObject.tag == "Ground" || other.gameObject.tag == "Wall") &&groundLeeway <= 0 ){
			//attacking = false;
			//print ("STOP!!");

			//DISABLED FOR BOUNCINESS ------------------------------------------------------------------------------------------
			//ownRigid.velocity = Vector3.zero;


			//buttDelayCountdown = 0;

			if (attackToPerform == 0)
			{
				//JabAttack();
				FlingMiniAttack(true);
			}
			if (attackToPerform == 1)
			{
				FlingSlowAttack(true); 
			}
			if (attackToPerform == 2)
			{
				FlingFastAttack(true); 
			}

			CameraShakeS.C.MicroShake();
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
