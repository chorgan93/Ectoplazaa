using UnityEngine;
using System.Collections;

public class PlayerScoreEffectS : MonoBehaviour {

	private PlayerS playerRef;

	private bool displaying = false;
	private float displayTimeMax = 1f;
	private float displayTimeCountdown;

	private int numFlickers = 10;
	private float flickerTime = 0.08f;
	private float flickerTimeCountdown;
	private int flickerCountdown;
	private bool isFlickering = false;

	private string currentString;
	private Vector3 currentStartPos;
	private Vector3 currentPos;

	private TextMesh myMesh;
	public TextMesh subMesh;
	private Color myColor;

	// Use this for initialization
	void Start () {

		playerRef = GetComponentInParent<PlayerS>();
		playerRef.SetScoreEffect(this);
		transform.parent = null;

		myMesh = GetComponent<TextMesh>();
		myMesh.text = "";

		if (CurrentModeS.isTeamMode){
			if (GlobalVars.teamNumber[playerRef.playerNum-1] == 1){
				myColor = Color.red;
			}
			else{
				myColor = Color.blue;
			}
		}
		else{
			myColor = playerRef.playerMats[playerRef.characterNum - 1].color;
		}

		myMesh.color = myColor;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		DoFlicker();
	
	}

	public void StartFlicker(string newString){

		transform.position = currentStartPos = currentPos = playerRef.transform.position + new Vector3(3,3,-1);
		flickerTimeCountdown = flickerTime;
		flickerCountdown = numFlickers;
		isFlickering = false;
		displayTimeCountdown = displayTimeMax;
		displaying = true;

		Color fixCol = myMesh.color;
		fixCol.a = 0;
		myMesh.color = fixCol;

		fixCol = subMesh.color;
		fixCol.a = 0;
		subMesh.color = fixCol;

		myMesh.text = subMesh.text = currentString = newString;
		gameObject.SetActive(true);

	}

	public void StartFlicker(string newString, Vector3 newTransform){

		// override player position on spawn
		transform.position = currentStartPos = currentPos = newTransform;
		flickerTimeCountdown = flickerTime;
		flickerCountdown = numFlickers;
		isFlickering = false;
		displayTimeCountdown = displayTimeMax;
		displaying = true;
		
		Color fixCol = myMesh.color;
		fixCol.a = 0;
		myMesh.color = fixCol;
		
		fixCol = subMesh.color;
		fixCol.a = 0;
		subMesh.color = fixCol;
		
		myMesh.text = subMesh.text = currentString = newString;
		gameObject.SetActive(true);
	}

	private void DoFlicker(){

		if (isFlickering){
			flickerTimeCountdown -= Time.deltaTime;
			if (flickerTimeCountdown <= 0){
				flickerTimeCountdown = flickerTime;

				if (myMesh.text != ""){
					myMesh.text = subMesh.text = "";
				}
				else{
					myMesh.text = subMesh.text = currentString;
				}
				flickerCountdown--;
				if (flickerCountdown <= 0){
					isFlickering = false;
				}
			}
		}
		else if (displaying){

			displayTimeCountdown -= Time.deltaTime;

			if (displayTimeMax - displayTimeCountdown < displayTimeMax/4f){

				float lerpOut = AnimCurveS.QuadEaseOut(displayTimeMax-displayTimeCountdown, 0, 6, displayTimeMax/4f);

				currentPos.y = currentStartPos.y + lerpOut;
				Color currentCol = myColor;
				currentCol.a = lerpOut/6f;
				myMesh.color = currentCol;

				currentCol = subMesh.color;
				currentCol.a = lerpOut/6f;
				subMesh.color = currentCol;
			}

			transform.position = currentPos;


			if (displayTimeCountdown <= 0){
				isFlickering = true;
				displaying = false;
			}

		}
		else{
			gameObject.SetActive(false);
		}

	}
}
