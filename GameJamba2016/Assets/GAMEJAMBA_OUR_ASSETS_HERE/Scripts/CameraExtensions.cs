using UnityEngine;
using System.Collections;

public static class CameraExtensions {

	public static Bounds GetOrthographicBounds(this Camera camera){
		float cameraHeight = camera.orthographicSize * 2;

		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * camera.aspect, cameraHeight, 0)
		);

		return bounds;
	}

}