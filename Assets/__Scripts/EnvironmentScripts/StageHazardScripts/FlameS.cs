using UnityEngine;
using System.Collections;

public class FlameS : MonoBehaviour {

	private SpriteRenderer mySprite;
	public SpriteRenderer smokeSprite;
	private Collider myCollider;

	public Sprite[] flameSprites;
	public Sprite[] smokeSprites;

	public float smokeAnimRate;
	public float flameAnimRate;

	public int flameResetSprite = 3;
	private float flameAnimRateCountdown;
	private float smokeAnimRateCountdown;
	private int currentFlameSprite;
	private int currentSmokeSprite;

	void Start(){

	

		if (!CurrentModeS.allowHazards){
			gameObject.SetActive(false);
		}
		else{
			mySprite = GetComponent<SpriteRenderer>();
			myCollider = GetComponent<Collider>();
			
			TurnOff();
		}

	}

	void FixedUpdate () {

		if (mySprite.enabled){
			smokeSprite.enabled = true;

			flameAnimRateCountdown -= Time.deltaTime;
			if (flameAnimRateCountdown <= 0){
				flameAnimRateCountdown = flameAnimRate;
				currentFlameSprite++;
				if (currentFlameSprite > flameSprites.Length-1){
					currentFlameSprite = flameResetSprite;
				}
			}

			smokeAnimRateCountdown -= Time.deltaTime;
			if (smokeAnimRateCountdown <= 0){
				smokeAnimRateCountdown = smokeAnimRate;
				currentSmokeSprite++;
				if (currentSmokeSprite > smokeSprites.Length-1){
					currentSmokeSprite = 0;
				}
			}

			mySprite.sprite = flameSprites[currentFlameSprite];
			smokeSprite.sprite = smokeSprites[currentSmokeSprite];
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
		smokeSprite.enabled = false;
		myCollider.enabled = false;

	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.GetComponent<PlayerS>()){

			if (other.gameObject.GetComponent<PlayerS>().respawnInvulnTime <= 0
			    && other.gameObject.GetComponent<PlayerS>().dodgeTimeCountdown <= 0){

				other.gameObject.GetComponent<PlayerS>().TakeDamage(9999, false);
			
			}

		}

	}
}
