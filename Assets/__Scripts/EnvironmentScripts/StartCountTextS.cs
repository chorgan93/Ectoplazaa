using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartCountTextS : MonoBehaviour {

	private float dropDelay = 0.2f;
	private float fadeRate = 1.5f;
	private float dropRate = 3f;

	public List<string> possTexts;
	public string textToDisplay;

	public List<TextMesh> myTextMeshes;

	// Use this for initialization
	void Start () {

		if (possTexts.Count > 0){
			int textToUse = Mathf.FloorToInt(Random.Range(0,possTexts.Count));

			textToDisplay = possTexts[textToUse];
		}

		for (int i = 0; i < myTextMeshes.Count; i++){
			myTextMeshes[i].text = textToDisplay;
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		if (dropDelay > 0){
			dropDelay -= Time.deltaTime;
		}
		else{
			Vector3 currentPos = transform.position;
			currentPos.y -= dropRate*Time.deltaTime;
			transform.position = currentPos;

			for (int i = 0; i < myTextMeshes.Count; i++){
				Color fadeCol = myTextMeshes[i].color;
				fadeCol.a -= Time.deltaTime*fadeRate;
				if (fadeCol.a > 0){
					myTextMeshes[i].color = fadeCol;
				}
				else{
					Destroy(gameObject);
				}
			}
		}
	
	}
}
