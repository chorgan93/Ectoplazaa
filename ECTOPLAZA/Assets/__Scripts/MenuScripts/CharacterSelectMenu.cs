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

							bool flag = false;

							foreach(GameObject player in players)
							{
								if(player != null)
								{
									if(player.GetComponent<PlayerS>().characterNum != j)
									{
										continue;
									}
									else
									{
										flag = true;
										break;
									}
								}
							}

							if(!flag)
							{
								defaultSkin = j; 
								break;
							}
						}
					}

					characterNumText[i-1].GetComponent<TextMesh>().text = "characterNum: " + defaultSkin;

					hasJoined[i-1] = true; 
					totalPlayers += 1; 

					GameObject newPlayer = Instantiate(playerPrefab,spawnPoints[i-1].transform.position,Quaternion.identity) as GameObject;
					newPlayer.GetComponent<PlayerS>().playerNum = i;
					newPlayer.GetComponent<PlayerS>().characterNum = defaultSkin;
					newPlayer.GetComponent<PlayerS>().SetSkin(); 

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
			else if (Input.GetButtonDown ("YButtonPlayer" + i + platformType))
			{
				if(hasJoined[i-1])
				{

					int newSkin = players[i-1].GetComponent<PlayerS>().characterNum; 
					bool stopLoop = false;

					for(int j= 1; j <= GlobalVars.totalSkins; j++) //loop once through all skins
					{
						newSkin += 1; //increment to next skin, check if available; 
						if(newSkin > GlobalVars.totalSkins) //loop if at end of skins
							newSkin = 1; 

						bool flag = false; 

						foreach(GameObject p in players)
						{
							if(p != null)
							{
								if(p.GetComponent<PlayerS>().characterNum != newSkin)
									continue;
								else
								{
									flag = true;
									break;
								}
							}
						}
						 
						if(!flag)
							break;

					}

					
					characterNumText[i-1].GetComponent<TextMesh>().text = "characterNum: " + newSkin;

					players[i-1].GetComponent<PlayerS>().characterNum = newSkin;
					players[i-1].GetComponent<PlayerS>().SetSkin();
				}


			}

			else if (Input.GetButton ("StartButtonAllPlayers"+ platformType) && totalPlayers >= 2) //START GAME---------------------------------------
			{
				//SET GLOBAL VARS
				GlobalVars.totalPlayers = totalPlayers; 

				for(int j = 0; j <= 3; j++)
				{
					if(players[j] != null)
					{
						GlobalVars.characterNumber[j] = players[j].GetComponent<PlayerS>().characterNum; 
					}
					else
					{
						GlobalVars.characterNumber[j] = 0; 
					}
				}

				GlobalVars.characterSelected = true; 
				GlobalVars.launchingFromScene = false; 

				Application.LoadLevel("Protoscene_Colin");
			}

		}

	}
}
