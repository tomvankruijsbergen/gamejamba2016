using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodSplat : MonoBehaviour {

	SpriteRenderer[] bloodSplatRenderers;
	List<GameObject> chosenBlootsplatters = new List<GameObject>();


	private float duration = 1.5f;
	private float startTime;

	void Awake(){
		bloodSplatRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
	}

	void Start(){
		startTime = Time.time;
		//pick two random bloodsplatters
		int randomInt;
		for(int i = 0; i < 1; i++){
			randomInt = Random.Range(0,bloodSplatRenderers.Length);
			chosenBlootsplatters.Add(bloodSplatRenderers[randomInt].gameObject);
		}

		foreach(GameObject rnd in chosenBlootsplatters){
			iTween.ScaleTo(rnd.gameObject, new Vector3(1,1,1), 2f);
		}
	}

	void Update(){
		float t = (Time.time - startTime) / duration;
		foreach(GameObject go in chosenBlootsplatters){

			SpriteRenderer rnd = go.GetComponent<SpriteRenderer>();
			rnd.color = new Color(1f,1f,1f,Mathf.SmoothStep(1, 0, t));
		}  
	}
}
