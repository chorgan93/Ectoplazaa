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
		if (headRigid.velocity.x > 0){
			Vector3 flipSize = headRender.transform.localScale;
			flipSize.x = -headRenderSize.x;
			headRender.transform.localScale = flipSize;
		}

		if (headRigid.velocity.x < 0){
			Vector3 flipSize = headRender.transform.localScale;
			flipSize.x = headRenderSize.x;
			headRender.transform.localScale = flipSize;
		}

		// turn tail according to velocity
		if (headRigid.velocity.x > 0){
			Vector3 flipSize = tailRender.transform.localScale;
			flipSize.x = -tailRenderSize.x;
			tailRender.transform.localScale = flipSize;

			//Vector3 tailOffSetPos = tailRender.transform.position;
			//tailOffSetPos.x = -tailOffSetPos;
			//tailRender.transform.position = tailOffSetPos;
		}
		
		if (headRigid.velocity.x < 0){
			Vector3 flipSize = tailRender.transform.localScale;
			flipSize.x = tailRenderSize.x;
			tailRender.transform.localScale = flipSize;

			//Vector3 tailOffSetPos = tailRender.transform.position;
			//tailOffSetPos.x = tailOffSetPos;
			//tailRender.transform.position = tailOffSetPos;
		}




	}

	void Animate () {

		idleFrameAnimCountdown -= Time.deltaTime*TimeManagerS.timeMult;

		if (idleFrameAnimCountdown <= 0){
			idleFrameAnimCountdown = idleFrameAnimRate;
			currentIdleFrame++;
			if (currentIdleFrame > headIdleFrames.Count-1){
				currentIdleFrame = 0;
			}
		}

		headRender.sprite = headIdleFrames[currentIdleFrame];
		tailRender.sprite = tailIdleFrames[currentIdleFrame];

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
