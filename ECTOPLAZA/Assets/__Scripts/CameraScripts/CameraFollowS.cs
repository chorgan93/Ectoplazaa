using UnityEngine;
using System.Collections;

public class CameraFollowS : MonoBehaviour {
	public GameObject			poi;
	public float				camEasing = 0.1f; //was 0.1f
	public Vector3				camOffset;
	public Vector3				camOffsetOnPause;

	private bool punchedIn = false;
	private float punchInMult = 0.99f;
	private float punchInMax = 0.08f;
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



	void Awake () {
		F = this;
	}
	
	void Start(){
		//poi = GameObject.Find("AdaptiveCameraPt");
		if (!newStartPos){
		Vector3 resetPos = poi.transform.position;
		resetPos.z = camOffset.z;
		transform.position = resetPos;
		}
		//camera.backgroundColor = Color.black;

		ownCam = GetComponent<Camera>();
		minSize = ownCam.orthographicSize;
		currentCamSize = minSize;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// [] get the position of the golfball
		Vector3 poiPos = poi.transform.position;
		if (TimeManagerS.paused){
			poiPos += camOffsetOnPause;
		}
		else{
			poiPos += camOffset;
		}
		// [] set the xy of this to that of the golfball
		cameraPos = this.transform.position;
		cameraPos.x = (1-camEasing)*cameraPos.x + camEasing*poiPos.x;
		cameraPos.y = (1-camEasing)*cameraPos.y + camEasing*poiPos.y;
		// Pull back based on Runner's speed
		//float speedU = (runnerScript.rigidbody.velocity.x)/runnerScript.maxSpeed;
		//float camZ = (1-speedU)*zNear + speedU*zFar;
		//camPos.z = (1-camEasing/4)*camPos.z + camEasing/4*camZ;


		this.transform.position = cameraPos;


		FindCurrentSize();

		if (punchedIn){
			punchTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			ownCam.orthographicSize = currentCamSize*punchInMult;
			if (punchTimeCountdown <= 0){
				punchedIn = false;
			}
		}
		else{
			ownCam.orthographicSize = currentCamSize;
		}
		

	}

	void FindCurrentSize(){
		targetCamSize = minSize + maxSizeDiff*adaptSizeMult;
		currentCamSize = (1-camSizeEasing)*currentCamSize + camSizeEasing*targetCamSize;
	}

	public void PunchIn(){
		punchedIn = true;
		punchTimeCountdown = punchInMax;
	}

	public void SetCamMult(float newMult){
		adaptSizeMult = newMult;
	}
}





