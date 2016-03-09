using UnityEngine;
using System.Collections;

public class CameraShakeS : MonoBehaviour {

	public static float shakeStrengthMult = 1f;

	// Variables for shaking
	public Vector3				originPosition;
	public float				shake_intensity;
	public float				shake_decay;
	
	public float 				camZ = -10;
	
	private float				smallShakeIntensity = 1.2f; // amount of shake
	private float				smallShakeDuration = 0.3f; // how long shake lasts
	
	private float				largeShakeIntensity = 2.4f;
	private float				largeShakeDuration = 0.4f;
	
	// how long time pauses when sleep function is called
	public float				sleepDuration;
	public bool 				shaking = false; // true when camera is shaking
	public bool					sleeping = false; // true when time is sleeping

	private int shakePriority = -1;

	private bool 				halfSleep = false;

	public bool dontPunch = false;

	private float delayShakeTime = 1f;

	private CameraFollowS ownFollow;
	
	public float microShakeDiv = 8;
	
	//private Vector3 startPos;
	
	public static CameraShakeS	C;

	public static bool timeSleepOn = true;
	
	void Start(){
		
		C = this;
		
		originPosition = transform.position;
		ownFollow =GetComponent<CameraFollowS>();
		//print (ownFollow);
		
	}
	
	// Update is called once per frame
	void Update(){

		// don't shake for first second of game
		if (delayShakeTime > 0){
			delayShakeTime -= Time.deltaTime;
		}
		else{

		if(shake_intensity > 0 && !sleeping){
			
			Vector3 camPos = ownFollow.cameraPos;
			camPos.x += Random.insideUnitSphere.x * shake_intensity * shakeStrengthMult;
			camPos.y += Random.insideUnitSphere.y*0.75f * shake_intensity * shakeStrengthMult;
			camPos.z = transform.position.z;
			transform.position = camPos;
			
			shake_intensity -= shake_decay*Time.deltaTime;
		}
		}

		if (sleeping){
			
			if (timeSleepOn){
				if (halfSleep){
					Time.timeScale = 0.5f;
				}
				else{
					Time.timeScale = 0f;
				}
			}
			
			sleepDuration -= Time.unscaledDeltaTime;
			if (sleepDuration <= 0){
				Time.timeScale = 1;
				sleeping = false;
				halfSleep = false;
				dontPunch = false;
			}
			
		}
		
		else{
			
			if (shake_intensity <= 0 && shaking){
				//originPosition = transform.position;
				originPosition = ownFollow.transform.position;
				this.transform.position = originPosition;
				shaking = false;
				shakePriority = -1;
			}
			
		}
		
	}
	
	void FixedUpdate () {
		

		
		
	}
	
	// tiniest shake
	public void MicroShake(){
		if (shakePriority <= 0){
		//originPosition = transform.position;
		originPosition = ownFollow.transform.position;
		shake_intensity = smallShakeIntensity/microShakeDiv;
		shake_decay = smallShakeIntensity/(microShakeDiv*smallShakeDuration);
		shaking = true;
		dontPunch = true;
			shakePriority = 0;
		}
	}
	
	// small amount of shake
	public void SmallShake(){
		if (shakePriority <= 1){
		//originPosition = transform.position;
		originPosition = ownFollow.transform.position;
		shake_intensity = smallShakeIntensity;
		shake_decay = smallShakeIntensity/smallShakeDuration;
		shaking = true;
			shakePriority = 1;
		}
	}
	
	// large amount of shake
	public void LargeShake(){
		if (shakePriority <= 2){
		//originPosition = transform.position;
		originPosition = ownFollow.transform.position;
		shake_intensity = largeShakeIntensity;
		shake_decay = largeShakeIntensity/largeShakeDuration;
		shaking = true;
			shakePriority = 2;
		}
	}
	
	// time freeze
	public void TimeSleep(float sleepTime) {

		if (timeSleepOn){
		Time.timeScale = 0.4f;
		}
		
		//print ("SLEEPING");
		sleepDuration = sleepTime;
		sleeping = true;
	}

	public void PunchIn(){
		ownFollow.DeathPunchIn();
	}

	// time freeze
	public void HalfTimeSleep(float sleepTime) {
		
		Time.timeScale = 0.4f;
		
		//print ("SLEEPING");
		sleepDuration = sleepTime;
		sleeping = true;
		halfSleep = true;
	}

	public void DisableShaking(){
		delayShakeTime = 1f;
	}
}














