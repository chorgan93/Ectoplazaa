using UnityEngine;
using System.Collections;

public class StageBGS : MonoBehaviour {

	private Renderer myRender;
	public Texture myTexture;

	public Vector2 scrollSpeed = new Vector3(0.2f, 0.1f);

	private float fadeInRate = 0.25f;

	public Color bgColor;

	// Use this for initialization
	void Start () {

		myRender = GetComponent<Renderer>();
		myRender.material.SetTexture("_MainTex", myTexture);

		Color startCol = bgColor;
		startCol.a = 0;
		myRender.material.color = startCol;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Color startCol = myRender.material.color;
		if (startCol.a < 1){
			startCol.a += fadeInRate*Time.deltaTime;
		}
		else{
			startCol.a = 1;
		}
		myRender.material.color = startCol;

		myRender.material.SetTextureOffset("_MainTex", 
		                                   myRender.material.GetTextureOffset("_MainTex")+scrollSpeed*Time.deltaTime);
	
	}
}
