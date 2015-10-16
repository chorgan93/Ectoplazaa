using UnityEngine;
using System.Collections;

public class StockDisplayS : MonoBehaviour {

	public PlayerS myPlayer;

	private SpriteRenderer mySprite;
	private SpriteRenderer myCharSprite;

	public TextMesh leftText; // player num
	public TextMesh rightText; // stock num

	public Color outCol;
	private Color textCol;

	// Use this for initialization
	void Start () {

		// get sprite renderer
		mySprite = GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (myPlayer){

			if (!myCharSprite){
				myCharSprite = myPlayer.spriteObject.GetComponent<SpriteRenderer>();

				// get color to match player color
				textCol = myPlayer.playerMats[myPlayer.characterNum].color;
				leftText.color = textCol;
				rightText.color = textCol;
				leftText.text = "P"+myPlayer.playerNum+":"; // show player num, set once
			}

			if (myPlayer.numLives != 0){
				mySprite.sprite = myCharSprite.sprite;
			}
			else{
				mySprite.color = outCol;
			}

			rightText.text = "x " + myPlayer.numLives; // show lives left, updated

		}
	
	}
}
