using UnityEngine;
using System.Collections;

public class BokehS : MonoBehaviour {

	// currently unused visual effect script
	// fades a sprite in and out randomly

	public float sizeDiff = 0.5f;
	private Vector3 startSize;

	public float fadeRateMin = 0.1f;
	public float fadeRateMax = 0.5f;
	private float fadeRate;

	public float maxFade = 0.5f;
	public float minFade = 0.25f;
	private float targetFade;

	private bool fadingIn = false;
	private bool fadingOut = false;
	private bool delaying = false;

	private float delayFadeMax = 1f;
	private float delayFadeMin = 0.25f;
	private float delayFade;
	private float delayFadeCountdown;

	private Color currentCol;
	private SpriteRenderer ownRender;

	public bool driftDown = false;
	private Vector3 startPos;
	private float driftRateMin = 0.2f;
	private float driftRateMax = 0.5f;
	private float driftRate;

	// Use this for initialization
	void Start () {

		// start at a random size

		startSize = transform.localScale;
		startSize += Random.insideUnitSphere*sizeDiff;
		startSize.z = 1;

		transform.localScale = startSize;

		// set target max alpha

		targetFade = Random.Range(minFade,maxFade);

		// set time between fade in and out

		delayFade = Random.Range(delayFadeMin,delayFadeMax);

		// set actual fade rate

		fadeRate = Random.Range(fadeRateMin,fadeRateMax);

		// get sprite renderer component (attached to gameObject)
		ownRender = GetComponent<SpriteRenderer>();

		// start bokeh at random stage of fade in/delay/fade out
		Color startCol = ownRender.color;

		int StageToStart = Mathf.FloorToInt(Random.Range(0,3));
		if (StageToStart == 0){
			fadingIn = true;
			startCol.a = 0;
		}
		else if (StageToStart == 1){
			fadingOut = true;
			startCol.a = targetFade;
		}
		else{
			delayFadeCountdown = delayFade;
		}

		// set color/transparency according to appropriate stage
		ownRender.color = startCol;

		startPos = transform.position;
		driftRate = Random.Range(driftRateMin,driftRateMax);
	
	}
	
	// Update is called once per frame
	void Update () {

		// in between fade out/fade in
		if (delaying){
			delayFadeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			if (delayFadeCountdown <= 0){
				fadingIn = true;
				transform.position = startPos;
			}
		}
		else if (fadingIn){
			// fade in to max alpha, then fade out
			currentCol = ownRender.color;
			currentCol.a -= Time.deltaTime*TimeManagerS.timeMult*fadeRate;
			ownRender.color = currentCol;
			if (currentCol.a >= targetFade){
				fadingIn = false;
				fadingOut = true;
			}
		}
		else{
			// fade out to zero alpha, then delay
			currentCol = ownRender.color;
			currentCol.a -= Time.deltaTime*TimeManagerS.timeMult*fadeRate;
			ownRender.color = currentCol;
			if (currentCol.a <= 0){
				fadingIn = false;
				fadingOut = false;
				delaying = true;
				delayFadeCountdown = delayFade;
			}
		}

		// if bokeh drift, move vertical position
		if (driftDown){
			Vector3 driftPos = transform.position;
			driftPos.y -= Time.deltaTime*TimeManagerS.timeMult*driftRate;
			transform.position = driftPos;
		}
	
	}
}
