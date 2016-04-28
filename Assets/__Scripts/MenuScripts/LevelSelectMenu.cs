﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectMenu : MonoBehaviour {

	public int currentCursorPos;

	public GameObject cursorObj;
	float cursorSpeed= .25f;
	public List<GameObject> cursorPositions;
	public List<GameObject> cameraPositions;
	public List<GameObject> levelCards;
	public List<string> nextLevelStrings;

	public static string selectedLevelString = "4Concierge";
	public string nextSceneString;
	public string backSceneString;

	public TextMesh cursorLabel;
	public List<Color> cursorCols;
	private float moveCursorSensitivity = 0.8f;
	private bool movedCursor = false;
	
	public int playerNum = 1;
	private string platformType; 

	private float delayInput = 0.5f;
	private float loadDelay = 0.5f;
	private bool startedLoadDelay = false;
	private bool startedLoading = false;

	public GameObject loadPt;

	private CameraFollowS followRef;

	public GameObject levelSelectSFXObj;
	public List<GameObject> scrollSFXObjs;

	public FadeObjS fadeIn;
	
	private float fadeItemTime = 0.2f;
	public GameObject[] fadeObjs;

	AsyncOperation async;


	// Use this for initialization
	void Start () {

		platformType = PlatformS.GetPlatform();

		//cursorObj.transform.position = Vector3.Lerp( cursorObj.transform.position, cursorPositions[currentCursorPos].transform.position,cursorSpeed);
		cursorObj.transform.position = cursorPositions[currentCursorPos].transform.position;

		followRef = GetComponent<CameraFollowS>();

		//playerNum = GlobalVars.lastWinningPlayer;
		//cursorLabel.color = cursorCols[GlobalVars.lastWinningPlayer-1];
		//cursorLabel.text = "P" + GlobalVars.lastWinningPlayer;
		cursorLabel.text = "";

		StartCoroutine(FadeMenuItems());
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (startedLoading){
			if (async.progress >= 0.9f){
				ActivateScene();
			}
		}

		else if (startedLoadDelay){
			loadDelay -= Time.deltaTime;
			followRef.poi = loadPt;

			if (loadDelay <= 0){
				startedLoading = true;
				StartLoading();
			}
		}

		else {
		if (delayInput > 0){
			delayInput -= Time.deltaTime;
		}
		else{

		// back function
		if (Input.GetButton("BButtonAllPlayers" + platformType)){
			Application.LoadLevel(backSceneString);
		}

		// select level function
				if (Mathf.Abs(Input.GetAxis("Horizontal")) > moveCursorSensitivity || Mathf.Abs(Input.GetAxis("Vertical")) > moveCursorSensitivity){
			if (!movedCursor){
						if ((Mathf.Abs(Input.GetAxis("Horizontal")) > moveCursorSensitivity && Input.GetAxis("Horizontal") > 0) ||
						    (Mathf.Abs(Input.GetAxis("Vertical")) > moveCursorSensitivity && Input.GetAxis("Vertical") < 0)){
					// add to current level selected
					currentCursorPos ++;
					if (currentCursorPos > cursorPositions.Count-1){
						currentCursorPos = 0;
					}
				}
				// else subtract from current pos
				else{
					currentCursorPos --;
					if (currentCursorPos < 0){
						currentCursorPos = cursorPositions.Count-1;
					}
				}
					int soundToPlay = Mathf.FloorToInt(Random.Range(0,scrollSFXObjs.Count));

						Instantiate(scrollSFXObjs[soundToPlay]);
				movedCursor = true;

						followRef.poi = cameraPositions[currentCursorPos];
			}
		}
		else{
			movedCursor = false;
		}

		// set position of cursor
		cursorObj.transform.position = Vector3.Lerp( cursorObj.transform.position, cursorPositions[currentCursorPos].transform.position,cursorSpeed);

 		// move to game
		if (Input.GetButton("AButtonAllPlayers" + platformType)){
			nextSceneString = selectedLevelString = nextLevelStrings[currentCursorPos];
			//Application.LoadLevel(nextSceneString);
					startedLoadDelay = true;
					fadeIn.FadeOut();
					Instantiate(levelSelectSFXObj);
					followRef.StartLevelCam(levelCards[currentCursorPos]);
		}
		}
		}
	
	}

	public void StartLoading() {
		StartCoroutine("load");
	}

	private IEnumerator FadeMenuItems(){
		
		delayInput = 1f;

		
		foreach(GameObject fade in fadeObjs){
			
			fade.SetActive(true);
			
			yield return new WaitForSeconds(fadeItemTime);
		}
		
		delayInput = 0;
	}
	
	IEnumerator load() {
		Debug.LogWarning("ASYNC LOAD STARTED - " +
		                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
		async = Application.LoadLevelAsync(nextSceneString);
		async.allowSceneActivation = false;
		yield return async;
	}
	
	public void ActivateScene() {
		async.allowSceneActivation = true;
	}
}
