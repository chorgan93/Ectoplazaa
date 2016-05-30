using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModeDescriptionS : MonoBehaviour {

	private bool fadingIn = false;
	private bool fadingOut = false;

	private float fadeInTime = 0.8f;
	private float fadeInCurrentTime = 0;
	private float bgMaxFade = 0.8f;

	
	private float fadeOutTime = 0.3f;
	private float fadeOutCurrentTime = 0;

	private int currentMode = 0;

	public string[] modeDescriptions;
	public Image bg;
	public Text text;

	// Use this for initialization
	void Start () {

		Color turnOffColor = bg.color;
		turnOffColor.a = 0;
		bg.color = turnOffColor;

		turnOffColor = text.color;
		turnOffColor.a = 0;
		text.color = turnOffColor;

		SetMode(0);
	
	}
	
	// Update is called once per frame
	void Update () {

		if (fadingIn){

			fadeInCurrentTime+=Time.deltaTime;
			
			Color bgColor = bg.color;
			Color textCol = text.color;

			bgColor.a = bgMaxFade * (fadeInCurrentTime/fadeInTime);
			
			if (bgColor.a >= bgMaxFade){
				bgColor.a = bgMaxFade;
				fadingIn = false;
			}
			
			textCol.a = bgColor.a;
			
			bg.color = bgColor;
			text.color = textCol;

		}

		if (fadingOut){

			fadeOutCurrentTime += Time.deltaTime;
			
			Color bgColor = bg.color;
			Color textCol = text.color;

			bgColor.a = bgMaxFade * (1-(fadeOutCurrentTime/fadeOutTime));
			textCol.a = bgColor.a;

			if (bgColor.a <= 0){
				bgColor.a = 0;
				textCol.a = 0;
				fadingOut = false;
			}

			bg.color = bgColor;
			text.color = textCol;

		}
	
	}

	public void SetMode(int newMode){

		currentMode = newMode;

		text.text = modeDescriptions[currentMode];

	}

	public void TurnOn(){

		fadingIn = true;
		fadingOut = false;

	}

	public void TurnOff(){

		fadingOut = true;
		fadingIn = false;

	}
}
