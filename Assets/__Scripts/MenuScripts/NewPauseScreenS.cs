using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NewPauseScreenS : MonoBehaviour {
	
	private Image ownRender;
	private bool pauseActive = false;
	
	private int playerWhoPaused = 0;
	private bool pauseButtonDownP1 = false;
	private bool pauseButtonDownP2 = false;
	private bool pauseButtonDownP3 = false;
	private bool pauseButtonDownP4 = false;
	
	private string platformType;
	
	private bool movedCursor;
	public GameObject cursorObj;
	public List<GameObject> cursorPositions;
	private int currentCursorPos = 0;
	
	public List<GameObject> allTextHolder;
	public Text playerPauseText;
	
	private string menuSceneString = "1StartMenu";
	
	private WiiUControllerManagerS wiiUInput2;
	private WiiUControllerManagerS wiiUInput3;
	private WiiUControllerManagerS wiiUInput4;

	public GameObject startSFX;
	public GameObject selectSFX;
	public GameObject moveSFX;
	public GameObject endSFX;
	
	// Use this for initialization
	void Start () {
		
		platformType = PlatformS.GetPlatform();
		ownRender = GetComponent<Image>();
		
		// turn off cursor obj
		cursorObj.SetActive(false);
		TurnOffText();
		
		
		currentCursorPos = 0;
		ownRender.enabled = false;
		
		#if UNITY_WIIU
		wiiUInput2 = GameObject.Find("WiiUControlHandlerPlayer2").GetComponent<WiiUControllerManagerS>();
		wiiUInput3 = GameObject.Find("WiiUControlHandlerPlayer3").GetComponent<WiiUControllerManagerS>();
		wiiUInput4 = GameObject.Find("WiiUControlHandlerPlayer4").GetComponent<WiiUControllerManagerS>();
		#endif
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (ScoreKeeperS.gameStarted && !ScoreKeeperS.gameEnd){
			
			// check for player to pause
			for(int i = 1; i < 4; i++){
				
				// this is super gross but im doing it because get button down doesn't want to work for some reason
				if (i == 1){
					#if UNITY_WIIU
					if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
						#else
						if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
							#endif
							pauseButtonDownP1 = false;
						}
						
						if (!pauseButtonDownP1){
							#if UNITY_WIIU
							if (Input.GetButton("StartButtonPlayer"+i+platformType)){
								#else
								if (Input.GetButton("StartButtonPlayer"+i+platformType)){
									#endif
									if (!pauseActive){
										pauseActive = true;
										playerWhoPaused = i;
										pauseButtonDownP1 = true;

										if (startSFX != null){
											Instantiate(startSFX);
										}
										
										// dont allow camera shake
										CameraShakeS.C.sleeping = true;
									}
									else{
										if (playerWhoPaused == i){
											pauseActive = false;
											pauseButtonDownP1 = true;
										
										
											// allow camera shake
											CameraShakeS.C.sleeping = false;
											Time.timeScale = 1;
										}
									}
								}
							}
						}
						
						if (i == 2){
							#if UNITY_WIIU
							if (!wiiUInput2.pauseButtonDown){
								#else
								if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
									#endif
									pauseButtonDownP2 = false;
								}
								
								if (!pauseButtonDownP2){
									#if UNITY_WIIU
									if (wiiUInput2.pauseButtonDown){
										#else
										if (Input.GetButton("StartButtonPlayer"+i+platformType)){
											#endif
											if (!pauseActive){
												pauseActive = true;
												playerWhoPaused = i;
												pauseButtonDownP2 = true;

												if (startSFX != null){
													Instantiate(startSFX);
												}
												
												// dont allow camera shake
												CameraShakeS.C.sleeping = true;
											}
											else{
												if (playerWhoPaused == i){
													pauseActive = false;
													pauseButtonDownP2 = true;
													
													
													// allow camera shake
													CameraShakeS.C.sleeping = false;
													Time.timeScale = 1;
												}
											}
										}
									}
								}
								
								if (i == 3){
									#if UNITY_WIIU
									if (!wiiUInput3.pauseButtonDown){
										#else
										if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
											#endif
											pauseButtonDownP3 = false;
										}
										
										if (!pauseButtonDownP3){
											#if UNITY_WIIU
											if (wiiUInput3.pauseButtonDown){
												#else
												if (Input.GetButton("StartButtonPlayer"+i+platformType)){
													#endif
													if (!pauseActive){
														pauseActive = true;
														playerWhoPaused = i;
														pauseButtonDownP3 = true;

														if (startSFX != null){
															Instantiate(startSFX);
														}
														
														
														// dont allow camera shake
														CameraShakeS.C.sleeping = true;
													}
													else{
														if (playerWhoPaused == i){
															pauseActive = false;
															pauseButtonDownP3 = true;
															
															
															// allow camera shake
															CameraShakeS.C.sleeping = false;
															Time.timeScale = 1;
														}
													}
												}
											}
										}
										
										if (i == 4){
											#if UNITY_WIIU
											if (!wiiUInput4.pauseButtonDown){
												#else
												if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
													#endif
													pauseButtonDownP4 = false;
												}
												
												if (!pauseButtonDownP4){
													#if UNITY_WIIU
													if (wiiUInput4.pauseButtonDown){
														#else
														if (Input.GetButton("StartButtonPlayer"+i+platformType)){
															#endif
															if (!pauseActive){
																pauseActive = true;
																playerWhoPaused = i;
																pauseButtonDownP4 = true;

																if (startSFX != null){
																	Instantiate(startSFX);
																}
																
																// turn off camera shake
																CameraShakeS.C.sleeping = true;
															}
															else{
																if (playerWhoPaused == i){
																	pauseActive = false;
																	pauseButtonDownP4 = true;
																	
																	
																	// allow camera shake
																	CameraShakeS.C.sleeping = false;
																	Time.timeScale = 1;
																}
															}
														}
													}
												}
												
												
											}
											
											if (pauseActive){
												Time.timeScale = 0;
												//CameraShakeS.C.shaking = false;
												ownRender.enabled = true;
												
												// turn on cursor obj
												cursorObj.SetActive(true);
												TurnOnText();
												
												
												// display who paused
												playerPauseText.text = "P" + playerWhoPaused + " Pause";
												
												// check for option input
												#if UNITY_WIIU
												if ((playerWhoPaused == 1 && Mathf.Abs(Input.GetAxis("VerticalPlayer" + playerWhoPaused + platformType))
												     > 0.8f) || 
												    (playerWhoPaused == 2 && Mathf.Abs(wiiUInput2.verticalAxis)
												 > 0.8f) || 
												    (playerWhoPaused == 3 && Mathf.Abs(wiiUInput3.verticalAxis)
												 > 0.8f) || 
												    (playerWhoPaused == 4 && Mathf.Abs(wiiUInput4.verticalAxis)
												 > 0.8f)){
													#else
													if (Mathf.Abs(Input.GetAxis("VerticalPlayer" + playerWhoPaused + platformType))
													    > 0.8f){
														#endif
														if (!movedCursor){
															
															#if UNITY_WIIU
															if ((playerWhoPaused == 1 && Input.GetAxis("VerticalPlayer" + playerWhoPaused + platformType)
															     > 0f) || 
															    (playerWhoPaused == 2 && wiiUInput2.verticalAxis
															 > 0f) || 
															    (playerWhoPaused == 3 && wiiUInput3.verticalAxis
															 > 0f) || 
															    (playerWhoPaused == 4 && wiiUInput4.verticalAxis
															 > 0f)){
																#else
																if (Input.GetAxis("VerticalPlayer" + playerWhoPaused + platformType)
																    > 0f){
																	#endif
																	currentCursorPos --;
																	if (currentCursorPos < 0){
																		currentCursorPos = cursorPositions.Count-1;
																	}
																}
																else{
																	currentCursorPos ++;
																	if (currentCursorPos > cursorPositions.Count-1){
																		currentCursorPos = 0;
																	}
																}
																
																movedCursor = true;
																if (moveSFX != null){
																	Instantiate(moveSFX);
																}
															}
														}
														else{
															movedCursor = false;
														}
														
														// move cursor to correct spot
														cursorObj.transform.position = cursorPositions[currentCursorPos].transform.position;
														
														// continue option
														if (currentCursorPos == 0){
															#if UNITY_WIIU
															if ((playerWhoPaused == 1 && Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)) ||
															    (playerWhoPaused == 2 && wiiUInput2.jumpButtonDown) || 
															    (playerWhoPaused == 3 && wiiUInput3.jumpButtonDown) ||
															    (playerWhoPaused == 4 && wiiUInput4.jumpButtonDown) ){
																#else
																if (Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)){
																	#endif
																	pauseActive = false;
																	//pauseButtonDownP4 = true;
																	
																	
																	// allow camera shake
																	CameraShakeS.C.sleeping = false;
																	Time.timeScale = 1;

																	if (selectSFX != null){
																		Instantiate(selectSFX);
																	}
																}
															}
															
															// restart option
															if (currentCursorPos == 1){
																#if UNITY_WIIU
																if ((playerWhoPaused == 1 && Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)) ||
																    (playerWhoPaused == 2 && wiiUInput2.jumpButtonDown) || 
																    (playerWhoPaused == 3 && wiiUInput3.jumpButtonDown) ||
																    (playerWhoPaused == 4 && wiiUInput4.jumpButtonDown)){
																	#else
																	if (Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)){
																		#endif
																		//pauseActive = false;
																		//pauseButtonDownP4 = true;
																		
																		// reset scene
																		CurrentModeS.ResetWinRecord();
																		Application.LoadLevel(Application.loadedLevel);
																		
																		// allow camera shake
																		CameraShakeS.C.sleeping = false;
																		Time.timeScale = 1;

																		if (selectSFX != null){
																			Instantiate(selectSFX);
																		}
																	}
																}
																
																// exit to main menu option
																if (currentCursorPos == 2){
																	#if UNITY_WIIU
																	if ((playerWhoPaused == 1 && Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)) ||
																	    (playerWhoPaused == 2 && wiiUInput2.jumpButtonDown) || 
																	    (playerWhoPaused == 3 && wiiUInput3.jumpButtonDown) ||
																	    (playerWhoPaused == 4 && wiiUInput4.jumpButtonDown)){
																		#else
																		if (Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)){
																			#endif
																			//pauseActive = false;
																			//pauseButtonDownP4 = true;
																			
																			CurrentModeS.ResetWinRecord();
																			CurrentModeS.isTeamMode = false;
																			Application.LoadLevel(menuSceneString);
																			
																			// allow camera shake
																			CameraShakeS.C.sleeping = false;
																			Time.timeScale = 1;

																			if (endSFX != null){
																				Instantiate(endSFX);
																			}
																		}
																	}
																	
																}
																else{
																	
																	// turn off cursor obj
																	cursorObj.SetActive(false);
																	TurnOffText();
																	
																	
																	currentCursorPos = 0;
																	ownRender.enabled = false;
																}
															}
															
															
														}
														
	private void TurnOnText(){


	
		foreach (GameObject textObj in allTextHolder){
			textObj.SetActive(true);
		}
															
	}
														
														private void TurnOffText(){
															
															foreach (GameObject textObj in allTextHolder){
																textObj.SetActive(false);
															}
															
														}
													}
