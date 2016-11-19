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

	[SerializeField]
	private float forceDelay;

	private GameObject bloodBurstParticles;

	void Awake(){
		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
		bloodBurstParticles = Resources.Load("Prefabs/BloodBurst") as GameObject;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.3F);
		Gizmos.DrawSphere(transform.position, areaOfAttack);
	}

	void LateUpdate () {
		enemiesToBeHakkedDoorDeMidden = Physics2D.OverlapCircleAll(transform.position, areaOfAttack, enemyLayers,0,99);

		foreach(Collider2D enemyCollider in enemiesToBeHakkedDoorDeMidden){
			enemyCollider.gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
			
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
		Debug.Log("heu");
		StartCoroutine(DelayedForce(output, slashStart, slashEnd));

		Container.instance.EnemyKilled(output.firstSideGameObject, output.secondSideGameObject, slashStart,slashEnd );
	}

	private IEnumerator DelayedForce(SpriteCutterOutput output, Vector2 slashStart, Vector2 slashEnd){
		
		Debug.Log("werwer");

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
