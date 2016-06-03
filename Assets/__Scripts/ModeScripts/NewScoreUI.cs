using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewScoreUI : MonoBehaviour {

	// test change

	public int playerNum;

	public Image charBorder;
	public Image charImage;
	private float charImageAlpha = 0.1f;
	public Image charImageFill;
	private float charImageFillAlpha = 0.8f;

	public Sprite[] charImages;
	public Sprite[] charImagesLeft;
	public Sprite[]	charBorders;
	public Sprite[] charLivesHeads;

	public Material spriteMaterialRedTeam;
	public Material spriteMaterialBlueTeam;

	public Text textDisplay;

	private PlayerS myPlayer;

	private Color charCol;

	private int teamNum = 0;

	public Image crown1;
	public Image crown2;
	public Image crown3;

	public Image[] afterlivesStock;
	public Image[] ballModeScore;

	public Sprite ballScored;
	public Sprite ballNotScored;

	public Sprite crownWon;
	public Sprite crownLost;

	private ScoreKeeperS scoreKeeper;

	void Start () {
		
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeperS>() as ScoreKeeperS;

		Vector3 crownReposition = Vector3.zero;

		if (CurrentModeS.numRoundsDefault < 3){
			crown3.gameObject.SetActive(false);
			crownReposition  = crown1.rectTransform.anchoredPosition;
			crownReposition.x += crown1.rectTransform.rect.width/2f;
			crown1.rectTransform.anchoredPosition = crownReposition;


			crownReposition = crown2.rectTransform.anchoredPosition;
			crownReposition.x += crown2.rectTransform.rect.width/2f;
			crown2.rectTransform.anchoredPosition = crownReposition;
		}
		if (CurrentModeS.numRoundsDefault < 2){
			crown2.gameObject.SetActive(false);

			crownReposition = crown1.rectTransform.anchoredPosition;
			crownReposition.x += crown1.rectTransform.rect.width/2f;
			crown1.rectTransform.anchoredPosition = crownReposition;
		}

		Color fixCol = charImage.color;
		fixCol.a = charImageAlpha;
		charImage.color = fixCol;

		fixCol = charImageFill.color;
		fixCol.a = charImageFillAlpha;
		charImageFill.color = fixCol;

		if (CurrentModeS.currentMode != 1){
			foreach (Image life in afterlivesStock){
				
				life.gameObject.SetActive(false);
				
			}
		}
		if (CurrentModeS.currentMode != 2){
			foreach (Image ball in ballModeScore){
				
				ball.gameObject.SetActive(false);
				
			}
		}
		else{
			if (!CurrentModeS.isTeamMode){

				// reposition for 3 ball set (5 is default setup)
				float ballX = ballModeScore[0].rectTransform.sizeDelta.x;
				ballModeScore[0].rectTransform.anchoredPosition += new Vector2(ballX*0.75f, 0);
				ballModeScore[1].rectTransform.anchoredPosition += new Vector2(ballX, 0);
				ballModeScore[2].rectTransform.anchoredPosition += new Vector2(ballX*1.25f, 0);

				// turn off extra two images
				ballModeScore[3].gameObject.SetActive(false);
				ballModeScore[4].gameObject.SetActive(false);
			}
		}



	}

	void LateUpdate () {

		if (ScoreKeeperS.gameEnd){
			gameObject.SetActive(false);
		}

		if (!myPlayer){

			if (GlobalVars.playerList[playerNum-1] == null){
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

				charBorder.sprite = charBorders[GlobalVars.characterNumber[playerNum-1]-1];
				charImageFill.sprite = charImage.sprite;

				int numWins = 0;

				foreach (Image life in afterlivesStock){
					life.sprite = charLivesHeads[GlobalVars.characterNumber[playerNum-1]-1];
				}

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

				if (CurrentModeS.isTeamMode){
					teamNum = GlobalVars.teamNumber[playerNum-1];
					if (teamNum == 1){
						charCol = Color.red;
					}
					else{
						charCol = Color.blue;
					}
				}
				else{
					charCol = myPlayer.playerParticleMats
						[myPlayer.characterNum-1].GetColor("_TintColor");
				}
				
				textDisplay.GetComponent<Outline>().effectColor = charCol;
			}


		}
		else{

			//charImage.sprite = playerSprite.sprite;

			textDisplay.text = "P" + playerNum;

			if (myPlayer.numLives == 0){
				textDisplay.text += ": " + " OUT";
				charImageFill.fillAmount = 0f;
				foreach (Image life in afterlivesStock){
				
					life.gameObject.SetActive(false);

				}
			}
			else{
				if (!CurrentModeS.isTeamMode){
				// ecto
				if (CurrentModeS.currentMode == 0){
	
					int collectPercent = Mathf.RoundToInt
						(100*(myPlayer.score)/ScoreKeeperS.scoreThresholdCollectoplaza);
	
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

						int i = 0;
						foreach (Image life in afterlivesStock){
							if (i < myPlayer.numLives){
								life.gameObject.SetActive(true); 
							}
							else{
								life.gameObject.SetActive(false);
							}
							i++;
						}


				}
	
				// ball mode
				if (CurrentModeS.currentMode == 2){

						int i = 0;
						foreach (Image ball in ballModeScore){
							if (ball.gameObject.activeSelf){
								if (i < myPlayer.score){
									ball.sprite = ballScored;
								}
								else{
									ball.sprite = ballNotScored;
								}
							}
							i++;
						}


						if (myPlayer.numLives > 0){
							charImageFill.fillAmount = 1f;
						}
						else{
							charImageFill.fillAmount = 0f;
						}
				}
				}
				else{

					// team mode stuff
					if (CurrentModeS.currentMode == 0){
						
						int collectPercent = Mathf.RoundToInt
							(100*(scoreKeeper.GetRedScore())/ScoreKeeperS.scoreThresholdCollectoplazaTeam);
						if (teamNum == 2){
							collectPercent = Mathf.RoundToInt
								(100*(scoreKeeper.GetBlueScore())/ScoreKeeperS.scoreThresholdCollectoplazaTeam);
						}
						
						if (collectPercent > 100){
							collectPercent = 100;
						}
						
						textDisplay.text += ": " + collectPercent + "%";
						
						charImageFill.fillAmount = collectPercent/100f;
					}
					
					// stock
					if (CurrentModeS.currentMode == 1){
						textDisplay.text += ": " + (myPlayer.numLives);
						//charImageFill.fillAmount = myPlayer.numLives*1f/ScoreKeeperS.numberLivesTeam*1f;
						int i = 0;
						foreach (Image life in afterlivesStock){
							if (i < myPlayer.numLives){
								life.gameObject.SetActive(true); 
							}
							else{
								life.gameObject.SetActive(false);
							}
							i++;
						}
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

						int i = 0;
						foreach (Image ball in ballModeScore){
							if (ball.gameObject.activeSelf){
								if (i < currentScore){
									ball.sprite = ballScored;
								}
								else{
									ball.sprite = ballNotScored;
								}
							}
							i++;
						}

						if (myPlayer.numLives > 0){
							charImageFill.fillAmount = 1f;
						}
						else{
							charImageFill.fillAmount = 0f;
						}
					}

				}
			}

			if (myPlayer.numLives == 0){
				textDisplay.text = "OUT";
			}
			else{
				textDisplay.text = "P" + playerNum;
			}


		}



	
	}

}
