using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioClip background;
    [SerializeField] private AudioClip normal;
	[SerializeField] private AudioClip busy;
    [SerializeField] private AudioClip dragging;

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
    [SerializeField] private AudioClip killTriple;
    [SerializeField] private AudioClip killMulti;
    [SerializeField] private AudioClip killMega;
    [SerializeField] private AudioClip killUltra;
    [SerializeField] private AudioClip killMonster;

    private Dictionary<AudioClip, AudioSource> audioSources;

    private AudioSource fxAudioSource;

    void Awake() {
        this.audioSources = new Dictionary<AudioClip, AudioSource>();

        // Initialises the music layers.
        AudioClip[] musicClips = new AudioClip[] { background, normal, busy, dragging};
        foreach (AudioClip clip in musicClips) {
            
            AudioSource source = gameObject.AddComponent<AudioSource>() as AudioSource;
            source.clip = clip;
            source.maxDistance = Mathf.Infinity;
            source.loop = true;
            source.volume = Container.instance.config.musicVolume;
            source.Play();

            audioSources[clip] = source;
        }

        audioSources[busy].volume = 0;
        audioSources[dragging].volume = 0;

        Container.instance.OnPlayerMoved += this.OnPlayerMoved;
        Container.instance.OnEnemyKilled += this.OnEnemyKilled;
        Container.instance.OnDragEnd += this.DoYuuaaa;
        Container.instance.OnDragEnd += this.StopStretch;
        // Container.instance.OnDragUpdate += this.DoStretch;
        Container.instance.OnDragIncrement += this.DoStretch;
        Container.instance.OnKillStreakChanged += this.OnKillStreakChanged;
        Container.instance.OnDragStart += this.StartDragSound;
        Container.instance.OnDragEnd += this.EndDragSound;
        Container.instance.OnEnemyHit += this.OnEnemyHit;

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

    private void OnEnemyHit(Transform hitby) {
        StartCoroutine(SwordHitRoutine(false));
    }
    
    private void OnEnemyKilled(GameObject enemyKilled){
        StartCoroutine(SwordHitRoutine(true));
	}

    private int amountOfDeadSoundsPlayedRightNow = 0;
    private IEnumerator SwordHitRoutine(bool isKill = true){
		PlaySoundClip(chinkSound);
        yield return new WaitForSeconds(.2f);
        if(isKill) {
            PlaySoundClip(hitSound);
        } else {
            PlaySoundClip(armorHitSound);
        }
        yield return new WaitForSeconds(.3f);
        if(isKill) {
            int randomIndex = Random.Range(0, deadSounds.Length);
            AudioClip randomSound = deadSounds[randomIndex];
            if(amountOfDeadSoundsPlayedRightNow < 3) {
                PlaySoundClip(randomSound);
                amountOfDeadSoundsPlayedRightNow++;
            }
        }
        yield return new WaitForSeconds(.3f);
        if(isKill) {
            amountOfDeadSoundsPlayedRightNow--;
        }
    }

    void OnPlayerMoved(Vector2 position, Vector2 velocity) {
        // Determine how loud the 'busy' sound is.
        float magnitude = velocity.magnitude;
        float volume = (magnitude - this.busyStartPlayingVelocity) / this.busyStopPlayingVelocity;

        if (volume < 0) {
            volume = 0;
        }
        if (volume > 1) {
            volume = Container.instance.config.musicVolume;
        }

        busyVolume = Mathf.SmoothDamp(busyVolume, volume, ref busyVolumeVelocity, this.busySoundVolumeFadeDuration);
		this.audioSources[busy].volume = busyVolume;
	}
    private float cachedKillCount = 0f;
    private bool soundInQue = false;
    void OnKillStreakChanged(float streakAmount){	
        if(cachedKillCount < streakAmount) {
            cachedKillCount = streakAmount;
        }
        if(!soundInQue) {
            StartCoroutine(DelayedKillStreakSound());
        }
	}


    private IEnumerator DelayedKillStreakSound() {
        soundInQue = true;
        yield return new WaitForSeconds(.5f);
        soundInQue = false;
        if (cachedKillCount == 2) {
            this.PlaySoundClip(this.killDouble);
        } else if (cachedKillCount == 3) {
            this.PlaySoundClip(this.killTriple);
        } else if (cachedKillCount == 4) {
            this.PlaySoundClip(this.killMulti);
        } else if (cachedKillCount == 5) {
            this.PlaySoundClip(this.killMega);
        } else if (cachedKillCount == 6) {
            this.PlaySoundClip(this.killUltra);
        } else if (cachedKillCount >= 7) {
            this.PlaySoundClip(this.killMonster);
        }
        cachedKillCount = 0;
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
        slave.Play(clip, maxDistance, Container.instance.config.sfxVolume);
    }

   // private bool firstTime = true;
    void StartDragSound(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
        // if(firstTime) {
        //     firstTime = false;
        //     return;
        // }
		this.audioSources[dragging].volume = Container.instance.config.musicVolume;
    }

    void EndDragSound(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		this.audioSources[dragging].volume = 0f;
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
        Container.instance.OnDragEnd -= this.DoYuuaaa;
        Container.instance.OnDragEnd -= this.StopStretch;
        // Container.instance.OnDragUpdate -= this.DoStretch;
        Container.instance.OnDragIncrement -= this.DoStretch;
        Container.instance.OnKillStreakChanged -= this.OnKillStreakChanged;
        Container.instance.OnDragStart -= this.StartDragSound;
        Container.instance.OnDragEnd -= this.EndDragSound;
        Container.instance.OnEnemyHit -= this.OnEnemyHit;
    } 


}