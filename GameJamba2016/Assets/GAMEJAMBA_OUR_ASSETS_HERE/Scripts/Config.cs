using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public float catapultForce = 18000f;

	// Use this for initialization
	void Awake () {
		Container.instance.AssignConfig(this);
	}
}
