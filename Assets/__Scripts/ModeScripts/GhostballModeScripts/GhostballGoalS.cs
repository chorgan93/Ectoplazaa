using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostballGoalS : MonoBehaviour {

	public  float		 	timeDelayUntilActive = 3,
							timePerPointTick = 1;
	ScoreKeeperS 			scoreKeeper;
	GameObject				ghostBall;
	bool 					bIsActivated = false,
							bBallDetected =false,
							bChangedSkin = false;
							
	float 					delayBirthday = 0,
							pointBirthday = 0;
	int 				owningPlayerIndex  = -1;

	 public List<Material> tempColors;

	// Use this for initialization
	void Start () 
	{
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeperS>() as ScoreKeeperS;
		ghostBall = GameObject.FindGameObjectWithTag("Ghostball") as GameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (bBallDetected)							//If I have the ball...
		{
			 if(!bIsActivated)						//If activation time delay remains..
			{
				CheckDelayTimer();					//Countdown the delay!

			}
			else 									//Otherwise, if no more activation time delay
			{
				CheckPointTimer();					//Work on adding points!
			}
		}

	}

	public void ChangeSkin(int playerIndex)
	{
		print ("Changing goal from " + owningPlayerIndex +  " to " + playerIndex);
		owningPlayerIndex = playerIndex;
		GameObject myPlayer = GlobalVars.playerList[owningPlayerIndex];	//get character
	
		if (myPlayer == null)
		{ 
			this.gameObject.SetActive(false);							//If my character isn't active, disable myself
			print ("Deactivating goal " + owningPlayerIndex);
		}
		else
		{																//switch skin for goal depending on player color
			switch (playerIndex)									
			{

			case 0:
				gameObject.GetComponent<MeshRenderer>().material = tempColors[0];
				break;
			case 1:
				this.gameObject.GetComponent<MeshRenderer>().material = tempColors[1];
				break;
			case 2:
				this.gameObject.GetComponent<MeshRenderer>().material = tempColors[2];
				break;
			case 3:
				this.gameObject.GetComponent<MeshRenderer>().material = tempColors[3];
				break;
			default:
				break;
			}
			//print ("Switching Goal Skin to number '" + myPlayer.GetComponent<PlayerS>().colorNum + "' for player " + owningPlayerIndex);
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if (c == ghostBall.GetComponent<Collider>())
		{
			bBallDetected = true;			//Flip flag
			delayBirthday = Time.time;		//Start countdown
			//print("ball entered goal"); 			//visual effect hook?
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if (c == ghostBall.GetComponent<Collider>())
		{
			bBallDetected = false;			//End countdown
			bIsActivated = false;			//No longer activated
			//print("ball exited goal"); 			//visual effect hook?
		}
	}

	void CheckDelayTimer()
	{
		if(Time.time - delayBirthday > timeDelayUntilActive)
		{
			Activated();
		}
	}

	void Activated()
	{
		bIsActivated = true; 				//Flip flag
		pointBirthday = Time.time;			//Update point timer
		//print("Activate Ghostball Goal"); //Visual effect hook?
	}
	
	void CheckPointTimer()
	{
		if(Time.time - pointBirthday > timePerPointTick)
		{
			pointBirthday = Time.time;					//Update timer
			//print("add Ghostball points!");
			scoreKeeper.AddPoints(owningPlayerIndex);	//Talk to ScoreKeeperS;

		}
	}

	public int GetOwningPlayerIndex()
	{
		return owningPlayerIndex;
	}

	public void Disable()
	{
		owningPlayerIndex = -1;
		this.gameObject.SetActive(false);
	}
}
