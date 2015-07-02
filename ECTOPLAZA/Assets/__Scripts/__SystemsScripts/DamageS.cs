using UnityEngine;
using System.Collections;

public class DamageS : MonoBehaviour {

	private float pauseTime = 0.8f;
	private PlayerS playerRef;

	void Start () {
		playerRef = transform.parent.GetComponent<PlayerS>();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player"){
			PlayerS otherPlayer = other.GetComponent<PlayerS>();

			otherPlayer.TakeDamage(playerRef.maxHealth);
			otherPlayer.SleepTime(pauseTime);
			playerRef.SleepTime(pauseTime);

			CameraShakeS.C.LargeShake();
		}

		if (other.gameObject.tag == "PlayerTrail"){

			print ("yeah");

			PlayerS otherPlayer = other.GetComponent<DotColliderS>().whoCreatedMe;

			if (otherPlayer != playerRef && otherPlayer.health > 0){
			
				otherPlayer.TakeDamage(playerRef.maxHealth);
				otherPlayer.SleepTime(pauseTime);
				playerRef.SleepTime(pauseTime/4);

				CameraShakeS.C.LargeShake();

			}
		}
	}
}
