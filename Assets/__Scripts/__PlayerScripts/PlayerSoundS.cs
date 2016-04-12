using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSoundS : MonoBehaviour {

	// script placed on player controller that holds all player sound effects and source
	// referenced in player script whenever player needs to made a noise

	private PlayerS playerRef;

	// all sound effects are kept as objects that are instantiated when needed
	// this allows us to play multiple sounds at once when needed without overloading one audiosource

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
	public List<GameObject> dodgeSoundObjs;
	public List<GameObject> groundPoundReleaseSoundObjs;
	public List<GameObject> charIntroSoundObjs;
	public List<GameObject> joinSoundObjs;
	public List<GameObject> deathSoundObjs;

	public List<GameObject> ninjaDeathSoundObjs;
	public List<GameObject> acidDeathSoundObjs;
	public List<GameObject> mummyDeathSoundObjs;
	public List<GameObject> pinkDeathSoundObjs;
	public List<GameObject> pinkSpecialDeathSoundObjs;
	public List<GameObject> jabberDeathSoundObjs;
	public List<GameObject> blobbyDeathSoundObjs;
	
	public List<GameObject> ninjaLv1AttackSoundObjs;
	public List<GameObject> acidLv1AttackSoundObjs;
	public List<GameObject> mummyLv1AttackSoundObjs;
	public List<GameObject> pinkWhipLv1AttackSoundObjs;
	public List<GameObject> jabberLv1AttackSoundObjs;
	public List<GameObject> blobbyLv1AttackSoundObjs;

	public List<GameObject> ninjaLv2AttackSoundObjs;
	public List<GameObject> acidLv2AttackSoundObjs;
	public List<GameObject> mummyLv2AttackSoundObjs;
	public List<GameObject> pinkWhipLv2AttackSoundObjs;
	public List<GameObject> jabberLv2AttackSoundObjs;
	public List<GameObject> blobbyLv2AttackSoundObjs;

	public List<GameObject> ninjaLv3AttackSoundObjs;
	public List<GameObject> acidLv3AttackSoundObjs;
	public List<GameObject> mummyLv3AttackSoundObjs;
	public List<GameObject> pinkWhipLv3AttackSoundObjs;
	public List<GameObject> jabberLv3AttackSoundObjs;
	public List<GameObject> blobbyLv3AttackSoundObjs;

	public GameObject earnSpecialSoundObj;
	public GameObject zoomSpecialSoundObj;
	
	public GameObject spectoSlashObj;
	public GameObject pinkSpecialObj;
	public GameObject mummySpecialObj;
	public GameObject acidSpecialObj;
	public GameObject acidSpecialObjEnd;
	public GameObject blobbySpecialObj;
	public GameObject jabberSpecialObj;
	public GameObject jabberSpecialObjEnd;

	private GameObject currentSpecialSoundObj;
	private GameObject currentLaserSoundObj;


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

	public void PlaySpectoSlash(){
		Instantiate(spectoSlashObj);
	}

	public void PlayPinkSpecial(){
		Instantiate(pinkSpecialObj);
	}

	public void PlayMummySpecial(){
		Instantiate(mummySpecialObj);
	}

	public void PlayAcidSpecial(){
		currentLaserSoundObj = (GameObject)Instantiate(acidSpecialObj);
	}

	public void PlayAcidSpecialEnd(){
		Instantiate(acidSpecialObjEnd);

		if (currentLaserSoundObj != null){
			Destroy(currentLaserSoundObj);
		}
	}

	public void PlayJabberSpecial(){
		Instantiate(jabberSpecialObj);
	}
	
	public void PlayJabberSpecialEnd(){
		Instantiate(jabberSpecialObjEnd);
	}

	public void PlayBlobbySpecial(){
		Instantiate(blobbySpecialObj);
	}

	public void PlaySpecialZoom(){

		Instantiate(zoomSpecialSoundObj);

		if (currentSpecialSoundObj != null){
			Destroy(currentSpecialSoundObj);
		}

	}

	public void PlaySpecialEarn(){

		if (!currentSpecialSoundObj){
			currentSpecialSoundObj = (GameObject)Instantiate(earnSpecialSoundObj);
		}

	}

	
	public void PlayWallHit(){

		int wallHitToPlay = Mathf.FloorToInt(Random.Range(0,wallHitSoundObjs.Count));

		Instantiate(wallHitSoundObjs[wallHitToPlay]);

		//print ("played wall hit sound");

	}

	public void PlayGroundPoundHit(){
		
		int groundHitToPlay = Mathf.FloorToInt(Random.Range(0,groundPoundHitSoundObjs.Count));
		
		Instantiate(groundPoundHitSoundObjs[groundHitToPlay]);
		
	}

	public void PlayJumpSound(){
		
		int jumpToPlay = Mathf.FloorToInt(Random.Range(0,jumpSoundObjs.Count));
		
		Instantiate(jumpSoundObjs[jumpToPlay]);
		
	}

	public void PlayDodgeSound(){
		
		int dodgeToPlay = Mathf.FloorToInt(Random.Range(0,dodgeSoundObjs.Count));
		
		Instantiate(dodgeSoundObjs[dodgeToPlay]);
		
	}

	public void PlayCharAttackSound(int lv){
		
		int soundToPlay = 0;

		switch (lv){

		case 0:
			if (playerRef.characterNum == 1){

				soundToPlay = Mathf.FloorToInt(Random.Range(0, ninjaLv1AttackSoundObjs.Count));
				Instantiate(ninjaLv1AttackSoundObjs[soundToPlay]);

			}
			if (playerRef.characterNum == 2){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, acidLv1AttackSoundObjs.Count));
				Instantiate(acidLv1AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 3){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, mummyLv1AttackSoundObjs.Count));
				Instantiate(mummyLv1AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 4){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, pinkWhipLv1AttackSoundObjs.Count));
				Instantiate(pinkWhipLv1AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 5){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, jabberLv1AttackSoundObjs.Count));
				Instantiate(jabberLv1AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 6){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, blobbyLv1AttackSoundObjs.Count));
				Instantiate(blobbyLv1AttackSoundObjs[soundToPlay]);
			}
			break;
		case 1:
			if (playerRef.characterNum == 1){
				
				soundToPlay = Mathf.FloorToInt(Random.Range(0, ninjaLv2AttackSoundObjs.Count));
				Instantiate(ninjaLv2AttackSoundObjs[soundToPlay]);
				
			}
			if (playerRef.characterNum == 2){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, acidLv2AttackSoundObjs.Count));
				Instantiate(acidLv2AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 3){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, mummyLv2AttackSoundObjs.Count));
				Instantiate(mummyLv2AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 4){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, pinkWhipLv2AttackSoundObjs.Count));
				Instantiate(pinkWhipLv2AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 5){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, jabberLv2AttackSoundObjs.Count));
				Instantiate(jabberLv2AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 6){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, blobbyLv2AttackSoundObjs.Count));
				Instantiate(blobbyLv2AttackSoundObjs[soundToPlay]);
			}
			break;
		case 2:
			if (playerRef.characterNum == 1){
				
				soundToPlay = Mathf.FloorToInt(Random.Range(0, ninjaLv3AttackSoundObjs.Count));
				Instantiate(ninjaLv3AttackSoundObjs[soundToPlay]);
				
			}
			if (playerRef.characterNum == 2){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, acidLv3AttackSoundObjs.Count));
				Instantiate(acidLv3AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 3){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, mummyLv3AttackSoundObjs.Count));
				Instantiate(mummyLv3AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 4){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, pinkWhipLv3AttackSoundObjs.Count));
				Instantiate(pinkWhipLv3AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 5){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, jabberLv3AttackSoundObjs.Count));
				Instantiate(jabberLv3AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 6){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, blobbyLv3AttackSoundObjs.Count));
				Instantiate(blobbyLv3AttackSoundObjs[soundToPlay]);
			}
			break;
		default:
			if (playerRef.characterNum == 1){
				
				soundToPlay = Mathf.FloorToInt(Random.Range(0, ninjaLv1AttackSoundObjs.Count));
				Instantiate(ninjaLv1AttackSoundObjs[soundToPlay]);
				
			}
			if (playerRef.characterNum == 2){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, acidLv1AttackSoundObjs.Count));
				Instantiate(acidLv1AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 3){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, mummyLv1AttackSoundObjs.Count));
				Instantiate(mummyLv1AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 4){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, pinkWhipLv1AttackSoundObjs.Count));
				Instantiate(pinkWhipLv1AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 5){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, jabberLv1AttackSoundObjs.Count));
				Instantiate(jabberLv1AttackSoundObjs[soundToPlay]);
			}
			if (playerRef.characterNum == 6){
				soundToPlay = Mathf.FloorToInt(Random.Range(0, blobbyLv1AttackSoundObjs.Count));
				Instantiate(blobbyLv1AttackSoundObjs[soundToPlay]);
			}
			break;
		}

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

		// need to hear these
		
		//int releaseToPlay = Mathf.FloorToInt(Random.Range(0,groundPoundReleaseSoundObjs.Count));
		
		//characterNum = playerRef.characterNum;

		int numToPlay = characterNum-1;
		if (numToPlay < 0){
			numToPlay = 0;
		}
		if (numToPlay > charIntroSoundObjs.Count-1){
			numToPlay = 0;
		}

		Instantiate(charIntroSoundObjs[numToPlay]);
		//print (characterNum);
		
	}

	public void PlayCharIntroSoundQuickFix(){

		if (playerRef){

		
			characterNum = playerRef.characterNum;
		}
		else{
			characterNum = 0;
		}
		
		// need to hear these
		
		//int releaseToPlay = Mathf.FloorToInt(Random.Range(0,groundPoundReleaseSoundObjs.Count));
		
		int numToPlay = characterNum-1;
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

		
		//Instantiate(lv1ChargeSoundObj);

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
		
		
		//Instantiate(lv3ChargeSoundObj);

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
		if (currentSpecialSoundObj != null){
			Destroy(currentSpecialSoundObj);
		}

		
		int numToPlay = characterNum;
		if (numToPlay < 0){
			numToPlay = 0;
		}
		if (numToPlay > charIntroSoundObjs.Count-1){
			numToPlay = 0;
		}

		int randomChoice = 0;
		switch (numToPlay){
		default:
			Instantiate(ninjaDeathSoundObjs[randomChoice]);
			break;
		case 1:
			randomChoice = Mathf.FloorToInt(Random.Range(0, ninjaDeathSoundObjs.Count));
			Instantiate(ninjaDeathSoundObjs[randomChoice]);
			break;
		case 2:
			randomChoice = Mathf.FloorToInt(Random.Range(0, acidDeathSoundObjs.Count));
			Instantiate(acidDeathSoundObjs[randomChoice]);
			break;
		case 3:
			randomChoice = Mathf.FloorToInt(Random.Range(0, mummyDeathSoundObjs.Count));
			Instantiate(mummyDeathSoundObjs[randomChoice]);
			break;
		case 4:
			randomChoice = Mathf.FloorToInt(Random.Range(0, pinkDeathSoundObjs.Count));
			Instantiate(pinkDeathSoundObjs[randomChoice]);
			Debug.Log(pinkDeathSoundObjs[randomChoice].name);
			break;
		case 5:
			randomChoice = Mathf.FloorToInt(Random.Range(0, jabberDeathSoundObjs.Count));
			Instantiate(jabberDeathSoundObjs[randomChoice]);
			break;
		case 6:
			randomChoice = Mathf.FloorToInt(Random.Range(0, blobbyDeathSoundObjs.Count));
			Instantiate(blobbyDeathSoundObjs[randomChoice]);
			break;
		}
		//Instantiate(deathSoundObjs[numToPlay]);
		
	}

	public void PlaySpecialDeathSounds(){
		
		
		//Instantiate(lv3ChargeSoundObj);
		if (currentSpecialSoundObj != null){
			Destroy(currentSpecialSoundObj);
		}

		
		int numToPlay = characterNum;
		if (numToPlay < 0){
			numToPlay = 0;
		}
		if (numToPlay > charIntroSoundObjs.Count-1){
			numToPlay = 0;
		}
		
		int randomChoice = 0;
		switch (numToPlay){
		default:
			Instantiate(ninjaDeathSoundObjs[randomChoice]);
			break;
		case 1:
			randomChoice = Mathf.FloorToInt(Random.Range(0, ninjaDeathSoundObjs.Count));
			Instantiate(ninjaDeathSoundObjs[randomChoice]);
			break;
		case 2:
			randomChoice = Mathf.FloorToInt(Random.Range(0, acidDeathSoundObjs.Count));
			Instantiate(acidDeathSoundObjs[randomChoice]);
			break;
		case 3:
			randomChoice = Mathf.FloorToInt(Random.Range(0, mummyDeathSoundObjs.Count));
			Instantiate(mummyDeathSoundObjs[randomChoice]);
			break;
		case 4:
			randomChoice = Mathf.FloorToInt(Random.Range(0, pinkSpecialDeathSoundObjs.Count));
			Instantiate(pinkSpecialDeathSoundObjs[randomChoice]);
			Debug.Log(pinkSpecialDeathSoundObjs[randomChoice].name);
			break;
		case 5:
			randomChoice = Mathf.FloorToInt(Random.Range(0, jabberDeathSoundObjs.Count));
			Instantiate(jabberDeathSoundObjs[randomChoice]);
			break;
		case 6:
			randomChoice = Mathf.FloorToInt(Random.Range(0, blobbyDeathSoundObjs.Count));
			Instantiate(blobbyDeathSoundObjs[randomChoice]);
			break;
		}
		//Instantiate(deathSoundObjs[numToPlay]);
		
	}
}
