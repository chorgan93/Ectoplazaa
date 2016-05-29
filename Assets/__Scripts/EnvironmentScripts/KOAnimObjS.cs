using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KOAnimObjS : MonoBehaviour {

	public float animRateMax = 0.1f;
	private float animRateCountdown;
	private int currentFrame = 0;

	public List<Sprite> animFrames;
	private SpriteRenderer ownRender;

	public bool addToCamFollow;


	// Use this for initialization
	void Start () {

		ownRender = GetComponent<SpriteRenderer>();

		Initialize();


	
	}
	
	// Update is called once per frame
	void Update () {

		animRateCountdown -= Time.deltaTime*TimeManagerS.timeMult;

		if (animRateCountdown <= 0){
			animRateCountdown = animRateMax;

			currentFrame++;
			ownRender.color = Color.black;
			if (currentFrame > animFrames.Count-1){
				if (addToCamFollow){
					AdaptiveCameraPtS.A.hitPositions.Remove(transform);
				}

				SpawnManagerS.Instance.ReturnKO(this);
				gameObject.SetActive(false);
			}else{
				ownRender.sprite = animFrames[currentFrame];
			}
		}
	
	}

	public void TurnOn(Vector3 pos, Quaternion rot){

		transform.position = pos;
		transform.rotation = rot;

		Initialize();
		gameObject.SetActive(true);

	}

	private void Initialize(){
		
		animRateCountdown = animRateMax;
		ownRender.enabled = true;
		currentFrame = 0;

		ownRender.color = Color.white;

		ownRender.sprite = animFrames[currentFrame];

		if (addToCamFollow){
			AdaptiveCameraPtS.A.hitPositions.Add(transform);
		}
	}
}
