using UnityEngine;
using System.Collections;

public class StageBGS : MonoBehaviour {

	private Renderer myRender;
	public Texture myTexture;

	public Vector2 scrollSpeed = new Vector3(0.2f, 0.1f);

	private float fadeInRate = 0.25f;

	public Color bgColor;
	private Color supportColor = new Color(0,0,0,0.6f);

	public Renderer supportBG;

	public Texture[] gameEndTextures;
	private ScoreKeeperS scoreKeeperRef;

	// Use this for initialization
	void Start () {

		myRender = GetComponent<Renderer>();

		if (gameEndTextures.Length > 0){
			if (!CurrentModeS.DoAnotherRound()){
			scoreKeeperRef = GameObject.Find("Main Camera").GetComponent<ScoreKeeperS>();
			myTexture = gameEndTextures[GlobalVars.characterNumber[scoreKeeperRef.GetWinningPlayer()-1]-1];
			}
			else{
				gameObject.SetActive(false);
			}
		}

		myRender.material.SetTexture("_MainTex", myTexture);

		Color startCol = bgColor;
		startCol.a = 0;
		myRender.material.color = startCol;


		if (supportBG){
			
			startCol = supportColor;
			startCol.a = 0;
			myRender.material.color = startCol;
			supportBG.material.color = startCol;
		}
	
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
		if (supportBG){
			if (startCol.a > 0.6f){
				startCol.a = 0.6f;
			}
			supportBG.material.color = startCol;
		}


		myRender.material.SetTextureOffset("_MainTex", 
		                                   myRender.material.GetTextureOffset("_MainTex")+scrollSpeed*Time.deltaTime);
	
	}
}
