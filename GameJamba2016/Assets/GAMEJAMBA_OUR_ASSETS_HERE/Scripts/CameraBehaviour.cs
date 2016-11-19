using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	[SerializeField]
	private float moveSpeed = 1f;

	[SerializeField]
	private float snapDistance = 0.1f;

	[SerializeField]
	private float zoomBase = 9;
	[SerializeField]
	private float zoomMax = 15;
	[SerializeField]
	private float zoomTime = 0.35f;

	private float animationZoomValue;

	private const string AnimationZoom = "AnimationZoom";

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

		// Initialising animation vars
		this.animationZoomValue = 0;
		this.camera.orthographicSize = zoomBase;
	}

	void OnDragStart(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
		iTween.StopByName(CameraBehaviour.AnimationZoom);
		iTween.ValueTo(gameObject, iTween.Hash(
			"name", CameraBehaviour.AnimationZoom,
            "from", animationZoomValue,
            "to", 1,
            "onupdate", "TweenedZoomValue",
            "time", zoomTime
        ));
	}
	void OnDragEnd(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
		iTween.StopByName(CameraBehaviour.AnimationZoom);
		iTween.ValueTo(gameObject, iTween.Hash(
			"name", CameraBehaviour.AnimationZoom,
            "from", this.animationZoomValue,
            "to", 0,
            "onupdate", "TweenedZoomValue",
            "time", zoomTime
        ));
	}

    public void TweenedZoomValue(float value) {
		this.animationZoomValue = value;
        this.camera.orthographicSize = zoomBase + (zoomMax - zoomBase) * value;
    }

	private Vector3 moveVelocity = new Vector3();
	void OnPlayerMoved(Vector2 playerPosition, Vector2 velocity) {
		// Interpolate to the player.

		Vector2 position = new Vector2(transform.position.x, transform.position.y);
		
		Debug.Log(velocity.magnitude);
		Vector2 difference = (playerPosition - position);
		Vector2 extra = velocity.normalized * 12;
		difference += extra;

		Vector2 newTarget = position + difference;

		Vector3 newPosition = new Vector3(newTarget.x, newTarget.y, transform.position.z);

		transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref moveVelocity, 0.6f);
	}
	
	void OnDestroy() {
		Container.instance.OnDragStart -= this.OnDragStart;
		Container.instance.OnDragEnd -= this.OnDragEnd;

		Container.instance.RemoveCamera(transform);
	}
}
