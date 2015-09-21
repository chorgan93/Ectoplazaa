using UnityEngine;
using System.Collections;

public class FadeSpriteS : MonoBehaviour {

	public GameObject fadeObj;

	public float spawnRate = 0.1f;
	private float spawnRateCountdown;

	
	// Update is called once per frame
	void FixedUpdate () {

		spawnRateCountdown -= Time.deltaTime;
		if (spawnRateCountdown <= 0){
			spawnRateCountdown = spawnRate;
			GameObject newFade = Instantiate(fadeObj,transform.position,Quaternion.identity)
				as GameObject;
			newFade.transform.localScale = transform.localScale;
			SpriteRenderer newFadeRender = newFade.GetComponent<SpriteRenderer>();
			newFadeRender.color = GetComponent<SpriteRenderer>().color;
			newFadeRender.sprite = GetComponent<SpriteRenderer>().sprite;
		}
	
	}
}
