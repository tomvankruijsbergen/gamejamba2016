using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioClip background;
    [SerializeField] private AudioClip normal;
	[SerializeField] private AudioClip busy;

    [SerializeField] private float busyStartPlayingVelocity = 5f;
    [SerializeField] private float busyStopPlayingVelocity = 5f;
    [SerializeField] private float busySoundVolumeFadeDuration = 0.5f;

    [SerializeField] private float maxSoundDistanceForFX = 10f;

    [SerializeField]
    private AudioClip killEnemy;

    private Dictionary<AudioClip, AudioSource> audioSources;

    private AudioSource fxAudioSource;

    void Awake() {
        this.audioSources = new Dictionary<AudioClip, AudioSource>();

        AudioClip[] clips = new AudioClip[] { background, normal, busy };
        foreach (AudioClip clip in clips) {
            
            AudioSource source = gameObject.AddComponent<AudioSource>() as AudioSource;
            source.clip = clip;
            source.maxDistance = Mathf.Infinity;
            source.loop = true;
            source.volume = 1;
            source.Play();

            audioSources[clip] = source;
        }

        audioSources[busy].volume = 0;

        Container.instance.OnPlayerMoved += this.OnPlayerMoved;
        Container.instance.OnEnemyKilled += this.OnEnemyKilled;
    }

    private float busyVolume = 0;
    private float busyVolumeVelocity = 0;

    void OnPlayerMoved(Vector2 position, Vector2 velocity) {
        // Determine how loud the 'busy' sound is.
        float magnitude = velocity.magnitude;
        float volume = (magnitude - this.busyStartPlayingVelocity) / this.busyStopPlayingVelocity;

        if (volume < 0) {
            volume = 0;
        }
        if (volume > 1) {
            volume = 1;
        }

        busyVolume = Mathf.SmoothDamp(busyVolume, volume, ref busyVolumeVelocity, this.busySoundVolumeFadeDuration);
		this.audioSources[busy].volume = busyVolume;
	}

    private void OnEnemyKilled(GameObject enemyKilled){
		this.PlaySoundClip(this.killEnemy, enemyKilled);
	}

    private void PlaySoundClip(AudioClip clip, GameObject onGameObject = null) {
        GameObject soundObject = new GameObject();
        AudioSlave slave = soundObject.AddComponent<AudioSlave>() as AudioSlave;

        float maxDistance;
        if (onGameObject != null) {
            maxDistance = maxSoundDistanceForFX;
            soundObject.transform.parent = onGameObject.transform;
        } else {
            maxDistance = Mathf.Infinity;
            soundObject.transform.parent = transform;
        }
        slave.Play(clip, maxDistance);
    }

    void Destroy() {
        Container.instance.OnPlayerMoved -= this.OnPlayerMoved;
        Container.instance.OnEnemyKilled -= this.OnEnemyKilled;
    } 


}
