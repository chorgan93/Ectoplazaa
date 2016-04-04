using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EctoGoalS : MonoBehaviour {


	public int playerNum; // what player i'm attached to
	// set player num to 5 for one goal
	public PlayerS myPlayer; // playerRef

	public List<Color> playerCols;
	private SpriteRenderer mySprite;

	public GameObject sfxObject;

	public List<GameObject> spawnPts; // 0 is spawn for 2 player, 1 for 3 player, 2 for 4 player

	// Use this for initialization
	void Start () {

		// turn off if not in ecto mode
		if (CurrentModeS.currentMode != 0){
			gameObject.SetActive(false);
		}

		// determine if needed based on playerNum

		if (playerNum != 5){
		if (GlobalVars.totalPlayers < playerNum){
			gameObject.SetActive(false);
		}
		else{
		// set position based on num of players
			transform.position = spawnPts[GlobalVars.totalPlayers-2].transform.position;

		}
		}
		else{
			transform.position = spawnPts[GlobalVars.totalPlayers-2].transform.position;
		}
		


	
	}

	void Update () {
		if (!myPlayer && playerNum != 5){
			myPlayer = GameObject.Find("Player"+playerNum).GetComponent<PlayerS>();
			
			mySprite = GetComponent<SpriteRenderer>();
			mySprite.color = playerCols[myPlayer.characterNum-1];
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player"){
			if (other.gameObject.GetComponent<PlayerS>().playerNum == playerNum || playerNum == 5){
				myPlayer = other.gameObject.GetComponent<PlayerS>();
				GameObject newParticles =  Instantiate(myPlayer.deathParticles,transform.position,Quaternion.identity) as GameObject;

				newParticles.GetComponent<ParticleSystem>().startColor = 
					playerCols[myPlayer.characterNum-1];

				// add to player score
				int numToAdd = Mathf.RoundToInt(myPlayer.health-myPlayer.initialHealth);
				//myPlayer.health = myPlayer.initialHealth;

				//print (numToAdd);
				if (numToAdd > 0){
					myPlayer.initialHealth = myPlayer.health;
					myPlayer.score += numToAdd;
	
					myPlayer.GetComponent<TrailHandlerRedubS>().updateDots = true;

					Instantiate(sfxObject);
				}
			}
		}
	}
}
