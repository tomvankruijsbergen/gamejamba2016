using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct BackgroundSingleData {
	public Sprite sprite;
	public float parallax;
	public float y;
	public float scale;
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

			GameObject activeBackground = new GameObject();
			activeBackground.transform.parent = transform;
			SpriteRenderer spriteRenderer = activeBackground.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = single.data.sprite;
			
			int zIndex = (this.backgrounds.Length - i);
			if (single.data.parallax <= 1) {
				zIndex *= -1;
			}
			activeBackground.transform.position = new Vector3(0, 0, (this.backgrounds.Length - i));
			activeBackground.transform.localScale = new Vector3(single.data.scale, single.data.scale, single.data.scale);

			single.gameObject = activeBackground;

			activeBackgrounds.Add(single);
		}
		

		Container.instance.OnPlayerMoved += this.OnPlayerMoved;
	}

	void OnPlayerMoved(Vector2 position, Vector2 velocity) {
		// move the parallax in each background layer
		foreach (BackgroundSingle backgroundSingle in this.activeBackgrounds) {
			Vector2 newTarget = position * backgroundSingle.data.parallax;
			newTarget.y += backgroundSingle.data.y;
			Vector3 newPosition = new Vector3(newTarget.x, newTarget.y, backgroundSingle.gameObject.transform.position.z);
			backgroundSingle.gameObject.transform.position = newPosition;
		}
	}

	void Destroy() {
        Container.instance.OnPlayerMoved -= this.OnPlayerMoved;
    }
}
