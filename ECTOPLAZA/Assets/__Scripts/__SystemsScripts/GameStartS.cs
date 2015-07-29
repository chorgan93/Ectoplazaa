using UnityEngine;
using System.Collections;

public class GameStartS : MonoBehaviour {

	public GameObject playerPrefab; 
	public GameObject [] spawnPts; 

	// Use this for initialization
	void Start () 
	{

		if (!GlobalVars.characterSelected) //create players when opening level scenes directly from unity based on total joysticks
		{ 
			//string [] joystickNames = Input.GetJoystickNames ();
			//int numberOfPlayers = joystickNames.Length; 

			for (int i = 0; i < 4; i++) 
			{
				GameObject newPlayer = Instantiate (playerPrefab, spawnPts [i].transform.position, Quaternion.identity) as GameObject;
				newPlayer.GetComponent<PlayerS> ().playerNum = i + 1; 
				newPlayer.GetComponent<PlayerS>().characterNum = GlobalVars.characterNumber[i]; 
				newPlayer.gameObject.name = "Player" + (i+1);
				//print("Spawned Player " + (i+1)); 
				GlobalVars.characterIsPlaying[i] = true; 

				GlobalVars.playerList[i] = newPlayer; 

			}
		} 
		else 
		{
			for (int i = 0; i < 4; i++) {
				if (GlobalVars.characterNumber [i] != 0) {
					GlobalVars.characterIsPlaying[i] = true; 
					print("Spawned Player " + (i+1)); 
					GameObject newPlayer = Instantiate (playerPrefab, spawnPts [i].transform.position, Quaternion.identity) as GameObject;
					newPlayer.GetComponent<PlayerS> ().playerNum = i + 1; 
					newPlayer.GetComponent<PlayerS>().characterNum = GlobalVars.characterNumber[i];
					newPlayer.gameObject.name = "Player" + (i+1);

					GlobalVars.playerList[i] = newPlayer; 
				}

			}
		}

		//print ("Character Skins: " + GlobalVars.characterNumber[0] + "," +GlobalVars.characterNumber[1] + "," + GlobalVars.characterNumber[2] +"," + GlobalVars.characterNumber[3]); 

			



	}

}
