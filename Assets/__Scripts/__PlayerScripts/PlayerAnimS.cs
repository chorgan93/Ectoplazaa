using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimS : MonoBehaviour {

	// script to handle player character animations
	// scroll through appropriate list of sprites according to player state

	/*
	NEILSON NEUROTIC WAY OF ANIMATION
	ALL HEAD SPRITES FOR ONE CHARACTER ARE IN THE SAME ARRAY, AND ALL THE TAIL SPRITES ARE IN AN ARRAY
	For each animation the script just references different parts in the array 
	*/



	public string Description; 

	public SpriteRenderer headRender;
	public SpriteRenderer headRenderGreenGlow;

	public PlayerS playerRef;
	
	public Rigidbody headRigid;

	private Vector3 headRenderSize;

	private bool isIdling;
	public List<Sprite> headIdleFrames;
	public float idleFrameAnimRate;
	private float idleFrameAnimCountdown;
	private int currentIdleFrame;

	public bool isFacingDirection = false;

	public Sprite [] ninjaHeadSprites;
	public Sprite [] mummyHeadSprites; 
	public Sprite [] acidHeadSprites; 
	public Sprite [] pinkHeadSprites; 

	// add alt color sprites

	[HideInInspector]
	public int myColor = 0;

	public Sprite [] ninjaHeadSpritesB;
	public Sprite [] mummyHeadSpritesB; 
	public Sprite [] acidHeadSpritesB; 
	public Sprite [] pinkHeadSpritesB; 
	
	public Sprite [] ninjaHeadSpritesC;
	public Sprite [] mummyHeadSpritesC; 
	public Sprite [] acidHeadSpritesC; 
	public Sprite [] pinkHeadSpritesC;

	public Sprite [] ninjaHeadSpritesD;
	public Sprite [] mummyHeadSpritesD; 
	public Sprite [] acidHeadSpritesD; 
	public Sprite [] pinkHeadSpritesD;


	public int myCharNum;

	public Sprite [] currentHeadSprites;

	private bool isRunning;
	public List<Sprite> headRunFrames;
	public float runFrameAnimRate;
	private float runFrameAnimCountdown;
	private int currentRunFrame;

	private bool isJumping;
	private bool didJump = false;
	public List<Sprite> headJumpFrames;
	public float jumpFrameAnimRate;
	private float jumpFrameAnimCountdown;
	private int currentJumpFrame;


	public int jumpingAnimStart,runAnimStart,idleAnimStart; 

	private float invulnAlpha = 0.5f;

	private Color startColor;
	private Color redTeamColor = new Color (0.5f,0,0);
	private Color blueTeamColor = new Color (0,0,0.5f);



	// Use this for initialization
	void Start () {


		headRenderSize = headRender.transform.localScale;

		isFacingDirection = true;

		currentJumpFrame = jumpingAnimStart; 
		currentRunFrame = runAnimStart;
		currentIdleFrame = idleAnimStart; 

		if (playerRef.characterNum == 0) {
			currentHeadSprites = ninjaHeadSprites;
		} else{
			SetCurrentSprites (playerRef.characterNum, playerRef.colorNum);
		}

		if (CurrentModeS.isTeamMode){
			if (GlobalVars.teamNumber[playerRef.playerNum-1] == 1){
				startColor = redTeamColor;
			}
			else{
				startColor = blueTeamColor;
			}
		}
		else{
			startColor = headRender.color;
		}


	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Animate(); // actual animation

		if (!playerRef.effectPause && !playerRef.GetSpecialState()){
			FaceTarget(); // rotates head when character is in air to face direction of movement
			//print(isFacingDirection);
		}

		ManageAlpha();

	
	}

	void ManageAlpha () {

		// reduce alpha when invuln
		if (playerRef.respawnInvulnTime > 0){
			Color animColHead = startColor;
			Color animColGlow = headRenderGreenGlow.color;

			if (playerRef.dodging){
				animColGlow.a = animColHead.a = 0.1f;
			}
			else{
				animColGlow.a = animColHead.a = invulnAlpha;
			}

			headRender.color = animColHead;
			headRenderGreenGlow.color = animColGlow;
		}
		else{
			Color animColHead = startColor;
			Color animColGlow = headRenderGreenGlow.color;
			
			animColGlow.a = animColHead.a = 1f;
			
			headRender.color = animColHead;
			headRenderGreenGlow.color = animColGlow;
		}

	}



	void Animate () {
		


		// determine if jumping
		if (playerRef.groundDetect.Grounded()){
			didJump = false;
			if (headRigid.velocity.x != 0){
				isRunning =  true;
			}
			else{
				isRunning = false;
			}
			isJumping = false;
		}
		else{
			if (!didJump){
				didJump = true;
				isJumping = true;
			}
		}

		if (isJumping){
			jumpFrameAnimCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			
			if (jumpFrameAnimCountdown <= 0){
				jumpFrameAnimCountdown = jumpFrameAnimRate;
				currentJumpFrame++;
				if (currentJumpFrame > runAnimStart-1){ //CHANGED BECAUSE ALL ANIM SPRITES IN ONE ARRAY
					currentJumpFrame = jumpingAnimStart;
					// end jump animation
					isJumping = false;
				}
			}

			
			headRender.sprite =currentHeadSprites[currentJumpFrame];
		}

		else if (isRunning){

			// apply character run mult
			float animRateMult = 1;
			if (playerRef.characterNum == 0){
				animRateMult /= PlayerCharStatsS.ninja_SpeedMult;
			}
			if (playerRef.characterNum == 1){
				animRateMult /= PlayerCharStatsS.acidMouth_SpeedMult;
			}
			if (playerRef.characterNum == 2){
				animRateMult /= PlayerCharStatsS.mummy_SpeedMult;
			}
			if (playerRef.characterNum == 3){
				animRateMult /= PlayerCharStatsS.pinkWhip_SpeedMult;
			}

			//print ("I'm running");
			runFrameAnimCountdown -= Time.deltaTime*TimeManagerS.timeMult*animRateMult;
			
			if (runFrameAnimCountdown <= 0){
				runFrameAnimCountdown = runFrameAnimRate;
				currentRunFrame++;
				//print (currentRunFrame);
				if (currentRunFrame > idleAnimStart-1){
					currentRunFrame = runAnimStart;
				}
			}
			
			headRender.sprite = currentHeadSprites[currentRunFrame];
		}
		else{
			idleFrameAnimCountdown -= Time.deltaTime*TimeManagerS.timeMult;
	
			if (idleFrameAnimCountdown <= 0){
				idleFrameAnimCountdown = idleFrameAnimRate;
				currentIdleFrame++;
				if (currentIdleFrame > idleAnimStart+3){
					currentIdleFrame = idleAnimStart;
				}
			}
	
			headRender.sprite = currentHeadSprites[currentIdleFrame];

		}

	}

	void FaceTarget () {

		// rotate head and tail according to velocity

		
		if (!playerRef.groundDetect.Grounded() && !playerRef.charging && !playerRef.GetSpecialState()){
			isFacingDirection= true;
		}
		else{
			isFacingDirection = false;
			if (!playerRef.GetSpecialState()){
				headRender.transform.rotation = Quaternion.identity;
			}
		}

		// face direction based on velocity
		Vector3 faceSize = headRender.transform.localScale;
		if (headRigid.velocity.x > 0){
			faceSize.x = -headRenderSize.x;
		}
		if (headRigid.velocity.x < 0){
			faceSize.x = headRenderSize.x;
		}
		headRender.transform.localScale = faceSize;

		if (isFacingDirection){

			//print(playerRef.playerNum + " " + isFacingDirection);

			// for head

			float rotateZHead = 0;
		
			if(headRigid.velocity.x == 0){
				if (headRigid.velocity.y > 0){
					rotateZHead = 90;
				}
				else{
					rotateZHead = -90;
				}
			}
			else{
				rotateZHead = Mathf.Rad2Deg*Mathf.Atan((headRigid.velocity.y/headRigid.velocity.x));
			}	
		
			

			// when not active in character select, be at rotation 0
			if (playerRef.nonActive){
				rotateZHead = 0;
			}
			
			headRender.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateZHead));


			
		}

		// make sure green glow sprite have same sprite as main sprite
		if (headRender.enabled){
			if (!headRenderGreenGlow.enabled){
				headRenderGreenGlow.enabled = true;
			}
			headRenderGreenGlow.sprite = headRender.sprite;
		}
		else{
			headRenderGreenGlow.enabled = false;
		}

	}
	public float GetFaceTarget(Vector3 targetDir){
		
		
		targetDir = targetDir.normalized;
		
		// for head
		
		float rotateZHead = 0;
		
		if(targetDir.x == 0){
			if (targetDir.y > 0){
				rotateZHead = 90;
			}
			else{
				rotateZHead = -90;
			}
		}
		else{
			rotateZHead = Mathf.Rad2Deg*Mathf.Atan((targetDir.y/targetDir.x));
		}	
		
		
		if (targetDir.x < 0){
			//rotateZHead += 180;
		}
		
		return rotateZHead;
		
		
		
		
	}

	public void FaceTargetInstant(Vector3 targetDir){
		
		
		targetDir = targetDir.normalized;
		
		// for head
		
		float rotateZHead = 0;
		
		if(targetDir.x == 0){
			if (targetDir.y > 0){
				rotateZHead = 90;
			}
			else{
				rotateZHead = -90;
			}
		}
		else{
			rotateZHead = Mathf.Rad2Deg*Mathf.Atan((targetDir.y/targetDir.x));
		}	
		
		
		if (targetDir.x < 0){
			//rotateZHead += 180;
		}
		
		headRender.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateZHead));

		
		
		
	}

	public void SetCurrentSprites (int characterNumber, int colorNum)
	{


		myCharNum = characterNumber;
		myColor = colorNum;

		// Ninja sprite sets
		if (characterNumber == 1) {
			if (myColor == 3){
				currentHeadSprites = ninjaHeadSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = ninjaHeadSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = ninjaHeadSpritesB;
			}
			else{
				currentHeadSprites = ninjaHeadSprites;
			}

		}
		// Acidmouth sprite sets
		else if (characterNumber == 2) {
			if (myColor == 3){
				currentHeadSprites = acidHeadSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = acidHeadSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = acidHeadSpritesB;
			}
			else{
				currentHeadSprites = acidHeadSprites;
			}
			
		}
		// Mr Wraps sprite sets
		else if (characterNumber == 3) {
			if (myColor == 3){
				currentHeadSprites = mummyHeadSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = mummyHeadSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = mummyHeadSpritesB;
			}
			else{
				currentHeadSprites = mummyHeadSprites;
			}
			
		}
		// 
		else if (characterNumber == 4) {
			if (myColor == 3){
				currentHeadSprites = pinkHeadSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = pinkHeadSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = pinkHeadSpritesB;
			}
			else{
				currentHeadSprites = pinkHeadSprites;
			}
			
		}
	}

	public void RefreshTeamColor(){
		if (GlobalVars.teamNumber[playerRef.playerNum-1] == 1){
			startColor = redTeamColor;
		}
		else{
			startColor = blueTeamColor;
		}
	}

}
