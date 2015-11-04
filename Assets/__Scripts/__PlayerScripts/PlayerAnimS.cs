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
	public SpriteRenderer tailRender;

	public PlayerS playerRef;
	
	public Rigidbody headRigid;
	public Rigidbody tailRigid;

	public float tailXPosOffset = 0;

	private Vector3 headRenderSize;
	private Vector3 tailRenderSize;

	private bool isIdling;
	public List<Sprite> headIdleFrames;
	public List<Sprite> tailIdleFrames;
	public float idleFrameAnimRate;
	private float idleFrameAnimCountdown;
	private int currentIdleFrame;

	public bool isFacingDirection = false;

	public Sprite [] ninjaHeadSprites, ninjaTailSprites;
	public Sprite [] mummyHeadSprites, mummyTailSprites; 
	public Sprite [] acidHeadSprites, acidTailSprites; 
	public Sprite [] pinkHeadSprites, pinkTailSprites; 

	// add alt color sprites

	[HideInInspector]
	public int myColor = 0;

	public Sprite [] ninjaHeadSpritesB, ninjaTailSpritesB;
	public Sprite [] mummyHeadSpritesB, mummyTailSpritesB; 
	public Sprite [] acidHeadSpritesB, acidTailSpritesB; 
	public Sprite [] pinkHeadSpritesB, pinkTailSpritesB; 
	
	public Sprite [] ninjaHeadSpritesC, ninjaTailSpritesC;
	public Sprite [] mummyHeadSpritesC, mummyTailSpritesC; 
	public Sprite [] acidHeadSpritesC, acidTailSpritesC; 
	public Sprite [] pinkHeadSpritesC, pinkTailSpritesC;

	public Sprite [] ninjaHeadSpritesD, ninjaTailSpritesD;
	public Sprite [] mummyHeadSpritesD, mummyTailSpritesD; 
	public Sprite [] acidHeadSpritesD, acidTailSpritesD; 
	public Sprite [] pinkHeadSpritesD, pinkTailSpritesD;


	public int myCharNum;

	public Sprite [] currentHeadSprites, currentTailSprites;

	private bool isRunning;
	public List<Sprite> headRunFrames;
	public List<Sprite> tailRunFrames;
	public float runFrameAnimRate;
	private float runFrameAnimCountdown;
	private int currentRunFrame;

	private bool isJumping;
	private bool didJump = false;
	public List<Sprite> headJumpFrames;
	public List<Sprite> tailJumpFrames;
	public float jumpFrameAnimRate;
	private float jumpFrameAnimCountdown;
	private int currentJumpFrame;

	// turn needs to be seperated to allow for head and tail to turn independently
	private bool isTurningHead;
	private bool didTurnHead = false;
	public List<Sprite> headTurnFrames;

	private bool isTurningTail;
	private bool didTurnTail = false;
	public List<Sprite> tailTurnFrames;
	public float turnFrameAnimRate;
	private float turnFrameAnimCountdown;

	private int currentHeadTurnFrame;
	private int currentTailTurnFrame;

	public int jumpingAnimStart,runAnimStart,idleAnimStart; 

	private float invulnAlpha = 0.5f;



	// Use this for initialization
	void Start () {

		//headRigid = headRender.GetComponent<Rigidbody>();
		//tailRigid = tailRender.GetComponent<Rigidbody>();

		headRenderSize = headRender.transform.localScale;
		tailRenderSize = tailRender.transform.localScale;

		isFacingDirection = true;

		currentJumpFrame = jumpingAnimStart; 
		currentRunFrame = runAnimStart;
		currentIdleFrame = idleAnimStart; 

		if (playerRef.characterNum == 0) {
			currentHeadSprites = ninjaHeadSprites;
			currentTailSprites = ninjaTailSprites; 
		} else
			SetCurrentSprites (playerRef.characterNum, playerRef.colorNum);


	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Turn(); // currently not in use for actual animation
		Animate(); // actual animation

		FaceTarget(); // rotates head when character is in air to face direction of movement
		//print(isFacingDirection);

		ManageAlpha();

	
	}

	void ManageAlpha () {

		// reduce alpha when invuln
		if (playerRef.respawnInvulnTime > 0){
			Color animColHead = headRender.color;
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
			Color animColHead = headRender.color;
			Color animColGlow = headRenderGreenGlow.color;
			
			animColGlow.a = animColHead.a = 1f;
			
			headRender.color = animColHead;
			headRenderGreenGlow.color = animColGlow;
		}

	}

	void Turn(){

		// turn head according to velocity

		// if head is not already turned, turn in isTurning
		if (headRigid.velocity.x > 0){
			Vector3 flipSize = headRender.transform.localScale;
			if (flipSize.x != -headRenderSize.x && !didTurnHead)
			{
				isTurningHead = true;
				//didTurnHead = true;
				flipSize.x = -headRenderSize.x;
			}
			headRender.transform.localScale = flipSize;
		}

		if (headRigid.velocity.x < 0){
			Vector3 flipSize = headRender.transform.localScale;
			if (flipSize.x != headRenderSize.x && !didTurnHead)
			{
				isTurningHead = true;
				//didTurnHead = true;
				flipSize.x = headRenderSize.x;
			}
			headRender.transform.localScale = flipSize;
		}


		// turn tail according to velocity
		if (headRigid.velocity.x > 0){
			Vector3 flipSize = tailRender.transform.localScale;
			if (flipSize.x != -tailRenderSize.x && !didTurnTail){
				//isTurningTail = true; //HAD TO COMMENT OUT FOR NOW, SORRY COLIN, THIS IS ALWAYS TURNED ON FOR SOME REASON, it stops the tail from animating
				//didTurnTail = true;
				flipSize.x = -tailRenderSize.x;
			}
			tailRender.transform.localScale = flipSize;

			Vector3 tailOffSetPos = tailRender.transform.localPosition;
			tailOffSetPos.x = -tailXPosOffset;
			//tailRender.transform.localPosition = tailOffSetPos;
		}
		
		if (headRigid.velocity.x < 0){
			Vector3 flipSize = tailRender.transform.localScale;
			if (flipSize.x != tailRenderSize.x && !didTurnTail){
				//isTurningTail = true; //HAD TO COMMENT OUT FOR NOW, SORRY COLIN, THIS IS ALWAYS TURNED ON FOR SOME REASON
				//didTurnTail = true;
				flipSize.x = tailRenderSize.x;
			}
			tailRender.transform.localScale = flipSize;

			Vector3 tailOffSetPos = tailRender.transform.localPosition;
			tailOffSetPos.x = tailXPosOffset;
			//tailRender.transform.localPosition = tailOffSetPos;
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
			// set tail frame if tail is not turning
			//if (!isTurningTail){
				//tailRender.sprite = currentTailSprites[currentJumpFrame];
			//}
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
			//if (!isTurningTail){
				//tailRender.sprite = currentTailSprites[currentRunFrame];
			//}
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

			//if (!isTurningTail){
				//tailRender.sprite = currentTailSprites[currentIdleFrame];
			//}
		}

		// seperate clause for tail turning when head is not turning
		/*if (isTurningTail){
			idleFrameAnimCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			
			if (idleFrameAnimCountdown <= 0){
				idleFrameAnimCountdown = idleFrameAnimRate;
				currentIdleFrame++;
				if (currentIdleFrame > headIdleFrames.Count-1){
					currentIdleFrame = 0;
				}
			}
			
			headRender.sprite = headIdleFrames[currentIdleFrame];
		}*/

	}

	void FaceTarget () {

		// rotate head and tail according to velocity

		
		if (!playerRef.groundDetect.Grounded() && !playerRef.charging){
			isFacingDirection= true;
		}
		else{
			isFacingDirection = false;
			headRender.transform.rotation = tailRender.transform.rotation = Quaternion.identity;
		}

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
		
			
			if (headRigid.velocity.x < 0){
				//rotateZHead += 180;
			}
			// when not active in character select, be at rotation 0
			if (playerRef.nonActive){
				rotateZHead = 0;
			}
			
			headRender.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateZHead));

			// for tail

			float rotateZTail = 0;
			
			if(tailRigid.velocity.x == 0){
				rotateZTail = 0;
			}
			else{
				rotateZTail = Mathf.Rad2Deg*Mathf.Atan((tailRigid.velocity.y/tailRigid.velocity.x));
			}	
			
			
			if (tailRigid.velocity.x < 0){
				//rotateZTail += 180;
			}


			
			//tailRender.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateZTail));
			
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

	public void SetCurrentSprites (int characterNumber, int colorNum)
	{


		myCharNum = characterNumber;
		myColor = colorNum;

		// Ninja sprite sets
		if (characterNumber == 1) {
			if (myColor == 3){
				currentHeadSprites = ninjaHeadSpritesD;
				currentTailSprites= ninjaTailSprites; 
			}
			else if (myColor == 2){
				currentHeadSprites = ninjaHeadSpritesC;
				currentTailSprites= ninjaTailSprites; 
			}
			else if (myColor == 1){
				currentHeadSprites = ninjaHeadSpritesB;
				currentTailSprites= ninjaTailSprites; 
			}
			else{
				currentHeadSprites = ninjaHeadSprites;
				currentTailSprites= ninjaTailSprites; 
			}

		}
		// Acidmouth sprite sets
		else if (characterNumber == 2) {
			if (myColor == 3){
				currentHeadSprites = acidHeadSpritesD;
				currentTailSprites= acidTailSprites;
			}
			else if (myColor == 2){
				currentHeadSprites = acidHeadSpritesC;
				currentTailSprites= acidTailSprites;
			}
			else if (myColor == 1){
				currentHeadSprites = acidHeadSpritesB;
				currentTailSprites= acidTailSprites;
			}
			else{
				currentHeadSprites = acidHeadSprites;
				currentTailSprites= acidTailSprites;
			}
			
		}
		// Mr Wraps sprite sets
		else if (characterNumber == 3) {
			if (myColor == 3){
				currentHeadSprites = mummyHeadSpritesD;
				currentTailSprites= mummyTailSprites;
			}
			else if (myColor == 2){
				currentHeadSprites = mummyHeadSpritesC;
				currentTailSprites= mummyTailSprites;
			}
			else if (myColor == 1){
				currentHeadSprites = mummyHeadSpritesB;
				currentTailSprites= mummyTailSprites;
			}
			else{
				currentHeadSprites = mummyHeadSprites;
				currentTailSprites= mummyTailSprites;
			}
			
		}
		// 
		else if (characterNumber == 4) {
			if (myColor == 3){
				currentHeadSprites = pinkHeadSpritesD;
				currentTailSprites= pinkTailSprites; 
			}
			else if (myColor == 2){
				currentHeadSprites = pinkHeadSpritesC;
				currentTailSprites= pinkTailSprites; 
			}
			else if (myColor == 1){
				currentHeadSprites = pinkHeadSpritesB;
				currentTailSprites= pinkTailSprites; 
			}
			else{
				currentHeadSprites = pinkHeadSprites;
				currentTailSprites= pinkTailSprites; 
			}
			
		}
	}

}
