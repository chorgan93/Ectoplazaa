using UnityEngine;
using System.Collections;

public class GlobS : MonoBehaviour {

	// script for ecto mode ectoplasm orbs

	public GameObject parentGO;

	Vector3 originalScale; 

	bool activated = false; 
	float deletionTimer = 10f, deletionCounter; 

	private float triggerFlashesTime = 3;
	private int numSuperSlowFlashes = 8;
	private int numSlowFlashes = 12;
	private int numFastFlashes = 16;
	private float currentFlashCountdown;
	private float timeBetweenFlashes;
	private int currentFlash;

	float invulnTime = .75f;

	float sizeRandomizer = 0.25f;

	float fadeColRate = 0.005f;
	private Renderer ownRender;
	public SpriteRenderer ownSprite;

	public Renderer outline;

	public SpriteRenderer ectoGlow;
	private float ectoAlpha = 0.5f;

	public float gravTime = 3f;

	public GameObject sfxObj;

	public GameObject touchEffect;

	public Rigidbody ownRigid;

	// Use this for initialization
	void Start () {
		deletionCounter = deletionTimer;

		// randomize size of orbs

		Vector3 newSize = parentGO.transform.localScale;
		newSize.x += sizeRandomizer*Random.insideUnitCircle.x;
		newSize.y = newSize.x;
		parentGO.transform.localScale = newSize;
		originalScale = parentGO.transform.localScale; 


		ownRender = parentGO.GetComponent<Renderer>();

		// set color to player color from which ecto was spawned
		Color ectoColor = ownSprite.color;
		ectoColor.a = ectoAlpha;
		ectoGlow.color = ectoColor;
		//activated = true;

		timeBetweenFlashes = currentFlashCountdown = triggerFlashesTime/
			(numSuperSlowFlashes*2+numSlowFlashes+numFastFlashes*0.5f)*1f;
	}

	void FixedUpdate()
	{

		Color parentCol = ownRender.material.color;
		//print (parentCol);
		parentCol.r += fadeColRate*TimeManagerS.timeMult*TimeManagerS.timeMult;
		parentCol.g += fadeColRate*TimeManagerS.timeMult*TimeManagerS.timeMult;
		parentCol.b += fadeColRate*TimeManagerS.timeMult*TimeManagerS.timeMult;

		if (parentCol.r > 1){
			parentCol.r = 1;
		}
		if (parentCol.b > 1){
			parentCol.b = 1;
		}
		if (parentCol.g > 1){
			parentCol.g= 1;
		}
		//parentCol.g = parentCol.b = parentCol.r;
		ownRender.material.color = parentCol;

		// set short delay for pick up so they're not immediately picked up on spawn
		invulnTime -= Time.deltaTime*TimeManagerS.timeMult;

		// globs decrease in size as time goes on, then destroy selves at end of lifespan
		if (activated) {


			deletionCounter -= 1f*Time.deltaTime*TimeManagerS.timeMult; 

			ownRigid.useGravity = false;

		}
		else{
			
			deletionCounter -= 1f*Time.deltaTime*TimeManagerS.timeMult; 
		}

		if (deletionCounter <= triggerFlashesTime){
			DoFlashes();
		}

		if (deletionCounter < 0) {

			GameObject.Destroy(parentGO.gameObject);
			GameObject.Destroy(this.gameObject);

		}

		ectoGlow.transform.rotation = Quaternion.Euler(Vector3.zero);

		// globs start at zero grav, then drop after gravTime hits zero
		gravTime -= Time.deltaTime*TimeManagerS.timeMult;
		if (gravTime <= 0){
			if (!activated){
				ownRigid.useGravity = true;
			}
			else{
				ownRigid.useGravity = false;
			}
		}
	}

	private void DoFlashes(){

		currentFlashCountdown -= Time.deltaTime*TimeManagerS.timeMult;
		if (currentFlashCountdown <= 0){
			currentFlash++;
			if (currentFlash < numSuperSlowFlashes){
				currentFlashCountdown = timeBetweenFlashes*2f;
			}
			else if (currentFlash > numSuperSlowFlashes &&
			         currentFlash < numFastFlashes){
				currentFlashCountdown = timeBetweenFlashes;
			}
			else{
				currentFlashCountdown = timeBetweenFlashes/2f;
			}

			ownRender.enabled = outline.enabled = ownSprite.enabled = ectoGlow.enabled = !ownRender.enabled;

		}

	}

	void OnTriggerEnter(Collider other) 
	{

		// when colliding with player, add to player score (ectomode)
		if (other.tag == "Player" && invulnTime <= 0) {
			parentGO.GetComponent<SphereCollider> ().enabled = false; 
			parentGO.GetComponent<Rigidbody> ().useGravity = false;  
			parentGO.GetComponent<Rigidbody> ().velocity = Vector3.zero; 

			PlayerS playerRef = other.gameObject.GetComponent<PlayerS> ();
			if (playerRef){
				if (playerRef.playerNum > 0){
					GlobalVars.totalGlobsEaten[playerRef.playerNum -1] ++; 
				}
			}
			activated = true; 
			playerRef.health += 2; 

			this.GetComponent<SphereCollider>().enabled = false ;

			// instantiate touch effect
			GameObject newTouch = Instantiate(touchEffect,transform.position,Quaternion.identity)
				as GameObject;
			newTouch.GetComponent<SpriteRenderer>().color = playerRef.playerParticleMats[playerRef.characterNum-1].GetColor("_TintColor");
			//newTouch.GetComponent<SpriteRenderer>().sprite = ectoGlow.sprite;
			newTouch.transform.localScale = ectoGlow.transform.localScale;

			// sfx
			GameObject newSFX = Instantiate(sfxObj) as GameObject;
			// pitch shift?
			newSFX.GetComponent<AudioSource>().pitch += 1*(playerRef.health/playerRef.maxHealth);
			GameObject.Destroy(parentGO.gameObject);
			GameObject.Destroy(this.gameObject);
		}
	}

	public void SetVelocityMaterial(Vector3 newVelocity, GameObject playerRef)
	{
		if (parentGO != null) {

			parentGO.GetComponent<Rigidbody> ().velocity = newVelocity; 
			parentGO.GetComponent<Renderer>().material = playerRef.GetComponent<PlayerS>().playerMats[playerRef.GetComponent<PlayerS>().characterNum -1]; 
		}
	}


}

