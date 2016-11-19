using UnityEngine;
using System.Collections;

public class HakkemDoorDeMidden : MonoBehaviour {

	[SerializeField]
	private LayerMask enemyLayers;

	[SerializeField]
	private float areaOfAttack;

	Collider2D[] enemiesToBeHakkedDoorDeMidden;

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.3F);
		Gizmos.DrawSphere(transform.position, areaOfAttack);
	}

	void LateUpdate () {
		enemiesToBeHakkedDoorDeMidden = Physics2D.OverlapCircleAll(transform.position, areaOfAttack, enemyLayers,0,99);

		foreach(Collider2D enemyCollider in enemiesToBeHakkedDoorDeMidden){
			Debug.Log("HAKKEM");
			enemyCollider.gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
			// enemyCollider.enabled = false;
		}
	}
}
