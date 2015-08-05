using UnityEngine;
using System.Collections;

public class BedParticles : MonoBehaviour {

	// Use this for initialization


	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player") {

			Vector3 newPos = new Vector3(other.transform.position.x, transform.GetChild(0).transform.position.y, transform.GetChild(0).transform.position.z); 
			transform.GetChild(0).transform.position = newPos; 
			GetComponentInChildren<ParticleSystem>().Play(); 

		}
	}

}
