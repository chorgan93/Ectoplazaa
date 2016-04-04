using UnityEngine;
using System.Collections;

public class FadeInOutBGObjS : MonoBehaviour {

	public float fadeRate = 1f;

	private bool fadeOut = true;
	private SpriteRenderer myRender;

	// Use this for initialization
	void Start () {

		myRender = GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {

		Color fadeCol = myRender.color;

		if (fadeOut){

			fadeCol.a -= Time.deltaTime*fadeRate;
			if (fadeCol.a <= 0){
				fadeCol.a = 0;
				fadeOut = false;
			}

		}
		else{
			fadeCol.a += Time.deltaTime*fadeRate;
			if (fadeCol.a >= 1){
				fadeCol.a = 1;
				fadeOut = true;
			}
		}

		myRender.color = fadeCol;
	
	}
}
