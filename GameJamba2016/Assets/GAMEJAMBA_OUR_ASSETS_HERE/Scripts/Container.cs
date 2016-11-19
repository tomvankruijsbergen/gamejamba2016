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

	// These are all the events that this class sends. They should always return void.
	public delegate void _AudioChanged(string test);
	public event _AudioChanged AudioChanged;

	// public delegate void 

	public override void Init () {
		this.audioManager = new AudioManager();	
	}



	// These functions are called by objects.
	
	public void PlayerMoved(Vector3 newPosition) {
		// Tell the audio manager that the background music should change.
		this.AudioChanged("the audio has changed to something");
	}

	public void DragStart(Vector2 position) {
		
	}
	public void DragUpdate(Vector2 position) {

	}
	public void DragRelease(Vector2 position) {

	}
}
