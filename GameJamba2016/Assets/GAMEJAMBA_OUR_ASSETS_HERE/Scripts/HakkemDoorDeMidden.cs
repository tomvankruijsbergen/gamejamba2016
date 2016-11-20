using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySpriteCutter;

public class HakkemDoorDeMidden : MonoBehaviour {

	[SerializeField]
	private Object[] removeDezeKankerScriptsAlsIkIemandKapotMaakAUB;

	[SerializeField]
	private LayerMask enemyLayers;

	[SerializeField]	private float areaOfAttack;

	Collider2D[] enemiesToBeHakkedDoorDeMidden;

	[SerializeField]	private SpriteRenderer drawSword;
	[SerializeField]	private SpriteRenderer hakSword;

	Rigidbody2D myRigidBody;

	[SerializeField]	private float forceDelay;

	private GameObject bloodBurstParticles;
	private GameObject spriteBurst;
	private bool checking = true;
	private List<GameObject> enemiesIHaveHittedAndDidNotLandInBetween = new List<GameObject> {};

	void Awake(){
		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
		bloodBurstParticles = Resources.Load("Prefabs/BloodBurst") as GameObject;
		spriteBurst = Resources.Load("Prefabs/SpriteSplat") as GameObject;
		Container.instance.OnPlayerDied += PlayerKilled;
		Container.instance.OnEnemyHit += EnemyHit;
		Container.instance.OnDragEnd += ClearEnemiesThisDrag;
	}

	private void PlayerKilled(Transform byWhom) {
		checking = false;
		if(this == null) {
			return;
		}
		this.Hakkem(gameObject, byWhom.transform.position, transform.position, false);
		gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
	}

	private void ClearEnemiesThisDrag(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
		enemiesIHaveHittedAndDidNotLandInBetween = new List<GameObject> {};
	}

	// private void OnDrawGizmos()
	// {
	// 	Gizmos.color = new Color(1, 0, 0, 0.3F);
	// 	Gizmos.DrawSphere(transform.position, areaOfAttack);
	// }

	void LateUpdate () {
		if(!checking) {
			return;
		}
		enemiesToBeHakkedDoorDeMidden = Physics2D.OverlapCircleAll(transform.position, areaOfAttack, enemyLayers,0,99);
		foreach(Collider2D enemyCollider in enemiesToBeHakkedDoorDeMidden){
			Health potentialHealthComponent = enemyCollider.gameObject.GetComponent<Health>();
			if(enemiesIHaveHittedAndDidNotLandInBetween.IndexOf(enemyCollider.gameObject) == -1) {
				if(potentialHealthComponent != null) {
					potentialHealthComponent.health--;
				}
				StartCoroutine(SwordAnimations());
				if(
					(potentialHealthComponent == null) || 
					(potentialHealthComponent != null && potentialHealthComponent.health == 0)
				){
					enemyCollider.gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
					Vector2 slashDirection = enemyCollider.transform.position;
					Hakkem(enemyCollider.gameObject,transform.position, slashDirection);
					Container.instance.EnemyKilled(enemyCollider.gameObject);
				} else {
					Container.instance.DoEnemyHit(transform);
				}
			}
			enemiesIHaveHittedAndDidNotLandInBetween.Add(enemyCollider.gameObject);
		}
	}

	private void Hakkem(GameObject enemyToBeHakkedDoorDeMidden, Vector2 slashStart, Vector2 slashEnd, bool isEnemy = true){
		//remove potentials script the ugly way
		for(int i = 0; i < removeDezeKankerScriptsAlsIkIemandKapotMaakAUB.Length; i++) {
			//TODO: haal kanker scripts van kanker object af kanker
			var potentialRemove = enemyToBeHakkedDoorDeMidden.GetComponent(removeDezeKankerScriptsAlsIkIemandKapotMaakAUB[i].GetType().ToString());
			if(potentialRemove != null) {
				Destroy(potentialRemove);
			}
		}
		SpriteCutterOutput output = SpriteCutter.Cut( new SpriteCutterInput() {
			lineStart = slashStart,
			lineEnd = slashEnd,
			gameObject = enemyToBeHakkedDoorDeMidden,
			gameObjectCreationMode = isEnemy ? SpriteCutterInput.GameObjectCreationMode.CUT_OFF_COPY : SpriteCutterInput.GameObjectCreationMode.CUT_INTO_TWO,
		} );
		if(isEnemy) {
			gameObject.GetComponent<DragCatapultMovement>().ResetJumpCount();
			StartCoroutine(DelayedForce(output, slashStart, slashEnd));
			
		} else {
			Destroy(gameObject);
			StartCoroutine(KillMyself(output, slashStart, slashEnd));
		}
	}

	private void EnemyHit(Transform hitBy) {
		bool hasEnemyPatrol = hitBy.gameObject.GetComponent<EnemyPatrol>() != null;
		if(hasEnemyPatrol) {
			//do specefiek shit
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

	private IEnumerator SwordAnimations(){
		drawSword.enabled = true;
		yield return new WaitForSeconds(0.1f);
		drawSword.enabled = false;
		hakSword.enabled = true;
		yield return new WaitForSeconds(0.2f);
		hakSword.enabled = false;
	}

	private IEnumerator DelayedForce(SpriteCutterOutput output, Vector2 slashStart, Vector2 slashEnd){
		//fix the position of the second part
		output.secondSideGameObject.transform.parent = output.firstSideGameObject.transform.parent;
		output.secondSideGameObject.transform.position = output.firstSideGameObject.transform.position;

		yield return new WaitForSeconds(forceDelay);

		GameObject particles1 = GameObject.Instantiate( spriteBurst, output.firstSideGameObject.transform.position, Quaternion.identity) as GameObject;
		GameObject particles2 = GameObject.Instantiate( spriteBurst, output.secondSideGameObject.transform.position, Quaternion.identity) as GameObject;
		GameObject particles3 = GameObject.Instantiate( bloodBurstParticles, output.firstSideGameObject.transform.position, Quaternion.identity) as GameObject;
		particles3.transform.parent = output.firstSideGameObject.transform;
		GameObject particles4 = GameObject.Instantiate( bloodBurstParticles, output.secondSideGameObject.transform.position, Quaternion.identity) as GameObject;
		particles4.transform.parent = output.secondSideGameObject.transform;

		particles1.transform.Rotate(new Vector3(-90,0,0));
		particles2.transform.Rotate(new Vector3(90,0,0));

		particles3.transform.Rotate(new Vector3(-90,0,0));
		particles4.transform.Rotate(new Vector3(90,0,0));

		Vector2 distance = output.firstSideGameObject.transform.position - output.secondSideGameObject.transform.position;

		float angle = Mathf.Atan2(distance.y, distance.x);
		angle += 0.5f * Mathf.PI;

		Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 9991; // je force

		Rigidbody2D rbdy1 = output.firstSideGameObject.GetComponent<Rigidbody2D>();
		Rigidbody2D rbdy2 = output.secondSideGameObject.GetComponent<Rigidbody2D>();

		rbdy1.isKinematic = false;
		rbdy2.isKinematic = false;

		rbdy1.AddForceAtPosition(-force, slashStart);
		rbdy2.AddForceAtPosition(force, slashEnd);

		rbdy1.AddTorque(9001);
		rbdy2.AddTorque(9001);
	}

	void OnDestroy() {
		Container.instance.OnPlayerDied -= PlayerKilled;
		Container.instance.OnEnemyHit   -= EnemyHit;
		Container.instance.OnDragEnd  -= ClearEnemiesThisDrag;
	}
}
