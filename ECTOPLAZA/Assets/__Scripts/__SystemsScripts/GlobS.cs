using UnityEngine;
using System.Collections;

public class GlobS : MonoBehaviour {

	public GameObject parentGO;

	Vector3 originalScale; 

	bool activated = false; 
	float deletionTimer = 24f, deletionCounter; 
	// Use this for initialization
	void Start () {
		deletionCounter = deletionTimer;
		originalScale = parentGO.transform.localScale; 
	}

	void FixedUpdate()
	{
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

		if (other.tag == "Player") {
			parentGO.GetComponent<SphereCollider> ().enabled = false; 
			parentGO.GetComponent<Rigidbody> ().useGravity = false;  
			parentGO.GetComponent<Rigidbody> ().velocity = Vector3.zero; 

			PlayerS playerRef = other.gameObject.GetComponent<PlayerS> ();
			activated = true; 
			playerRef.health += 1; 

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

