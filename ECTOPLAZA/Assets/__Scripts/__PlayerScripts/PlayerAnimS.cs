using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimS : MonoBehaviour {

	/*
	NEILSON NEUROTIC WAY OF ANIMATION
	ALL HEAD SPRITES FOR ONE CHARACTER ARE IN THE SAME ARRAY, AND ALL THE TAIL SPRITES ARE IN AN ARRAY
	For each animation the script just references different parts in the array 
	*/



	public string Description; 

	public SpriteRenderer headRender;
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
			SetCurrentSprites (playerRef.characterNum);


	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Turn();
		Animate();
		FaceTarget();
	
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
			tailRender.transform.localPosition = tailOffSetPos;
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
			tailRender.transform.localPosition = tailOffSetPos;
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
				tailRender.sprite = currentTailSprites[currentJumpFrame];
			//}
		}

		else if (isRunning){
			print (isRunning);
			runFrameAnimCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			
			if (runFrameAnimCountdown <= 0){
				runFrameAnimCountdown = runFrameAnimRate;
				currentRunFrame++;
				if (currentRunFrame > idleAnimStart-1){
					currentRunFrame = runAnimStart;
				}
			}
			
			headRender.sprite = currentHeadSprites[currentRunFrame];
			//if (!isTurningTail){
				tailRender.sprite = currentTailSprites[currentRunFrame];
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
				tailRender.sprite = currentTailSprites[currentIdleFrame];
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

		if (isFacingDirection){

			// for head

			float rotateZHead = 0;
		
			if(headRigid.velocity.x == 0){
				rotateZHead = 0;
			}
			else{
				rotateZHead = Mathf.Rad2Deg*Mathf.Atan((headRigid.velocity.y/headRigid.velocity.x));
			}	
		
			
			if (headRigid.velocity.x < 0){
				//rotateZHead += 180;
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
			
			tailRender.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateZTail));
			
		}

	}

	public void SetCurrentSprites (int characterNumber)
	{
		if (characterNumber == 1) {
			currentHeadSprites = ninjaHeadSprites;
			currentTailSprites= ninjaTailSprites; 

		}
		else if (characterNumber == 2) {
			currentHeadSprites = acidHeadSprites;
			currentTailSprites= acidTailSprites; 
			
		}
		else if (characterNumber == 3) {
			currentHeadSprites = mummyHeadSprites;
			currentTailSprites= mummyTailSprites; 
			
		}
		else if (characterNumber == 4) {
			currentHeadSprites = pinkHeadSprites;
			currentTailSprites= pinkTailSprites; 
			
		}
	}

}
