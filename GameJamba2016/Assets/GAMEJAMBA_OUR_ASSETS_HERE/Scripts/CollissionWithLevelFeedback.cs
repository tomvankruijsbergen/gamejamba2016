using UnityEngine;
using System.Collections;

public class CollissionWithLevelFeedback : MonoBehaviour {

	[SerializeField] private Transform particleSpawnPosition;
	[SerializeField] private GameObject particlePrefab;
	[SerializeField] private LayerMask levelLayer;
	[SerializeField] private float delayBetweenParticeSpawns = 1f;
	[SerializeField] private float nextAvailableSpawnTime = 0f;

	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(levelLayer == 1 << collision.gameObject.layer) {
			DoTheFuckinFade();
			Container.instance.DoPlayerLevelCollide(collision);
		}
	}
	
	// Do the fuckin fade
	void DoTheFuckinFade() {
		if(nextAvailableSpawnTime < Time.time) {
			Instantiate(particlePrefab, particleSpawnPosition.position, particleSpawnPosition.rotation);
			nextAvailableSpawnTime = Time.time + delayBetweenParticeSpawns;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
