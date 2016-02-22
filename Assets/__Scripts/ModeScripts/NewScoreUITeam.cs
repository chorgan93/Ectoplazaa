using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewScoreUITeam : MonoBehaviour {

	public int teamNum;

	public Image teamImage;
	public Text textDisplay;

	void Start () {
		//if (!CurrentModeS.isTeamMode){
			gameObject.SetActive(false);
		//}

		teamImage = GetComponent<Image>();

		if (teamNum == 1){
			teamImage.color = new Color(1,0,0,0.75f);
		}
		else{
			teamImage.color = new Color(0,0,1,0.75f);
		}
	}

	// Update is called once per frame
	void Update () {

		if (ScoreKeeperS.gameEnd){
			gameObject.SetActive(false);
		}

		if (teamNum == 1){
			textDisplay.text = CurrentModeS.GetRedWins().ToString();
		}
		else{
			textDisplay.text = CurrentModeS.GetBlueWins().ToString();
		}

	
	}

}
