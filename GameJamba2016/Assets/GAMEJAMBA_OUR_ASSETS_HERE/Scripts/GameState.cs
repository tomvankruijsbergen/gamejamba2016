using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	private float score = 0;
	private float killsThisJump = 0;

	public void Awake() {
		Container.instance.OnDragEnd += this.OnDragEnd;
		Container.instance.OnEnemyKilled += this.OnEnemyKilled;
	}
	void Start() {
		Container.instance.ScoreChanged(0); 
	}

	void OnDragEnd(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
		this.killsThisJump = 0;
	}
	void OnEnemyKilled(GameObject enemyKilled){
		score += 1;
		killsThisJump += 1;

		Container.instance.ScoreChanged(score);
		if (this.killsThisJump > 1) {
			Container.instance.KillStreakChanged(killsThisJump);
		}
	}

	public void OnDestroy() {
		Container.instance.OnDragEnd -= this.OnDragEnd;
		Container.instance.OnEnemyKilled += this.OnEnemyKilled;
	}
}
