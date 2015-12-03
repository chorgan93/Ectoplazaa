using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BedSoundS : MonoBehaviour {

	// Use this for initialization

	public List <GameObject> bedSounds;


	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player") {

			int bedSoundToPlay = Mathf.FloorToInt(Random.Range(0,bedSounds.Count));

			Instantiate(bedSounds[bedSoundToPlay]);

		}
	}

}
