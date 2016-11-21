using UnityEngine;
using System.Collections;

public class YouMustBeBelowThisHeightToDie : MonoBehaviour {

	[SerializeField] private float yHeightToDie = -80f;
	private bool died = false;
	
	// Update is called once per frame
	void Update () {
		if(died) return;
		if(transform.position.y < yHeightToDie) {
			died = true;
			Container.instance.DoPlayerKill(transform);
		}
	}
}
