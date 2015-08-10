using UnityEngine;
using System.Collections;

public class LightningScreenS : MonoBehaviour {

	private float lifeTime = 0.04f;

	private float startAlpha = 0.1f;

	//public Color characterTint;

	// Use this for initialization
	void Start () {
	
		transform.parent= null;

		Renderer ownRender = GetComponent<Renderer>();
		Color newAlpha = ownRender.material.color;
		newAlpha.a = startAlpha;
		ownRender.material.color = newAlpha;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		lifeTime -= Time.deltaTime*TimeManagerS.timeMult;

		if (lifeTime <= 0){
			Destroy(gameObject);
		}
	
	}
}
