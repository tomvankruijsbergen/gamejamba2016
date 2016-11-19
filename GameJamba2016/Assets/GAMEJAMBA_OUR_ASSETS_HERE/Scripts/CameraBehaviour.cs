using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private float snapDistance = 0.1f;

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

		float speedPerTime = speed * Time.deltaTime;

		Vector2 difference = new Vector2(
			playerPosition.x - transform.position.x, 
			playerPosition.y - transform.position.y
		);

		// Don't do anything if we're on the object.
		if (difference.sqrMagnitude == 0) {
			return;	
		}

		// Now either interpolate or snap
		Vector2 newPosition;
		if (difference.magnitude < this.snapDistance) {
			newPosition = playerPosition;
		} else {
			newPosition = new Vector2(
				transform.position.x + speedPerTime * difference.x,
				transform.position.y + speedPerTime * difference.y
			);
		}

		transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
	}
	
	void OnDestroy() {
		Container.instance.OnDragStart -= this.OnDragChanged;
		Container.instance.OnDragEnd -= this.OnDragChanged;
		Container.instance.OnDragUpdate -= this.OnDragChanged;

		Container.instance.RemoveCamera(transform);
	}
}
