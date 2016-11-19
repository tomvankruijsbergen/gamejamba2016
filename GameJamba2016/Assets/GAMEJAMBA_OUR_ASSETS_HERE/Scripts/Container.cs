using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Container : MonoSingleton<Container> {

	// All messages between objects should go through this class.
	// An object always calls this class, and this class then sends events.
	// Example flow:
	// - Player.cs:  		sends Container.instance.playerMoved(this.transform.position);
	// - Container.cs: 		sends this.AudioChanged("something");
	// - AudioManager.cs:	does things like this.UpdateBackgroundSoundForPlayerPosition(newPosition);

	private AudioManager audioManager;

	// These are all the events that this class sends.
	public delegate void _AudioChanged(string test);
	public event _AudioChanged AudioChanged;

	public override void Init () {
		this.audioManager = new AudioManager();	
	}

	// These functions are called by objects.
	
	public void playerMoved(Vector3 newPosition) {
		// Tell the audio manager that the background music should change.
		this.AudioChanged("the audio has changed to something");
	}
}
