using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeSelectMenuS : MonoBehaviour {
	
	public int currentCursorPos;
	
	public GameObject cursorObj;
	float cursorSpeed= .25f;
	public List<GameObject> cursorPositions;
	//public List<string> nextLevelStrings;
	public List<int> modesToChoose;

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

	public FadeObjS fadeIn;
	
	public GameObject loadPt;
	public GameObject loadBackPt;
	
	private CameraFollowS followRef;
	
	public GameObject levelSelectSFXObj;
	public List<GameObject> scrollSFXObjs;

	public List<GameObject> cameraPts;

	private bool goingBack = false;

	
	private float fadeItemTime = 0.2f;
	public FadeObjS[] fadeObjs;
	public FadeObjS[] fadeObjsText;
	
	AsyncOperation async;
	
	
	// Use this for initialization
	void Start () {
		
		platformType = PlatformS.GetPlatform();


		cursorObj.transform.position = cursorPositions[currentCursorPos].transform.position;
		
		followRef = GetComponent<CameraFollowS>();
		followRef.poi = cameraPts[0];

		loadBackPt.transform.position = followRef.transform.position;

		cursorLabel.text = "";

		StartCoroutine(FadeMenuItems());
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (startedLoading){
			Debug.Log(async.progress);
			if (async.progress >= 0.89f){
				ActivateScene();

			}
		}
		
		else if (startedLoadDelay){
			loadDelay -= Time.deltaTime;
			if (goingBack){
				followRef.poi = loadBackPt;
			}
			else{
			followRef.poi = loadPt;
			}
			if (loadDelay <= 0){
				startedLoading = true;
				if (goingBack){
					StartLoadingOut();
				}
				else{
				StartLoading();
				}
			}
		}
		
		else {
			if (delayInput > 0){
				delayInput -= Time.deltaTime;
			}
			else{
				// follow cursor
				followRef.poi = cameraPts[currentCursorPos];
				
				// back function
				if (Input.GetButton("BButtonAllPlayers" + platformType)){
					fadeIn.FadeOut();
					startedLoadDelay = true;
					goingBack = true;
					Instantiate(levelSelectSFXObj);
				}
				
				// select level function
				if (Mathf.Abs(Input.GetAxis("Horizontal")) > moveCursorSensitivity){
					if (!movedCursor){
						if (Input.GetAxis("Horizontal") > 0){
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
					}
				}
				else{
					movedCursor = false;
				}
				
				// set position of cursor
				cursorObj.transform.position = Vector3.Lerp( cursorObj.transform.position, cursorPositions[currentCursorPos].transform.position,cursorSpeed);
				
				// move to character select function
				if (Input.GetButton("AButtonAllPlayers" + platformType)){
					CurrentModeS.currentMode = modesToChoose[currentCursorPos];
					startedLoadDelay = true;
					
					fadeIn.FadeOut();
					Instantiate(levelSelectSFXObj);
				}
			}
		}
		
	}
	
	public void StartLoading() {
		StartCoroutine(load());
	}

	public void StartLoadingOut(){
		StartCoroutine(loadOut ());
	}

	private IEnumerator FadeMenuItems(){
		
		delayInput = 1000f;
		
		yield return new WaitForSeconds(0.3f);
		
		foreach(FadeObjS fade in fadeObjs){

			fade.gameObject.SetActive(true);
			
			yield return new WaitForSeconds(fadeItemTime);
		}

		delayInput = 0;
	}

	private IEnumerator FadeMenuText(){

		
		yield return new WaitForSeconds(0.3f);
		
		foreach(FadeObjS fade in fadeObjsText){
			
			fade.gameObject.SetActive(true);
			
			yield return new WaitForSeconds(fadeItemTime);
		}

	}

	
	IEnumerator load() {
		Debug.LogWarning("ASYNC LOAD STARTED - " +
		                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");

		async = Application.LoadLevelAsync(nextSceneString);
		async.allowSceneActivation = false;
		yield return async;
	}

	IEnumerator loadOut (){
		Debug.LogWarning("ASYNC LOAD STARTED - " +
		                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
		
		async = Application.LoadLevelAsync(backSceneString);
		async.allowSceneActivation = false;
		yield return async;
	}
	
	public void ActivateScene() {
		async.allowSceneActivation = true;
	}


}
