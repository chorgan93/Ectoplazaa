using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleAnimObjS : MonoBehaviour {

	public float animRateMax = 0.1f;
	private float animRateCountdown;
	private int currentFrame = 0;

	public List<Sprite> animFrames;
	private SpriteRenderer ownRender;

	public bool addToCamFollow;

	// Use this for initialization
	void Start () {

		animRateCountdown = animRateMax;
		ownRender = GetComponent<SpriteRenderer>();

		ownRender.sprite = animFrames[currentFrame];

		if (addToCamFollow){
			AdaptiveCameraPtS.A.hitPositions.Add(transform);
			CameraFollowS.F.PunchIn();
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		animRateCountdown -= Time.deltaTime*TimeManagerS.timeMult;

		if (animRateCountdown <= 0){
			animRateCountdown = animRateMax;

			currentFrame++;
			if (currentFrame > animFrames.Count-1){
				if (addToCamFollow){
					AdaptiveCameraPtS.A.hitPositions.Remove(transform);
				}
				Destroy(gameObject);
			}else{
				ownRender.sprite = animFrames[currentFrame];
			}
		}
	
	}
}
