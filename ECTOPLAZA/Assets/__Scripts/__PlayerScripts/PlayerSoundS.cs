using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSoundS : MonoBehaviour {

	private PlayerS playerRef;

	public List<GameObject> wallHitSoundObjs;
	public List<GameObject> groundPoundHitSoundObjs;
	public List<GameObject> jumpSoundObjs;

	public GameObject lv1ChargeSoundObj;
	public GameObject lv2ChargeSoundObj;
	public GameObject lv3ChargeSoundObj;

	public List<GameObject> attackReleaseSoundObjs;

	// Use this for initialization
	void Start () {

		playerRef = GetComponent<PlayerS>();
		playerRef.soundSource = this;
	
	}
	
	public void PlayWallHit(){

		int wallHitToPlay = Mathf.FloorToInt(Random.Range(0,wallHitSoundObjs.Count));

		Instantiate(wallHitSoundObjs[wallHitToPlay]);

	}

	public void PlayGroundPoundHit(){
		
		int groundHitToPlay = Mathf.FloorToInt(Random.Range(0,groundPoundHitSoundObjs.Count));
		
		Instantiate(groundPoundHitSoundObjs[groundHitToPlay]);
		
	}

	public void PlayJumpSound(){
		
		int jumpToPlay = Mathf.FloorToInt(Random.Range(0,jumpSoundObjs.Count));
		
		Instantiate(jumpSoundObjs[jumpToPlay]);
		
	}

	public void PlayReleaseSound(){
		
		int releaseToPlay = Mathf.FloorToInt(Random.Range(0,attackReleaseSoundObjs.Count));
		
		Instantiate(attackReleaseSoundObjs[releaseToPlay]);
		
	}


	public void PlayChargeLv1(){

		
		Instantiate(lv1ChargeSoundObj);
		
	}

	public void PlayChargeLv2(){
		
		
		Instantiate(lv2ChargeSoundObj);
		
	}

	public void PlayChargeLv3(){
		
		
		Instantiate(lv3ChargeSoundObj);
		
	}
}
