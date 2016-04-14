using UnityEngine;
using System.Collections;

public class FadeObjS : MonoBehaviour {

	public float fadeRate = 1;
	private SpriteRenderer ownRender;
	private TextMesh ownText;

	public float delayFade = 0.25f;

	public bool dontDestroy = false;

	private bool stopFade = false;
	private bool fadingOut = false;

	public GameObject loadingObj;

	// Use this for initialization
	void Start () {

		ownRender = GetComponent<SpriteRenderer>();
		if (!ownRender){
			ownText = GetComponent<TextMesh>();
		}

		if (loadingObj){
			loadingObj.SetActive(false);
		}
	
	}
	
	// Update is called once per frame
	void Update () {


		if (delayFade > 0){
			delayFade -= Time.deltaTime * TimeManagerS.timeMult;
		}
	else{
			if (ownRender){
			if (!stopFade){
		Color fadeCol = ownRender.color;
				if (fadingOut){
					fadeCol.a += fadeRate*Time.deltaTime*TimeManagerS.timeMult;
					Debug.Log("FADING");
				}
				else{
		fadeCol.a -= fadeRate*Time.deltaTime*TimeManagerS.timeMult;
				}
		ownRender.color = fadeCol;
				if (!fadingOut){
		if (fadeCol.a <= 0){
				if (dontDestroy){
					fadeCol.a = 0;
						stopFade = true;
							Debug.Log("STOP FADE!");
				}
				else{
					Destroy(gameObject);
				}
		}
				}
				else{
			if (fadeCol.a >= 1){
				fadeCol.a = 1;
			}
				}
		}
			}
			else{
				if (!stopFade){
					Color fadeCol = ownText.color;
					if (fadingOut){
						fadeCol.a += fadeRate*Time.deltaTime*TimeManagerS.timeMult;
					}
					else{
						fadeCol.a -= fadeRate*Time.deltaTime*TimeManagerS.timeMult;
					}
					ownText.color = fadeCol;
					Debug.Log("Fading text");
					if (!fadingOut){
						if (fadeCol.a <= 0){
							if (dontDestroy){
								fadeCol.a = 0;
								stopFade = true;
							}
							else{
								Destroy(gameObject);
							}
						}
					}
					else{
						if (fadeCol.a >= 1){
							fadeCol.a = 1;
						}
					}
				}
			}
		}

	}

	public void FadeOut(){
		if (!fadingOut){
		fadeRate *= 5f;
		stopFade = false;
		fadingOut = true;

		if (loadingObj){
			loadingObj.SetActive(true);
		}
		}
	}
}
