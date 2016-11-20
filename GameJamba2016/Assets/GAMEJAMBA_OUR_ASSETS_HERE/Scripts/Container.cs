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
	new private CameraBehaviour camera;

	// These are all the events that this class sends. They should always return void.

	public delegate void _EnemyKilled(GameObject enemyKilled);
	public event _EnemyKilled OnEnemyKilled;

	public delegate void _EnemyHit(Transform hitBy);
	public event _EnemyHit OnEnemyHit;

	public delegate void _TimeChanged(float timeScale);
	public event _TimeChanged OnTimeChanged;

	public delegate void _DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition);
	public event _DragChanged OnDragStart;
	public event _DragChanged OnDragUpdate;
	public event _DragChanged OnDragEnd;
	public event _DragChanged OnDragIncrement;

	public delegate void _PlayerMoved(Vector2 newPosition, Vector2 velocity);
	public event _PlayerMoved OnPlayerMoved;
	public delegate void _PlayerDied(Transform KilledBy);
	public event _PlayerDied OnPlayerDied;

	public delegate void _PlayerCollisionWithLevel(Collision2D collision);
	public event _PlayerCollisionWithLevel OnPlayerCollidedWithLevel;

	public delegate void _CameraMoved(Vector2 newPosition);
	public event _CameraMoved OnCameraMoved;

	public delegate void _CameraZoomed(float newZoom);
	public event _CameraZoomed OnCameraZoomed;

	public delegate void _ScoreChanged(float newScore);
	public event _ScoreChanged OnScoreChanged;
	public delegate void _KillStreakChanged(float streakAmount);
	public event _KillStreakChanged OnKillStreakChanged;

	public delegate void _BossKilled();
	public event _BossKilled OnBossKilled;

	public override void Init () {
		//instantiate the config
		GameObject configObject = Instantiate(Resources.Load("Prefabs/Config") as GameObject);
		this.config = configObject.GetComponent<Config>();

		GameObject audioManagerObject = Instantiate(Resources.Load("Prefabs/AudioManager") as GameObject);
		this.audioManager = audioManagerObject.GetComponent<AudioManager>();

		// GameObject backgroundManagerObject = Instantiate(Resources.Load("Prefabs/Background") as GameObject);
		// this.backgroundManager = audioManagerObject.GetComponent<BackgroundManager>();
	}

	// Assigns. Objects register themselves with the container on Awake, so that the container can access them.
	public void AssignCamera(CameraBehaviour cameraBehaviour) {
		this.camera = cameraBehaviour;
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
	public int GetJumpsLeft() {
		return player.GetComponent<DragCatapultMovement>().jumpCount;
	}
	
	public Vector2 GetCameraPosition() {
		return camera.transform.position;
	}
	public float GetCameraSize() {
		return camera.GetCameraSize();
	}
	public float GetCameraAspect() {
		return camera.GetCameraAspect();
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
	
	public void DoEnemyHit(Transform hitBy) {
		this.OnEnemyHit(hitBy);
	}

	public void PlayerMoved(Vector2 position, Vector2 velocity) {
		this.OnPlayerMoved(position, velocity);
	}

	public void CameraMoved(Vector2 position) {
		this.OnCameraMoved(position);
	}
	public void CameraZoomed(float newZoom) {
		this.OnCameraZoomed(newZoom);
	}

	public void DragStart(Vector2 dragPosition) {
		this.OnDragStart(dragPosition, player.position, camera.transform.position);
	}
	public void DragUpdate(Vector2 dragPosition) {
		this.OnDragUpdate(dragPosition, player.position, camera.transform.position);
	}
	public void DragRelease(Vector2 dragPosition) {
		this.OnDragEnd(dragPosition, player.position, camera.transform.position);
	}
	public void DragIncrement(Vector2 dragPosition){
		this.OnDragIncrement(dragPosition, player.position, camera.transform.position);
	}

	public void EnemyKilled(GameObject enemyKilled){
		this.OnEnemyKilled(enemyKilled);

		if (enemyKilled.GetComponent<EnemyBoss>() != null) {
			this.OnBossKilled();
		}
	}


	public void DoPlayerLevelCollide(Collision2D collider) {
		this.OnPlayerCollidedWithLevel(collider);
	}

	public void ScoreChanged(float newScore) {
		this.OnScoreChanged(newScore);
	}
	public void KillStreakChanged(float streakAmount) {
		this.OnKillStreakChanged(streakAmount);
	}
}
