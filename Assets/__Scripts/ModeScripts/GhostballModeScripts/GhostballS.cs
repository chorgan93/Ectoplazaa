using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostballS : MonoBehaviour {

	public ParticleSystem respawnParticles;
	private ParticleSystem currentRespawnParticles;
	private SpriteRenderer myRenderer;

	private int playerOwner;
	private Color startColor;

	// Use this for initialization
	void Start () {

		myRenderer = GetComponent<SpriteRenderer>();
		startColor = myRenderer.color;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (currentRespawnParticles){
			if (!currentRespawnParticles.isPlaying){
				Destroy(currentRespawnParticles);
			}
		}
	
	}

	void OnCollisionEnter(Collision other){

		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();
			playerOwner = playerRef.playerNum;
			myRenderer.color = playerRef.GetComponentInChildren<TrailRenderer>().materials[0].color;
		}

	}

	public void ResetBall(Vector3 respawnPos){

		currentRespawnParticles = Instantiate(respawnParticles, transform.position, Quaternion.identity)
			as ParticleSystem;
		myRenderer.color = startColor;
		transform.position = respawnPos;
		GetComponent<Rigidbody>().velocity = Vector3.zero;

	}

	public int GetCurrentPlayer(){

		return playerOwner;

	}
}
