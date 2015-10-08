using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartMenu : MonoBehaviour {

	string platformType; 
	int playerNum = 1; 

	static bool started = false; 

	public float inputDelayStart = 1f;
	public float inputDelayTransition = 0.5f;
	private float inputDelay = 2f;


	public GameObject [] postcards; 

	public GameObject cursorObj;
	float cursorSpeed = 0.2f; 
	public List<GameObject> cursorPositions;
	private bool movedCursor = false;
	public float cursorSensitivity = 0.8f;
	private int currentCursorPos = 0;

	public GameObject mainMenuCenterPt;
	public GameObject loadingCenterPt;
	public GameObject creditsCenterPt;
	private bool onCredits = false;
	public GameObject optionsCenterPt;
	private bool onOptions = false;

	private CameraFollowS cameraFollow;

	// options menu variables
	public List<GameObject> optionsCursorPositions;
	private bool movedCursorLeftRight = false;
	public TextMesh musicVolumeDisplay;
	public float musicVolumeChangeStep = 0.25f;
	public TextMesh sfxVolumeDisplay;
	public float sfxVolumeChangeStep = 0.25f;
	public TextMesh screenShakeDisplay;
	public float screenShakeChangeStep = 0.25f;

	private bool fullscreenOn = true;
	public TextMesh fullScreenDisplay;

	private bool startedLoading = false;
	private bool startCountdown = false;
	private float delayLoadTime = 0.5f;

	public List<GameObject> scrollSoundObjs;
	public List<GameObject> selectSoundObjs;
	public GameObject advSoundObj;

	private string competitiveNextScene = "7CompetitiveModeSelect";
	private string partyNextScene = "8PartyModeSelect";
	private string nextScene;

	AsyncOperation async;

	void Start () 
	{

		fullscreenOn = Screen.fullScreen;
		inputDelay = inputDelayStart;

		platformType = PlatformS.GetPlatform (); 
		cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollowS>();

		if (started){
			foreach (GameObject postcard in postcards) 
			{
				
				postcard.SetActive(false); 

			}
			Instantiate(selectSoundObjs[0]);
			inputDelay = 0;
		}

		cursorObj.transform.position = cursorPositions[currentCursorPos].transform.position;
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
			if (Input.GetButton ("AButtonAllPlayers" + platformType) || Input.GetKey (KeyCode.KeypadEnter)) 
			{
				started = true;
				int i = 1; 
				foreach (GameObject postcard in postcards) 
				{
					int direction = 0; 
					if(i>=0)
						direction = 1; 
					else
						direction = -1; 

					postcard.GetComponent<Rigidbody> ().AddForce (Vector3.right * Random.Range(200000,400000f)*direction*Time.deltaTime); 
					postcard.GetComponent<Rigidbody> ().AddTorque (Vector3.forward * Random.Range(200000000f,400000000f)*direction*Time.deltaTime); 
					i--; 
				}

				Instantiate(advSoundObj);
			}
		}

		// once started, execute this code

		else{
			inputDelay -= Time.deltaTime;



			if (inputDelay <= 0){

				cursorObj.SetActive(true);

				// move cursor function
				if (Mathf.Abs(Input.GetAxis("Vertical")) > cursorSensitivity){
					if (!movedCursor){
						if (Input.GetAxis("Vertical") < 0){
							// add to current level selected
							currentCursorPos ++;
							if (onOptions){
								if (currentCursorPos > optionsCursorPositions.Count-1){
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
								else{
									currentCursorPos = cursorPositions.Count-1;
								}
							}
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

					// select menu option
					if (Input.GetButton ("AButtonAllPlayers" + platformType) 
					    || Input.GetKey (KeyCode.KeypadEnter)) 
					{
						// "play" option
						if (currentCursorPos == 0){
							//Application.LoadLevel(nextSceneString);

							// start level select load
							//StartLoading();
							//startedLoading = true;
							startCountdown = true;
							cameraFollow.poi = loadingCenterPt;
							nextScene = competitiveNextScene;
							Instantiate(advSoundObj);
						}

						// "party" option
						if (currentCursorPos == 1){
							//Application.LoadLevel(nextSceneString);
							
							// start level select load
							//StartLoading();
							//startedLoading = true;
							startCountdown = true;
							cameraFollow.poi = loadingCenterPt;
							nextScene = partyNextScene;
							Instantiate(advSoundObj);
						}
	
						// "options" option
						if (currentCursorPos == 2){
							onOptions = true;
							cameraFollow.poi = optionsCenterPt;
							inputDelay = inputDelayTransition;
							currentCursorPos = 0;
							int soundToPlay = Mathf.FloorToInt(Random.Range(0,selectSoundObjs.Count));
							Instantiate(selectSoundObjs[soundToPlay]);
						}
		
						// "credits" option
						if (currentCursorPos == 3){
							onCredits = true;
							cameraFollow.poi = creditsCenterPt;
							inputDelay = inputDelayTransition;
							int soundToPlay = Mathf.FloorToInt(Random.Range(0,selectSoundObjs.Count));
							Instantiate(selectSoundObjs[soundToPlay]);
						}
					}
				}

				else{
					// when not in main menu, give option to go back (catch-all)
					if (Input.GetButton ("BButtonAllPlayers" + platformType)) {
						onCredits = false;
						onOptions = false;
						cameraFollow.poi = mainMenuCenterPt;
						movedCursor = false;
						currentCursorPos = 0;
						inputDelay = inputDelayTransition;

						int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSoundObjs.Count));
						Instantiate(scrollSoundObjs[soundToPlay]);
					}
					
					// options menu navigation
					if (onOptions){

						// set cursor pos to correct options cursor pos
						cursorObj.transform.position = Vector3.Lerp(cursorObj.transform.position,  optionsCursorPositions[currentCursorPos].transform.position, cursorSpeed);

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

						// fullscreen options
						if (currentCursorPos == 3){
							if (Mathf.Abs(Input.GetAxis("Horizontal")) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("Horizontal") > 0){
										// fullscreen off
										//Screen.SetResolution(Screen.width, Screen.height, false);

										fullscreenOn = false;
										Screen.fullScreen = fullscreenOn;
									}
									// else fullscreen on
									else{
										
										//Screen.SetResolution(Screen.width, Screen.height, true);
										fullscreenOn = true;
										Screen.fullScreen = fullscreenOn;
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
						// display current fullscreen mult
						if (fullscreenOn){
							fullScreenDisplay.text = "On";
						}
						else{
							fullScreenDisplay.text = "Off";
						}
					}
				}
			}
			// when input delay IS active
			else{
				// turn off cursor
					cursorObj.SetActive(false);
				}

			}
			
		}

	// for pre loading of level select

	public void StartLoading() {
		StartCoroutine("load");
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
