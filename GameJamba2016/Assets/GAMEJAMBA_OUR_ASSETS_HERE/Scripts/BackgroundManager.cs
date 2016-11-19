using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct BackgroundSingleData {
	public Material material;
	public Vector3 parallax;
	public float yOffset;
}

public struct BackgroundSingle {
	public GameObject gameObject;
	public BackgroundSingleData data;
}

public class BackgroundManager : MonoBehaviour {

	public BackgroundSingleData[] backgrounds;

	private List<BackgroundSingle> activeBackgrounds = new List<BackgroundSingle>();
	
	void Awake() {
		for (int i = 0; i < this.backgrounds.Length; i++) {
			BackgroundSingleData backgroundData = backgrounds[i];

			BackgroundSingle single = new BackgroundSingle();

			single.data = backgroundData;

			GameObject activeBackground = Instantiate(Resources.Load("Prefabs/BackgroundSingle") as GameObject);
			activeBackground.transform.parent = transform;
			
			int zIndex = (this.backgrounds.Length - i);
			if (single.data.parallax.x <= 1) {
				zIndex *= -1;
			}
			activeBackground.GetComponent<MeshRenderer>().material = single.data.material;
			activeBackground.transform.position = new Vector3(0, 0, (this.backgrounds.Length - i) * 0.01f);
			//activeBackground.transform.localScale = new Vector3(single.data.scale, single.data.scale, single.data.scale);

			single.gameObject = activeBackground;

			activeBackgrounds.Add(single);
		}
		

		Container.instance.OnPlayerMoved += this.OnPlayerMoved;
		Container.instance.OnCameraMoved += this.OnCameraMoved;
		Container.instance.OnCameraZoomed += this.OnCameraZoomed;
	}

	void Start() {
		this.OnCameraZoomed(Container.instance.GetCameraSize());
	}

	void OnCameraMoved(Vector2 newPosition) {
		foreach (BackgroundSingle backgroundSingle in this.activeBackgrounds) {
			backgroundSingle.gameObject.transform.position = new Vector3(
				newPosition.x, 
				newPosition.y + backgroundSingle.data.yOffset,
				backgroundSingle.gameObject.transform.position.z
			);

			Material material = backgroundSingle.gameObject.GetComponent<MeshRenderer>().material;
			material.mainTextureOffset = new Vector2(
				newPosition.x * backgroundSingle.data.parallax.x / backgroundSingle.gameObject.transform.localScale.x,
				newPosition.y * backgroundSingle.data.parallax.y / backgroundSingle.gameObject.transform.localScale.y
			);
		} 
	}
	void OnCameraZoomed(float newZoom) {
		float aspect = Container.instance.GetCameraAspect();
		float width = 2f * newZoom;
		float height = width * aspect;

		foreach (BackgroundSingle backgroundSingle in this.activeBackgrounds) {
			backgroundSingle.gameObject.transform.localScale = new Vector3(height, width, 1);
		}
	}

	void OnPlayerMoved(Vector2 position, Vector2 velocity) {
		// move the parallax in each background layer
		/*
		foreach (BackgroundSingle backgroundSingle in this.activeBackgrounds) {
			Vector2 newTarget = position * backgroundSingle.data.parallax;
			newTarget.y += backgroundSingle.data.y;
			Vector3 newPosition = new Vector3(newTarget.x, newTarget.y, backgroundSingle.gameObject.transform.position.z);
			backgroundSingle.gameObject.transform.position = newPosition;
		}
		*/
	}

	void Destroy() {
        Container.instance.OnPlayerMoved -= this.OnPlayerMoved;
		Container.instance.OnCameraMoved -= this.OnCameraMoved;
		Container.instance.OnCameraZoomed -= this.OnCameraZoomed;
    }
}
