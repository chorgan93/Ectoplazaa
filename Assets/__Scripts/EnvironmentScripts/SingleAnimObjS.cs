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

	public bool dontDestroy = false;
	public bool stopAtEnd = false;

	// Use this for initialization
	void Start () {

		animRateCountdown = animRateMax;
		ownRender = GetComponent<SpriteRenderer>();

		ownRender.sprite = animFrames[currentFrame];

		if (addToCamFollow){
			AdaptiveCameraPtS.A.hitPositions.Add(transform);
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
				if (!dontDestroy){
					Destroy(gameObject);
				}
				else{
					if (stopAtEnd){
						currentFrame = animFrames.Count-1;
					}
					else{
						currentFrame = 0;
					}
					ownRender.sprite = animFrames[currentFrame];
				}
			}else{
				ownRender.sprite = animFrames[currentFrame];
			}
		}
	
	}
}
