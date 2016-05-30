using UnityEngine;
using System.Collections;

public class ConfettiS : MonoBehaviour {

	public Sprite[] possTextures;

	private SpriteRenderer myRender;

	public float fadeDelay = 1.6f;
	private float fadeRate = 1f;

	public float fallRate = 120f;
	public float fallRateRange = 20f;
	private Vector3 myFallRate;

	public float rotateRate = 50f;
	public float rotateRateRange = 15f;
	private Vector3 myRotateRate;

	// Use this for initialization
	void Start () {

		myRender = GetComponent<SpriteRenderer>();

		Initialize();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.position -= myFallRate*Time.unscaledDeltaTime;

		transform.Rotate(myRotateRate*Time.unscaledDeltaTime);

		fadeDelay -= Time.unscaledDeltaTime;

		if (fadeDelay <= 0){
			Color fadeCol = myRender.color;
			fadeCol.a -= fadeRate*Time.unscaledDeltaTime;

			if (fadeCol.a <= 0){
				Destroy(gameObject);
			}
			else{
				myRender.color = fadeCol;
			}
		}
	
	}


	private void Initialize(){

		myRender.sprite = possTextures[Mathf.RoundToInt(Random.Range(0, possTextures.Length-1))];

		myFallRate = new Vector3(0, fallRate + fallRateRange*Random.insideUnitCircle.x, 0);

		myRotateRate = (rotateRate+rotateRateRange*Random.insideUnitCircle.x)*Random.insideUnitSphere;

	}
}
