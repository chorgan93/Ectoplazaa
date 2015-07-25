using UnityEngine;
using System.Collections;

public class CharacterSelectMenu : MonoBehaviour {

	public GameObject playerPrefab; 

	string platformType; 
	int playerNum = 1; 

	public bool[] hasJoined = new bool[4];
	public GameObject [] joinTexts; 
	public GameObject [] spawnPoints;
	public GameObject [] characterNumText; 

	GameObject [] players = new GameObject[4]; 

	int totalPlayers = 0; 

	// Use this for initialization
	void Start () {
	
		platformType = PlatformS.GetPlatform (); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i = 1; i < 4; i++)
		{
			if (Input.GetButton ("AButtonPlayer" + i + platformType))  //ADD PLAYER---------------------------------------
			{

				if(!hasJoined[i-1])
				{
					int defaultSkin = 0; 

					if(totalPlayers == 0)
					{
						print("First Player Skinned"); 
						defaultSkin = 1; 
					}
					else
					{

						for(int j = 1; j < GlobalVars.totalSkins; j++)
						{

							foreach(GameObject player in players)
							{
								if(defaultSkin != 0)
									break;

								if(player != null)
								{
									if(player.GetComponent<PlayerS>().characterNum != j)
									{
										defaultSkin = j;
									}
							
								
								}
							}
							if(defaultSkin != 0)
								break; 

						}
					}

					characterNumText[i-1].GetComponent<TextMesh>().text = "characterNum: " + defaultSkin;

					hasJoined[i-1] = true; 
					totalPlayers += 1; 

					GameObject newPlayer = Instantiate(playerPrefab,spawnPoints[i-1].transform.position,Quaternion.identity) as GameObject;
					newPlayer.GetComponent<PlayerS>().playerNum = i;
					newPlayer.GetComponent<PlayerS>().characterNum = defaultSkin;

					newPlayer.GetComponent<PlayerS>().spawnPt = spawnPoints[i-1];
					players[i-1] = newPlayer; 
					//joinTexts[i-1].GetComponent<Renderer>().enabled = false; 
					print("Total Players: " + totalPlayers); 

					Renderer [] renderers = joinTexts[i-1].GetComponentsInChildren<Renderer>();

					foreach(Renderer r in renderers)
					{
						r.enabled = false; 
					}
				
				
				}
			}
			else if (Input.GetButton ("XButtonPlayer" + i + platformType)) //REMOVE PLAYER---------------------------------------
			{
				if(hasJoined[i-1])
				{
					hasJoined[i-1] = false; 
					totalPlayers -= 1; 

					GameObject.Destroy( players[i-1].gameObject);
					players[i-1]= null; 

					print("Total Players: " + totalPlayers); 

					characterNumText[i-1].GetComponent<TextMesh>().text = "";

					Renderer [] renderers = joinTexts[i-1].GetComponentsInChildren<Renderer>();
					
					foreach(Renderer r in renderers)
					{
						r.enabled = true; 
					}
				}
			}
			else if (Input.GetButton ("StartButtonAllPlayers"+ platformType) && totalPlayers >= 2) //START GAME---------------------------------------
			{
				GlobalVars.totalPlayers = totalPlayers; 

				for(int j = 1; j < 4; j++)
				{
					if(players[j-1] != null)
						GlobalVars.characterNumber[j-1] = players[j-1].GetComponent<PlayerS>().characterNum; 
				}

				Application.LoadLevel("Protoscene_Colin");
			}

		}

	}
}
