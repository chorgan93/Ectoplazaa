using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEffectS : MonoBehaviour {

	// script for attack effect that is placed on effect sprite
	// there is an effect sprite behind every player sprite and player tail collider
	// turns on once player reaches level 2 charge
	// animates as electricity at level 2
	// animates as fire at level 3

	public int attackNum = 1; // 1 is electric, 2 is fire, 0 is air

	public List<Sprite> effectFrames;
	public float animRateMax;
	private float animRateCountdown;
	private int currentFrame = 0;

	public PlayerS playerRef;
	private SpriteRenderer playerRender;

	private SpriteRenderer ownRender;

	private Quaternion startRotation;
	private Quaternion attackRotation;
	
	public GameObject dustObj;
	public float spawnRad = 3f;
	public float spawnRate = 0.08f;
	private float spawnRateCountdown;

	public bool isSpecial = false;

	// Use this for initialization
	void Start () {

		attackRotation = transform.localRotation;
		startRotation = Quaternion.Euler(attackRotation.x, attackRotation.y, attackRotation.z+90f);

		currentFrame = Mathf.FloorToInt(Random.Range(0,effectFrames.Count));

		ownRender = GetComponent<SpriteRenderer>();
		playerRender = playerRef.spriteObject.GetComponent<SpriteRenderer>();

		// turn on if player is at appropriate charge
		if (playerRef != null){
			if (playerRender.enabled){
		if ((playerRef.charging && playerRef.GetChargeTime() > playerRef.GetChargeLv2Min()
		     && attackNum == 1) ||
		    (playerRef.charging && playerRef.GetChargeTime() > playerRef.GetChargeLv3Min()
		 && attackNum == 2)
		|| (playerRef.attacking && playerRef.attackToPerform >= attackNum)){
			ownRender.enabled = true;

				
		}
		else{
			ownRender.enabled = false;
		}
			}
			else{
				ownRender.enabled = false;
			}
		}
		
		ownRender.sprite = effectFrames[currentFrame];
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// turn on at appropriate charge
		if (playerRef != null){

			if (isSpecial){



			}
			else{
			if (attackNum == 0){
				if ((playerRef.attacking || (playerRef.groundPounded && !playerRef.groundDetect.Grounded())) 
				    && playerRef.GetComponent<Rigidbody>().velocity != Vector3.zero){ownRender.enabled = true;}
				else{ownRender.enabled = false;}
			}

		if (attackNum == 1){
			if (playerRef.charging && playerRef.GetChargeTime() > playerRef.GetChargeLv2Min()){
				ownRender.enabled = true;
			}
		}
		if (attackNum == 2){
			if (playerRef.charging && playerRef.GetChargeTime() > playerRef.GetChargeLv3Min()){
				ownRender.enabled = true;
			}

					if (playerRef.attacking){
						transform.localRotation = attackRotation;
					}
					else{
						transform.localRotation = startRotation;
					}
		}
		// turn off once not attacking
		if (!playerRef.isDangerous && !playerRef.charging){
			ownRender.enabled = false;
		}

			// animate while sprite is rendered
		if (ownRender.enabled){
			animRateCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			if (animRateCountdown <= 0){
				animRateCountdown = animRateMax;
				currentFrame++;
				if (currentFrame > effectFrames.Count-1){
					currentFrame = 0;
				}
			}
			ownRender.sprite = effectFrames[currentFrame];

				

					if (attackNum >= 1 && playerRef.attacking){
						SpawnSmoke();
					}
			}

			else{
				Vector3 fixPos = Vector3.zero;
				fixPos.z = transform.localPosition.z;
				transform.localPosition = fixPos;
				transform.localRotation = Quaternion.identity;
			}
		}
		}

	}

	void SpawnSmoke(){

		spawnRateCountdown -= Time.deltaTime;
		if (spawnRateCountdown <= 0){
			spawnRateCountdown = spawnRate;

			Vector3 spawnPos = transform.position+Random.insideUnitSphere*spawnRad;
			spawnPos.z = transform.position.z+1f;

			if (attackNum == 2){
				SpawnManagerS.Instance.SpawnSmoke(spawnPos, Quaternion.Euler(new Vector3(0,0,Random.Range(0,360))));
			}
			if (attackNum == 1){
				SpawnManagerS.Instance.SpawnElectricity(spawnPos, Quaternion.Euler(new Vector3(0,0,Random.Range(0,360))));
			}
		}

	}
	

}
