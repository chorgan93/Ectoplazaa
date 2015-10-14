using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeperS : MonoBehaviour {

	// keeps track of player scores and initializes scoreboard ui
	// currently only works with ecto mode, will need to be repurposed for multiple modes


	public GameObject scoreBarObj; 
	public GameObject uiObj; 
	private int winningPlayerNum;
	public GameObject winningPlayerTail, winningPlayerSprite; 

	public Sprite [] playerHighResSprites; 

	public int scoreThresholdCollectoplaza;
	public int numberLives;								//stock mode
	private int numPlayersLeft = 0; 					//stock mode
	private bool [] playersPlaying = new bool[4]{false,false,false,false}; //Stock mode
	public static bool gameEnd = false;

	public GameObject endGameObj;
	public TextMesh  winText;

	bool spawnedScoreboard = false;

	public static bool gameStarted = false;

	public GameObject timer3;
	public GameObject timer2;
	public GameObject timer1;
	public GameObject goText;
	private int currentText = 4;

	public GameObject statText1, statText2, statText3, statText4; 

	private float countdownRateMax = .5f;
	private float startCountdown;

	public GameObject endFlash;

	//public GameObject endGameSound;

	// don't allow exit of game until this time is up
	public float gameEndMinTime = 2f;

	float cameraSizeSmall = 20f;

	Vector3 worldPos = Vector3.zero;

	private int currentMode =-1;

	void Start () 
	{
		endGameObj.SetActive(false);
		gameStarted = false;
		gameEnd = false;


		currentMode = CurrentModeS.currentMode;						//get what mode to use.


		//SetupMode();
		
		startCountdown = countdownRateMax;
	}


	void SetupMode()
	{
		currentMode = CurrentModeS.currentMode;
			
		//Begin by disabling all (for safety);
/*		for (int i = 0; i < modeObjectOwners.Count-1; i++)
		{
			modeObjectOwners[i].SetActive(false);
		}
*/		
			

		switch (currentMode)										//Find parent object for desired mode and enable
		{
			case 0:
				//enable Ecto Mode
				//modeObjectOwners[0].SetActive (true);
				scoreBarObj.GetComponent<ScoreBar>().scoreThreshold = scoreThresholdCollectoplaza; 
				scoreBarObj.GetComponent<ScoreBar>().SpawnScoreboard(); 
				break;

			case 1:
				print("ScoreKeeper setting up stock mode");
				
				
				for (int i = 0; i < 4; i++) {						//Tell players how many lives they have
					if (GlobalVars.characterNumber [i] != 0) {
						PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
						currentPlayer.numLives = numberLives;
						print ("Set lives to " + numberLives);
						numPlayersLeft ++;								//Add to list of players remaining
						playersPlaying[i] = true;
					}
					
				}
				
				
				break;

			case 2:
				break;

			case 3:
				break;

			case -1:
				print (" CurrMode = -1");
				break;

			default:
				print("Default = " + currentMode);

				break;
		}



		spawnedScoreboard = true; 
			//Have mode owners set up everything?	
		
	}
	
	// Update is called once per frame
	void Update () {
		 

		if (!spawnedScoreboard)
		{
			SetupMode (); 
		}


		if(gameEnd)
		{
			UpdateEndScreen();
		}
		else
		{
			UpdateScoreboard(); 
		}

		RepositionScoreboard(); 

		// start game stuff
		if (!gameStarted){
			startCountdown -= Time.deltaTime;
			if (startCountdown <= 0){
				currentText --;
				startCountdown = countdownRateMax;

				if (currentText == 0){
					gameStarted = true;
					goText.SetActive(true);
					CameraShakeS.C.SmallShake();
				}
				if (currentText == 1){
					timer1.SetActive(true);
				}
				if (currentText == 2){
					timer2.SetActive(true);
				}
				if (currentText == 3){
					timer3.SetActive(true);
				}
			}
		}

	}

	void SpawnScoreboard()
	{

		//Spawn depending on which mode is active
		if(currentMode == 0) //Collectoplaza mode
		{

			print ("(ScoreKeeperS)Spawning Scoreboard for Collectoplaza mode");
		}
		else if(currentMode == 1) //Stock Mode
		{
			print ("(ScoreKeeperS)Spawning Scoreboard for Stock mode");
		}
		else if(currentMode == 2) //Ghostball Mode
		{
			print ("(ScoreKeeperS)Spawning Scoreboard for Ghostball mode");
		}

		spawnedScoreboard = true; 
		//print ("trying to set up scoreboard"); 
	}

	public void PlayerDied(PlayerS p, bool hasMoreLives)
	{
		
		
		if(currentMode == 1)
		{	
			
			scoreBarObj.GetComponent<ScoreBar>().UpdateScoreboardStockMode(p);					//Update Scorebar
			
			if ( !hasMoreLives )																//If Eliminated...
			{										
				numPlayersLeft--;																//Decrement counter
				playersPlaying[p.playerNum-1] = false;											//Update list of remaining players
				print ("Player " + p.playerNum + " died!");
				if (numPlayersLeft  == 1)														//If only one player remains left
				{
					for(int i = 0; i < 4 ; i ++)												//Find Winner
					{
						if (playersPlaying[i] == true)
							winningPlayerNum = i + 1;											//Record Winning Player Number
					}
					SpawnEndScreen();															//Spawn  End Screen
				}
				else{
					print ("Number of players left: " + numPlayersLeft);
				}
				
				
				//end the game
				//SpawnEndScreen();
			}
			else
			{
				//Update Scorebar ??
				
			}
			
			
		}
	}
	
	void UpdateScoreboard()
	{
		
		for (int i = 0; i < 4; i++) {
			if (GlobalVars.characterNumber [i] != 0) {
				PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
				
				// Endgame for Collectoplaza mode
				if(currentMode == 0)
				{
					if (currentPlayer.score >= scoreThresholdCollectoplaza 
					    || currentPlayer.health-currentPlayer.startEctoNum >= scoreThresholdCollectoplaza) {
						
						gameEnd = true;
						winningPlayerNum = i + 1;
						
						SpawnEndScreen();
						
					}
				}
				// Stock Mode
				else if(currentMode == 1)
				{
					//if player loses life
				}
				//Ghostball Mode
				else if(currentMode == 2)
				{
				}
			} 
		}
	}

	void RepositionScoreboard()
	{
		worldPos = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 1f, 0f));
		//print("worldPos: " + worldPos.x + " " + worldPos.y + " " + worldPos.z); 	
		
		Vector3 newPos = new Vector3 (worldPos.x, worldPos.y -0.0f, 0f); 
		uiObj.transform.position = newPos; 

		float cameraScale = Camera.main.orthographicSize / cameraSizeSmall;

		Vector3 newScale = new Vector3 (1f * cameraScale, 1f * cameraScale, 1f); 

		uiObj.transform.localScale = newScale; 
	}
	
	void SpawnEndScreen()
	{

		
		int characterNum = GlobalVars.playerList [winningPlayerNum - 1].GetComponent<PlayerS> ().characterNum-1;

		scoreBarObj.SetActive (false); 

		endGameObj.SetActive (true);

		CameraShakeS.C.LargeShake();
		CameraShakeS.C.HalfTimeSleep(1.5f);

		Vector3 newPos = endGameObj.transform.position; 
		newPos.x = Camera.main.ViewportToWorldPoint (new Vector3 (1f, .5f, 0f)).x + 50f;

		endGameObj.transform.position = newPos; 

		GlobalVars.playerList[winningPlayerNum-1].GetComponent<PlayerSoundS>().PlayCharIntroSound();

		GlobalVars.lastWinningPlayer = winningPlayerNum;

		endFlash.GetComponent<Renderer>().material.color = GlobalVars.playerList 
			[winningPlayerNum - 1].GetComponent<PlayerS> ().playerParticleMats [characterNum].GetColor("_TintColor"); 


		winText.text = "P" + winningPlayerNum + "\nwins!";

		winningPlayerSprite.GetComponent<SpriteRenderer> ().sprite = playerHighResSprites [characterNum];
		winningPlayerTail.GetComponent<Renderer> ().material = GlobalVars.playerList [winningPlayerNum - 1].GetComponent<PlayerS> ().playerMats [characterNum]; 
	
		SpawnEndText(); 

		foreach (GameObject p in GlobalVars.playerList) {

			if(p != null)
			{
				p.SetActive (false); 
				//GameObject.Destroy(p.gameObject); 
			}

		}

	}

	void SpawnEndText()
	{
		int maxVal = 0; 
		int maxIndex = 0; 
	
		for(int i = 0; i < 4; i++)
		{
			if(GlobalVars.totalGlobsEaten[i] > maxVal)
			{
				maxVal = GlobalVars.totalGlobsEaten[i]; 
				maxIndex = i; 
			}
		}

		statText1.GetComponent<TextMesh>().text = "P" + (maxIndex+1) + " had a huge appetite"; 

		maxVal = 0;
		maxIndex = 0; 

		for(int i = 0; i < 4; i++)
		{
			if(GlobalVars.totalDeaths[i] > maxVal)
			{
				maxVal = GlobalVars.totalDeaths[i]; 
				maxIndex = i; 
			}
		}
		
		statText2.GetComponent<TextMesh>().text = "P" + (maxIndex+1) + " was eviscerated most"; 
		
		maxVal = 0;
		maxIndex = 0; 

		for(int i = 0; i < 4; i++)
		{
			if(GlobalVars.totalFlings[i] > maxVal)
			{
				maxVal = GlobalVars.totalFlings[i]; 
				maxIndex = i; 
			}
		}
		
		statText3.GetComponent<TextMesh>().text = "P" + (maxIndex+1) + " fling king"; 
		
		maxVal = 0;
		maxIndex = 0; 

		for(int i = 0; i < 4; i++)
		{
			if(GlobalVars.totalKills[i] > maxVal)
			{
				maxVal = GlobalVars.totalKills[i]; 
				maxIndex = i; 
			}
		}
		
		statText4.GetComponent<TextMesh>().text = "P" + (maxIndex+1) + " at large"; 
		
		maxVal = 0;
		maxIndex = 0; 

		GlobalVars.ResetGameStats(); 
	}
	
	void UpdateEndScreen()
	{


		// once gameEnd is on, start countdown
		// restart map on countdown (replace this once menu infrastructure is in place



		gameEndMinTime -= Time.deltaTime;

		Vector3 newPos = endGameObj.transform.position; 
		newPos.x = Camera.main.ViewportToWorldPoint (new Vector3 (0.0f, .5f, 0f)).x ;

		endGameObj.transform.position = Vector3.Lerp (endGameObj.transform.position, newPos, 0.2f
		                                              *Time.deltaTime*TimeManagerS.timeMult*50f); 
		
		if (Input.GetButton ("BButtonAllPlayers"+	PlatformS.GetPlatform ()) && gameEndMinTime < 0) 
		{
			gameEnd = false;
			// hard coding in return to map select
			Application.LoadLevel("2MapSelect");
		}
		if (Input.GetButton ("AButtonAllPlayers" + PlatformS.GetPlatform ()) && gameEndMinTime < 0) {

			print("RESETTINGLEVEL"); 
			//GlobalVars.ResetVariables(); 
			Application.LoadLevel(Application.loadedLevel);
		}

	}
}
