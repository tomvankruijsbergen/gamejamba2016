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
	private Transform player;
	new private Transform camera;

	// These are all the events that this class sends. They should always return void.
	public delegate void _AudioChanged(string test);
	public event _AudioChanged AudioChanged;

	public delegate void _DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition);
	public event _DragChanged OnDragStart;
	public event _DragChanged OnDragUpdate;
	public event _DragChanged OnDragEnd;

	public override void Init () {
		this.audioManager = new AudioManager();
	}

	// Assigns. Objects register themselves with the container on Awake, so that the container can access them.
	
	public void AssignCamera(Transform camera) {
		this.camera = camera;
	}
	public void AssignPlayer(Transform player) {
		this.player = player;
	}

	// Deassigns. Call this when an object should die.

	public void RemoveCamera(Transform camera) {
		this.camera = null;
	}
	public void RemovePlayer(Transform player) {
		this.player = null;
	}

	// These functions are called by objects.

	public void PlayerMoved(Vector3 newPosition) {
		// Tell the audio manager that the background music should change.
		this.AudioChanged("the audio has changed to something");
	}

	public void DragStart(Vector2 position) {
		this.OnDragStart(position, player.position, camera.position);
	}
	public void DragUpdate(Vector2 position) {
		this.OnDragUpdate(position, player.position, camera.position);
	}
	public void DragRelease(Vector2 position) {
		this.OnDragEnd(position, player.position, camera.position);
	}
}
