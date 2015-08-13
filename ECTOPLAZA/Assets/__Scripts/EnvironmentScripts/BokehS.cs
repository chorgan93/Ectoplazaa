using UnityEngine;
using System.Collections;

public class BokehS : MonoBehaviour {

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

		startSize = transform.localScale;
		startSize += Random.insideUnitSphere*sizeDiff;
		startSize.z = 1;

		transform.localScale = startSize;

		targetFade = Random.Range(minFade,maxFade);

		delayFade = Random.Range(delayFadeMin,delayFadeMax);

		fadeRate = Random.Range(fadeRateMin,fadeRateMax);

		ownRender = GetComponent<SpriteRenderer>();
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

		ownRender.color = startCol;

		startPos = transform.position;
		driftRate = Random.Range(driftRateMin,driftRateMax);
	
	}
	
	// Update is called once per frame
	void Update () {

		if (delaying){
			delayFadeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			if (delayFadeCountdown <= 0){
				fadingIn = true;
				transform.position = startPos;
			}
		}
		else if (fadingIn){
			currentCol = ownRender.color;
			currentCol.a -= Time.deltaTime*TimeManagerS.timeMult*fadeRate;
			ownRender.color = currentCol;
			if (currentCol.a >= targetFade){
				fadingIn = false;
				fadingOut = true;
			}
		}
		else{
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

		if (driftDown){
			Vector3 driftPos = transform.position;
			driftPos.y -= Time.deltaTime*TimeManagerS.timeMult*driftRate;
			transform.position = driftPos;
		}
	
	}
}
