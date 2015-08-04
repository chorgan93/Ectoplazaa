﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEffectS : MonoBehaviour {

	public int attackNum = 1; // 1 is electric, 2 is fire

	public List<Sprite> effectFrames;
	public float animRateMax;
	private float animRateCountdown;
	private int currentFrame = 0;

	public PlayerS playerRef;

	private SpriteRenderer ownRender;

	// Use this for initialization
	void Start () {

		currentFrame = Mathf.FloorToInt(Random.Range(0,effectFrames.Count));

		ownRender = GetComponent<SpriteRenderer>();
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
		
		ownRender.sprite = effectFrames[currentFrame];
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (attackNum == 1){
			if (playerRef.charging && playerRef.GetChargeTime() > playerRef.GetChargeLv2Min()){
				ownRender.enabled = true;
			}
		}
		if (attackNum == 2){
			if (playerRef.charging && playerRef.GetChargeTime() > playerRef.GetChargeLv3Min()){
				ownRender.enabled = true;
			}
		}
		// turn off once not attacking
		if (!playerRef.isDangerous && !playerRef.charging){
			ownRender.enabled = false;
		}

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

			if (playerRef.charging){
				transform.localRotation = Quaternion.Euler(new Vector3(0,0,90));
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