using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewScoreUI : MonoBehaviour {

	public int playerNum;

	public Image charImage;
	public Text textDisplay;

	private PlayerS myPlayer;
	private SpriteRenderer playerSprite;

	private Color charCol;

	private int teamNum = 0;
	
	// Update is called once per frame
	void Update () {

		if (ScoreKeeperS.gameEnd){
			gameObject.SetActive(false);
		}

		if (!myPlayer){

			if (GlobalVars.totalPlayers < playerNum){
				gameObject.SetActive(false);
			}
			else{
				myPlayer = GlobalVars.playerList[playerNum-1].GetComponent<PlayerS>();
	
				playerSprite = myPlayer.spriteObject.GetComponent<SpriteRenderer>();
	
				charCol = playerSprite.color;
				charImage.color = charCol;

				if (CurrentModeS.isTeamMode){
					teamNum = GlobalVars.teamNumber[playerNum-1];
				}
			}

		}
		else{

			//charImage.sprite = playerSprite.sprite;

			textDisplay.text = "P" + playerNum;

			if (myPlayer.numLives == 0){
				textDisplay.text += ": " + " OUT";
			}
			else{
				if (!CurrentModeS.isTeamMode){
				// ecto
				if (CurrentModeS.currentMode == 0){
	
					int collectPercent = Mathf.RoundToInt
						(100*(myPlayer.health-myPlayer.startEctoNum)/ScoreKeeperS.scoreThresholdCollectoplaza);
	
					if (collectPercent > 100){
						collectPercent = 100;
					}
						
					textDisplay.text += ": " + collectPercent + "%";
				}
	
				// stock
				if (CurrentModeS.currentMode == 1){
					textDisplay.text += ": " + (myPlayer.numLives);
				}
	
				// stock
				if (CurrentModeS.currentMode == 2){
					textDisplay.text += ": " + (100*myPlayer.score/ScoreKeeperS.scoreThresholdGhostball);
				}
				}
				else{

					// team mode stuff
					if (CurrentModeS.currentMode == 0){
						
						int collectPercent = Mathf.RoundToInt
							(100*(myPlayer.health-myPlayer.startEctoNum)/ScoreKeeperS.scoreThresholdCollectoplaza);
						
						if (collectPercent > 100){
							collectPercent = 100;
						}
						
						textDisplay.text += ": " + collectPercent + "%";
					}
					
					// stock
					if (CurrentModeS.currentMode == 1){
						textDisplay.text += ": " + (myPlayer.numLives);
					}
					
					// stock
					if (CurrentModeS.currentMode == 2){
						textDisplay.text += ": " + (100*myPlayer.score/ScoreKeeperS.scoreThresholdGhostball);
					}

				}
			}

		}
	
	}

}
