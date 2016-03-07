using UnityEngine;
using System.Collections;

public class BreakablePlatformS: MonoBehaviour {


	private int durability = 5;

	private SpriteRenderer mySprite;
	private Collider myCollider;

	public Sprite[] destructStates;
		private int currentSprite = 0;

	private float shakeIntensity = 1.5f;
	private float shakeDecay = 5f;
	private float currentShakeIntensity;
	private Vector3 spriteOffset;

	void Start(){

		if (!CurrentModeS.allowHazards){

			gameObject.SetActive(false);

		}
		else{
		mySprite = GetComponentInChildren<SpriteRenderer>();
		myCollider = GetComponent<Collider>();

			mySprite.sprite = destructStates[currentSprite];
		}

		spriteOffset = Vector3.zero;

	}

	void FixedUpdate () {

		if (currentShakeIntensity > 0){

			currentShakeIntensity -= shakeDecay*Time.deltaTime;

			spriteOffset = Random.insideUnitSphere*currentShakeIntensity;
			spriteOffset.z = 0;

		}
		else{
			spriteOffset = Vector3.zero;
		}
		
		mySprite.transform.localPosition = spriteOffset;

	}

	private void TriggerShake(){
		CameraShakeS.C.SmallShake();
		currentShakeIntensity = shakeIntensity;
	}

	private void TakeDamage(){

		durability--;
		currentSprite++;
		if (durability <= 0){
			mySprite.enabled = false;
			myCollider.enabled = false;
		}
		else{
			mySprite.sprite = destructStates[currentSprite];
			TriggerShake();
		}


	}

	void OnCollisionEnter(Collision other){

		if (other.gameObject.GetComponent<PlayerS>()){

			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();

			if (playerRef.isDangerous || playerRef.GetChompState() || playerRef.GetSpecialState()){

				TakeDamage();

			}

		}



	}


}