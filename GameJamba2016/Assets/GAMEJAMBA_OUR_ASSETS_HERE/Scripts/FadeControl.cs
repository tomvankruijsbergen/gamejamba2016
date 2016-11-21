using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour {

	private RawImage fadeImage;
	private Color tmpFade;
	[SerializeField] private float fadeInDuration = 4f;
	[SerializeField] private float fadeOutDuration = 2f;
	private bool fadingIn = false;
	private bool fadingOut = false;
	private float fadeStart = 0f;

	// Use this for initialization
	void Awake () {
		fadeImage = GetComponent<RawImage>();
		tmpFade = fadeImage.color;
		fadeStart = Time.time;
		fadingIn = true;
		Container.instance.OnPlayerDied += FadeOut;
	}
	
	void FadeOut(Transform kankerOp) {
		fadingOut = true;
		fadeStart = Time.time;
	}

	// Update is called once per frames
	void Update () {
		if(fadingIn) {
			float alpha = 1f - Mathf.Clamp((Time.time - fadeStart) / fadeInDuration, 0f, 1f);
			if(alpha == 0f) {
				fadingIn = false;
			} else {
				//easing
				alpha = Mathf.Sqrt(alpha);
			}
			tmpFade.a = alpha;
		} else if (fadingOut) {
			float alpha = Mathf.Clamp((Time.time - fadeStart) / fadeOutDuration, 0f, 1f);
			if(alpha == 1f) {
				fadingOut = false;
			} else {
				//easing
				alpha = Mathf.Sqrt(alpha);
			}
			tmpFade.a = alpha;
		}
		if(fadingIn || fadingOut) {
			fadeImage.color = tmpFade;
		}
	}

	void OnDestroy() {
		Container.instance.OnPlayerDied -= FadeOut;
	}
}
