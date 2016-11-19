using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Container.instance.AssignCamera(transform);

		Container.instance.OnDragStart += this.OnDragChanged;
		Container.instance.OnDragEnd += this.OnDragChanged;
		Container.instance.OnDragUpdate += this.OnDragChanged;

		Container.instance.OnPlayerMoved += this.OnPlayerMoved;
	}
	void OnDragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
		// Debug.Log("changed");
	}

	void OnPlayerMoved(Vector2 playerPosition) {
		// Interpolate to the player.
		transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
	}
	
	void OnDestroy() {
		Container.instance.OnDragStart -= this.OnDragChanged;
		Container.instance.OnDragEnd -= this.OnDragChanged;
		Container.instance.OnDragUpdate -= this.OnDragChanged;

		Container.instance.RemoveCamera(transform);
	}
}
