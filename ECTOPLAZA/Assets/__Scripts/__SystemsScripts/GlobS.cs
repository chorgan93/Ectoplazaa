using UnityEngine;
using System.Collections;

public class GlobS : MonoBehaviour {

	public GameObject parentGO;

	Vector3 originalScale; 

	bool activated = false; 
	float deletionTimer = 24f, deletionCounter; 

	float invulnTime = 0.5f;

	float sizeRandomizer = 1.25f;

	// Use this for initialization
	void Start () {
		deletionCounter = deletionTimer;
		Vector3 newSize = parentGO.transform.localScale;
		newSize.x += sizeRandomizer*Random.insideUnitCircle.x;
		newSize.y = newSize.x;
		parentGO.transform.localScale = newSize;
		originalScale = parentGO.transform.localScale; 
	}

	void FixedUpdate()
	{

		invulnTime -= Time.deltaTime*TimeManagerS.timeMult;

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

		if (other.tag == "Player" && invulnTime <= 0) {
			parentGO.GetComponent<SphereCollider> ().enabled = false; 
			parentGO.GetComponent<Rigidbody> ().useGravity = false;  
			parentGO.GetComponent<Rigidbody> ().velocity = Vector3.zero; 

			PlayerS playerRef = other.gameObject.GetComponent<PlayerS> ();
			activated = true; 
			playerRef.health += 2; 

			this.GetComponent<SphereCollider>().enabled = false ;
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

