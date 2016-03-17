using UnityEngine;
using System.Collections;

public class CharacterSelectBGS : MonoBehaviour {

	public int myPlayerNum;

	public Texture[] possBgs;
	public Color[] possColors;

	public int[] speedStats;
	public int[] jumpStats;
	public int[] chargeStats;

	public SpriteRenderer[] speedStars;
	public SpriteRenderer[] jumpStars;
	public SpriteRenderer[] chargeStars;

	private Renderer myRenderer;
	public Renderer childRender;

	private Vector2 offsetChangeRate = new Vector2(0.25f,0.1f);

	public string[] charNames;
	public string[] specialNames;

	public TextMesh charNameHolder;
	public TextMesh specialNameHolder;

	public CharacterSelectMenu menuRef;

	public Sprite[] filledStars;
	public Sprite unfilledStar;

	// Use this for initialization
	void Start () {

		myRenderer = GetComponent<Renderer>();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!menuRef.hasJoined[myPlayerNum-1]){
			myRenderer.enabled = false;
		}
		else{
			if (!menuRef.hasSelected[myPlayerNum-1]){
				myRenderer.enabled = true;

				myRenderer.material.SetTexture("_MainTex", possBgs[GlobalVars.characterNumber[myPlayerNum-1]-1]);

				myRenderer.material.SetTextureOffset("_MainTex", 
				                                     myRenderer.material.GetTextureOffset("_MainTex") 
				                                     + offsetChangeRate*Time.deltaTime);


				childRender.material.color = possColors[GlobalVars.characterNumber[myPlayerNum-1]-1];

				charNameHolder.text = charNames[GlobalVars.characterNumber[myPlayerNum-1]-1];
				specialNameHolder.text = specialNames[GlobalVars.characterNumber[myPlayerNum-1]-1];

				for (int i = 0; i < speedStars.Length; i++){
					if (i < speedStats[GlobalVars.characterNumber[myPlayerNum-1]-1]){
						//speedStars[i].SetActive(true);
						speedStars[i].sprite = filledStars[GlobalVars.characterNumber[myPlayerNum-1]-1];
					}
					else{
						//speedStars[i].SetActive(false);
						speedStars[i].sprite = unfilledStar;
					}
				}

				for (int j = 0; j < jumpStars.Length; j++){
					if (j < jumpStats[GlobalVars.characterNumber[myPlayerNum-1]-1]){
						//jumpStars[j].SetActive(true);
						jumpStars[j].sprite = filledStars[GlobalVars.characterNumber[myPlayerNum-1]-1];
					}
					else{
						//jumpStars[j].SetActive(false);
						jumpStars[j].sprite = unfilledStar;
					}
				}

				for (int k = 0; k < chargeStars.Length; k++){
					if (k < chargeStats[GlobalVars.characterNumber[myPlayerNum-1]-1]){
						//chargeStars[k].SetActive(true);
						chargeStars[k].sprite = filledStars[GlobalVars.characterNumber[myPlayerNum-1]-1];
					}
					else{
						//chargeStars[k].SetActive(false);
						chargeStars[k].sprite = unfilledStar;
					}
				}
			}
			else{

				myRenderer.enabled = false;

			}
		}
	
	}
}
