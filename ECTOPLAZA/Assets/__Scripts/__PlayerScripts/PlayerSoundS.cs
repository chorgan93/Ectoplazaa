using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSoundS : MonoBehaviour {

	private PlayerS playerRef;

	public List<GameObject> wallHitSoundObjs;
	public List<GameObject> groundPoundHitSoundObjs;
	public List<GameObject> jumpSoundObjs;

	public GameObject lv1ChargeSoundObj;
	public List<GameObject> lv1CharChargeSoundObj;
	public GameObject lv2ChargeSoundObj;
	public List<GameObject> lv2CharChargeSoundObj;
	public GameObject lv3ChargeSoundObj;
	public List<GameObject> lv3CharChargeSoundObj;

	public List<GameObject> attackReleaseSoundObjs;
	public List<GameObject> groundPoundReleaseSoundObjs;
	public List<GameObject> charIntroSoundObjs;
	public List<GameObject> joinSoundObjs;
	public List<GameObject> deathSoundObjs;


	private int characterNum;

	// Use this for initialization
	void Start () {

		playerRef = GetComponent<PlayerS>();
		playerRef.soundSource = this;
		characterNum = playerRef.characterNum;
	
	}

	void FixedUpdate () {
		
		characterNum = playerRef.characterNum;
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

	public void PlayGroundPoundReleaseSound(){
		
		//int releaseToPlay = Mathf.FloorToInt(Random.Range(0,groundPoundReleaseSoundObjs.Count));
		
		Instantiate(groundPoundReleaseSoundObjs[characterNum-1]);
		//print (characterNum);
		
	}

	public void PlayCharIntroSound(){
		
		//int releaseToPlay = Mathf.FloorToInt(Random.Range(0,groundPoundReleaseSoundObjs.Count));

		int numToPlay = characterNum;
		if (numToPlay < 0){
			numToPlay = 0;
		}
		if (numToPlay > charIntroSoundObjs.Count-1){
			numToPlay = 0;
		}

		Instantiate(charIntroSoundObjs[numToPlay]);
		//print (characterNum);
		
	}

	public void PlayPlayerJoinSound(int numToPlay){
		Instantiate(joinSoundObjs[numToPlay]);
	}


	public void PlayChargeLv1(){

		
		Instantiate(lv1ChargeSoundObj);

		int numToPlay = characterNum-1;
		if (numToPlay < 0){
			numToPlay = 0;
		}
		if (numToPlay > charIntroSoundObjs.Count-1){
			numToPlay = 0;
		}

		Instantiate(lv1CharChargeSoundObj[numToPlay]);
		
	}

	public void PlayChargeLv2(){
		
		
		Instantiate(lv2ChargeSoundObj);

		int numToPlay = characterNum-1;
		if (numToPlay < 0){
			numToPlay = 0;
		}
		if (numToPlay > charIntroSoundObjs.Count-1){
			numToPlay = 0;
		}
		
		Instantiate(lv2CharChargeSoundObj[numToPlay]);

	}

	public void PlayChargeLv3(){
		
		
		Instantiate(lv3ChargeSoundObj);

		int numToPlay = characterNum-1;
		if (numToPlay < 0){
			numToPlay = 0;
		}
		if (numToPlay > charIntroSoundObjs.Count-1){
			numToPlay = 0;
		}
		
		Instantiate(lv3CharChargeSoundObj[numToPlay]);
		
	}

	public void PlayDeathSounds(){
		
		
		//Instantiate(lv3ChargeSoundObj);
		
		int numToPlay = characterNum-1;
		if (numToPlay < 0){
			numToPlay = 0;
		}
		if (numToPlay > charIntroSoundObjs.Count-1){
			numToPlay = 0;
		}
		
		Instantiate(deathSoundObjs[numToPlay]);
		
	}
}
