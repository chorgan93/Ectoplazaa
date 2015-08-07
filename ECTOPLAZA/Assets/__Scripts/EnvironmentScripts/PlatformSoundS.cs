using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformSoundS : MonoBehaviour {

	public List<GameObject> impactSoundObjs;

	public void PlayPlatformSounds(){

		int soundToPlay = Mathf.FloorToInt(Random.Range(0,impactSoundObjs.Count));

		Instantiate(impactSoundObjs[soundToPlay]);

	}

}
