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
	
	private AudioManager audioManager;
	private SlowTimeManager slowTimeManager;
	private Transform player;
	new private Transform camera;

	// These are all the events that this class sends. They should always return void.
	public delegate void _AudioChanged(string test);
	public event _AudioChanged AudioChanged;


	public delegate void _EnemyKilled(GameObject part1, GameObject part2, Vector2 slashStart, Vector2 slashEnd);
	public event _EnemyKilled OnEnemyKilled;

	public delegate void _DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition);
	public event _DragChanged OnDragStart;
	public event _DragChanged OnDragUpdate;
	public event _DragChanged OnDragEnd;

	public delegate void _PlayerMoved(Vector2 newPosition);
	public event _PlayerMoved OnPlayerMoved;

	public override void Init () {
		//
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

	// Deassigns. Call this when an object should die.
	public void RemoveCamera(Transform camera) {
		this.camera = null;
	}
	public void RemovePlayer(Transform player) {
		this.player = null;
	}

	// Gets. These must not have side effects. Write a function that returns exactly what you need. 
	public Vector2 getPlayerPosition() {
		return player.position;
	}
	public Vector2 getCameraPosition() {
		return camera.position;
	}

	// These functions are called by objects.

	public void PlayerMoved(Vector2 position) {
		this.OnPlayerMoved(position);
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

	public void EnemyKilled(GameObject part1, GameObject part2, Vector2 slashStart, Vector2 slashEnd){
		// this.OnEnemyKilled(part1, part2, slashStart,slashEnd);
	}
}
