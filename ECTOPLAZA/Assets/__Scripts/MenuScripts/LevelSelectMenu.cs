using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectMenu : MonoBehaviour {

	public int currentCursorPos;

	public GameObject cursorObj;

	public List<GameObject> cursorPositions;
	public List<string> nextLevelStrings;

	public static string selectedLevelString = "4Concierge";
	public string nextSceneString;
	public string backSceneString;

	private float moveCursorSensitivity = 0.8f;
	private bool movedCursor = false;

	public int playerNum = 1;
	private string platformType; 

	// Use this for initialization
	void Start () {

		platformType = PlatformS.GetPlatform();

	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// back function
		if (Input.GetButton("XButtonPlayer" + playerNum + platformType)){
			Application.LoadLevel(backSceneString);
		}

		// select level function
		if (Mathf.Abs(Input.GetAxis("HorizontalPlayer" + playerNum + platformType)) > moveCursorSensitivity){
			if (!movedCursor){
				if (Input.GetAxis("HorizontalPlayer" + playerNum + platformType) > 0){
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
	
				movedCursor = true;
			}
		}
		else{
			movedCursor = false;
		}

		// set position of cursor
		cursorObj.transform.position = cursorPositions[currentCursorPos].transform.position;

		// move to character select function
		if (Input.GetButton("AButtonPlayer" + playerNum + platformType)){
			selectedLevelString = nextLevelStrings[currentCursorPos];
			Application.LoadLevel(nextSceneString);
		}
	
	}
}
