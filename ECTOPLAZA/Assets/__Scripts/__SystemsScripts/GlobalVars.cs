using UnityEngine;
using System.Collections;

public class GlobalVars : MonoBehaviour 

{
	public static int totalPlayers = 2; //amount of players added during character select, DEFAULT IS 2 FOR TESTING IN UNITY

	public static GameObject []  playerList = new GameObject [4]; 

	public const int totalSkins = 5; //amount of skins available
	public static int [] characterNumber = new int[4]{1,2,3,4}; //each players character number, DEFAULT IS SET FOR TESTING IN UNITY //0 DENOTES THE PERSON IS NOT PLAYING
	public static bool [] characterIsPlaying = new bool[4]{false,false,false,false};
	public static int mapNumber; //current map selection

	public static bool characterSelected = false; //check if players have gone through character select screen, for using in unity to check if spawn characters with default properties or unique ones
	public static bool launchingFromScene = true; 
}
