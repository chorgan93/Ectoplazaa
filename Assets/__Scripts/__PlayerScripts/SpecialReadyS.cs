using UnityEngine;
using System.Collections;

public class SpecialReadyS : MonoBehaviour {

	private SpriteRenderer myRender;

	public PlayerS playerRef;

	public Sprite[] animFrames;
	public float animFrameRate;
	private float animFrameCountdown;
	private int animFrame;

	public int frameToLoopAt;

	// Use this for initialization
	void Start () {

		myRender = GetComponent<SpriteRenderer>();

	//	transform.parent = null;
	//	transform.rotation = Quaternion.Euler(new Vector3(0,0,90f));

		myRender.color = playerRef.playerParticleMats
			[playerRef.characterNum-1].GetColor("_TintColor");
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// Vector3 fireOffset = new Vector3(0,3,0);

		if (playerRef.numKOsInRow >= playerRef.numKOSPublic){
			myRender.enabled = true;

			//transform.position = playerRef.transform.position + fireOffset;
		}
		else{
			myRender.enabled = false;
			animFrame = 0;
			myRender.sprite = animFrames[animFrame];
			animFrameCountdown = animFrameRate;
		}

		if (myRender.enabled){

			animFrameCountdown -= Time.deltaTime;

			if (animFrameCountdown <= 0){
				animFrameCountdown = animFrameRate;

				animFrame++;
				if (animFrame >= animFrames.Length-1){
					animFrame = frameToLoopAt;
				}

				myRender.sprite = animFrames[animFrame];
			}

		}
	
	}
}
