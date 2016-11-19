using UnityEngine;

public class PlayerEvents : MonoBehaviour {

	void Awake(){
		Container.instance.AssignPlayer(transform);
	}

	void LateUpdate() {
		// Todo: Check if the player actually moved.
		Container.instance.PlayerMoved(transform.position);
	}

	void OnDestroy() {
		Container.instance.RemovePlayer(transform);
	}
}
