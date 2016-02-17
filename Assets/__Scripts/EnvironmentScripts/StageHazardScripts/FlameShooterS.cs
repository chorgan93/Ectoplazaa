using UnityEngine;
using System.Collections;

public class FlameShooterS : MonoBehaviour {

	[Header("Flame Positioning")]
	public float flameMoveRate = 100f;
	private bool flamesMoving = false;
	private Vector3 flameTargetPos;
	private Vector3 flameResetPos;

	[Header("Flame Timing (In Seconds)")]
	public float flameShootRate = 7f;
	private float flameShootCountdown;
	public float flameActiveTime = 3f;
	private float flameActiveTimeCountdown;
	private bool flamesActive = false;

	private SpriteRenderer mySprite;
	public Color warningColor;
	private Color startColor;

	private int currentFlicker;
	private float warningStartTime = 3;
	private int slowFlickerCount = 4;
	private int fastFlickerCount = 21;
	private float flickerTimeMax;
	private float flickerCountdown;
	private bool startFlicker = false;

	private FlameS myFlames;

	// Use this for initialization
	void Start () {

		if (!CurrentModeS.allowHazards){
			gameObject.SetActive(false);
		}
		else{

		flameShootCountdown = flameShootRate;

		myFlames = GetComponentInChildren<FlameS>();

		flameResetPos = transform.position;
		flameResetPos.y -= myFlames.transform.localScale.y/2+transform.localScale.y/2;
		flameResetPos.z = myFlames.transform.position.z;

		//myFlames.transform.position = flameTargetPos;

		flameTargetPos = transform.position;
		flameTargetPos.y += myFlames.transform.localScale.y/2+transform.localScale.y/2;

		mySprite = GetComponent<SpriteRenderer>();
		startColor = mySprite.color;

		flickerTimeMax = warningStartTime/(slowFlickerCount*0.5f + fastFlickerCount*1.0f);

		}
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (flamesActive){

			startFlicker = false;

			flameActiveTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			if (flameActiveTimeCountdown <= 0){
				flamesActive = false;
				//myFlames.transform.position = flameResetPos;
				myFlames.TurnOff();
				flameShootCountdown = flameShootRate;
			}

			// flames no longer move so code below is deprecated
			/*
			if (flamesMoving){

				Vector3 changeFlamePos = myFlames.transform.position;
				//changeFlamePos.y += flameMoveRate*Time.deltaTime*TimeManagerS.timeMult;
				changeFlamePos.y = flameTargetPos.y;
				if (changeFlamePos.y >= flameTargetPos.y){
					changeFlamePos.y = flameTargetPos.y;
					myFlames.transform.position = changeFlamePos;
					flamesMoving = false;
				}
				myFlames.transform.position = changeFlamePos;

			}
			else{
				flameActiveTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
				if (flameActiveTimeCountdown <= 0){
					flamesActive = false;
					myFlames.transform.position = flameResetPos;
					myFlames.TurnOff();
					flameShootCountdown = flameShootRate;
				}
			}
			*/
		}
		else{

			flameShootCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			if (flameShootCountdown <= warningStartTime){
				startFlicker = true;
			}
			
			WarningFlashes();

			if (flameShootCountdown <= 0){
				flamesActive = true;
				flamesMoving = true;
				myFlames.TurnOn();
				flameActiveTimeCountdown = flameActiveTime;
			}

		}
	
	}

	private void WarningFlashes(){

		if (startFlicker){

			flickerCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			if (flickerCountdown <= 0){

				currentFlicker++;
				if (currentFlicker < slowFlickerCount){
					flickerCountdown = flickerTimeMax*2;
				}
				else{
					flickerCountdown = flickerTimeMax;
				}

				if (mySprite.color == startColor){
					mySprite.color = warningColor;
				}
				else{
					mySprite.color = startColor;
				}

				if (currentFlicker > slowFlickerCount + fastFlickerCount){
					startFlicker = false;
				}
			}
		}
		else{
			mySprite.color = startColor;
			currentFlicker = 0;
			flickerCountdown = flickerTimeMax*2;
		}

	}
}
