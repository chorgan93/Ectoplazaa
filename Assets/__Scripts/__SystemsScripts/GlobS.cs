using UnityEngine;
using System.Collections;

public class GlobS : MonoBehaviour {

	public GameObject parentGO;

	Vector3 originalScale; 

	bool activated = false; 
	float deletionTimer = 24f, deletionCounter; 

	float invulnTime = .75f;

	float sizeRandomizer = 0.25f;

	float fadeColRate = 0.005f;
	private Renderer ownRender;
	public SpriteRenderer ownSprite;

	public SpriteRenderer ectoGlow;
	private float ectoAlpha = 0.5f;

	public float gravTime = 3f;

	public GameObject sfxObj;

	public GameObject touchEffect;

	public Rigidbody ownRigid;

	// Use this for initialization
	void Start () {
		deletionCounter = deletionTimer;
		Vector3 newSize = parentGO.transform.localScale;
		newSize.x += sizeRandomizer*Random.insideUnitCircle.x;
		newSize.y = newSize.x;
		parentGO.transform.localScale = newSize;
		originalScale = parentGO.transform.localScale; 


		ownRender = parentGO.GetComponent<Renderer>();

		
		Color ectoColor = ownSprite.color;
		ectoColor.a = ectoAlpha;
		ectoGlow.color = ectoColor;
		//activated = true;
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

		invulnTime -= Time.deltaTime*TimeManagerS.timeMult;

		if (activated) {

			parentGO.transform.localScale = Vector3.Lerp(originalScale,Vector3.zero, 1f- ( deletionCounter/deletionTimer)); 

			deletionCounter -= 1f; 

			ownRigid.useGravity = false;

		}
		else{
			parentGO.transform.localScale = Vector3.Lerp(originalScale,Vector3.zero, 1f- ( deletionCounter/deletionTimer)); 
			
			deletionCounter -= 1f*Time.deltaTime*TimeManagerS.timeMult; 
		}

		if (deletionCounter < 0) {

			GameObject.Destroy(parentGO.gameObject);
			GameObject.Destroy(this.gameObject);

		}

		ectoGlow.transform.rotation = Quaternion.Euler(Vector3.zero);

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

	void OnTriggerEnter(Collider other) 
	{

		if (other.tag == "Player" && invulnTime <= 0) {
			parentGO.GetComponent<SphereCollider> ().enabled = false; 
			parentGO.GetComponent<Rigidbody> ().useGravity = false;  
			parentGO.GetComponent<Rigidbody> ().velocity = Vector3.zero; 

			PlayerS playerRef = other.gameObject.GetComponent<PlayerS> ();
			GlobalVars.totalGlobsEaten[playerRef.playerNum -1] ++; 
			activated = true; 
			playerRef.health += 2; 

			this.GetComponent<SphereCollider>().enabled = false ;

			// instantiate touch effect
			GameObject newTouch = Instantiate(touchEffect,transform.position,Quaternion.identity)
				as GameObject;
			newTouch.GetComponent<SpriteRenderer>().color = playerRef.playerParticleMats[playerRef.characterNum-1].GetColor("_TintColor");
			//newTouch.GetComponent<SpriteRenderer>().sprite = ectoGlow.sprite;
			newTouch.transform.localScale = ectoGlow.transform.localScale;

			// kinesthetics?
			CameraShakeS.C.MicroShake();
			CameraShakeS.C.TimeSleep(0.1f*playerRef.health/playerRef.maxHealth);

			// sfx
			GameObject newSFX = Instantiate(sfxObj) as GameObject;
			// pitch shift?
			newSFX.GetComponent<AudioSource>().pitch += 1*(playerRef.health/playerRef.maxHealth);
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

