using UnityEngine;
using System.Collections;

public class GlobS : MonoBehaviour {

	public GameObject parentGO;

	Vector3 originalScale; 
	float scaleVariance = 0.5f;

	bool activated = false; 
	float deletionTimer = 24f, deletionCounter; 

	private float startInvuln = 0.6f;
	// Use this for initialization
	void Start () {
		deletionCounter = deletionTimer;

		// vary size a bit
		originalScale = parentGO.transform.localScale;
		originalScale.x += Random.insideUnitCircle.x*scaleVariance;
		originalScale.y += Random.insideUnitCircle.y*scaleVariance;
		parentGO.transform.localScale = originalScale;

		//print ("created dot");


	}

	void FixedUpdate()
	{

		
		startInvuln -= Time.deltaTime*TimeManagerS.timeMult;

		if (activated) {


			parentGO.transform.localScale = Vector3.Lerp(originalScale,Vector3.zero, 1f- ( deletionCounter/deletionTimer)); 

			deletionCounter -= 1f; 

		}

		if (deletionCounter < 0) {

			GameObject.Destroy(parentGO.gameObject);
			GameObject.Destroy(this.gameObject);

		}
	}

	void OnTriggerEnter(Collider other) 
	{

		if (other.tag == "Player" && startInvuln <= 0) {
			parentGO.GetComponent<SphereCollider> ().enabled = false; 
			parentGO.GetComponent<Rigidbody> ().useGravity = false;  
			parentGO.GetComponent<Rigidbody> ().velocity = Vector3.zero; 

			PlayerS playerRef = other.gameObject.GetComponent<PlayerS> ();
			activated = true; 
			playerRef.health += 1; 

			this.GetComponent<SphereCollider>().enabled = false ;

			//print ("yeah");
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

