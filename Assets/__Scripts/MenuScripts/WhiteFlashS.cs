using UnityEngine;
using System.Collections;

public class WhiteFlashS : MonoBehaviour {

	private bool isFlashing = false;
	private bool fadingIn = false;


	public float fadeInRate = 1f;
	public float fadeOutRate = 1f;

	public float delayFadeOutMax = 1f;
	private float delayFade;

	private SpriteRenderer myRender;
	private Color myColor;

	public GameObject loadingObj;

	// Use this for initialization
	void Start () {

		myRender = GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!isFlashing){
			if (myRender.enabled){
				myRender.enabled = false;
			}
		}
		else{

			if (!myRender.enabled){
				myRender.enabled = true;
			}

			myColor = myRender.color;
			if (fadingIn){
				myColor.a += fadeInRate*Time.deltaTime;
				if (myColor.a >= 1){
					myColor.a = 1;
					fadingIn = false;
				}
			}
			else{
				if (delayFade > 0){
					delayFade -= Time.deltaTime;
				}
				else{
					loadingObj.SetActive(false);
				myColor.a -= fadeOutRate*Time.deltaTime;
				if (myColor.a <= 0){
					myColor.a = 0;
					isFlashing = false;
				}
				}
			}
			myRender.color = myColor;
		}
	
	}

	public void StartFlash(){

		myColor = myRender.color;
		myColor.a = 0;
		myRender.color = myColor;
		fadingIn = true;
		isFlashing = true;
		delayFade = delayFadeOutMax;

		if (loadingObj){
			loadingObj.SetActive(true);
		}

	}
}
