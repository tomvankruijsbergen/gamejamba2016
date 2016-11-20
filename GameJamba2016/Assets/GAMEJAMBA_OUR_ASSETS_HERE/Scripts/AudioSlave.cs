using UnityEngine;
using System.Collections;

public class AudioSlave : MonoBehaviour {

	AudioSource source = null;

	void Awake() {
		Container.instance.OnTimeChanged += TimeChanged;
	}

	private void TimeChanged(float timeScale) {
		if(source != null) {
			source.pitch = Mathf.Clamp(timeScale, 0.8f, 1.4f);
		}
	}

	public void Play(AudioClip clip, float maxHearDistance, float volume = 1f) {
		source = gameObject.AddComponent<AudioSource>() as AudioSource;
        source.clip = clip;
        source.loop = false;
		source.volume = volume;
		source.maxDistance = maxHearDistance;
		source.Play();
		
		this.Invoke("Remove", clip.length);
	}

	void Remove() {
		Destroy(gameObject);
	}

	void OnDestroy() {
		Container.instance.OnTimeChanged -= TimeChanged;
	}
}
