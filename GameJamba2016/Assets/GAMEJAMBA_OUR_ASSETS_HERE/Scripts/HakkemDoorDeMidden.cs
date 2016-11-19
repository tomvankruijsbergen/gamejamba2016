using UnityEngine;
using System.Collections;

public class HakkemDoorDeMidden : MonoBehaviour {

	[SerializeField]
	private LayerMask enemyLayers;

	[SerializeField]
	private float areaOfAttack;

	Collider2D[] enemiesToBeHakkedDoorDeMidden;

	void LateUpdate () {
		enemiesToBeHakkedDoorDeMidden = Physics2D.OverlapCircleAll(transform.position, areaOfAttack, enemyLayers,0,99);
		foreach(Collider2D enemy in enemiesToBeHakkedDoorDeMidden){
			
		}
	}
}
