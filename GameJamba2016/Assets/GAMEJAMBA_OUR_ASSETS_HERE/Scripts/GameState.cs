using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	private float score = 0;
	private float killsThisJump = 0;

	public void Awake() {
		Container.instance.OnPlayerCollidedWithLevel += this.OnPlayerCollission;
		Container.instance.OnEnemyKilled += this.OnEnemyKilled;
	}
	void Start() {
		Container.instance.ScoreChanged(0);  
	}

	void OnPlayerCollission(Collision2D colliderDieIkNIetGebruikKankerLekkerOp) {
		killsThisJump = 0;
	}

	void OnEnemyKilled(GameObject enemyKilled){
		score += 1;
		killsThisJump += 1;

		Container.instance.ScoreChanged(score);
		Container.instance.KillStreakChanged(killsThisJump);
	}

	public void OnDestroy() {
		Container.instance.OnPlayerCollidedWithLevel -= this.OnPlayerCollission;
		Container.instance.OnEnemyKilled += this.OnEnemyKilled;
	}
}
