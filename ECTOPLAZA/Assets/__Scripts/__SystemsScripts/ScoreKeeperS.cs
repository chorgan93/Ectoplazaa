using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeperS : MonoBehaviour {


	public GameObject scoreBarObj; 
	public GameObject uiObj; 
	private int winningPlayerNum;
	public GameObject winningSphere; 

	public int scoreThreshold;

	public static bool gameEnd = false;

	public GameObject endGameObj;
	public TextMesh  winText;

	bool spawnedScoreboard = false;

	// don't allow exit of game until this time is up
	public float gameEndMinTime = 2f;

	float cameraSizeSmall = 20f;

	Vector3 worldPos = Vector3.zero;

	void Start () 
	{
		endGameObj.SetActive(false);
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
			RepositionScoreboard(); 
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
		winText.text = "Player " + winningPlayerNum + " wins!!";
	

		winningSphere.SetActive (true); 
		Vector3 spawnPos = GlobalVars.playerList [winningPlayerNum - 1].transform.position; 
		spawnPos.z = winningSphere.transform.position.z; 
		winningSphere.transform.position = spawnPos; 
		winningSphere.GetComponent<Renderer> ().material = GlobalVars.playerList [winningPlayerNum - 1].GetComponent<PlayerS> ().playerMats [winningPlayerNum-1]; 

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

		winningSphere.transform.localScale = Vector3.Lerp (winningSphere.transform.localScale, new Vector3 (300f, 300, 0.1f), 0.075f); 

		// once gameEnd is on, start countdown
		// restart map on countdown (replace this once menu infrastructure is in place

		gameEndMinTime -= Time.deltaTime;
		
		if (Input.GetButton ("StartButtonAllPlayers"+	PlatformS.GetPlatform ()) && gameEndMinTime < 0) 
		{
			gameEnd = false;
			// hard coding in return to character select
			Application.LoadLevel("3CharacterSelect");
		}
	}
}
