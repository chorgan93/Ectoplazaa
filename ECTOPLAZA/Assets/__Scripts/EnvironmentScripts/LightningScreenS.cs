using UnityEngine;
using System.Collections;

public class LightningScreenS : MonoBehaviour {

	private float lifeTime = 0.04f;

	// Use this for initialization
	void Start () {
	
		transform.parent= null;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		lifeTime -= Time.deltaTime*TimeManagerS.timeMult;

		if (lifeTime <= 0){
			Destroy(gameObject);
		}
	
	}
}
