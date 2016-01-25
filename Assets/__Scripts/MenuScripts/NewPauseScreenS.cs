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


	// Use this for initialization
	void Start () {

		platformType = PlatformS.GetPlatform();
		ownRender = GetComponent<Image>();

		// turn off cursor obj
		cursorObj.SetActive(false);
		TurnOffText();

		
		currentCursorPos = 0;
		ownRender.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (ScoreKeeperS.gameStarted && !ScoreKeeperS.gameEnd){

		// check for player to pause
		for(int i = 1; i < 4; i++){

			// this is super gross but im doing it because get button down doesn't want to work for some reason
			if (i == 1){
				if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
					pauseButtonDownP1 = false;
				}

				if (!pauseButtonDownP1){
					if (Input.GetButton("StartButtonPlayer"+i+platformType)){
						if (!pauseActive){
							pauseActive = true;
							playerWhoPaused = i;
							pauseButtonDownP1 = true;
							
							// dont allow camera shake
							CameraShakeS.C.sleeping = true;
						}
						else{
							pauseActive = false;
							pauseButtonDownP1 = true;

							
							// allow camera shake
								CameraShakeS.C.sleeping = false;
								Time.timeScale = 1;
						}
					}
				}
			}

			if (i == 2){
				if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
					pauseButtonDownP2 = false;
				}
				
				if (!pauseButtonDownP2){
					if (Input.GetButton("StartButtonPlayer"+i+platformType)){
						if (!pauseActive){
							pauseActive = true;
							playerWhoPaused = i;
							pauseButtonDownP2 = true;

							
							// dont allow camera shake
							CameraShakeS.C.sleeping = true;
						}
						else{
							pauseActive = false;
							pauseButtonDownP2 = true;

							
							// allow camera shake
								CameraShakeS.C.sleeping = false;
								Time.timeScale = 1;
						}
					}
				}
			}

			if (i == 3){
				if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
					pauseButtonDownP3 = false;
				}
				
				if (!pauseButtonDownP3){
					if (Input.GetButton("StartButtonPlayer"+i+platformType)){
						if (!pauseActive){
							pauseActive = true;
							playerWhoPaused = i;
							pauseButtonDownP3 = true;

							
							// dont allow camera shake
							CameraShakeS.C.sleeping = true;
						}
						else{
							pauseActive = false;
							pauseButtonDownP3 = true;

							
							// allow camera shake
								CameraShakeS.C.sleeping = false;
								Time.timeScale = 1;
						}
					}
				}
			}

			if (i == 4){
				if (!Input.GetButton("StartButtonPlayer"+i+platformType)){
					pauseButtonDownP4 = false;
				}
				
				if (!pauseButtonDownP4){
					if (Input.GetButton("StartButtonPlayer"+i+platformType)){
						if (!pauseActive){
							pauseActive = true;
							playerWhoPaused = i;
							pauseButtonDownP4 = true;

							// turn off camera shake
							CameraShakeS.C.sleeping = true;
						}
						else{
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
			if (Mathf.Abs(Input.GetAxis("VerticalPlayer" + playerWhoPaused + platformType))
			    > 0.8f){
				if (!movedCursor){

					if (Input.GetAxis("VerticalPlayer" + playerWhoPaused + platformType) > 0){
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
				}
			}
			else{
				movedCursor = false;
			}

			// move cursor to correct spot
			cursorObj.transform.position = cursorPositions[currentCursorPos].transform.position;

			// continue option
			if (currentCursorPos == 0){
				if (Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)){
					pauseActive = false;
					//pauseButtonDownP4 = true;
					
					
					// allow camera shake
					CameraShakeS.C.sleeping = false;
						Time.timeScale = 1;
				}
			}

			// restart option
			if (currentCursorPos == 1){
				if (Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)){
					//pauseActive = false;
					//pauseButtonDownP4 = true;

					// reset scene
						CurrentModeS.ResetWinRecord();
					Application.LoadLevel(Application.loadedLevel);
					
					// allow camera shake
						CameraShakeS.C.sleeping = false;
						Time.timeScale = 1;
				}
			}

			// exit to main menu option
			if (currentCursorPos == 2){
				if (Input.GetButton("AButtonPlayer" + playerWhoPaused + platformType)){
					//pauseActive = false;
					//pauseButtonDownP4 = true;

						CurrentModeS.ResetWinRecord();
						CurrentModeS.isTeamMode = false;
					Application.LoadLevel(menuSceneString);
					
					// allow camera shake
						CameraShakeS.C.sleeping = false;
						Time.timeScale = 1;
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
