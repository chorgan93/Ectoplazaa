using UnityEngine;
using System.Collections;

public class OptionsArrowSet : MonoBehaviour {

	public bool isOn = false;

	public bool leftShowing;
	public bool rightShowing;

	public SpriteRenderer leftArrow;
	public SpriteRenderer rightArrow;

	private Color fadedColor = new Color(0,0,0,0.5f);

	
	// Update is called once per frame
	void Update () {

		if (isOn){
			leftArrow.enabled = true;
			rightArrow.enabled = true;
		if (leftShowing){
			leftArrow.color = Color.black;
		}
			else{
				leftArrow.color = fadedColor;
			}
		if (rightShowing){
			rightArrow.color = Color.black;
		}
			else{
				rightArrow.color = fadedColor;
			}
		}
		else{
			leftArrow.enabled = false;
			rightArrow.enabled = false;
		}
	
	}
}
