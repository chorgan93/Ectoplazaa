using UnityEngine;
using System.Collections;

public class GlobalVars : MonoBehaviour 

{
	public static int totalPlayers; //amount of players added during character select

	public const int totalSkins = 4; //amount of skins available
	public static int [] characterNumber = new int[4]; //each players character number
	public static int mapNumber; //current map selection

}
