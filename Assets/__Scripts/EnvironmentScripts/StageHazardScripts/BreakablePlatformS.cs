using UnityEngine;
using System.Collections;

public class BreakablePlatformS: MonoBehaviour {


	private int durability = 5;

	private SpriteRenderer mySprite;
	private Collider myCollider;

	public Sprite[] destructStates;
		private int currentSprite = 0;

	void Start(){

		if (!CurrentModeS.allowHazards){

			gameObject.SetActive(false);

		}
		else{
		mySprite = GetComponent<SpriteRenderer>();
		myCollider = GetComponent<Collider>();

			mySprite.sprite = destructStates[currentSprite];
		}

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