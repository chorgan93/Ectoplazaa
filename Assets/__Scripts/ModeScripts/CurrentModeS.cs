using UnityEngine;
using System.Collections;

public class CurrentModeS : MonoBehaviour {

	public static int currentMode = 0; // 0 = Ectoplasm, 1 = Stock, 2=Rocket League, 3=???

	public static bool isTeamMode = false;

	public static int numRoundsCollectoplaza =3;
	public static int numRoundsStock =3;
	public static int numRoundsBall =2;
	public static int numRoundsDefault =2;
	private static int numRounds = -1;
	private static int [] numberRoundsWon = new int[4] {0,0,0,0};
	private static int [] numberRoundsWonTeam = new int[2] {0,0};
	private static int numberRoundsCurrent =0;

	public static int maxRounds = 3;
	public static int minRounds = 1;

	public static bool firstGame = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static int GetNumberRounds()
	{
		//This list should be appended. Should also be updated via UI in the menus (selectable # of rounds?)
		switch (currentMode)										//Find parent object for desired mode and enable
		{
		case 0:
			numRounds = numRoundsCollectoplaza;
			break;
		case 1: 
			numRounds = numRoundsStock;
			break;
		case 2: 
			numRounds = numRoundsBall;
			break;
		default:
			numRounds = numRoundsDefault;
			break;


		}
		print ( "Current Mode S: Number Rounds..." + numRounds);
		return 	numRounds;
		
	}

	public static void AddToRoundsCompleted(int winningPlayerIndex)
	{
		numberRoundsWon[winningPlayerIndex] ++;
		numberRoundsCurrent ++;
		print ( "Current Mode S: Adding to rounds completed");
	}

	public static void AddToRoundsCompletedTeam(int teamNum){
		numberRoundsWonTeam[teamNum-1] ++;
		numberRoundsCurrent ++;
	}

	public static int GetRoundsCurrent(){

		return numberRoundsCurrent;

	}

	public static bool DoAnotherRound()
	{
		bool bDoAnother = true;
		//See if one player has one.
		if (isTeamMode){
			Debug.Log(numberRoundsWonTeam[0] + " : " + numRounds);
			if (numberRoundsWonTeam[0] >= numRounds || numberRoundsWonTeam[1] >= numRounds){
				bDoAnother = false;
			}
		}
		else{
		for (int i =0; i < 4; i ++)
		{
			if ( numberRoundsWon[i] >= numRounds )
			{
				bDoAnother = false;

			}
		}
		}
		print ("Current Mode S: Do Another Round?     " + bDoAnother);

		firstGame = !bDoAnother;

		//If we're done here, reset all the rounds won.
		return bDoAnother;

	}

	public static void ResetWinRecord()
	{
		numberRoundsWon = new int[4] {0,0,0,0};
		numberRoundsWonTeam = new int[2] {0,0};

	}

	public static int GetRedWins(){
		
		return numberRoundsWonTeam[0];
		
	}

	public static int GetBlueWins(){

		return numberRoundsWonTeam[1];

	}

	public static void SetNumRounds(int newRoundNum){

		numRoundsDefault = numRoundsBall = numRoundsCollectoplaza = numRoundsStock = newRoundNum;

	}
}
