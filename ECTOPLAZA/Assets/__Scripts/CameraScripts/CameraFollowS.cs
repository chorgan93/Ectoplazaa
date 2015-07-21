using UnityEngine;
using System.Collections;

public class CameraFollowS : MonoBehaviour {
	public GameObject			poi;
	public float				camEasing = 0.1f; //was 0.1f
	public Vector3				camOffset;
	public Vector3				camOffsetOnPause;
	//public Runner				runnerScript;
	//public float				zNear = -15;
	//public float				zFar  = -30;
	
	public Vector3 cameraPos;
	
	void Start(){
		//poi = GameObject.Find("AdaptiveCameraPt");
		Vector3 resetPos = poi.transform.position;
		resetPos.z = camOffset.z;
		transform.position = resetPos;
		//camera.backgroundColor = Color.black;

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
		

	}
}





