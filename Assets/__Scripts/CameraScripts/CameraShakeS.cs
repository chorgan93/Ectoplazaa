using UnityEngine;
using System.Collections;

public class CameraShakeS : MonoBehaviour {

	public static float shakeStrengthMult = 1f;

	// Variables for shaking
	public Vector3				originPosition;
	public float				shake_intensity;
	public float				shake_decay;
	
	public float 				camZ = -10;
	
	private float				smallShakeIntensity = 1.4f; // amount of shake
	private float				smallShakeDuration = 0.3f; // how long shake lasts
	
	private float				largeShakeIntensity = 2.5f;
	private float				largeShakeDuration = 0.6f;
	
	// how long time pauses when sleep function is called
	public float				sleepDuration;
	public bool 				shaking = false; // true when camera is shaking
	public bool					sleeping = false; // true when time is sleeping

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
			camPos.y += Random.insideUnitSphere.y/2 * shake_intensity * shakeStrengthMult;
			camPos.z = transform.position.z;
			transform.position = camPos;
			
			shake_intensity -= shake_decay*Time.deltaTime;
		}
		}
		
	}
	
	void FixedUpdate () {
		
		if (sleeping){

			if (timeSleepOn){
			if (halfSleep){
				Time.timeScale = 0.5f;
			}
			else{
				Time.timeScale = 0.1f;
			}
			}
			
			sleepDuration -= Time.deltaTime/Time.timeScale;
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
			}
			
		}
		
		
	}
	
	// tiniest shake
	public void MicroShake(){
		//originPosition = transform.position;
		originPosition = ownFollow.transform.position;
		shake_intensity = smallShakeIntensity/microShakeDiv;
		shake_decay = smallShakeIntensity/(microShakeDiv*smallShakeDuration);
		shaking = true;
		dontPunch = true;
	}
	
	// small amount of shake
	public void SmallShake(){
		//originPosition = transform.position;
		originPosition = ownFollow.transform.position;
		shake_intensity = smallShakeIntensity;
		shake_decay = smallShakeIntensity/smallShakeDuration;
		shaking = true;
	}
	
	// large amount of shake
	public void LargeShake(){
		//originPosition = transform.position;
		originPosition = ownFollow.transform.position;
		shake_intensity = largeShakeIntensity;
		shake_decay = largeShakeIntensity/largeShakeDuration;
		shaking = true;
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














