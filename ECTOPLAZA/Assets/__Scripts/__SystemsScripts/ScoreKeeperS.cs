using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeperS : MonoBehaviour {


	public GameObject scoreBarObj; 
	public GameObject uiObj; 
	private int winningPlayerNum;
	public GameObject winningPlayerTail, winningPlayerSprite; 

	public Sprite [] playerHighResSprites; 

	public int scoreThreshold;

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

	private float countdownRateMax = .5f;
	private float startCountdown;

	// don't allow exit of game until this time is up
	public float gameEndMinTime = 2f;

	float cameraSizeSmall = 20f;

	Vector3 worldPos = Vector3.zero;

	void Start () 
	{
		endGameObj.SetActive(false);
		gameStarted = false;
		gameEnd = false;

		startCountdown = countdownRateMax;
	}

	
	// Update is called once per frame
	void Update () {

		if (!spawnedScoreboard)
			SpawnScoreboard (); 

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
					CameraShakeS.C.LargeShake();
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


	void UpdateScoreboard()
	{
	
		for (int i = 0; i < 4; i++) {
			if (GlobalVars.characterNumber [i] != 0) {
				PlayerS currentPlayer = GlobalVars.playerList [i].GetComponent<PlayerS> ();
				
				if (currentPlayer.health >= scoreThreshold) {

					gameEnd = true;
					winningPlayerNum = i + 1;

					SpawnEndScreen();
				
				}
			} 
		}
	}

	void SpawnScoreboard()
	{
		spawnedScoreboard = true; 
		print ("trying to set up scoreboard"); 
			
		scoreBarObj.GetComponent<ScoreBar>().scoreThreshold = scoreThreshold; 
		scoreBarObj.GetComponent<ScoreBar>().SpawnScoreboard(); 


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
		scoreBarObj.SetActive (false); 

		endGameObj.SetActive (true);
		Vector3 newPos = endGameObj.transform.position; 
		newPos.x = Camera.main.ViewportToWorldPoint (new Vector3 (1f, .5f, 0f)).x + 50f;

		endGameObj.transform.position = newPos; 

		winText.text = "P" + winningPlayerNum + "\nwins!";
		winningPlayerSprite.GetComponent<SpriteRenderer> ().sprite = playerHighResSprites [winningPlayerNum - 1];
		winningPlayerTail.GetComponent<Renderer> ().material = GlobalVars.playerList [winningPlayerNum - 1].GetComponent<PlayerS> ().playerMats [winningPlayerNum - 1]; 
	

		foreach (GameObject p in GlobalVars.playerList) {

			if(p != null)
			{
				p.SetActive (false); 
				//GameObject.Destroy(p.gameObject); 
			}

		}

	}
	
	void UpdateEndScreen()
	{


		// once gameEnd is on, start countdown
		// restart map on countdown (replace this once menu infrastructure is in place



		gameEndMinTime -= Time.deltaTime;

		Vector3 newPos = endGameObj.transform.position; 
		newPos.x = Camera.main.ViewportToWorldPoint (new Vector3 (0.0f, .5f, 0f)).x ;

		endGameObj.transform.position = Vector3.Lerp (endGameObj.transform.position, newPos, 0.2f); 
		
		if (Input.GetButton ("BButtonAllPlayers"+	PlatformS.GetPlatform ()) && gameEndMinTime < 0) 
		{
			gameEnd = false;
			// hard coding in return to character select
			Application.LoadLevel("3CharacterSelect");
		}
		if (Input.GetButton ("AButtonAllPlayers" + PlatformS.GetPlatform ()) && gameEndMinTime < 0) {

			print("RESETTINGLEVEL"); 
			//GlobalVars.ResetVariables(); 
			Application.LoadLevel(Application.loadedLevel);// THIS DOESNT WORK, CAUSES GLITCHES ON RESTART, MIGHT HAVE TO LOAD BACK THROUGH CHARACTER SELECT SCREEN? 
		}

	}
}
