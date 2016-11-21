using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

	[SerializeField] private Transform patrolPointA;
	[SerializeField] private Transform patrolPointB;
	[SerializeField] private float hopSpeed = 8f;
	[SerializeField] private float patrolDuration = 5f;
	[SerializeField] private float maxHopHeight = .5f;
	private float baseScaleX;

	private float baseY;
	private float startTime;

	void Awake() {
		baseScaleX = transform.localScale.x;
		startTime = Time.time;
		baseY = transform.position.y;
		if(patrolPointA.position.y != patrolPointB.position.y) {
			Debug.Log("Is the enemy really walking on a slope faggot???");
		}
		Container.instance.OnEnemyKilled += CheckIfIGotKilled;
	}

	public void CheckIfIGotKilled(GameObject isThisMe) {
		if(isThisMe == gameObject) {
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update () {
		float deltaTime = Time.time - startTime;
		//value for going to B
		float distancePercentage = (deltaTime % patrolDuration) / patrolDuration;
		
		//Are we going to A ???
		// Debug.Log(deltaTime % patrolDuration * 2f);
		if(deltaTime % patrolDuration * 2f < patrolDuration) {
			//morph value for to A
			distancePercentage = 1f - distancePercentage;
		}
		distancePercentage = Mathf.Pow(distancePercentage, 3f);
		// do the actual X positioning
		Vector3 newPosition = Vector3.Lerp(patrolPointA.position, patrolPointB.position, distancePercentage);
		if(newPosition.x > transform.position.x) {
			transform.localScale = new Vector3(-baseScaleX, transform.localScale.y, transform.localScale.z);
		} else {
			transform.localScale = new Vector3(baseScaleX, transform.localScale.y, transform.localScale.z);
		}
		transform.position = newPosition;

		//now do the y
		transform.position = new Vector3(transform.position.x, baseY + Mathf.Abs(Mathf.Sin(deltaTime * hopSpeed)) *  maxHopHeight);
	}

	void OnDestroy() {
		Container.instance.OnEnemyKilled -= CheckIfIGotKilled;
	}
}
