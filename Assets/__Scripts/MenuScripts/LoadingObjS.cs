using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingObjS : MonoBehaviour {

	private Text myText;

	private float loadChangeTime = 0.4f;
	private float loadingCountdown;

	private int numOfDots = 1;

	// Use this for initialization
	void Start () {

		myText = GetComponent<Text>();
		myText.text = "Loading";
		loadingCountdown = loadChangeTime;
	
	}
	
	// Update is called once per frame
	void Update () {

		loadingCountdown -= Time.deltaTime;

		if (loadingCountdown <= 0){
			loadingCountdown = loadChangeTime;

			numOfDots++;
			if (numOfDots > 3){
				numOfDots = 0;
			}

			string loadingText = "Loading";

			for (int i = -1; i < numOfDots; i++){
				if (i > -1){
					loadingText += ".";
				}
			}

			myText.text = loadingText;
		}
	
	}
}
