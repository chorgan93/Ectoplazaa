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

	// the following string is just for playtest
	// will need to connect all of start menu in final version
	public string nextSceneString;

	public GameObject [] postcards; 

	public GameObject cursorObj;
	public List<GameObject> cursorPositions;
	private bool movedCursor = false;
	public float cursorSensitivity = 0.8f;
	private int currentCursorPos = 0;

	public GameObject mainMenuCenterPt;
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

	void Start () 
	{
		inputDelay = inputDelayStart;

		platformType = PlatformS.GetPlatform (); 
		cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollowS>();

		if (started){
			foreach (GameObject postcard in postcards) 
			{
				
				postcard.SetActive(false); 

			}

			inputDelay = 0;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!started) 
		{
			if (Input.GetButton ("AButtonPlayer" + playerNum + platformType) || Input.GetKey (KeyCode.KeypadEnter)) 
			{
				started = true;
				foreach (GameObject postcard in postcards) 
				{

					postcard.GetComponent<Rigidbody> ().AddForce (Vector3.right * Random.Range(5000f,10000f)); 
					postcard.GetComponent<Rigidbody> ().AddTorque (Vector3.forward * Random.Range(100000f,200000f)); 
				}
			}
		}

		// once started, execute this code

		else{
			inputDelay -= Time.deltaTime;



			if (inputDelay <= 0){

				cursorObj.SetActive(true);

				// move cursor function
				if (Mathf.Abs(Input.GetAxis("VerticalPlayer" + playerNum + platformType)) > cursorSensitivity){
					if (!movedCursor){
						if (Input.GetAxis("VerticalPlayer" + playerNum + platformType) < 0){
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
					}
				}
				else{
					movedCursor = false;
				}
			

				// main menu interactions
				if (!onCredits && !onOptions){
	
					// move cursor obj to correct pos
					cursorObj.transform.position = cursorPositions[currentCursorPos].transform.position;

					// select menu option
					if (Input.GetButton ("AButtonPlayer" + playerNum + platformType) 
					    || Input.GetKey (KeyCode.KeypadEnter)) 
					{
						// "play" option
						if (currentCursorPos == 0){
							Application.LoadLevel(nextSceneString);
						}
	
						// "options" option
						if (currentCursorPos == 1){
							onOptions = true;
							cameraFollow.poi = optionsCenterPt;
							inputDelay = inputDelayTransition;
							currentCursorPos = 0;
						}
		
						// "credits" option
						if (currentCursorPos == 2){
							onCredits = true;
							cameraFollow.poi = creditsCenterPt;
							inputDelay = inputDelayTransition;
						}
					}
				}

				else{
					// when not in main menu, give option to go back (catch-all)
					if (Input.GetButton ("XButtonPlayer" + playerNum + platformType)) {
						onCredits = false;
						onOptions = false;
						cameraFollow.poi = mainMenuCenterPt;
						movedCursor = false;
						currentCursorPos = 0;
						inputDelay = inputDelayTransition;
					}
					
					// options menu navigation
					if (onOptions){

						// set cursor pos to correct options cursor pos
						cursorObj.transform.position = optionsCursorPositions[currentCursorPos].transform.position;

						// allow for different options to change 

						// music volume
						if (currentCursorPos == 0){

							// allow movement of option up/down using left/right control

							if (Mathf.Abs(Input.GetAxis("HorizontalPlayer" + playerNum + platformType)) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("HorizontalPlayer" + playerNum + platformType) > 0){
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
							
							if (Mathf.Abs(Input.GetAxis("HorizontalPlayer" + playerNum + platformType)) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("HorizontalPlayer" + playerNum + platformType) > 0){
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
							if (Mathf.Abs(Input.GetAxis("HorizontalPlayer" + playerNum + platformType)) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("HorizontalPlayer" + playerNum + platformType) > 0){
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
							if (Mathf.Abs(Input.GetAxis("HorizontalPlayer" + playerNum + platformType)) 
							    > cursorSensitivity){
								if (!movedCursorLeftRight){
									if (Input.GetAxis("HorizontalPlayer" + playerNum + platformType) > 0){
										// fullscreen off
										Screen.SetResolution(Screen.width, Screen.height, false);

										fullscreenOn = false;
									}
									// else fullscreen on
									else{
										
										Screen.SetResolution(Screen.width, Screen.height, true);
										fullscreenOn = true;
									}
									
									movedCursorLeftRight = true;
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


}
