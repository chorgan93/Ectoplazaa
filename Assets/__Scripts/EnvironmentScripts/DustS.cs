using UnityEngine;
using System.Collections;

public class DustS : MonoBehaviour {

	public float fadeRate = 1f;
	public float delayFadeRate = 1f;
	private float startDelayFade;
	public float shrinkRate = 1f;

	private SpriteRenderer myRender;
	private Color myColor;

	private float startSize;

	public bool isSmoke = false;
	public bool isElec = false;

	// Use this for initialization
	void Start () {

		startSize = transform.localScale.x;
		myRender = GetComponent<SpriteRenderer>();

		startDelayFade = delayFadeRate;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (delayFadeRate <= 0.3f){
		Vector3 currentSize = transform.localScale;
		currentSize.x -= shrinkRate*Time.deltaTime;
		if (currentSize.x <= 0){
			currentSize.x = 0;
		}
		currentSize.y = currentSize.x;
		transform.localScale = currentSize;
		}
		delayFadeRate -= Time.deltaTime;


		if (delayFadeRate <= 0){
		myColor = myRender.color;
		myColor.a -= fadeRate*Time.deltaTime;
		if (myColor.a <= 0){
				TurnOff();
		}
		else{
			myRender.color = myColor;
		}
		}
	
	}

	void TurnOff(){
		gameObject.SetActive(false);

		if (isElec){
			SpawnManagerS.Instance.ReturnElec(this);
		}
		if (isSmoke){
			SpawnManagerS.Instance.ReturnSmoke(this);
		}

	}

	public void TurnOn(Vector3 newPos, Quaternion newRot){

		Vector3 resetSize = new Vector3(startSize, startSize, 1);

		transform.localScale = resetSize;
		delayFadeRate = startDelayFade;

		transform.position = newPos;
		transform.rotation = newRot;

		Color fadeCol = myRender.color;
		fadeCol.a = 1;
		myRender.color = fadeCol;

		gameObject.SetActive(true);

	}
}
