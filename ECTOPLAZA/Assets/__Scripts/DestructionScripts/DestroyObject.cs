using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {



	void OnTriggerEnter(Collider other) 
	{
		this.GetComponent<ParticleSystem> ().Play (); 
		GetComponent<Collider> ().enabled = false; 
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<ParticleSystemAutoDestroy> ().enabled = true; 

		CameraShakeS.C.MicroShake();
	}

}
