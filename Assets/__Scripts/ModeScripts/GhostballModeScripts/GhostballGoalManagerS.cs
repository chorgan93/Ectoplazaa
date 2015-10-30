using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostballGoalManagerS : MonoBehaviour {

	// Use this for initialization

	public List<GhostballGoalS> goals;
	List<int> playerIndices = new List<int>();


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
		RandomizeGoals();
	}


	// Update is called once per frame
	void Update () {
	
			//Count Down
	}



	void RandomizeGoals()
	{
		//Disable All Goals

		for( int i =0; i < goals.Count; i ++)
		{
			goals[i].gameObject.SetActive(false);
		}

		for ( int i =0; i < playerIndices.Count; i ++)
		{
			//Choose Random goal
			int goalToActivate = Random.Range(0,4);
			//make sure random goal wasn't last used
			
			//Make sure it hasn't already been chosen
			if(goals[goalToActivate].gameObject.activeSelf == false)
			{
				GhostballGoalS g = goals[goalToActivate];
				//Activate goal
				g.gameObject.SetActive(true);
				//Give goals player index to change skin
				g.ChangeSkin(i);
			}

		}

	}

	
}
