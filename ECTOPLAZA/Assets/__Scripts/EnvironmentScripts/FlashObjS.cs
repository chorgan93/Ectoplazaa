using UnityEngine;
using System.Collections;

public class FlashObjS : MonoBehaviour {

	private Renderer ownRender;
	private float fadeRate = 2f;
	public float fadeMax = 0.9f;

	private Color currentCol;

	public bool startFlash = false;

	// Use this for initialization
	void Start () {
	
		ownRender = GetComponent<Renderer>();

		currentCol = ownRender.material.color;
		if (!startFlash){
			currentCol.a = 0;
		}
		else{
			currentCol.a = fadeMax;
		}
		ownRender.material.color = currentCol;

	}
	
	// Update is called once per frame
	void Update () {

		if (ownRender.enabled){
			currentCol = ownRender.material.color;
			currentCol.a -= fadeRate*Time.deltaTime*TimeManagerS.timeMult;
			ownRender.material.color = currentCol;

			if (currentCol.a <= 0){
				ownRender.enabled = false;
			}
		}
	
	}

	public void ResetFade(){
	
		currentCol.a = fadeMax;
		ownRender.material.color = currentCol;
		ownRender.enabled = true;
	
	}

}
