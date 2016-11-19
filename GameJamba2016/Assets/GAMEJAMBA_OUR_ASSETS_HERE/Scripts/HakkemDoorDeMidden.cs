using UnityEngine;
using System.Collections;
using UnitySpriteCutter;

public class HakkemDoorDeMidden : MonoBehaviour {

	[SerializeField]
	private LayerMask enemyLayers;

	[SerializeField]
	private float areaOfAttack;

	Collider2D[] enemiesToBeHakkedDoorDeMidden;

	Rigidbody2D myRigidBody;

	void Awake(){
		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
	}

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
			// Container.instance.EnemyKilled(enemyCollider.gameObject, transform.position, transform.InverseTransformDirection(myRigidBody.velocity));
			// Vector2 slashDirection =  transform.InverseTransformDirection(myRigidBody.velocity).normalized * 100;
			Vector2 slashDirection = enemyCollider.transform.position;
			Hakkem(enemyCollider.gameObject,transform.position, slashDirection);
		}
	}

	private void Hakkem(GameObject enemyToBeHakkedDoorDeMidden, Vector2 slashStart, Vector2 slashEnd){

		SpriteCutterOutput output = SpriteCutter.Cut( new SpriteCutterInput() {
			lineStart = slashStart,
			lineEnd = slashEnd,
			gameObject = enemyToBeHakkedDoorDeMidden,
			gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_OFF_COPY,
		} );

		// Vector2 distance = slashEnd - slashStart;
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
