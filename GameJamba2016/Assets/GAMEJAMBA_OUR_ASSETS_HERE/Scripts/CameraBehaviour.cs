using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private float snapDistance = 0.1f;

	[SerializeField]
	private float zoomBase = 9;
	[SerializeField]
	private float zoomPerDistance = 10;

	[SerializeField]
	new private Camera camera;

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

		float speedPerTime = this.speed * Time.deltaTime;

		Vector2 position = new Vector2(transform.position.x, transform.position.y);
		Vector2 difference = new Vector2(
			playerPosition.x - position.x, 
			playerPosition.y - position.y
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
				position.x + speedPerTime * difference.x,
				position.y + speedPerTime * difference.y
			);
		}
		
		float movedDistance = Vector2.Distance(newPosition, position);
		this.camera.orthographicSize = zoomBase + movedDistance * zoomPerDistance;

		transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
	}
	
	void OnDestroy() {
		Container.instance.OnDragStart -= this.OnDragChanged;
		Container.instance.OnDragEnd -= this.OnDragChanged;
		Container.instance.OnDragUpdate -= this.OnDragChanged;

		Container.instance.RemoveCamera(transform);
	}
}
