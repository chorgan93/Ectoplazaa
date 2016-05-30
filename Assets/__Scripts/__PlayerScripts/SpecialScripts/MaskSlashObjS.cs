using UnityEngine;
using System.Collections;

public class MaskSlashObjS : MonoBehaviour {

	public PlayerS targetPlayer;

	public Sprite[] slashFrames;
	private int currentFrame = 0;
	public float animRateMax = 0.08f;
	private float animRateCountdown;

	private SpriteRenderer myRender;

	public GameObject lightingScreen;

	public GameObject soundObj;

	// Use this for initialization
	void Start () {
	
		myRender = GetComponent<SpriteRenderer>();
		myRender.sprite = slashFrames[currentFrame];

		animRateCountdown = animRateMax;

		if (soundObj != null){
			Instantiate(soundObj);
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		animRateCountdown -= Time.deltaTime;
		if (animRateCountdown <= 0){

			animRateCountdown = animRateMax;
			currentFrame++;

			if (currentFrame >= slashFrames.Length -1 ){
				Destroy(gameObject);
			}
			else{
				myRender.sprite = slashFrames[currentFrame];
			}


		if (currentFrame == slashFrames.Length - 3){
			
				Instantiate(lightingScreen, Vector3.zero, Quaternion.identity);
			CameraShakeS.C.LargeShake();
			CameraShakeS.C.TimeSleep(0.18f);

		}
		if (currentFrame == slashFrames.Length - 2){
			targetPlayer.UnpauseCharacter();
			targetPlayer.TakeDamage(999999, true);
		}

		}
	
	}
}
