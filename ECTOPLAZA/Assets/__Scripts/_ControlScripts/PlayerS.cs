using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerS : MonoBehaviour {

	private string platformType;
	private Rigidbody ownRigid;

	public GameObject spriteObject;
	private SpriteRenderer spriteObjRender;

	public ButtObjS buttObj;
	private Rigidbody buttObjRigid;

	public int playerNum;

	public float walkSpeed;
	public float maxSpeed;
	public float airControlMult;

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
	private bool dontCorrectSpeed = false;

	public bool isDangerous = false;
	public float maxHealth = 3;
	public float health = 3;

	public bool facingRight = false;

	private bool groundPounded = false;
	public float groundPoundForce;

	private float pauseDelay;
	private bool prevGravState;
	private Vector3 prevVel;
	private Vector3 prevButtVel;
	private bool effectPause = false;

	private float groundPoundPauseTime = 0.3f;
	private float flingPauseTime = 0.45f;

	public GameObject dangerObj;
	public bool respawning = false;
	public float respawnTimeMax = 2f;
	private float respawnTimeCountdown;

	private Vector3 spawnPos;
	public GameObject spawnPt;

	// raycasting to prevent wall sticking
	private bool rightCheck;
	private bool leftCheck;
	private RaycastHit hit;


	// Use this for initialization
	void Start () {

		platformType = PlatformS.GetPlatform();

		ownRigid = GetComponent<Rigidbody>();
		spriteObjRender = spriteObject.GetComponent<SpriteRenderer>();

		buttObjRigid = buttObj.GetComponent<Rigidbody>();

		transform.position = spawnPos = spawnPt.transform.position;
	
	}


	void FixedUpdate () {

		if (!TimeManagerS.paused){

			ManageDelay();
			//print (leftCheck);

			if (!effectPause && !respawning){

				
				CheckWallCast();

				Walk();
				Jump ();
				Stretch();
				Snap();

			}

			Respawn();

			if (isDangerous && !respawning){
				if (!dangerObj.activeSelf){
					dangerObj.SetActive(true);
				}
			}
			else{
				dangerObj.SetActive(false);
			}
		}

		ManageFollow();
		ManageGravity ();

	
	}

	void CheckWallCast(){

		RaycastHit rightHit;
		RaycastHit leftHit;

		Physics.Raycast(transform.position,Vector3.right, out rightHit, 5f);
		Physics.Raycast(transform.position,-Vector3.right, out leftHit, 5f);

		if (rightHit.collider != null){

			//print (rightHit.collider.name);
		
			if (rightHit.collider.gameObject.tag != "Player" &&
			    rightHit.collider.gameObject.tag != "PlayerTrail"){
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
			
			if (leftHit.collider.gameObject.tag != "Player" ||
			    leftHit.collider.gameObject.tag != "PlayerTrail"){
				leftCheck = true;
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
			buttObjRigid.velocity = Vector3.zero;

			pauseDelay -= Time.deltaTime*TimeManagerS.timeMult;

			ownRigid.useGravity = false;

			if (pauseDelay <= 0){
				effectPause = false;
				ownRigid.velocity = prevVel;
				ownRigid.useGravity = prevGravState;
				buttObjRigid.velocity = prevButtVel;
			}
		}
	}

	void Walk () {

		if (dontCorrectSpeed && groundDetect.Grounded()){
			dontCorrectSpeed = false;
		}

		if (!isSnapping && !stretching && !groundPounded){

			float xForce = Input.GetAxis("HorizontalPlayer" + playerNum + platformType);
			//float xForce = Input.GetAxis("Horizontal");
			xForce *= walkSpeed*TimeManagerS.timeMult*Time.deltaTime;

			if (!groundDetect.Grounded()){
				xForce*=airControlMult;
			}

			// make sure not to do force stuff when up against wall
			if ((xForce > 0 && !rightCheck) || xForce < 0 && !leftCheck){
	
			// only add force if not flinging/hit
			if (!dontCorrectSpeed){
				ownRigid.AddForce(new Vector3(xForce,0,0));
			}
	
			Vector3 fixVel = ownRigid.velocity;
	
			if (!dontCorrectSpeed){

				if (fixVel.x < -maxSpeed){
					fixVel.x = -maxSpeed;
				}
				if (fixVel.x > maxSpeed){
					fixVel.x = maxSpeed;
				}
			}

			if (fixVel.x > 0){
				facingRight = true;
			}
			if (fixVel.x < 0){
				facingRight = false;
			}
	
			ownRigid.velocity = fixVel;
			}
	
			//print (Input.GetAxis("Horizontal"));

		}


	}

	void Jump () {

		// turn danger off
		if (groundDetect.Grounded()){
			isDangerous = false;
		}

		// turn jump ability on/off depending on grounded status

		if (jumped){
			if (groundDetect.Grounded()){
				jumped = false;
				groundPounded = false;
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
		if (!isSnapping && !stretching){
			if (Input.GetButton("AButtonPlayer" + playerNum + platformType) && !jumpButtonDown){
	
				//print ("Jump!");
	
				jumpButtonDown = true;
			
				if (!jumped){
					Vector3 jumpForce = Vector3.zero;
				
					jumpForce.y = jumpSpeed*Time.deltaTime*TimeManagerS.timeMult;
		
					ownRigid.AddForce(jumpForce);
		
					addingJumpTime = 0;
					stopAddingJump = false;
					jumped = true;
				}
				else{
					// do ground pound
					if (!groundPounded){
						Vector3 groundPoundVel = Vector3.zero;
						groundPoundVel.y = -groundPoundForce*Time.deltaTime*TimeManagerS.timeMult;
						ownRigid.velocity = groundPoundVel;
						isDangerous = true;
						groundPounded = true;

						// allow for air control
						dontCorrectSpeed = false;

						SleepTime(groundPoundPauseTime);
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
		}
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

					buttObj.transform.position = transform.position;

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
							snapVel= buttObjRigid.velocity.normalized*flingForceMult*Time.deltaTime;
						}
						
						//CameraShakeS.C.TimeSleep(0.1f);
						//capturedVel = snapVel;
						//capturedGravity = true;
						//attackDelay = attackDelayMax;


					}
					else{
						snapVel = buttObjRigid.velocity;
					}
					prevVel = ownRigid.velocity = snapVel;
					buttObjRigid.velocity = Vector3.zero;

					dontCorrectSpeed = true;

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
					
					Vector3 snapDir = (movePositions[currentTarget]-buttObj.transform.position);
					
					snapVel = snapDir/(placeDotCountdownMax/snapSpeedMult);
					
				}
				
				moveToNextPosCountdown = placeDotCountdownMax/snapSpeedMult;
			}
			
			
			buttObjRigid.velocity = snapVel*TimeManagerS.timeMult;
			
			
		
		}

	}

	void Respawn () {

		if (respawning){
			respawnTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			ownRigid.useGravity = false;
			ownRigid.velocity = Vector3.zero;
			dangerObj.SetActive(false);
			if (!effectPause){
				spriteObjRender.enabled = false;
			}
			if (respawnTimeCountdown <= 0){
				spriteObjRender.enabled = true;
				respawning = false;
				transform.position = spawnPos;
				ownRigid.useGravity = true;
			}
		}

	}

	void ManageFollow(){

		if (stretching || isSnapping){
			buttObj.isFollowing = false;
		}
		else{
			buttObj.isFollowing = true;
			buttObjRigid.velocity = Vector3.zero;
		}

	}

	void ManageGravity(){
		if (stretching || isSnapping || TimeManagerS.timeMult == 0 ||
		    TimeManagerS.paused || effectPause){
			ownRigid.useGravity = false;
		}
		else{
			ownRigid.useGravity = true;
		}
	}

	public void TakeDamage(float dmg){
		health -= dmg;
		if (health <= 0){
			respawning = true;
			respawnTimeCountdown = respawnTimeMax;
		}
	}

	public void SleepTime(float delayTime){
		pauseDelay = delayTime;
		prevGravState = ownRigid.useGravity;
		prevVel = ownRigid.velocity;
		prevButtVel = buttObjRigid.velocity;
		effectPause = true;

		//print ("PAUSE");
	}
}
