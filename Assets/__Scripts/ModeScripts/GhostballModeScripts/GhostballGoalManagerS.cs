using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostballGoalManagerS : MonoBehaviour {

	// Use this for initialization

	public List<GhostballGoalS> goals;
	List<int> playerIndices = new List<int>();


	public float timeToSwitchGoals = 3;

	void Start () {
		Setup();

	}


	//Coroutine so I can wait a frame -- warning was popping up since globalVars wasn't created yet.
	void Setup()
	{

		//Get Identity of those playing
		for ( int i = 0 ; i < GlobalVars.totalPlayers ; i ++)
		{
			if(GlobalVars.characterIsPlaying[i] == true)
			{
				//Save Player Index
				playerIndices.Add(i);
			}
		}

		//Activate random goals.
		InvokeRepeating("RandomizeGoals",0,timeToSwitchGoals);


	}


	// Update is called once per frame
	void Update () {
	
			//Count Down

	}



	void RandomizeGoals()
	{
		/*
		//Disable All Goals
		for( int i =0; i < goals.Count; i ++)
		{
			goals[i].gameObject.SetActive(false);
		}
*/


		bool [] goalJustClaimed = new bool[]{false,false,false,false};

		//Select new goal
		for ( int i =0; i < playerIndices.Count; i ++)
		{
			//Safety check 
			bool isGoalGood = false;
			int safetyCounter =0;
			while(isGoalGood == false)
			{	//Choose Random goal
				int goalToActivate = Random.Range(0,4);

				//Checking credentials
				if(goalJustClaimed[goalToActivate] == false &&				//Make sure it hasn't already been chosen
				   goals[goalToActivate].GetOwningPlayerIndex() != playerIndices[i] )	//Make sure random goal wasn't last used
				{
					GhostballGoalS g = goals[goalToActivate];
					//Activate goal
					g.gameObject.SetActive(true);
					//Give goals player index to change skin
					g.ChangeSkin(i);
					//change flag to exit while loop
					isGoalGood = true;
					goalJustClaimed[goalToActivate] = true;
					//print ("Goal " + goalToActivate + " is changing");
				}
				else{
					safetyCounter ++;
					if(safetyCounter >= 50)
					{
						isGoalGood = true;
						//print ("failed");
						//print ("Goal " + goalToActivate + " Active: " + goals[goalToActivate].gameObject.activeSelf);
						//print ("owning player index: " + goals[goalToActivate].GetOwningPlayerIndex() + "   player index: " + playerIndices[i]);
					}

				}
			}
		}

		//Cleanup goals unused

		for (int i =0; i < 4; i ++)
		{
			if(goalJustClaimed[i] == false)
			{
				goals[i].Disable();
			}
		}

	}
}
