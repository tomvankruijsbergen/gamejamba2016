﻿using UnityEngine;
using System.Collections;
using UnitySpriteCutter;

public class HakkemDoorDeMidden : MonoBehaviour {

	[SerializeField]
	private LayerMask enemyLayers;

	[SerializeField]
	private float areaOfAttack;

	Collider2D[] enemiesToBeHakkedDoorDeMidden;

	Rigidbody2D myRigidBody;

	[SerializeField]
	private float forceDelay;

	private GameObject bloodBurstParticles;
	private bool checking = true;

	void Awake(){
		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
		bloodBurstParticles = Resources.Load("Prefabs/BloodBurst") as GameObject;
		Container.instance.OnPlayerDied += PlayerKilled;
	}

	private void PlayerKilled(Transform byWhom) {
		checking = false;
		if(this == null) {
			return;
		}
		this.Hakkem(gameObject, byWhom.transform.position, transform.position, false);
		gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.3F);
		Gizmos.DrawSphere(transform.position, areaOfAttack);
	}

	void LateUpdate () {
		if(!checking) {
			return;
		}
		enemiesToBeHakkedDoorDeMidden = Physics2D.OverlapCircleAll(transform.position, areaOfAttack, enemyLayers,0,99);

		foreach(Collider2D enemyCollider in enemiesToBeHakkedDoorDeMidden){
			enemyCollider.gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
			
			Vector2 slashDirection = enemyCollider.transform.position;
			Hakkem(enemyCollider.gameObject,transform.position, slashDirection);
		}
	}

	private void Hakkem(GameObject enemyToBeHakkedDoorDeMidden, Vector2 slashStart, Vector2 slashEnd, bool isEnemy = true){
		SpriteCutterOutput output = SpriteCutter.Cut( new SpriteCutterInput() {
			lineStart = slashStart,
			lineEnd = slashEnd,
			gameObject = enemyToBeHakkedDoorDeMidden,
			gameObjectCreationMode = isEnemy ? SpriteCutterInput.GameObjectCreationMode.CUT_OFF_COPY : SpriteCutterInput.GameObjectCreationMode.CUT_INTO_TWO,
		} );
		if(isEnemy) {
			StartCoroutine(DelayedForce(output, slashStart, slashEnd));
			Container.instance.EnemyKilled();
		} else {
			Destroy(gameObject);
			StartCoroutine(KillMyself(output, slashStart, slashEnd));
		}
	}

	private IEnumerator KillMyself(SpriteCutterOutput output, Vector2 slashStart, Vector2 slashEnd){
		output.firstSideGameObject.AddComponent<Rigidbody2D>();
		output.secondSideGameObject.AddComponent<Rigidbody2D>();

		Rigidbody2D rbdy1 = output.firstSideGameObject.GetComponent<Rigidbody2D>();
		Rigidbody2D rbdy2 = output.secondSideGameObject.GetComponent<Rigidbody2D>();

		Vector2 distance = output.firstSideGameObject.transform.position - output.secondSideGameObject.transform.position;

		float angle = Mathf.Atan2(distance.y, distance.x);
		angle += 0.5f * Mathf.PI;

		Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 9991; // je force

		rbdy1.AddForceAtPosition(-force, slashStart);
		rbdy2.AddForceAtPosition(force, slashEnd);

		rbdy1.AddTorque(9001);
		rbdy2.AddTorque(9001);

		yield return new WaitForSeconds(forceDelay);
	}

	private IEnumerator DelayedForce(SpriteCutterOutput output, Vector2 slashStart, Vector2 slashEnd){
		
		yield return new WaitForSeconds(forceDelay);
		
		GameObject particles1 = GameObject.Instantiate( bloodBurstParticles, output.firstSideGameObject.transform.position, Quaternion.identity) as GameObject;
		particles1.transform.parent = output.firstSideGameObject.transform;
		GameObject particles2 = GameObject.Instantiate( bloodBurstParticles, output.secondSideGameObject.transform.position, Quaternion.identity) as GameObject;
		particles2.transform.parent = output.secondSideGameObject.transform;

		Vector2 distance = output.firstSideGameObject.transform.position - output.secondSideGameObject.transform.position;

		float angle = Mathf.Atan2(distance.y, distance.x);
		angle += 0.5f * Mathf.PI;

		Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 9991; // je force

		Rigidbody2D rbdy1 = output.firstSideGameObject.GetComponent<Rigidbody2D>();
		Rigidbody2D rbdy2 = output.secondSideGameObject.GetComponent<Rigidbody2D>();

		rbdy1.AddForceAtPosition(-force, slashStart);
		rbdy2.AddForceAtPosition(force, slashEnd);

		rbdy1.AddTorque(9001);
		rbdy2.AddTorque(9001);
	}
}
