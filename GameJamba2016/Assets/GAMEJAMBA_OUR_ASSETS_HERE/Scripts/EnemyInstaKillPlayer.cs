using UnityEngine;
using System.Collections;

public class EnemyInstaKillPlayer : MonoBehaviour {

	[SerializeField]
	private LayerMask playerMask;

	void OnTriggerEnter2D(Collider2D collider) {
		if(playerMask == 1 << collider.gameObject.layer) {
			Container.instance.DoPlayerKill(transform);
		}
	}
}
