using UnityEngine;
using System.Collections;

public class FlameS : MonoBehaviour {

	private SpriteRenderer mySprite;
	private Collider myCollider;

	void Start(){

		mySprite = GetComponent<SpriteRenderer>();
		myCollider = GetComponent<Collider>();

		TurnOff();

		if (!CurrentModeS.allowHazards){
			gameObject.SetActive(false);
		}

	}

	public void TurnOn(){

		if (CurrentModeS.allowHazards){
			mySprite.enabled = true;
			myCollider.enabled = true;
		}
		
	}

	public void TurnOff(){

		mySprite.enabled = false;
		myCollider.enabled = false;

	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.GetComponent<PlayerS>()){

			other.gameObject.GetComponent<PlayerS>().TakeDamage(9999, false);

		}

	}
}
