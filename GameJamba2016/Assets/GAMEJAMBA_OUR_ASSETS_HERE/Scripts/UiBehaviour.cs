using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiBehaviour : MonoBehaviour {

	[SerializeField] private Text scoreText;
	[SerializeField] private GameObject winText;
	[SerializeField] private GameObject lostText;
	[SerializeField] private GameObject panel;

	private float lastScore;
	public void Awake() {
		Container.instance.OnEnemyKilled += this.OnEnemyKilled;
		Container.instance.OnScoreChanged += this.OnScoreChanged;
		Container.instance.OnKillStreakChanged += this.OnKillStreakChanged;

		Container.instance.OnBossKilled += this.OnBossKilled;
		Container.instance.OnPlayerDied += this.OnPlayerDied;
		panel.SetActive(false);
		lostText.SetActive(false);
		winText.SetActive(false);
		this.OnScoreChanged(0);
	}

	void OnScoreChanged(float newScore){	
		scoreText.text = "" + newScore;
		//this.lastScore = newScore;
	}
	void OnKillStreakChanged(float streakAmount){	
		// Debug.Log(streakAmount);
		this.lastScore = streakAmount;
	}
	void OnEnemyKilled(GameObject enemyKilled){
		GameObject slave = Instantiate(Resources.Load("Prefabs/UiKill") as GameObject);
		slave.transform.position = enemyKilled.transform.position;
		UiSlave uiSlave = slave.GetComponent<UiSlave>();
		uiSlave.InitWithText("" + this.lastScore);
	}

	void OnPlayerDied(Transform dontCare) {
		lostText.SetActive(true);
		panel.SetActive(true);
	}

	void OnBossKilled() {
		winText.SetActive(true);
		panel.SetActive(true);
	}

	public void OnDestroy() {
		Container.instance.OnEnemyKilled -= this.OnEnemyKilled;
		Container.instance.OnScoreChanged -= this.OnScoreChanged;
		Container.instance.OnKillStreakChanged -= this.OnKillStreakChanged;

		Container.instance.OnBossKilled -= this.OnBossKilled;
	}
}
