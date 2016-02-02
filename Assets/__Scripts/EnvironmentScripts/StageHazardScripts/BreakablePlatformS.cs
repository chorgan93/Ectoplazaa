using UnityEngine;
using System.Collections;

public class BreakablePlatformS: MonoBehaviour {


	private int durability = 5;

	private SpriteRenderer mySprite;
	private Collider myCollider;

	void Start(){

		if (!CurrentModeS.allowHazards){

			gameObject.SetActive(false);

		}

		mySprite = GetComponent<SpriteRenderer>();
		myCollider = GetComponent<Collider>();

	}

	private void TakeDamage(){

		durability--;
		if (durability <= 0){
			mySprite.enabled = false;
			myCollider.enabled = false;
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