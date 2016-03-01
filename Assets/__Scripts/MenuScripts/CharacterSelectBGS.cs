using UnityEngine;
using System.Collections;

public class CharacterSelectBGS : MonoBehaviour {

	public int myPlayerNum;

	public Texture[] possBgs;
	public Color[] possColors;

	private Renderer myRenderer;
	public Renderer childRender;

	private Vector2 offsetChangeRate = new Vector2(0.25f,0.1f);

	public string[] charNames;
	public string[] specialNames;

	public TextMesh charNameHolder;
	public TextMesh specialNameHolder;

	public CharacterSelectMenu menuRef;

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
			}
			else{

				myRenderer.enabled = false;

			}
		}
	
	}
}
