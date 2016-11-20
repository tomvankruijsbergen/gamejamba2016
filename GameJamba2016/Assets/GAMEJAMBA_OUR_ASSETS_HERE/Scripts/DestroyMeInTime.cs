using UnityEngine;
using System.Collections;

public class DestroyMeInTime : MonoBehaviour {

	// this many time to destroy myself
	[SerializeField] private float timeBeforeDestroy = .5f;
	private float destroyTime;

	// Use this for initialization
	void Start () {
		destroyTime = Time.time + timeBeforeDestroy;
	}
	
	// Update is called once per frame
	void Update () {
		if(destroyTime < Time.time) {
			Destroy(gameObject);
		}
	}
}
