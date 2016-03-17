using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectMenu : MonoBehaviour {
	
	public GameObject playerPrefab; 
	
	string platformType; 
	int playersChoosing = 0;
	
	public bool[] hasJoined = new bool[4]; // for inital # of player select
	public bool[] hasSelected = new bool[4]; // adding extra step of character select
	public GameObject [] joinTexts; 
	public GameObject [] spawnPoints;
	
	public List<GameObject> blackScreens;
	
	GameObject [] players = new GameObject[4]; 
	
	public string nextLevelString;
	public string backSceneString;
	
	public GameObject loadPt;
	private CameraFollowS followRef;
	
	private float inputDelay = 0.8f;
	private float loadDelay = 0.5f;
	private bool startedLoadDelay = false;
	private bool startedLoading = false;
	AsyncOperation async;
	
	
	// description texts for each character (name and ability)
	private string character1Text;
	private string character2Text;
	private string character3Text;
	private string character4Text;
	
	public GameObject selectSFXObj;
	
	// to make sure we're only reading button taps
	private bool [] buttonDown = new bool[4];
	private bool [] yButtonDown = new bool[4];
	private bool [] selectUp = new bool[4];
	private float selectThreshold = 0.2f;
	
	private WiiUControllerManagerS wiiUInput2;
	private WiiUControllerManagerS wiiUInput3;
	private WiiUControllerManagerS wiiUInput4;
	
	int totalPlayers = 0; 
	
	// Use this for initialization
	void Start () {
		
		platformType = PlatformS.GetPlatform (); 
		
		
		//nextLevelString = LevelSelectMenu.selectedLevelString;
		
		followRef = GetComponent<CameraFollowS>();
		
		ScoreKeeperS.gameStarted = true;
		
		
		#if UNITY_WIIU
		wiiUInput2 = GameObject.Find("WiiUControlHandlerPlayer" + 2).GetComponent<WiiUControllerManagerS>();
		wiiUInput3 = GameObject.Find("WiiUControlHandlerPlayer" + 3).GetComponent<WiiUControllerManagerS>();
		wiiUInput4 = GameObject.Find("WiiUControlHandlerPlayer" + 4).GetComponent<WiiUControllerManagerS>();
		
		#endif
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		inputDelay -= Time.deltaTime;
		if (inputDelay <= 0){
			
			if (startedLoading){
				if (async.progress >= 0.9f){
					ActivateScene();
				}
				CameraShakeS.C.DisableShaking();
			}
			
			else if (startedLoadDelay){
				loadDelay -= Time.deltaTime;
				followRef.poi = loadPt;
				if (loadDelay <= 0){
					startedLoading = true;
					StartLoading();
				}
				
				CameraShakeS.C.DisableShaking();
			}
			else{
				// back function, only leave if no one has joined
				playersChoosing = 0;
				for (int j = 0; j < 4; j++){
					
					if (hasJoined[j]){
						playersChoosing++;
					}
				}
				
				//print (playersChoosing + " : " + totalPlayers);
				
				if (Input.GetButtonDown("BButtonAllPlayers" + platformType)){
					if (totalPlayers <= 0 && playersChoosing ==0){
						Application.LoadLevel(backSceneString);
					}
				}
				
				
				
				
				for(int i = 1; i <= 4; i++)
				{
					
					
					// A BUTTON CODE, FOR SELECTION
					
					#if UNITY_WIIU
					if (((i == 1 && Input.GetButton ("AButtonPlayer" + i + platformType)) ||
					     (i > 1 && GetWiiUJumpButton(i))) && !buttonDown[i-1])
						#else
						if (Input.GetButton ("AButtonPlayer" + i + platformType) && !buttonDown[i-1])  //ADD PLAYER---------------------------------------
							
							#endif
					{
						
						// press button
						buttonDown[i-1] = true;
						
						//print ("AButtonPlayer" + i + platformType);
						
						//  for actual character selection (after player buys in)
						if (!hasSelected[i-1] && hasJoined[i-1]){
							
							
							// a to confirm and add
							players[i-1].GetComponent<PlayerS>().nonActive = false;
							players[i-1].GetComponent<PlayerS>().hasDoubleJumped = true;
							players[i-1].GetComponent<Rigidbody>().useGravity = true;
							
							hasSelected[i-1] = true;
							
							// take out the black overlay
							blackScreens[i-1].SetActive(false);
							
							// play char intro sound
							totalPlayers ++;
							//players[i-1].GetComponent<PlayerSoundS>().PlayPlayerJoinSound(totalPlayers);
							
							
						}
						else if(!hasJoined[i-1])
						{
							int defaultSkin = 0;
							
							int numJoined = 0;
							
							foreach (bool player in hasJoined){
								if (player){
									numJoined++;
								}
							}
							
							if(numJoined == 0)
							{
								//print("First Player Skinned"); 
								defaultSkin = 1; 
							}
							else
							{
								
								for(int j = 1; j <= GlobalVars.totalSkins; j++)
								{
									
									bool flag = false;
									
									foreach(GameObject player in players)
									{
										if(player != null)
										{
											if(player.GetComponent<PlayerS>().characterNum != j)
											{
												continue;
											}
											else
											{
												flag = true;
												break;
											}
										}
									}
									
									if(!flag)
									{
										defaultSkin = j; 
										break;
									}
								}
							}

							
							hasJoined[i-1] = true; 
							//totalPlayers += 1; 
							
							if (CurrentModeS.isTeamMode){
								if (i < 3){
									GlobalVars.teamNumber[i-1] = 1;
								}
								else{
									GlobalVars.teamNumber[i-1] = 2;
								}
							}
							
							GameObject newPlayer = Instantiate(playerPrefab,spawnPoints[i-1].transform.position,Quaternion.identity) 
								as GameObject;
							PlayerS newPlayerS = newPlayer.GetComponent<PlayerS>();
							newPlayerS.TurnOnCharSelect();
							newPlayerS.playerNum = i;
							newPlayerS.characterNum = defaultSkin;
							newPlayerS.respawnInvulnTime = 0;
							newPlayerS.SetSkin(); 
							
							GlobalVars.characterNumber[i-1] = newPlayerS.characterNum;
							
							newPlayerS.spawnPt = spawnPoints[i-1];
							players[i-1] = newPlayer; 
							
							// disable player before second selection
							newPlayerS.nonActive = true;
							newPlayerS.GetComponent<Rigidbody>().useGravity = false;
							
							// add in the black overlay
							blackScreens[i-1].SetActive(true);
							
							Renderer [] renderers = joinTexts[i-1].GetComponentsInChildren<Renderer>();
							
							foreach(Renderer r in renderers)
							{
								r.enabled = false; 
							}
							
							
							// play char intro sound
							//players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();
							//players[i-1].GetComponent<PlayerSoundS>().PlayPlayerJoinSound(totalPlayers-1);
							//print (players[i-1].GetComponent<PlayerS>().characterNum);
							
							
						}
					}
					#if UNITY_WIIU
					if((i == 1 && !Input.GetButton ("AButtonPlayer" + i + platformType)) || 
					   (i > 1 && !GetWiiUJumpButton(i))){
						#else
						if(!Input.GetButton ("AButtonPlayer" + i + platformType)){
							#endif
							buttonDown[i-1] = false;
						}
						
						// B BUTTON CODE, FOR DESELECTION
						
						#if UNITY_WIIU
						if((i == 1 && Input.GetButton ("BButtonPlayer" + i + platformType)) || 
						   (i > 1 && GetWiiUAttackButton(i)))
							#else
							if (Input.GetButton ("BButtonPlayer" + i + platformType)) //REMOVE PLAYER---------------------------------------
								#endif
						{
							if(hasJoined[i-1])
							{
								hasJoined[i-1] = false; 
								hasSelected[i-1] = false;
								totalPlayers -= 1; 
								if (totalPlayers <= 0){
									totalPlayers = 0;
								}
								
								if (CurrentModeS.isTeamMode){
									
									GlobalVars.teamNumber[i-1] = 0;
									
								}
								
								GameObject.Destroy( players[i-1].gameObject);
								players[i-1]= null; 
								
								// take out the black overlay
								blackScreens[i-1].SetActive(false);
								
								//print("Total Players: " + totalPlayers); 

								
								Renderer [] renderers = joinTexts[i-1].GetComponentsInChildren<Renderer>();
								
								foreach(Renderer r in renderers)
								{
									r.enabled = true; 
								}
							}
						}
						
						// LEFT SELECT CODE, FOR CHANGING CHARACTER
						#if UNITY_WIIU
						if (((i == 1 && Input.GetAxis("HorizontalPlayer" + i + platformType) < -selectThreshold)
						     || (i >1 && GetWiiUHorizontal(i) < -selectThreshold)) && !selectUp[i-1]){
							#else
							if (Input.GetAxis("HorizontalPlayer" + i + platformType) < -selectThreshold && !selectUp[i-1]){
								#endif
								selectUp [i-1] = true;
								
								// reappropriating Old Y Code for Character Select
								if(hasJoined[i-1] && !hasSelected[i-1])
								{
									
									int newSkin = players[i-1].GetComponent<PlayerS>().characterNum; 
									int newColor = 0;
									
									for(int j= 1; j <= GlobalVars.totalSkins; j++) //loop once through all skins
									{
										newSkin -= 1; //increment to next skin, check if available; 
										if(newSkin < 1) //loop if at end of skins
											newSkin = GlobalVars.totalSkins; 
										
										bool flag = false; 
										
										foreach(GameObject p in players)
										{
											if(p != null)
											{
												if(p.GetComponent<PlayerS>().characterNum != newSkin){
													newColor = 0;
													continue;
												}
												else
												{
													newColor = p.GetComponent<PlayerS>().colorNum+1;
													//flag = true;
													continue;
												}
											}
										}
										
										if(!flag)
											break;
										
									}
									

									
									
									players[i-1].GetComponent<PlayerS>().characterNum = newSkin;
									GlobalVars.characterNumber[i-1] = newSkin;
									players[i-1].GetComponent<PlayerS>().colorNum = newColor;
									players[i-1].GetComponent<PlayerS>().SetSkin();
									
									//print(newColor);
									
									
									// play char intro sound
									//players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();
									players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSoundQuickFix();
								}
							}
							// RIGHT SELECT CODE, FOR CHANGING CHARACTER
							#if UNITY_WIIU
							if (((i == 1 && Input.GetAxis("HorizontalPlayer" + i + platformType) > selectThreshold)
							     || (i >1 && GetWiiUHorizontal(i) > selectThreshold)) && !selectUp[i-1]){
								#else
								if (Input.GetAxis("HorizontalPlayer" + i + platformType) > selectThreshold && !selectUp[i-1]){
									#endif
									selectUp [i-1] = true;
									
									// reappropriating Old Y Code for Character Select
									if(hasJoined[i-1] && !hasSelected[i-1])
									{
										
										int newSkin = players[i-1].GetComponent<PlayerS>().characterNum; 
										int newColor = 0;
										
										for(int j= 1; j <= GlobalVars.totalSkins; j++) //loop once through all skins
										{
											newSkin += 1; //increment to next skin, check if available; 
											if(newSkin > GlobalVars.totalSkins) //loop if at end of skins
												newSkin = 1; 
											
											bool flag = false; 
											
											foreach(GameObject p in players)
											{
												if(p != null)
												{
													if(p.GetComponent<PlayerS>().characterNum != newSkin){
														newColor = 0;
														continue;
													}
													else
													{
														newColor = p.GetComponent<PlayerS>().colorNum+1;
														//flag = true;
														continue;
													}
												}
											}
											
											if(!flag)
												break;
											
										}
										

										
										players[i-1].GetComponent<PlayerS>().characterNum = newSkin;
										GlobalVars.characterNumber[i-1] = newSkin;
										players[i-1].GetComponent<PlayerS>().colorNum = newColor;
										players[i-1].GetComponent<PlayerS>().SetSkin();
										
										// print (newColor);
										
										// play char intro sound
										//players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();
										players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSoundQuickFix();
									}
								}
								// FOR ANALOG AT ZERO
								#if UNITY_WIIU
								if ((i == 1 && Mathf.Abs(Input.GetAxis("HorizontalPlayer" + i + platformType)) < selectThreshold) ||
								    (i > 1 && Mathf.Abs(GetWiiUHorizontal(i)) < selectThreshold)) {
									#else
									if (Mathf.Abs(Input.GetAxis("HorizontalPlayer" + i + platformType)) < selectThreshold) {
										#endif
										selectUp[i-1] = false;
									}
									
									// Y BUTTON CODE, FOR CHANGING COLOR (not active yet)
									
									#if UNITY_WIIU
									if (((i == 1 && Input.GetButton ("YButtonPlayer" + i + platformType)) ||
									     (i > 1 && GetWiiUDashButton(i))) && !yButtonDown[i-1])
										#else
										if (Input.GetButton ("YButtonPlayer" + i + platformType) && !yButtonDown[i-1])
											#endif
									{
										
										yButtonDown[i-1] = true;
										
										
										if(hasJoined[i-1] && !hasSelected[i-1])
										{
											
											if (!CurrentModeS.isTeamMode){
												
												//int newColor = players[i-1].GetComponent<PlayerS>().colorNum;
												int newColor = 0;
												for(int j= 0; j < GlobalVars.totalSkins; j++) //loop once through all skins
												{
													newColor += 0; //increment to next skin, check if available; 
													if(newColor > GlobalVars.totalSkins-1) //loop if at end of skins
														newColor = 0; 
													
													bool flag = false; 
													
													foreach(GameObject p in players)
													{
														if(p != null)
														{
															if(p.GetComponent<PlayerS>().characterNum != players[i-1].GetComponent<PlayerS>().characterNum)
																continue;
															else
															{
																if (p.GetComponent<PlayerS>().colorNum != newColor){
																	continue;
																}
																else{
																	flag = true;
																	break;
																}
															}
														}
													}
													
													if(!flag)
														break;
													
												}
												
												players[i-1].GetComponent<PlayerS>().colorNum = newColor;
												players[i-1].GetComponent<PlayerS>().SetSkin();
											}
											else{
												if (GlobalVars.IsRedTeam(i)){
													GlobalVars.teamNumber[i-1] = 2;
												}
												else{
													GlobalVars.teamNumber[i-1] = 1;
												}
												
												Debug.Log(players[i-1]);
												players[i-1].GetComponent<PlayerS>().GetAnimObj().RefreshTeamColor();
											}
										}
										
										
										
										
									}
									#if UNITY_WIIU
									if((i == 1 && !Input.GetButton ("YButtonPlayer" + i + platformType)) ||
									   (i > 1 && !GetWiiUDashButton(i))){
										#else
										if(!Input.GetButton ("YButtonPlayer" + i + platformType)){
											#endif
											yButtonDown[i-1] = false;
										}
										
										// START BUTTON CODE FOR GAME START
										#if UNITY_WIIU
										if ((Input.GetButton ("StartButtonAllPlayers"+ platformType) || 
										     i > 1 && GetWiiUPauseButton(i)) && 
										    ((totalPlayers >= 2 && !CurrentModeS.isTeamMode) || 
										 (totalPlayers >= 2 && CurrentModeS.isTeamMode && GlobalVars.ValidTeams()))){
											#else
										if (Input.GetButton ("StartButtonAllPlayers"+ platformType) && 
										    ((totalPlayers >= 2 && !CurrentModeS.isTeamMode) || 
										 (totalPlayers >= 2 && CurrentModeS.isTeamMode && GlobalVars.ValidTeams()))) //START GAME---------------------------------------
										{
#endif
											Debug.Log("Start game!!");
											//SET GLOBAL VARS
											GlobalVars.totalPlayers = totalPlayers; 
											
											for(int j = 0; j <= 3; j++)
											{
												if(players[j] != null)
												{
													GlobalVars.characterNumber[j] = players[j].GetComponent<PlayerS>().characterNum;
													GlobalVars.colorNumber[j] = players[j].GetComponent<PlayerS>().colorNum;  
												}
												else
												{
													GlobalVars.characterNumber[j] = 0; 
												}
											}
											
											GlobalVars.characterSelected = true; 
											GlobalVars.launchingFromScene = false; 
											
											//Application.LoadLevel(nextLevelString);
											startedLoadDelay = true;
											Instantiate(selectSFXObj);
										}
										
									}
									
									
									
									
								}
								
							}
							
						}
						
						public void StartLoading() {
							StartCoroutine("load");
						}
						
						IEnumerator load() {
							Debug.LogWarning("ASYNC LOAD STARTED - " +
							                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
							async = Application.LoadLevelAsync(nextLevelString);
							async.allowSceneActivation = false;
							yield return async;
						}
						
						public void ActivateScene() {
							async.allowSceneActivation = true;
						}
						
						private bool GetWiiUJumpButton(int player){
							if (player == 2) {
								
								return wiiUInput2.jumpButtonDown;
								
							} else if (player == 3) {
								
								return wiiUInput3.jumpButtonDown;
								
							} else if (player == 4) {
								
								return wiiUInput4.jumpButtonDown;
								
							} else {
								return false;
								
							}
						}
						
						private bool GetWiiUAttackButton(int player){
							if (player == 2) {
								
								return wiiUInput2.flingButtonDown;
								
							} else if (player == 3) {
								
								return wiiUInput3.flingButtonDown;
								
							} else if (player == 4) {
								
								return wiiUInput4.flingButtonDown;
								
							} else {
								return false;
								
							}
						}
						
						private bool GetWiiUDashButton(int player){
							if (player == 2) {
								
								return wiiUInput2.dashButtonDown;
								
							} else if (player == 3) {
								
								return wiiUInput3.dashButtonDown;
								
							} else if (player == 4) {
								
								return wiiUInput4.dashButtonDown;
								
							} else {
								return false;
								
							}
						}
						
						private float GetWiiUHorizontal(int player){
							if (player == 2) {
								
								return wiiUInput2.horizontalAxis;
								
							} else if (player == 3) {
								
								return wiiUInput3.horizontalAxis;
								
							} else if (player == 4) {
								
								return wiiUInput4.horizontalAxis;
								
							} else {
								return 0f;
								
							}
						}
					}
