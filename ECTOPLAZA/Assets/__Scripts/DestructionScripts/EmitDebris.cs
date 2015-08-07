using UnityEngine;
using System.Collections;

public class EmitDebris : MonoBehaviour {

	public ParticleSystem referencedParticleSystem; 

	void OnCollisionEnter(Collision other){

		if (referencedParticleSystem == null)
			this.GetComponent<ParticleSystem> ().Play ();
		else {
			referencedParticleSystem.Play(); 
		}

	}
}
