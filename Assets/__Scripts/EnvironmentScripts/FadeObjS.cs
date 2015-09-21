using UnityEngine;
using System.Collections;

public class FadeObjS : MonoBehaviour {

	public float fadeRate = 1;
	private SpriteRenderer ownRender;

	public float delayFade = 0.25f;

	// Use this for initialization
	void Start () {

		ownRender = GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {


		if (delayFade > 0){
			delayFade -= Time.deltaTime * TimeManagerS.timeMult;
		}
	else{
		Color fadeCol = ownRender.color;
		fadeCol.a -= fadeRate*Time.deltaTime*TimeManagerS.timeMult;
		ownRender.color = fadeCol;
		if (fadeCol.a <= 0){
			Destroy(gameObject);
		}
		}

	}
}
