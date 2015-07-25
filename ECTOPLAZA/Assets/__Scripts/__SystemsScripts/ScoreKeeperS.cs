using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeperS : MonoBehaviour {

	public GameObject[] scoreTexts;

	private int winningPlayerNum;

	public int scoreThreshold;

	public static bool gameEnd = false;

	public GameObject endGameObj;
	public TextMesh  winText;
	public TextMesh countdownText;

	// don't allow exit of game until this time is up
	public float gameEndMinTime = 2f;

	void Start () {
		endGameObj.SetActive(false);
	}

	
	// Update is called once per frame
	void Update () {

		

		// once gameEnd is on, start countdown
		// restart map on countdown (replace this once menu infrastructure is in place

		if(gameEnd){
			gameEndMinTime -= Time.deltaTime;
			countdownText.text = "Resetting..." + gameEndMinTime*1.00f;

			if (gameEndMinTime <= 0){
				gameEnd = false;
				Application.LoadLevel(Application.loadedLevelName);
			}

		}
		else{
			// check for winner
			for (int i = 0; i < 4; i++){
				if(GlobalVars.characterNumber[i] != 0)
				{
					PlayerS currentPlayer = GlobalVars.playerList[i].GetComponent<PlayerS>();
					scoreTexts[i].GetComponent<TextMesh>().text = "P" + currentPlayer.playerNum + ": " + currentPlayer.score + "";



					if (currentPlayer.score >= scoreThreshold){
						gameEnd = true;
						winningPlayerNum = i+1;
						endGameObj.SetActive(true);

						winText.text = "Player " + winningPlayerNum + " wins!!";
						countdownText.text = "Resetting..." + gameEndMinTime*1.00f;
					}
				}
				else
				{

					scoreTexts[i].GetComponent<TextMesh>().text = "";
				}

			}
		}
	
	}
}
