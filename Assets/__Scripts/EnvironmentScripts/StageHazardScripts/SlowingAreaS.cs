using UnityEngine;
using System.Collections;

public class SlowingAreaS : MonoBehaviour {

	// Update is called once per frame
	void OnTriggerEnter (Collider other) {

		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();

			if (!playerRef.GetSlowedState()){
				playerRef.TriggerSlow();
			}
		}
	
	}

	// Update is called once per frame
	void OnTriggerExit (Collider other) {
		
		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();
			
			if (playerRef.GetSlowedState()){
				playerRef.DisableSlow();
			}
		}
		
	}
}
