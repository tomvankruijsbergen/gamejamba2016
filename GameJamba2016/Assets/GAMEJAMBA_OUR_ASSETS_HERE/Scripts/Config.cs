using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public float catapultForce = 1000f;
	public float maxPullMagnitude = 1f;
	public float minPullMagnitude = 0f;

	private void OnDestroy() {
        Container.instance.RemoveConfig(this);
    }
}
