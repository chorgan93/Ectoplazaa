using UnityEngine;
using System.Collections;

public class SkySwayS : MonoBehaviour {
	
	public Vector3 skyOffset;
	public float swayIntensity = 1.5f;
	public float currentSwayTime = 0;
	public float swayDuration = 3f;
	
	public bool swayUp = true;
	
	Vector3 startPos;
	
	void Start () {

		swayIntensity += Random.insideUnitCircle.x*swayIntensity/2f;
		swayDuration += Random.insideUnitCircle.x*swayDuration/2f;
		
		startPos = transform.position;
		currentSwayTime = Random.Range(0, swayDuration / 2f);
		
		
		
		
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (swayUp){
			currentSwayTime += Time.deltaTime;
			if (currentSwayTime >= swayDuration){
				swayUp = !swayUp;
			}
		}
		else{
			currentSwayTime -= Time.deltaTime;
			if (currentSwayTime <= 0){
				swayUp = !swayUp;
			}
		}
		
		
		float t = AnimCurveS.QuadEaseInOut(currentSwayTime, 0, swayIntensity, swayDuration);
		
		
		skyOffset.y = t - swayIntensity/2f;
		
		transform.position = startPos + skyOffset;
		
		
		
		
	}
}
