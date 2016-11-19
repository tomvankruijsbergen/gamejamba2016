using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Container : MonoSingleton<Container> {

	// All messages between objects should go through this class.
	// An object always calls this class, and this class then sends events.
	// Example flow:
	// - Player.cs:  		sends Container.instance.PlayerMoved(this.transform.position);
	// - Container.cs: 		sends this.AudioChanged("something");
	// - AudioManager.cs:	does things like this.UpdateBackgroundSoundForPlayerPosition(newPosition);
	
	// These are objects that are instantiated from a Prefab.
	public Config config;
	private AudioManager audioManager;
	private BackgroundManager backgroundManager;

	private SlowTimeManager slowTimeManager;
	private Transform player;
	new private Transform camera;

	// These are all the events that this class sends. They should always return void.

	public delegate void _EnemyKilled(GameObject enemyKilled);
	public event _EnemyKilled OnEnemyKilled;

	public delegate void _TimeChanged(float timeScale);
	public event _TimeChanged OnTimeChanged;

	public delegate void _DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition);
	public event _DragChanged OnDragStart;
	public event _DragChanged OnDragUpdate;
	public event _DragChanged OnDragEnd;

	public delegate void _PlayerMoved(Vector2 newPosition, Vector2 velocity);
	public event _PlayerMoved OnPlayerMoved;
	public delegate void _PlayerDied(Transform KilledBy);
	public event _PlayerDied OnPlayerDied;

	public override void Init () {
		//instantiate the config
		GameObject configObject = Instantiate(Resources.Load("Prefabs/Config") as GameObject);
		this.config = configObject.GetComponent<Config>();

		GameObject audioManagerObject = Instantiate(Resources.Load("Prefabs/AudioManager") as GameObject);
		this.audioManager = audioManagerObject.GetComponent<AudioManager>();

		GameObject backgroundManagerObject = Instantiate(Resources.Load("Prefabs/Background") as GameObject);
		this.backgroundManager = audioManagerObject.GetComponent<BackgroundManager>();
	}

	// Assigns. Objects register themselves with the container on Awake, so that the container can access them.
	public void AssignCamera(Transform camera) {
		this.camera = camera;
	}
	public void AssignPlayer(Transform player) {
		this.player = player;
	}
	public void AssignSlowTimeManager(SlowTimeManager slowTimeManager) {
		this.slowTimeManager = slowTimeManager;
	}
	public void AssignConfig(Config config) {
		this.config = config;
	}

	// Deassigns. Call this when an object should die.
	public void RemoveCamera(Transform camera) {
		this.camera = null;
	}
	public void RemovePlayer(Transform player) {
		this.player = null;
	}

	public void RemoveSlowTimeManager(SlowTimeManager slowTimeManager) {
		this.slowTimeManager = null;
	}

	public void RemoveConfig(Config config) {
		this.config = null;
	}

	// Gets. These must not have side effects. Write a function that returns exactly what you need. 
	public Vector2 GetPlayerPosition() {
		return player.position;
	}
	public Vector2 GetCameraPosition() {
		return camera.position;
	}

	// These functions are called by objects.
	public void TimeChanged(float timeScale) {
		//DIT MAG ECHT ALLEEN HIER FAGGOTS
		if(this.OnTimeChanged != null) {
			this.OnTimeChanged(timeScale);
		}
	}

	public void DoPlayerKill(Transform killedBy) {
		this.OnPlayerDied(killedBy);
	}

	public void PlayerMoved(Vector2 position, Vector2 velocity) {
		this.OnPlayerMoved(position, velocity);
	}

	public void DragStart(Vector2 dragPosition) {
		this.OnDragStart(dragPosition, player.position, camera.position);
	}
	public void DragUpdate(Vector2 dragPosition) {
		this.OnDragUpdate(dragPosition, player.position, camera.position);
	}
	public void DragRelease(Vector2 dragPosition) {
		this.OnDragEnd(dragPosition, player.position, camera.position);
	}

	public void EnemyKilled(GameObject enemyKilled){
		this.OnEnemyKilled(enemyKilled);
	}
}
