using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EctoGoalS : MonoBehaviour {


	public int playerNum; // what player i'm attached to
	public PlayerS myPlayer; // playerRef

	public List<Color> playerCols;
	private SpriteRenderer mySprite;

	public List<GameObject> spawnPts; // 0 is spawn for 2 player, 1 for 3 player, 2 for 4 player

	// Use this for initialization
	void Start () {

		// determine if needed based on playerNum
		if (GlobalVars.totalPlayers < playerNum){
			gameObject.SetActive(false);
		}
		else{
		// set position based on num of players
			transform.position = spawnPts[GlobalVars.totalPlayers-2].transform.position;

		}


	
	}

	void Update () {
		if (!myPlayer){
			myPlayer = GameObject.Find("Player"+playerNum).GetComponent<PlayerS>();
			
			mySprite = GetComponent<SpriteRenderer>();
			mySprite.color = playerCols[myPlayer.characterNum-1];
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player"){
			if (other.gameObject.GetComponent<PlayerS>().playerNum == playerNum){
				GameObject newParticles =  Instantiate(myPlayer.deathParticles,transform.position,Quaternion.identity) as GameObject;
				newParticles.GetComponent<ParticleSystem>().startColor = 
					mySprite.color;

				// add to player score
				int numToAdd = Mathf.RoundToInt(myPlayer.health-myPlayer.initialHealth);
				myPlayer.health = myPlayer.initialHealth;
				myPlayer.score += numToAdd;
			}
		}
	}
}
