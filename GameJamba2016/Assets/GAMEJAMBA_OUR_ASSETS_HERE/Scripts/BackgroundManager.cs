using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct BackgroundSingleData {
	public Material material;
	public Vector2 parallax;
	public float yOffset;
}

public struct BackgroundSingle {
	public GameObject gameObject;
	public BackgroundSingleData data;
}

public class BackgroundManager : MonoBehaviour {

	public BackgroundSingleData[] backgrounds;

	private List<BackgroundSingle> activeBackgrounds = new List<BackgroundSingle>();

	private Vector3 initialScale;
	
	void Awake() {
		for (int i = 0; i < this.backgrounds.Length; i++) {
			BackgroundSingleData backgroundData = backgrounds[i];

			BackgroundSingle single = new BackgroundSingle();

			single.data = backgroundData;

			GameObject activeBackground = Instantiate(Resources.Load("Prefabs/BackgroundSingle") as GameObject);
			activeBackground.transform.parent = transform;
			
			int zIndex = (this.backgrounds.Length - i);
			if (single.data.parallax.x <= 1) {
				//zIndex *= -1;
			}
			activeBackground.GetComponent<MeshRenderer>().material = single.data.material;
			activeBackground.transform.position = new Vector3(0, 0, (this.backgrounds.Length - i) * 0.01f);

			single.gameObject = activeBackground;

			activeBackgrounds.Add(single);
		}

		Container.instance.OnCameraMoved += this.OnCameraMoved;
		Container.instance.OnCameraZoomed += this.OnCameraZoomed;
	}

	void Start() {
		float zoom = Container.instance.GetCameraSize();
		this.OnCameraZoomed(zoom);
		this.initialScale = this.GetLocalScaleForZoom(zoom);
	}

	void OnCameraMoved(Vector2 newPosition) {
		foreach (BackgroundSingle backgroundSingle in this.activeBackgrounds) {
			backgroundSingle.gameObject.transform.position = new Vector3(
				newPosition.x, 
				newPosition.y + backgroundSingle.data.yOffset,
				backgroundSingle.gameObject.transform.position.z
			);

			Material material = backgroundSingle.gameObject.GetComponent<MeshRenderer>().material;
			Vector3 scale = backgroundSingle.gameObject.transform.localScale;
			Vector2 mainTextureOffset = new Vector2(
			 	(newPosition.x * backgroundSingle.data.parallax.x) / scale.x,
			 	(newPosition.y * backgroundSingle.data.parallax.y) / scale.y
			);
			// Beweeg de coordinates 0.5 * het absolute verschil tussen scale en initialscale gedeeld door max
			Vector2 additions = new Vector2(
				((scale.x - initialScale.x) / scale.x) * backgroundSingle.data.parallax.x * (newPosition.x / initialScale.x),
				((scale.y - initialScale.y) / scale.y) * backgroundSingle.data.parallax.y
			);
			
			mainTextureOffset += additions;
			material.mainTextureOffset = mainTextureOffset;
		} 
	}
	void OnCameraZoomed(float newZoom) {
		Vector2 newScale = this.GetLocalScaleForZoom(newZoom);

		foreach (BackgroundSingle backgroundSingle in this.activeBackgrounds) {
			backgroundSingle.gameObject.transform.localScale = new Vector3(newScale.x, newScale.y, 1);
		}
	}
	Vector2 GetLocalScaleForZoom(float zoom) {
		float aspect = Container.instance.GetCameraAspect();
		float width = 2f * zoom;
		float height = width * aspect;

		return new Vector2(height, width);
	}

	void Destroy() {
		Container.instance.OnCameraMoved -= this.OnCameraMoved;
		Container.instance.OnCameraZoomed -= this.OnCameraZoomed;
    }
}
