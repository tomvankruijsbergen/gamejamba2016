using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Container.instance.AssignCamera(transform);
		Container.instance.OnDragStart += this.OnDragChanged;
		Container.instance.OnDragEnd += this.OnDragChanged;
		Container.instance.OnDragUpdate += this.OnDragChanged;
	}
	void OnDragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
		
	}
}
