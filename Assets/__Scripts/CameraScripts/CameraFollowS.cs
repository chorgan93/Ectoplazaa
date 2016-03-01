using UnityEngine;
using System.Collections;

public class CameraFollowS : MonoBehaviour {
	public GameObject			poi;
	public float				camEasing = 0.1f; //was 0.1f
	public Vector3				camOffset;
	public Vector3				camOffsetOnPause;

	private float punchTimeCountdown;
	private float currentCamSize;
	private float targetCamSize;
	public float camSizeEasing = 0.1f;
	private Camera ownCam;
	//public Runner				runnerScript;
	//public float				zNear = -15;
	//public float				zFar  = -30;

	private float adaptSizeMult;
	private float minSize;
	public float maxSizeDiff;
	
	public Vector3 cameraPos;

	public static CameraFollowS F;

	public bool newStartPos = false;

	private AdaptiveCameraPtS inGameFollow;

	private bool focusOnCharacter;
	private float deathPunchMult = 0.96f;
	private GameObject focusChar;
	private float specialCamEaseMult = 4f;
	private float specialCamSize = 12f;
	private float specialTimeMax = 0.6f;
	private float endSpecialCountdown;



	void Awake () {
		F = this;
	}
	
	void Start(){

		if (!newStartPos){
		Vector3 resetPos = poi.transform.position;
		resetPos.z = camOffset.z;
		transform.position = resetPos;
		}
		//camera.backgroundColor = Color.black;

		ownCam = GetComponent<Camera>();
		minSize = ownCam.orthographicSize;
		currentCamSize = minSize;

		Cursor.visible = false;

		if (poi.GetComponent<AdaptiveCameraPtS>()){
			inGameFollow = poi.GetComponent<AdaptiveCameraPtS>();
		}

	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (Cursor.visible){
				Application.Quit();
			}
			else{
				Cursor.visible = true;
			}
		}
		if (Input.GetMouseButtonDown(0)){
			Cursor.visible = false;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		EndSpecialCam();

		// [] get the position of the golfball
		Vector3 poiPos = poi.transform.position;

		if (focusOnCharacter){
			poiPos = focusChar.transform.position;
		}

		if (TimeManagerS.paused){
			poiPos += camOffsetOnPause;
		}
		else{
			poiPos += camOffset;
		}
		// [] set the xy of this to that of the golfball
		cameraPos = this.transform.position;
		if (focusOnCharacter){
			cameraPos.x = (1-camEasing*specialCamEaseMult)*cameraPos.x + camEasing*specialCamEaseMult*poiPos.x;
			cameraPos.y = (1-camEasing*specialCamEaseMult)*cameraPos.y + camEasing*specialCamEaseMult*poiPos.y;
		}
		else{
			cameraPos.x = (1-camEasing)*cameraPos.x + camEasing*poiPos.x;
			cameraPos.y = (1-camEasing)*cameraPos.y + camEasing*poiPos.y;
		}
		// Pull back based on Runner's speed
		//float speedU = (runnerScript.rigidbody.velocity.x)/runnerScript.maxSpeed;
		//float camZ = (1-speedU)*zNear + speedU*zFar;
		//camPos.z = (1-camEasing/4)*camPos.z + camEasing/4*camZ;


		this.transform.position = cameraPos;


		FindCurrentSize();


		if (CameraShakeS.C.sleeping && !CameraShakeS.C.dontPunch){
			ownCam.orthographicSize = currentCamSize;
		}
		else{
			ownCam.orthographicSize = currentCamSize;
		}
		

	}

	void FindCurrentSize(){
		if (!focusOnCharacter){
			targetCamSize = minSize + maxSizeDiff*adaptSizeMult;
			currentCamSize = (1-camSizeEasing)*currentCamSize + camSizeEasing*targetCamSize;
		}
		else{
			targetCamSize = specialCamSize;
			currentCamSize = (1-camSizeEasing*specialCamEaseMult)*currentCamSize + 
				camSizeEasing*specialCamEaseMult*targetCamSize;
		}



	}

	public void SetCamMult(float newMult){
		adaptSizeMult = newMult;
	}

	public void StartSpecialCam(GameObject newTarget){
		focusOnCharacter = true;
		focusChar = newTarget;
		endSpecialCountdown = specialTimeMax;

	}

	private void EndSpecialCam(){
		if (focusOnCharacter){
			endSpecialCountdown -= Time.deltaTime/Time.timeScale;
			if (endSpecialCountdown<=0){
				focusOnCharacter = false;
			}
		}
	}

	public void DeathPunchIn(){
		if (!focusOnCharacter){
			ownCam.orthographicSize = (minSize + maxSizeDiff*adaptSizeMult)*deathPunchMult;
		}
	}
}





