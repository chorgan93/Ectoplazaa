using UnityEngine;
using System.Collections;

public class StockDisplayS : MonoBehaviour {

	public PlayerS myPlayer;

	private SpriteRenderer mySprite;
	private SpriteRenderer myCharSprite;

	public Color outCol;

	// Use this for initialization
	void Start () {

		// get sprite renderer
		mySprite = GetComponent<SpriteRenderer>();
		myCharSprite = myPlayer.spriteObject.GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (myPlayer.numLives != 0){
			mySprite.sprite = myCharSprite.sprite;
		}
		else{
			mySprite.color = outCol;
		}
	
	}
}
