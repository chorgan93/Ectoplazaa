using UnityEngine;
using System.Collections;

public class CharacterSelectMenu : MonoBehaviour {

	public GameObject playerPrefab; 

	string platformType; 
	int playerNum = 1; 

	public bool[] hasJoined = new bool[4]; // for inital # of player select
	public bool[] hasSelected = new bool[4]; // adding extra step of character select
	public GameObject [] joinTexts; 
	public GameObject [] spawnPoints;
	public GameObject [] characterNumText; 

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

	int totalPlayers = 0; 

	// Use this for initialization
	void Start () {
	
		platformType = PlatformS.GetPlatform (); 

		
		//nextLevelString = LevelSelectMenu.selectedLevelString;

		followRef = GetComponent<CameraFollowS>();

		ScoreKeeperS.gameStarted = true;

	//	string[] joysticks = Input.GetJoystickNames();
	//	print (joysticks.Length);
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

		// back function
		if (Input.GetButtonDown("BButtonAllPlayers" + platformType) && totalPlayers == 0){
			Application.LoadLevel(backSceneString);
		}

		for(int i = 1; i <= 4; i++)
		{

					// A BUTTON CODE, FOR SELECTION

			if (Input.GetButton ("AButtonPlayer" + i + platformType) && !buttonDown[i-1])  //ADD PLAYER---------------------------------------
			{

						// press button
						buttonDown[i-1] = true;

				//print ("AButtonPlayer" + i + platformType);
						
						//  for actual character selection (after player buys in)
						if (!hasSelected[i-1] && hasJoined[i-1]){
							
							// a to confirm and add
							players[i-1].GetComponent<PlayerS>().nonActive = false;
							players[i-1].GetComponent<Rigidbody>().useGravity = true;
							
							hasSelected[i-1] = true;
							
							// play char intro sound
							totalPlayers ++;
							players[i-1].GetComponent<PlayerSoundS>().PlayPlayerJoinSound(totalPlayers-1);

							
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

					characterNumText[i-1].GetComponent<TextMesh>().text = "characterNum: " + defaultSkin;

					hasJoined[i-1] = true; 
					//totalPlayers += 1; 

					GameObject newPlayer = Instantiate(playerPrefab,spawnPoints[i-1].transform.position,Quaternion.identity) 
								as GameObject;
							PlayerS newPlayerS = newPlayer.GetComponent<PlayerS>();
							newPlayerS.TurnOnCharSelect();
					newPlayerS.playerNum = i;
							newPlayerS.characterNum = defaultSkin;
							newPlayerS.respawnInvulnTime = 0;
					newPlayerS.SetSkin(); 

					newPlayerS.spawnPt = spawnPoints[i-1];
					players[i-1] = newPlayer; 
					//joinTexts[i-1].GetComponent<Renderer>().enabled = false; 
					//print("Total Players: " + totalPlayers); 

							// disable player before second selection
							newPlayerS.nonActive = true;
							newPlayerS.GetComponent<Rigidbody>().useGravity = false;

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
					if(!Input.GetButton ("AButtonPlayer" + i + platformType)){
						buttonDown[i-1] = false;
					}

					// B BUTTON CODE, FOR DESELECTION

			 if (Input.GetButton ("BButtonPlayer" + i + platformType)) //REMOVE PLAYER---------------------------------------
			{
				if(hasJoined[i-1])
				{
					hasJoined[i-1] = false; 
							hasSelected[i-1] = false;
					totalPlayers -= 1; 

					GameObject.Destroy( players[i-1].gameObject);
					players[i-1]= null; 

					//print("Total Players: " + totalPlayers); 

					characterNumText[i-1].GetComponent<TextMesh>().text = "";

					Renderer [] renderers = joinTexts[i-1].GetComponentsInChildren<Renderer>();
					
					foreach(Renderer r in renderers)
					{
						r.enabled = true; 
					}
				}
			}

					// LEFT SELECT CODE, FOR CHANGING CHARACTER
					if (Input.GetAxis("HorizontalPlayer" + i + platformType) < -selectThreshold && !selectUp[i-1]){
						selectUp [i-1] = true;

						// reappropriating Old Y Code for Character Select
						if(hasJoined[i-1] && !hasSelected[i-1])
						{
							
							int newSkin = players[i-1].GetComponent<PlayerS>().characterNum; 
							int newColor = 0;
							bool stopLoop = false;
							
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
							
							
							characterNumText[i-1].GetComponent<TextMesh>().text = "characterNum: " + newSkin;
							
							
							players[i-1].GetComponent<PlayerS>().characterNum = newSkin;
							players[i-1].GetComponent<PlayerS>().colorNum = newColor;
							players[i-1].GetComponent<PlayerS>().SetSkin();

							print(newColor);
							
							
							// play char intro sound
							//players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();
							players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSoundQuickFix();
						}
					}
					// RIGHT SELECT CODE, FOR CHANGING CHARACTER
					if (Input.GetAxis("HorizontalPlayer" + i + platformType) > selectThreshold && !selectUp[i-1]){
						selectUp [i-1] = true;

						// reappropriating Old Y Code for Character Select
						if(hasJoined[i-1] && !hasSelected[i-1])
						{
							
							int newSkin = players[i-1].GetComponent<PlayerS>().characterNum; 
							int newColor = 0;
							bool stopLoop = false;
							
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
							
							
							characterNumText[i-1].GetComponent<TextMesh>().text = "characterNum: " + newSkin;
							
							
							players[i-1].GetComponent<PlayerS>().characterNum = newSkin;
							players[i-1].GetComponent<PlayerS>().colorNum = newColor;
							players[i-1].GetComponent<PlayerS>().SetSkin();

							print (newColor);
							
							// play char intro sound
							//players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();
							players[i-1].GetComponent<PlayerSoundS>().PlayCharIntroSoundQuickFix();
						}
					}
					// FOR ANALOG AT ZERO
					if (Mathf.Abs(Input.GetAxis("HorizontalPlayer" + i + platformType)) < selectThreshold) {
						selectUp[i-1] = false;
					}

					// Y BUTTON CODE, FOR CHANGING COLOR (not active yet)

			if (Input.GetButton ("YButtonPlayer" + i + platformType) && !yButtonDown[i-1])
			{

						yButtonDown[i-1] = true;


				if(hasJoined[i-1])
				{

							int newColor = players[i-1].GetComponent<PlayerS>().colorNum;
					bool stopLoop = false;

					for(int j= 0; j < GlobalVars.totalSkins; j++) //loop once through all skins
					{
						newColor += 1; //increment to next skin, check if available; 
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
						 


			}
					if(!Input.GetButton ("YButtonPlayer" + i + platformType)){
						yButtonDown[i-1] = false;
					}

					// START BUTTON CODE FOR GAME START

			if (Input.GetButton ("StartButtonAllPlayers"+ platformType) && totalPlayers >= 2) //START GAME---------------------------------------
			{
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
}
