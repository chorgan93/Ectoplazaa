using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartMenu : MonoBehaviour {

	string platformType; 

	static bool started = false; 

	public float inputDelayStart = 1f;
	public float inputDelayTransition = 0.5f;
	private float inputDelay = 2f;


	//public GameObject [] postcards; 

	public GameObject cursorObj;
	float cursorSpeed = 0.2f; 
	public List<GameObject> cursorPositions;
	private bool movedCursor = false;
	public float cursorSensitivity = 0.8f;
	private int currentCursorPos = 0;

	
	public GameObject [] startMenuPoiPos; 

	public GameObject mainMenuCenterPt;
	public GameObject loadingCenterPt;
	public GameObject creditsCenterPt;
	private bool onCredits = false;
	public GameObject optionsCenterPt;
	private bool onOptions = false;

	private CameraFollowS cameraFollow;

	// options menu variables
	public List<GameObject> optionsCursorPositions;
	public List<GameObject> creditsCursorPositions;
	public List<GameObject> creditsCameraPositions;
	private bool movedCursorLeftRight = false;
	public TextMesh musicVolumeDisplay;
	public float musicVolumeChangeStep = 0.25f;
	public TextMesh sfxVolumeDisplay;
	public float sfxVolumeChangeStep = 0.25f;
	public TextMesh screenShakeDisplay;
	public float screenShakeChangeStep = 0.25f;

	
	public GameObject [] optionMenuPoiPos; 

	private bool fullscreenOn = true;
	public TextMesh fullScreenDisplay;

	
	public TextMesh roundNumDisplay;
	
	public TextMesh specialAllowDisplay;
	public TextMesh hazardOnDisplay;

	private bool startedLoading = false;
	private bool startCountdown = false;
	private float delayLoadTime = 0.5f;

	public List<GameObject> scrollSoundObjs;
	public List<GameObject> selectSoundObjs;
	public GameObject advSoundObj;
	public GameObject bellSoundObj;

	private string competitiveNextScene = "7CompetitiveModeSelect";
	private string partyNextScene = "8PartyModeSelect";
	private string nextScene;

	private float holdBTime = 0;
	private float holdBMaxTime = 1;

	private bool showTitle = false;
	public TextMesh flickerText;
	public float flickerTimeMax = 0.8f;
	private float flickerTimeCountdown;
	public GameObject titleCameraPos;
	private bool buttonPressed = false;
	public GameObject bell;
	public GameObject fadeIn;

	public GameObject startObject;
	public GameObject optionSlashEffect;

	public Material greenSpriteMat;
	public Material startSpriteMat;
	public SpriteRenderer[] menuOptionSprites;

	private bool didInitialFade = false;

	private float fadeItemTime = 0.2f;
	public FadeObjS[] fadeObjs;

	public WhiteFlashS whiteFlash;

	AsyncOperation async;

	void Start () 
	{

		flickerTimeCountdown = flickerTimeMax;

		fullscreenOn = Screen.fullScreen;
		inputDelay = inputDelayStart;

		platformType = PlatformS.GetPlatform (); 
		cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollowS>();

		if(started){
			cameraFollow.poi = cursorPositions[0];
			bell.gameObject.SetActive(false);
			flickerText.gameObject.SetActive(false);
			StartCoroutine(FadeMenuItems());
		}
		else{
			
			fadeIn.SetActive(false);
		}

		cursorObj.transform.position = cursorPositions[currentCursorPos].transform.position;

		menuOptionSprites[0].material = greenSpriteMat;
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		// for loading next scene
		
		
		if (startCountdown && !startedLoading){
			delayLoadTime -= Time.deltaTime;
			if (delayLoadTime <= 0){
				StartLoading();
				startedLoading = true;
			}

			inputDelay = 1000f;
		}

		if (startedLoading &&  async.progress >= 0.9f){
			ActivateScene();
		}

		if (!started) 
		{
			flickerTimeCountdown -= Time.deltaTime;

			if (Input.GetButton ("AButtonAllPlayers" + platformType) || Input.GetKey (KeyCode.KeypadEnter)) 
			{
				if (!buttonPressed){

				Instantiate(bellSoundObj);
				showTitle = true;
					CameraShakeS.C.CancelShakeDelay();
				CameraShakeS.C.SmallShake();


					buttonPressed = true;

					startObject.SetActive(true);


				Invoke ("StartGame", 1f); 
				}
			}

			if (buttonPressed){
				
				flickerTimeCountdown -= Time.deltaTime*12f;
			}

			if (flickerTimeCountdown <= 0){
				flickerTimeCountdown = flickerTimeMax;
				flickerText.gameObject.SetActive(!flickerText.gameObject.activeSelf);
			}
		}

		// once started, execute this code

		else{
			inputDelay -= Time.deltaTime;



			if (inputDelay <= 0){

				showTitle = false;
				bell.gameObject.SetActive(false);

				if (didInitialFade){
					cursorObj.SetActive(true);
				}

				// move cursor function
				if (Mathf.Abs(Input.GetAxis("Vertical")) > cursorSensitivity || (Mathf.Abs(Input.GetAxis("Horizontal")) > cursorSensitivity && !onOptions)){
					if (!movedCursor){
						if ((Mathf.Abs(Input.GetAxis("Vertical")) > cursorSensitivity && Input.GetAxis("Vertical") < 0) ||
						    (Mathf.Abs(Input.GetAxis("Horizontal")) > cursorSensitivity && Input.GetAxis("Horizontal") > 0)){
							// add to current level selected
							currentCursorPos ++;
							if (onOptions){
								if (currentCursorPos > optionsCursorPositions.Count-1){
									currentCursorPos = 0;
								}
							}
							else if (onCredits){
								if (currentCursorPos > creditsCursorPositions.Count-1){
									currentCursorPos = 0;
								}
							}
							else{
								if (currentCursorPos > cursorPositions.Count-1){
									currentCursorPos = 0;
								}
							}
						}
						// else subtract from current pos
						else{
							currentCursorPos --;
							if (currentCursorPos < 0){
								if (onOptions){
									currentCursorPos = optionsCursorPositions.Count-1;
								}
								else if (onCredits){
									currentCursorPos = creditsCursorPositions.Count - 1;
								}
								else{
									currentCursorPos = cursorPositions.Count-1;
								}
							}
						}

						int i = 0;
						foreach(SpriteRenderer render in menuOptionSprites){
							if (i == currentCursorPos){
								render.material = greenSpriteMat;
							}
							else{
								render.material = startSpriteMat;
							}
							i++;
						}

						movedCursor = true;

						int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
						Instantiate(scrollSoundObjs[soundToPlay]);
					}
				}
				else{
					movedCursor = false;
				}
			

				// main menu interactions
				if (!onCredits && !onOptions){
	
					// move cursor obj to correct pos
					cursorObj.transform.position = Vector3.Lerp(cursorObj.transform.position, cursorPositions[currentCursorPos].transform.position, cursorSpeed);
					cameraFollow.poi = startMenuPoiPos[currentCursorPos];

					// select menu option
					if (Input.GetButton ("AButtonAllPlayers" + platformType)) 
					{
						// "play" option
						if (currentCursorPos == 0){
							
							CurrentModeS.isTeamMode = false;

							// start level select load
							startCountdown = true;
							cameraFollow.poi = loadingCenterPt;
							nextScene = competitiveNextScene;
							Instantiate(advSoundObj);

							//fadeIn.GetComponent<FadeObjS>().fadeRate*=2f;
							fadeIn.GetComponent<FadeObjS>().FadeOut();


						}

						// team option
						if (currentCursorPos == 1){

							CurrentModeS.isTeamMode = true;

							startCountdown = true;
							cameraFollow.poi = loadingCenterPt;
							nextScene = competitiveNextScene;
							Instantiate(advSoundObj);

							//fadeIn.GetComponent<FadeObjS>().fadeRate*=2f;
							fadeIn.GetComponent<FadeObjS>().FadeOut();

						}
	
						// "options" option
						if (currentCursorPos == 2){
							onOptions = true;
							whiteFlash.StartFlash();
							//cameraFollow.poi = optionsCenterPt;
							inputDelay = inputDelayTransition;
							currentCursorPos = 0;
							int soundToPlay = Mathf.FloorToInt(Random.Range(0,selectSoundObjs.Count));
							Instantiate(selectSoundObjs[soundToPlay]);

						}
		
						// "credits" option
						if (currentCursorPos == 3){
							onCredits = true;
							currentCursorPos = 0;
							whiteFlash.StartFlash();
							//cameraFollow.poi = creditsCenterPt;
							inputDelay = inputDelayTransition;
							int soundToPlay = Mathf.FloorToInt(Random.Range(0,selectSoundObjs.Count));
							Instantiate(selectSoundObjs[soundToPlay]);

						}
					}

					// exit by holding b button
				
					if (Input.GetButton ("BButtonAllPlayers" + platformType)) {
						holdBTime += Time.deltaTime;
					}
					else{
						holdBTime = 0;
					}

					if (holdBTime >= holdBMaxTime){
						Application.Quit();
					}
				}

				else{
					// when not in main menu, give option to go back (catch-all)
					if (Input.GetButton ("BButtonAllPlayers" + platformType)) {
						onCredits = false;
						onOptions = false;
						whiteFlash.StartFlash();
						//cameraFollow.poi = mainMenuCenterPt;
						movedCursor = false;
						currentCursorPos = 0;
						inputDelay = inputDelayTransition;

						int i = 0;
						foreach(SpriteRenderer render in menuOptionSprites){
							if (i == currentCursorPos){
								render.material = greenSpriteMat;
							}
							else{
								render.material = startSpriteMat;
							}
							i++;
						}

						int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
						Instantiate(scrollSoundObjs[soundToPlay]);
					}
					
					// options menu navigation
					if (onOptions){

						// set cursor pos to correct options cursor pos
						cursorObj.transform.position = Vector3.Lerp(cursorObj.transform.position,  optionsCursorPositions[currentCursorPos].transform.position, cursorSpeed);
						cameraFollow.poi = optionMenuPoiPos[currentCursorPos];

						// allow for different options to change 

						// music volume
						if (currentCursorPos == 0){

							// allow movement of option up/down using left/right control

							if (Mathf.Abs(Input.GetAxis("Horizontal")) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("Horizontal") > 0){
										// increase option
										BGMS.bgmVolumeMult += musicVolumeChangeStep;
										if (BGMS.bgmVolumeMult > 1){
											BGMS.bgmVolumeMult = 1;
										}
									}
 									// else decrease option
									else{
										BGMS.bgmVolumeMult -= musicVolumeChangeStep;
										if (BGMS.bgmVolumeMult < 0){
											BGMS.bgmVolumeMult = 0;
										}
									}
									
									movedCursorLeftRight = true;
									int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
									Instantiate(scrollSoundObjs[soundToPlay]);
								}
							}
							else{
								movedCursorLeftRight = false;
							}
						}
						// display current bgm volume mult
						musicVolumeDisplay.text = BGMS.bgmVolumeMult*100 + "%";

						// sfx volume
						if (currentCursorPos == 1){

							// allow movement of option up/down using left/right control
							
							if (Mathf.Abs(Input.GetAxis("Horizontal")) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("Horizontal") > 0){
										// increase option
										SFXObjS.sfxVolumeMult += sfxVolumeChangeStep;
										if (SFXObjS.sfxVolumeMult > 1){
											SFXObjS.sfxVolumeMult = 1;
										}
									}
									// else decrease option
									else{
										SFXObjS.sfxVolumeMult -= sfxVolumeChangeStep;
										if (SFXObjS.sfxVolumeMult < 0){
											SFXObjS.sfxVolumeMult = 0;
										}
									}
									
									movedCursorLeftRight = true;
									int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
									Instantiate(scrollSoundObjs[soundToPlay]);
								}
							}
							else{
								movedCursorLeftRight = false;
							}
							
						}
						// display current bgm volume mult
						sfxVolumeDisplay.text = SFXObjS.sfxVolumeMult*100 + "%";

						// screenshake options
						if (currentCursorPos == 2){
							if (Mathf.Abs(Input.GetAxis("Horizontal")) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("Horizontal") > 0){
										// increase option
										CameraShakeS.shakeStrengthMult += screenShakeChangeStep;
										if (CameraShakeS.shakeStrengthMult > 2){
											CameraShakeS.shakeStrengthMult = 2;
										}
									}
									// else decrease option
									else{
										CameraShakeS.shakeStrengthMult -= screenShakeChangeStep;
										if (CameraShakeS.shakeStrengthMult < 0){
											CameraShakeS.shakeStrengthMult = 0;
										}
									}
									
									movedCursorLeftRight = true;
									int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
									Instantiate(scrollSoundObjs[soundToPlay]);
								}
							}
							else{
								movedCursorLeftRight = false;
							}
						}
						// display current shake mult
						screenShakeDisplay.text = CameraShakeS.shakeStrengthMult*100 + "%";

						// time options
						if (currentCursorPos == 3){
							if (Mathf.Abs(Input.GetAxis("Horizontal")) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("Horizontal") > 0){
										// time off

										CameraShakeS.timeSleepOn = false;
									}
									// else sleep on
									else{

										CameraShakeS.timeSleepOn = true;
									}
									
									movedCursorLeftRight = true;
									int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
									Instantiate(scrollSoundObjs[soundToPlay]);
								}
							}
							else{
								movedCursorLeftRight = false;
							}
						}
						// display current time mult
						if (CameraShakeS.timeSleepOn){
							fullScreenDisplay.text = "On";
						}
						else{
							fullScreenDisplay.text = "Off";
						}

						// rounds options
						if (currentCursorPos == 4){
							if (Mathf.Abs(Input.GetAxis("Horizontal")) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("Horizontal") > 0){
										// increase option
										CurrentModeS.SetNumRounds(CurrentModeS.numRoundsDefault+=1);
										if (CurrentModeS.numRoundsDefault > CurrentModeS.maxRounds){
											CurrentModeS.SetNumRounds(CurrentModeS.maxRounds);
										}
									}
									// else decrease option
									else{
										CurrentModeS.SetNumRounds(CurrentModeS.numRoundsDefault-=1);
										if (CurrentModeS.numRoundsDefault < CurrentModeS.minRounds){
											CurrentModeS.SetNumRounds(CurrentModeS.minRounds);
										}
									}
									
									movedCursorLeftRight = true;
									int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
									Instantiate(scrollSoundObjs[soundToPlay]);
								}
							}
							else{
								movedCursorLeftRight = false;
							}
						}

						// specials options
						if (currentCursorPos == 5){
							if (Mathf.Abs(Input.GetAxis("Horizontal")) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("Horizontal") > 0){
										// increase option
										CurrentModeS.allowSpecials = false;
									}
									// else decrease option
									else{
										
										CurrentModeS.allowSpecials = true;
									}
									
									movedCursorLeftRight = true;
									int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
									Instantiate(scrollSoundObjs[soundToPlay]);
								}
							}
							else{
								movedCursorLeftRight = false;
							}
						}
						// rounds options
						if (currentCursorPos == 6){
							if (Mathf.Abs(Input.GetAxis("Horizontal")) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("Horizontal") > 0){
										// increase option
										CurrentModeS.allowHazards = false;
									}
									// else decrease option
									else{
										
										CurrentModeS.allowHazards = true;
									}
									
									movedCursorLeftRight = true;
									int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
									Instantiate(scrollSoundObjs[soundToPlay]);
								}
							}
							else{
								movedCursorLeftRight = false;
							}
						}

						// display current round amt
						if (CurrentModeS.numRoundsDefault == 1){
							roundNumDisplay.text = "1 WIN";
						}
						else{
							roundNumDisplay.text = CurrentModeS.numRoundsDefault + " WINS";
						}

						// display special allow
						if (CurrentModeS.allowSpecials){
							specialAllowDisplay.text = "ON";
						}
						else{
							specialAllowDisplay.text = "OFF";
						}

						// display hazard allow
						if (CurrentModeS.allowHazards){
							hazardOnDisplay.text = "ON";
						}
						else{
							hazardOnDisplay.text = "OFF";
						}
					}
					if (onCredits){
						cameraFollow.poi = creditsCameraPositions[currentCursorPos];
						cursorObj.transform.position = Vector3.Lerp(cursorObj.transform.position, creditsCursorPositions[currentCursorPos].transform.position, cursorSpeed);

					}
				}
			}
			// when input delay IS active
			else{
				// turn off cursor
					cursorObj.SetActive(false);

				if (showTitle){
					cameraFollow.poi = titleCameraPos;
				}
				}

			}
			
		}

	// for pre loading of level select

	public void StartLoading() {
		StartCoroutine(load ());
	}

	private void StartGame(){
		started = true;
		Instantiate(advSoundObj);
		flickerText.gameObject.SetActive(false);
		fadeIn.SetActive(true);
		Invoke("StartFade", 1f);
	}

	private void StartFade(){

		StartCoroutine(FadeMenuItems());

	}


	private IEnumerator FadeMenuItems(){

		cursorObj.SetActive(false);

		yield return new WaitForSeconds(0.3f);

		foreach(FadeObjS fade in fadeObjs){

			inputDelay = 1000f;
			fade.gameObject.SetActive(true);

			yield return new WaitForSeconds(fadeItemTime);
		}

		inputDelay = 0;
		cursorObj.SetActive(true);

		didInitialFade = true;

	}
	
	IEnumerator load() {
		Debug.LogWarning("ASYNC LOAD STARTED - " +
		                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");



		async = Application.LoadLevelAsync(nextScene);
		async.allowSceneActivation = false;
		yield return async;
	}
	
	public void ActivateScene() {
		async.allowSceneActivation = true;
	}


}
