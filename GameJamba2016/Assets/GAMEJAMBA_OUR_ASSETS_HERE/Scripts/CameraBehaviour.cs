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
	private float zoomPerRemainingDistance = 1;

	[SerializeField]
	new private Camera camera;

	// Use this for initialization
	void Awake () {
		Container.instance.AssignCamera(transform);

		Container.instance.OnDragStart += this.OnDragStart;
		Container.instance.OnDragEnd += this.OnDragEnd;

		Container.instance.OnPlayerMoved += this.OnPlayerMoved;
	}

	void Start() {
		transform.position = Container.instance.GetPlayerPosition();
	}

	private float previousValue = 0;
	void OnDragStart(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
		iTween.Stop(gameObject);
		iTween.ValueTo(gameObject, iTween.Hash(
            "from", previousValue,
            "to", 1,
            "onupdate", "TweenedZoomValue",
            "time", 0.35f
        ));
	}
	void OnDragEnd(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
		iTween.Stop(gameObject);
		iTween.ValueTo(gameObject, iTween.Hash(
            "from", this.previousValue,
            "to", 0,
            "onupdate", "TweenedZoomValue",
            "time", 0.35f
        ));
	}

    public void TweenedZoomValue(float value) {
		this.previousValue = value;
        this.camera.orthographicSize = zoomBase + zoomBase * value;
    }

	void OnPlayerMoved(Vector2 playerPosition, Vector2 velocity) {
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
		
		//float remainingDistance = Vector2.Distance(newPosition, position);
		//this.camera.orthographicSize = zoomBase + remainingDistance * zoomPerRemainingDistance;

		transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
	}
	
	void OnDestroy() {
		Container.instance.OnDragStart -= this.OnDragStart;
		Container.instance.OnDragEnd -= this.OnDragEnd;

		Container.instance.RemoveCamera(transform);
	}
}
