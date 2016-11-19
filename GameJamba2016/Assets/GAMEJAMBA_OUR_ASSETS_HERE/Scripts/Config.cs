using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public float catapultForce = 18000f;

	private void OnDestroy() {
        Container.instance.RemoveConfig(this);
    }
}
