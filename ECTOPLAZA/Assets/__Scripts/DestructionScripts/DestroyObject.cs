using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

	public GameObject audioObj;

	void OnTriggerEnter(Collider other) 
	{
		this.GetComponent<ParticleSystem> ().Play (); 
		GetComponent<Collider> ().enabled = false; 
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<ParticleSystemAutoDestroy> ().enabled = true; 

		CameraShakeS.C.MicroShake();

		if (audioObj != null){
			Instantiate(audioObj);
		}
	}

}
