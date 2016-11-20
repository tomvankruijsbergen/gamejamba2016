using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BackgroundScroller : MonoBehaviour {
	public Vector2 scrollSpeed;
	public List<Sprite> sprites = new List<Sprite>();
	public Vector2 poolSize = new Vector2(3, 3); //We don't do this dynamically because we don't want to allocate memory during runtime
	public bool verticalRepeat = true;

	private Vector3 startPosition;
	private List<List<GameObject>> spriteRows = new List<List<GameObject>>();

	void Start(){
		startPosition = transform.localPosition;

		FillPool ();
	}

	void FillPool(){
		Vector2 spawnPosition = Vector2.zero;

		foreach (Sprite sprite in sprites) {
			int rowsForThisSprite = 1;

			if (sprites.IndexOf (sprite) == sprites.Count - 1) {
				// This is the last sprite, so top up with some buffer rows 
				rowsForThisSprite = (int)poolSize.y;
			}

			for (var idx = 0; idx < rowsForThisSprite; idx++) {
				List<GameObject> spritePool = new List<GameObject> ();
				spriteRows.Add (spritePool);

				for (var i = 0; i < poolSize.x; i++) {
					GameObject spriteObject = new GameObject ();
					SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer> ();

					spriteRenderer.sprite = sprite;
					spriteRenderer.sortingLayerName = "background";
		
					spriteObject.transform.parent = transform;

					// Position the sprite next to the last one in the pool
					spriteObject.transform.localPosition = new Vector2 (
						spawnPosition.x - (sprite.bounds.size.x * .5f * (poolSize.x - 1)) + sprite.bounds.size.x * i,
						spawnPosition.y
					);

					spritePool.Add (spriteObject);

					if (i == poolSize.x - 1) {
						spawnPosition = new Vector2 (
							spawnPosition.x,
							spawnPosition.y + sprite.bounds.size.y
						);
					}
				}
			}
		}
	}

	void FixedUpdate (){
		transform.position = new Vector3 (
			startPosition.x + Camera.main.transform.position.x * scrollSpeed.x * 0.1f,// * Time.deltaTime * 100,
			startPosition.y + Camera.main.transform.position.y * scrollSpeed.y * 0.1f,// * Time.deltaTime * 100,
			startPosition.z
		);

		// Check bounds and see if we need more sprites
		CheckSpritePositions();
	}

	void CheckSpritePositions(){
		if (spriteRows.Count == 0)
			return;

		Bounds cameraBounds = Camera.main.GetOrthographicBounds ();

		if (poolSize.x > 1) {
			foreach (List<GameObject> spritePool in spriteRows) {
				GameObject mostRightSprite = null;
				GameObject mostLeftSprite = null;
				foreach (GameObject sprite in spritePool) {
					if (mostRightSprite == null || sprite.transform.localPosition.x > mostRightSprite.transform.localPosition.x) {
						mostRightSprite = sprite;
					}

					if (mostLeftSprite == null || sprite.transform.localPosition.x < mostLeftSprite.transform.localPosition.x) {
						mostLeftSprite = sprite;
					}
				}

				SpriteRenderer spriteRenderer = mostRightSprite.GetComponent<SpriteRenderer> ();

				if (cameraBounds.max.x > mostRightSprite.GetComponent<SpriteRenderer> ().bounds.max.x) {
					mostLeftSprite.transform.localPosition = new Vector2 (
						mostRightSprite.transform.localPosition.x + spriteRenderer.bounds.size.x,
						mostRightSprite.transform.localPosition.y
					);
				} else if (cameraBounds.min.x < mostLeftSprite.GetComponent<SpriteRenderer> ().bounds.min.x) {
					mostRightSprite.transform.localPosition = new Vector2 (
						mostLeftSprite.transform.localPosition.x - spriteRenderer.bounds.size.x,
						mostLeftSprite.transform.localPosition.y
					);
				}
			}
		}

		if (verticalRepeat && poolSize.y > 1) {
			// Check if we need to move vertically (up)
			List<GameObject> lastRow = spriteRows.Last ();
			if (lastRow.First ().GetComponent<SpriteRenderer> ().bounds.max.y < cameraBounds.max.y) {
				// Find the row to be moved, which depends on spritePool.y
				List<GameObject> rowToBeMoved = spriteRows.ElementAt (spriteRows.Count - (int)poolSize.y);

				foreach (GameObject sprite in rowToBeMoved) {
					sprite.transform.localPosition = new Vector2 (
						sprite.transform.localPosition.x,
						sprite.transform.localPosition.y + poolSize.y * sprite.GetComponent<SpriteRenderer> ().bounds.size.y
					);
				}

				// Re-add the row so it's now last in the list - beautiful
				spriteRows.Remove (rowToBeMoved);
				spriteRows.Add (rowToBeMoved);
			} else {
				// Check if we need to move vertically (down)
				List<GameObject> firstRepeatRow = spriteRows.ElementAt (spriteRows.Count - (int)poolSize.y);
				List<GameObject> lastNotRepeatRow = spriteRows.ElementAt (spriteRows.Count - (int)poolSize.y - 1);

				if (firstRepeatRow.First ().GetComponent<SpriteRenderer> ().bounds.min.y > cameraBounds.min.y &&
				   lastNotRepeatRow.First ().GetComponent<SpriteRenderer> ().bounds.max.y < cameraBounds.min.y) {
					foreach (GameObject sprite in lastRow) {
						sprite.transform.localPosition = new Vector2 (
							sprite.transform.localPosition.x,
							sprite.transform.localPosition.y - poolSize.y * sprite.GetComponent<SpriteRenderer> ().bounds.size.y
						);
					}

					// Re-add the row so it's now last in the list - beautiful
					spriteRows.Remove (lastRow);
					spriteRows.Insert (spriteRows.Count - (int)poolSize.y + 1, lastRow);
				}
			}
		}

	}
				
}