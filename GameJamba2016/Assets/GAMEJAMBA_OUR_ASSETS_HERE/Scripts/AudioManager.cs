﻿using UnityEngine;
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

    [SerializeField] private AudioClip[] yuuuuaaaaas;
    [SerializeField] private AudioClip[] deadSounds;
    [SerializeField] private AudioClip chinkSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip armorHitSound;

    [SerializeField] private AudioClip stretchSound;
    [SerializeField] private AudioClip release;

    [SerializeField] private AudioClip killDouble;
    [SerializeField] private AudioClip killMulti;
    [SerializeField] private AudioClip killUltra;

    private Dictionary<AudioClip, AudioSource> audioSources;

    private AudioSource fxAudioSource;

    void Awake() {
        this.audioSources = new Dictionary<AudioClip, AudioSource>();

        // Initialises the music layers.
        AudioClip[] musicClips = new AudioClip[] { background, normal, busy };
        foreach (AudioClip clip in musicClips) {
            
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
        Container.instance.OnDragEnd += this.DoYuuaaa;
        Container.instance.OnDragEnd += this.StopStretch;
        // Container.instance.OnDragUpdate += this.DoStretch;
        Container.instance.OnDragIncrement += this.DoStretch;

        Container.instance.OnKillStreakChanged += this.OnKillStreakChanged;

        // Container.instance. 'OnEnemyDoDamage' of zoiets += this.DoArmorHit;
    }

    private float busyVolume = 0;
    private float busyVolumeVelocity = 0;

    void DoYuuaaa(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
        int randomYuuaSoundIndex = Random.Range(0, yuuuuaaaaas.Length);
        AudioClip randomSound = yuuuuaaaaas[randomYuuaSoundIndex];
        PlaySoundClip(randomSound);
    }

    void DoStretch(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
        PlaySoundClip(stretchSound);
    }

    void StopStretch(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
        StopSound(stretchSound);
        PlaySoundClip(release);
    }

    void DoArmorHit(GameObject armorHenk) {
        //StartCoroutine(SwordHitDelay(1f));
    }
    
    private void OnEnemyKilled(GameObject enemyKilled){
		StartCoroutine(SwordKillCoroutine());
	}

    private IEnumerator SwordKillCoroutine(){
		PlaySoundClip(chinkSound);
        yield return new WaitForSeconds(.2f);
        PlaySoundClip(hitSound);
        yield  return new WaitForSeconds(.3f);
        int randomIndex = Random.Range(0, deadSounds.Length);
        AudioClip randomSound = deadSounds[randomIndex];
        PlaySoundClip(randomSound);
    }

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

    void OnKillStreakChanged(float streakAmount){	
		if (streakAmount == 2) {
            this.PlaySoundClip(this.killDouble);
        } else if (streakAmount == 3) {
            this.PlaySoundClip(this.killMulti);
        } else if (streakAmount > 3) {
            this.PlaySoundClip(this.killUltra);
        }
	}

    private void PlaySoundClip(AudioClip clip, GameObject onGameObject = null) {
        if (clip == null) {
            Debug.Log("a sound clip was null");
            return;
        }
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

    private void StopSound(AudioClip soundToStop){
        foreach(KeyValuePair<AudioClip, AudioSource>  entry in audioSources)
        {
            if(entry.Key == soundToStop){
                entry.Value.Stop();
            }
        }
    }

    void Destroy() {
        Container.instance.OnPlayerMoved -= this.OnPlayerMoved;
        Container.instance.OnEnemyKilled -= this.OnEnemyKilled;

        Container.instance.OnKillStreakChanged -= this.OnKillStreakChanged;
    } 


}