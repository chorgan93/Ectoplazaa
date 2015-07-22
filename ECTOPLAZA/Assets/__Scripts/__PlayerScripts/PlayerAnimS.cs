using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimS : MonoBehaviour {

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





	// Use this for initialization
	void Start () {

		//headRigid = headRender.GetComponent<Rigidbody>();
		//tailRigid = tailRender.GetComponent<Rigidbody>();

		headRenderSize = headRender.transform.localScale;
		tailRenderSize = tailRender.transform.localScale;

		isFacingDirection = true;
	
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
				isTurningTail = true;
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
				isTurningTail = true;
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
				if (currentJumpFrame > headJumpFrames.Count-1){
					currentJumpFrame = 0;
					// end jump animation
					isJumping = false;
				}
			}
			
			headRender.sprite = headJumpFrames[currentJumpFrame];
			// set tail frame if tail is not turning
			if (!isTurningTail){
				tailRender.sprite = tailJumpFrames[currentJumpFrame];
			}
		}

		else if (isRunning){
			runFrameAnimCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			
			if (runFrameAnimCountdown <= 0){
				runFrameAnimCountdown = runFrameAnimRate;
				currentRunFrame++;
				if (currentRunFrame > headRunFrames.Count-1){
					currentRunFrame = 0;
				}
			}
			
			headRender.sprite = headRunFrames[currentRunFrame];
			if (!isTurningTail){
				tailRender.sprite = tailRunFrames[currentRunFrame];
			}
		}
		else{
			idleFrameAnimCountdown -= Time.deltaTime*TimeManagerS.timeMult;
	
			if (idleFrameAnimCountdown <= 0){
				idleFrameAnimCountdown = idleFrameAnimRate;
				currentIdleFrame++;
				if (currentIdleFrame > headIdleFrames.Count-1){
					currentIdleFrame = 0;
				}
			}
	
			headRender.sprite = headIdleFrames[currentIdleFrame];

			if (!isTurningTail){
				tailRender.sprite = tailIdleFrames[currentIdleFrame];
			}
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

}
