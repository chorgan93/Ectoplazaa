using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewScoreUI : MonoBehaviour {

	public int playerNum;

	public Image charImage;
	private float charImageAlpha = 0.4f;
	public Image charImageFill;
	private float charImageFillAlpha = 0.8f;

	public Sprite[] charImages;
	public Sprite[] charImagesLeft;

	public Material spriteMaterialRedTeam;
	public Material spriteMaterialBlueTeam;

	public Text textDisplay;

	private PlayerS myPlayer;

	private Color charCol;

	private int teamNum = 0;

	public Image crown1;
	public Image crown2;
	public Image crown3;

	public Sprite crownWon;
	public Sprite crownLost;

	private ScoreKeeperS scoreKeeper;

	void Start () {
		
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeperS>() as ScoreKeeperS;

		if (CurrentModeS.numRoundsDefault < 3){
			crown3.gameObject.SetActive(false);
		}
		if (CurrentModeS.numRoundsDefault < 2){
			crown2.gameObject.SetActive(false);
		}

		Color fixCol = charImage.color;
		fixCol.a = charImageAlpha;
		charImage.color = fixCol;

		fixCol = charImageFill.color;
		fixCol.a = charImageFillAlpha;
		charImageFill.color = fixCol;

	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (ScoreKeeperS.gameEnd){
			gameObject.SetActive(false);
		}

		if (!myPlayer){

			if (GlobalVars.totalPlayers < playerNum){
				gameObject.SetActive(false);
			}
			else{
				myPlayer = GlobalVars.playerList[playerNum-1].GetComponent<PlayerS>();

				if (playerNum == 1 || playerNum == 3){
					charImage.sprite = charImagesLeft[GlobalVars.characterNumber[playerNum-1]-1];
				}
				else{
					charImage.sprite = charImages[GlobalVars.characterNumber[playerNum-1]-1];
				}

				charImageFill.sprite = charImage.sprite;

				int numWins = 0;

				if (CurrentModeS.isTeamMode){
					teamNum = GlobalVars.teamNumber[playerNum-1];

					int teamWins = 0;

					if (teamNum == 1){
						charCol = Color.red;
						charImage.material = spriteMaterialRedTeam;
						charImageFill.material = spriteMaterialRedTeam;
						teamWins = CurrentModeS.GetRedWins();
					}
					else{
						charCol = Color.blue;
						charImage.material = spriteMaterialBlueTeam;
						charImageFill.material = spriteMaterialBlueTeam;
						teamWins = CurrentModeS.GetBlueWins();
					}
					//charImage.color = charCol;

					numWins = teamWins;

				}
				else{
					numWins = CurrentModeS.GetPlayerWins(playerNum);
				}

				if (numWins >= 1){
					crown1.sprite = crownWon;
				}
				else{
					crown1.sprite = crownLost;
				}
				if (numWins >= 2){
					crown2.sprite = crownWon;
				}
				else{
					crown2.sprite = crownLost;
				}
				crown3.sprite = crownLost;
			}

		}
		else{

			//charImage.sprite = playerSprite.sprite;

			textDisplay.text = "P" + playerNum;

			if (myPlayer.numLives == 0){
				textDisplay.text += ": " + " OUT";
				charImageFill.fillAmount = 0f;
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
						if (collectPercent < 0){
							collectPercent = 0;
						}
						
					textDisplay.text += ": " + collectPercent;

						charImageFill.fillAmount = collectPercent/100f;
				}
	
				// stock
				if (CurrentModeS.currentMode == 1){
					textDisplay.text += ": " + (myPlayer.numLives);
						
						charImageFill.fillAmount = myPlayer.numLives*1f/ScoreKeeperS.numberLives*1f;
				}
	
				// ball mode
				if (CurrentModeS.currentMode == 2){
						//textDisplay.text += ": " + (100*myPlayer.score/ScoreKeeperS.scoreThresholdGhostball);
						textDisplay.text += ": " + myPlayer.score;

						charImageFill.fillAmount = myPlayer.score*1f/ScoreKeeperS.scoreThresholdGhostballTeam*1f;
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
						
						charImageFill.fillAmount = collectPercent/100f;
					}
					
					// stock
					if (CurrentModeS.currentMode == 1){
						textDisplay.text += ": " + (myPlayer.numLives);
						charImageFill.fillAmount = myPlayer.numLives*1f/ScoreKeeperS.numberLivesTeam*1f;
					}
					
					// ball mode
					if (CurrentModeS.currentMode == 2){
						float currentScore = 0;
						if (teamNum == 1){
							currentScore = scoreKeeper.GetRedScore();
						}
						if (teamNum == 2){
							currentScore = scoreKeeper.GetBlueScore();
						}
						textDisplay.text += ": " + currentScore;
						
						charImageFill.fillAmount = currentScore*1f/ScoreKeeperS.scoreThresholdGhostballTeam*1f;
					}

				}
			}

		}
	
	}

}
