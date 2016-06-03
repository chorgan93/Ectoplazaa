using UnityEngine;
using System.Collections;

public class OptionsArrowHandler : MonoBehaviour {

	public OptionsArrowSet[] optionArrows;


	public void TurnArrowsOnOff (int arrow) {

		int i = 0;
		foreach (OptionsArrowSet optArrow in optionArrows){
			if (i != arrow){
				optArrow.isOn = false;
			}
			i++;
		}
	
	}
}
