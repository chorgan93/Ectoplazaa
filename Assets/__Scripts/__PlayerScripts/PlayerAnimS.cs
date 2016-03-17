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

	// original color

	public Sprite [] ninjaHeadSprites;
	public Sprite [] ninjaHeadSpecialSprites;

	public Sprite [] mummyHeadSprites;
	public Sprite [] mummyHeadSpecialSprites;  


	public Sprite [] acidHeadSprites;
	public Sprite [] acidHeadSpecialSprites; 

	public Sprite [] pinkHeadSprites; 
	public Sprite [] pinkHeadSpecialSprites; 
	
	public Sprite [] char5HeadSprites; 
	public Sprite [] char5SpecialSprites; 

	public Sprite [] char6HeadSprites; 
	public Sprite [] char6SpecialSprites; 

	public Sprite [] char7HeadSprites; 
	public Sprite [] char7SpecialSprites; 

	// add alt color sprites

	[HideInInspector]
	public int myColor = 0;

	// alt color 1
	
	public Sprite [] ninjaHeadSpritesB;
	public Sprite [] ninjaHeadSpecialSpritesB;
	
	public Sprite [] mummyHeadSpritesB;
	public Sprite [] mummyHeadSpecialSpritesB;  
	
	
	public Sprite [] acidHeadSpritesB;
	public Sprite [] acidHeadSpecialSpritesB; 
	
	public Sprite [] pinkHeadSpritesB; 
	public Sprite [] pinkHeadSpecialSpritesB; 
	
	public Sprite [] char5HeadSpritesB; 
	public Sprite [] char5SpecialSpritesB; 
	
	public Sprite [] char6HeadSpritesB; 
	public Sprite [] char6SpecialSpritesB; 
	
	public Sprite [] char7HeadSpritesB; 
	public Sprite [] char7SpecialSpritesB; 

	// alt color 2
	
	public Sprite [] ninjaHeadSpritesC;
	public Sprite [] ninjaHeadSpecialSpritesC;
	
	public Sprite [] mummyHeadSpritesC;
	public Sprite [] mummyHeadSpecialSpritesC;  
	
	
	public Sprite [] acidHeadSpritesC;
	public Sprite [] acidHeadSpecialSpritesC; 
	
	public Sprite [] pinkHeadSpritesC; 
	public Sprite [] pinkHeadSpecialSpritesC; 
	
	public Sprite [] char5HeadSpritesC; 
	public Sprite [] char5SpecialSpritesC; 
	
	public Sprite [] char6HeadSpritesC; 
	public Sprite [] char6SpecialSpritesC; 
	
	public Sprite [] char7HeadSpritesC; 
	public Sprite [] char7SpecialSpritesC; 

	// alt color 3
	
	public Sprite [] ninjaHeadSpritesD;
	public Sprite [] ninjaHeadSpecialSpritesD;
	
	public Sprite [] mummyHeadSpritesD;
	public Sprite [] mummyHeadSpecialSpritesD;  
	
	
	public Sprite [] acidHeadSpritesD;
	public Sprite [] acidHeadSpecialSpritesD; 
	
	public Sprite [] pinkHeadSpritesD; 
	public Sprite [] pinkHeadSpecialSpritesD; 
	
	public Sprite [] char5HeadSpritesD; 
	public Sprite [] char5SpecialSpritesD; 
	
	public Sprite [] char6HeadSpritesD; 
	public Sprite [] char6SpecialSpritesD; 
	
	public Sprite [] char7HeadSpritesD; 
	public Sprite [] char7SpecialSpritesD; 


	public int myCharNum;

	public Sprite [] currentHeadSprites;
	public Sprite [] currentSpecialSprites;

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

	public bool animatingSpecial = false;
	public bool finishedAnimatingSpecial = false;
	private float specialFrameRate = 0.08f;
	private float specialFrameRateCountdown;
	private int currentSpecialFrame = 0;



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

		// determine if special
		CheckSpecialAnimation();

		if (animatingSpecial){

			if (!finishedAnimatingSpecial){
				playerRef.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}

			specialFrameRateCountdown -= Time.deltaTime/Time.timeScale;
			if (specialFrameRateCountdown <= 0){
				specialFrameRateCountdown = specialFrameRate;
				currentSpecialFrame++;
				if (currentSpecialFrame > currentSpecialSprites.Length - 1){
					if (!finishedAnimatingSpecial){
						playerRef.SpecialIsDoneAnimating();
						finishedAnimatingSpecial = true;
					}
					currentSpecialFrame = currentSpecialSprites.Length - 2;
				}
			}

			headRender.sprite =currentSpecialSprites[currentSpecialFrame];

		}
		else if (isJumping){
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
		if (headRigid.velocity.x > 0 || (headRigid.velocity.x == 0 && headRigid.velocity.y != 0)){
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
		myColor = 0;

		// Ninja sprite sets
		if (characterNumber == 1) {
			if (myColor == 3){
				currentHeadSprites = ninjaHeadSpritesD;
				currentSpecialSprites = ninjaHeadSpecialSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = ninjaHeadSpritesC;
				currentSpecialSprites = ninjaHeadSpecialSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = ninjaHeadSpritesB;
				currentSpecialSprites = ninjaHeadSpecialSpritesB;
			}
			else{
				currentHeadSprites = ninjaHeadSprites;
				currentSpecialSprites = ninjaHeadSpecialSprites;
			}

		}
		// Acidmouth sprite sets
		else if (characterNumber == 2) {
			if (myColor == 3){
				currentHeadSprites = acidHeadSpritesD;
				currentSpecialSprites = acidHeadSpecialSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = acidHeadSpritesC;
				currentSpecialSprites = acidHeadSpecialSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = acidHeadSpritesB;
				currentSpecialSprites = acidHeadSpecialSpritesB;
			}
			else{
				currentHeadSprites = acidHeadSprites;
				currentSpecialSprites = acidHeadSpecialSprites;
			}
			
		}
		// Mr Wraps sprite sets
		else if (characterNumber == 3) {
			if (myColor == 3){
				currentHeadSprites = mummyHeadSpritesD;
				currentSpecialSprites = mummyHeadSpecialSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = mummyHeadSpritesC;
				currentSpecialSprites = mummyHeadSpecialSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = mummyHeadSpritesB;
				currentSpecialSprites = mummyHeadSpecialSpritesB;
			}
			else{
				currentHeadSprites = mummyHeadSprites;
				currentSpecialSprites = mummyHeadSpecialSprites;
			}
			
		}
		// 
		else if (characterNumber == 4) {
			if (myColor == 3){
				currentHeadSprites = pinkHeadSpritesD;
				currentSpecialSprites = pinkHeadSpecialSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = pinkHeadSpritesC;
				currentSpecialSprites = pinkHeadSpecialSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = pinkHeadSpritesB;
				currentSpecialSprites = pinkHeadSpecialSpritesB;
			}
			else{
				currentHeadSprites = pinkHeadSprites;
				currentSpecialSprites = pinkHeadSpecialSprites;
			}
			
		}

		else if (characterNumber == 5) {
			if (myColor == 3){
				currentHeadSprites = char5HeadSpritesD;
				currentSpecialSprites = char5SpecialSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = char5HeadSpritesC;
				currentSpecialSprites = char5SpecialSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = char5HeadSpritesB;
				currentSpecialSprites = char5SpecialSpritesB;
			}
			else{
				currentHeadSprites = char5HeadSprites;
				currentSpecialSprites = char5SpecialSprites;
			}
			
		}

		else if (characterNumber == 6) {
			if (myColor == 3){
				currentHeadSprites = char6HeadSpritesD;
				currentSpecialSprites = char6SpecialSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = char6HeadSpritesC;
				currentSpecialSprites = char6SpecialSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = char6HeadSpritesB;
				currentSpecialSprites = char6SpecialSpritesB;
			}
			else{
				currentHeadSprites = char6HeadSprites;
				currentSpecialSprites = char6SpecialSprites;
			}
			
		}

		else if (characterNumber == 7) {
			if (myColor == 3){
				currentHeadSprites = char7HeadSpritesD;
				currentSpecialSprites = char7SpecialSpritesD;
			}
			else if (myColor == 2){
				currentHeadSprites = char7HeadSpritesC;
				currentSpecialSprites = char7SpecialSpritesC;
			}
			else if (myColor == 1){
				currentHeadSprites = char7HeadSpritesB;
				currentSpecialSprites = char7SpecialSpritesB;
			}
			else{
				currentHeadSprites = char7HeadSprites;
				currentSpecialSprites = char7SpecialSprites;
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

	public void CheckSpecialAnimation(){

		if (playerRef.GetSpecialState() && !animatingSpecial){

			animatingSpecial = true;
			finishedAnimatingSpecial = false;
			currentSpecialFrame = 0;

			specialFrameRateCountdown = specialFrameRate;

		}

		if (!playerRef.GetSpecialState() && animatingSpecial){
			animatingSpecial = false;
			finishedAnimatingSpecial = false;
		}

	}

}
