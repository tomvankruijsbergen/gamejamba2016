using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodSplat : MonoBehaviour {

	SpriteRenderer[] bloodSplatRenderers;
	List<SpriteRenderer> chosenBlootsplatters = new List<SpriteRenderer>();

	private float duration = 1f;
	private float startTime;

	void Awake(){
		bloodSplatRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
	}

	void Start(){
		startTime = Time.time;
		//pick two random bloodsplatters
		int randomInt;
		for(int i = 0; i < 2; i++){
			randomInt = Random.Range(0,bloodSplatRenderers.Length);
			chosenBlootsplatters.Add(bloodSplatRenderers[randomInt]);
		}

		foreach(SpriteRenderer rnd in chosenBlootsplatters){
			rnd.enabled = true;
			iTween.ScaleTo(rnd.gameObject, new Vector3(1,1,1), 0.3f + Random.Range(0.2f,0.4f));
		}
	}

	void Update(){
		float t = (Time.time - startTime) / duration;
		foreach(SpriteRenderer rnd in chosenBlootsplatters){
			rnd.color = new Color(1f,1f,1f,Mathf.SmoothStep(1, 0, t));
		}  
	}
}
