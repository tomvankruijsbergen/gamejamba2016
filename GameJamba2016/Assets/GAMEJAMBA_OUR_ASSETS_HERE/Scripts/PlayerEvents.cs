using UnityEngine;

public class PlayerEvents : MonoBehaviour {

	void Awake(){
		Container.instance.AssignPlayer(transform);
		previousPosition = transform.position;
	}

	private Vector2 previousPosition;
	void LateUpdate() {
		Vector2 velocity = new Vector2(
			transform.position.x - previousPosition.x,
			transform.position.y - previousPosition.y
		);
		Container.instance.PlayerMoved(transform.position, velocity);

		previousPosition = transform.position;
	}

	void OnDestroy() {
		Container.instance.RemovePlayer(transform);
	}
}
