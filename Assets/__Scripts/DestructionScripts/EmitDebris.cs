using UnityEngine;
using System.Collections;

public class EmitDebris : MonoBehaviour {

	public ParticleSystem referencedParticleSystem; 

	int currentSystem = 1; 

	void OnCollisionEnter(Collision other){

		if (other.gameObject.tag == "Player") {

			if (this.GetComponent<ParticleSystem>() != null)
				this.GetComponent<ParticleSystem> ().Play ();

			//referencedParticleSystem.Play (); 
			ParticleSystem [] otherParticleSystems = GetComponentsInChildren<ParticleSystem>(); 

			if(otherParticleSystems.Length > 0)
			{
				otherParticleSystems[0].Play(); 


				if(otherParticleSystems.Length > 1)
				{
					otherParticleSystems[currentSystem].Play(); 

					currentSystem += 1; 

					if(currentSystem >= otherParticleSystems.Length)
					{
						currentSystem = 1; 
					}

					//int randomSystem = Random.Range(1, otherParticleSystems.Length); 

					/*
					foreach(ParticleSystem ps in otherParticleSystems)
					{
						ps.Play(); 
					}
					*/
				}


			}
		}

	}
}
