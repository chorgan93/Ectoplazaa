using UnityEngine;
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

		ownRender = GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (playerRef.charging && playerRef.GetChargeTime() > playerRef.GetChargeLv2Min()){
			ownRender.enabled = true;
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
		}
	
	}
}
