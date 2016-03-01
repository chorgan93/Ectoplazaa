using UnityEngine;
using System.Collections;

public class GlobalVars : MonoBehaviour 

{

	// various variables we want to keep track of between scenes

	public static int totalPlayers = 4; //amount of players added during character select, DEFAULT IS 4 FOR TESTING IN UNITY

	public static GameObject []  playerList = new GameObject [4]; 

	public const int totalSkins = 6; //amount of skins available 
	public static int [] characterNumber = new int[4]{1,2,3,4}; //each players character number, DEFAULT IS SET FOR TESTING IN UNITY //0 DENOTES THE PERSON IS NOT PLAYING
	public static int [] colorNumber = new int[4]{0,0,0,0};
	public static int [] teamNumber = new int[4]{0,0,0,0}; // 1 = red, 2 = blue, 0 = not playing
	public static bool [] characterIsPlaying = new bool[4]{false,false,false,false};
	public static int mapNumber; //current map selection

	public static int lastWinningPlayer = 1;

	public static bool characterSelected = false; //check if players have gone through character select screen, for using in unity to check if spawn characters with default properties or unique ones
	public static bool launchingFromScene = true; 

	public static int [] totalFlings = new int[4], totalDeaths = new int[4], totalKills = new int[4], totalJumps = new int[4], totalGroundPounds= new int[4] , totalGlobsEaten = new int[4];  

	public static int numberRounds =1;
	public static int currRound = 0;


	public static void ResetVariables()
	{
		characterIsPlaying = new bool[4]{false,false,false,false};
		characterNumber = new int[4]{1,2,3,4};
		teamNumber = new int[4]{0,0,0,0};
	}

	public static void ResetGameStats()
	{
		totalFlings = new int[4];
		totalDeaths = new int[4];
		totalKills = new int[4];
		totalJumps = new int[4];
		totalGroundPounds = new int[4];
		totalGlobsEaten = new int[4];
	}

	// TEAM VARS
	public static bool IsRedTeam(int playerNum){

		if (teamNumber[playerNum-1] == 1){
			return true;
		}
		else{
			return false;
		}

	}

	public static bool IsBlueTeam(int playerNum){
		
		if (teamNumber[playerNum-1] == 2){
			return true;
		}
		else{
			return false;
		}
		
	}

	public static bool ValidTeams(){

		int numRedTeam = 0;
		int numBlueTeam = 0;

		foreach (int i in teamNumber){
			if (i == 1){
				numRedTeam++;
			}
			if (i == 2){
				numBlueTeam ++;
			}
		}

		if (numBlueTeam >0 && numRedTeam >0){

			return true;

		}
		else{
			return false;
		}

	}

	public static int NumPlayersRedTeam(){
		int numRedTeam = 0;
		int numBlueTeam = 0;
		
		foreach (int i in teamNumber){
			if (i == 1){
				numRedTeam++;
			}
			if (i == 2){
				numBlueTeam ++;
			}
		}

		return numRedTeam;
	}

	public static int NumPlayersBlueTeam(){
		int numRedTeam = 0;
		int numBlueTeam = 0;
		
		foreach (int i in teamNumber){
			if (i == 1){
				numRedTeam++;
			}
			if (i == 2){
				numBlueTeam ++;
			}
		}
		
		return numBlueTeam;
	}

	public static bool OnSameTeam(PlayerS p1, PlayerS p2){

		int p1Team = teamNumber[p1.playerNum-1];
		int p2Team = teamNumber[p2.playerNum-1];

		return (p1Team == p2Team);

	}



}


