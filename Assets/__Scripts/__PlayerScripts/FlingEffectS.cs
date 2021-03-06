﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlingEffectS : MonoBehaviour {

	private SpriteRenderer ownRender;
	private float lifeTime = 0.22f;
	private float lifeTimeCountdown;
	private float frameRateCountdown;
	private int currentFrame = 0;

	public List<Sprite> chompReleaseFrames;
	public List<Sprite> elecReleaseFrames;
	public List<Sprite> fireReleaseFrames;
	public List<Sprite> specialReleaseFrames;

	private List<Sprite> spritesToUse = new List<Sprite>(); 

	private Color fadeCol;
	private float growRate = 50f;

	private Vector3 growSize;


	// Use this for initialization
	void Initialize () {

		ownRender = GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		frameRateCountdown -= Time.deltaTime*TimeManagerS.timeMult;

		if (frameRateCountdown <= 0){
			currentFrame++;
			if (currentFrame > spritesToUse.Count-1){
				Destroy(gameObject);
			}
			else{
				ownRender.sprite = spritesToUse[currentFrame];
				frameRateCountdown = lifeTime/spritesToUse.Count;
			}
		}
	
	}

	public void SetAttack(int attackNum){

		Initialize();
		switch(attackNum){
		case(-1):
			spritesToUse = chompReleaseFrames;
			break;
		case(1):
			spritesToUse = elecReleaseFrames;
			break;
		case(2):
			spritesToUse = fireReleaseFrames;
			break;
		default:
			spritesToUse = chompReleaseFrames;
			break;
		}
		frameRateCountdown = lifeTime/spritesToUse.Count;
		ownRender.sprite = spritesToUse[0];
			

	}
}
