using UnityEngine;
using System.Collections;

public class GameStartS : MonoBehaviour {

	public GameObject playerPrefab; 
	public GameObject [] spawnPts; 

	// spawns players appropriately at start of match

	// Use this for initialization
	void Start () 
	{

		spawnPts = GameObject.FindGameObjectsWithTag("Spawn");
		Vector3 spawnPos = Vector3.zero;

		if (!GlobalVars.characterSelected) //create a default 4 players on scene open if they didnt come from character select 
		{ 
			//string [] joystickNames = Input.GetJoystickNames ();
			//int numberOfPlayers = joystickNames.Length; 

			for (int i = 0; i < 4; i++) 
			{
				spawnPos = spawnPts [i].transform.position;
				//spawnPos.z = -1;
				GameObject newPlayer = Instantiate (playerPrefab, spawnPos, Quaternion.identity) as GameObject;
				newPlayer.GetComponent<PlayerS> ().playerNum = i + 1; 
				newPlayer.GetComponent<PlayerS>().characterNum = GlobalVars.characterNumber[i]; 
				newPlayer.gameObject.name = "Player" + (i+1);
				//print("Spawned Player " + (i+1)); 
				GlobalVars.characterIsPlaying[i] = true; 

				GlobalVars.playerList[i] = newPlayer; 

			}
		} 
		else //CHARACTERS WENT THROUGH CHARACTER SELECT
		{

			for (int i = 0; i < 4; i++) {
				if (GlobalVars.characterNumber [i] > 0) {
					GlobalVars.characterIsPlaying[i] = true; 
					print("Spawned Player " + (i+1)); 
					spawnPos = spawnPts [i].transform.position;
					//spawnPos.z = -1;
					GameObject newPlayer = Instantiate (playerPrefab, spawnPos, Quaternion.identity) as GameObject;
					newPlayer.GetComponent<PlayerS> ().playerNum = i + 1; 
					newPlayer.GetComponent<PlayerS>().characterNum = GlobalVars.characterNumber[i];
					newPlayer.GetComponent<PlayerS>().colorNum = GlobalVars.colorNumber[i];
					newPlayer.gameObject.name = "Player" + (i+1);

					GlobalVars.playerList[i] = newPlayer; 
				}

			}
		}

		//print ("Character Skins: " + GlobalVars.characterNumber[0] + "," +GlobalVars.characterNumber[1] + "," + GlobalVars.characterNumber[2] +"," + GlobalVars.characterNumber[3]); 

			



	}

}
