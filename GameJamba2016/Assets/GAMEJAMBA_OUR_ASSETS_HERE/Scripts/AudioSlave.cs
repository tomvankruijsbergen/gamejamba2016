using UnityEngine;
using System.Collections;

public class AudioSlave : MonoBehaviour {

	public void Play(AudioClip clip, float maxHearDistance) {
		AudioSource source = gameObject.AddComponent<AudioSource>() as AudioSource;
        source.clip = clip;
        source.loop = false;
		source.maxDistance = maxHearDistance;
		source.Play();
		
		this.Invoke("Remove", clip.length);
	}

	void Remove() {
		Destroy(gameObject);
	}
}
