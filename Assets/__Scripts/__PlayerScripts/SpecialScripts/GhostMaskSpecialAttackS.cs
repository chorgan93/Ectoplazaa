using UnityEngine;
using System.Collections;

public class GhostMaskSpecialAttackS : MonoBehaviour {

	// put this on a collider that is spawned by ghostmask when special attack triggers
	public float spawnDist = 50;
	public PlayerS playerRef;

	public float lifeTime = 1f;

	public Vector3 slashVel;

	void Start(){
		
		transform.Translate(new Vector3(spawnDist, 0, -2f));
		CameraShakeS.C.SmallShake();
	}


	void FixedUpdate () {

		if (playerRef){
			GetComponent<DamageS>().MakeSpecial(playerRef);
			GetComponent<Rigidbody>().velocity = slashVel*Time.deltaTime;

		}

		lifeTime -= Time.deltaTime*TimeManagerS.timeMult;
		if (lifeTime <= 0){
			Destroy(gameObject);
		}

	}

	void PauseAllPlayers () {
		foreach (GameObject player in GlobalVars.playerList){
			player.GetComponent<PlayerS>().PauseCharacter();
		}
	}

	void UnPauseAllPlayers () {
		foreach (GameObject player in GlobalVars.playerList){
			player.GetComponent<PlayerS>().UnpauseCharacter();
		}
	}
}
