using UnityEngine;
using System.Collections;

public class EnemySwordControl : MonoBehaviour {

	[SerializeField] private GameObject klik;
	[SerializeField] private GameObject hak;
	[SerializeField] private float hakInterval = 3f;
	[SerializeField] private float klikHakdelay = .2f;
	[SerializeField] private float hakTime = .4f;
	[SerializeField] private bool alwaysShowClick = true;

	private float lastHak = 0f;
	private bool didKill = false;
	private bool isHacking = false;

	// Use this for initialization
	void Awake () {
		lastHak = Time.time;
		Container.instance.OnEnemyKilled += MyMasterDied;
		hak.SetActive(false);
		klik.SetActive(alwaysShowClick);
	}

	void MyMasterDied(GameObject isThisMyMaster) {
		if(isThisMyMaster == transform.parent.gameObject) {
			Destroy(gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if(!didKill && isHacking) {
			if(other.gameObject.tag == "Player") {
				Container.instance.DoPlayerKill(transform);
				didKill = true;
			}
		}
	}

	void OnDestroy() {
		Container.instance.OnEnemyKilled -= MyMasterDied;
	}
	
	void Update() {
		float timeToKlik = lastHak + hakInterval;
		float timeToHak = timeToKlik + klikHakdelay;
		float timeToStop = timeToHak + hakTime;

		if(timeToStop < Time.time) {
			klik.SetActive(alwaysShowClick);
			hak.SetActive(false);
			isHacking = false;
			lastHak = Time.time;
			return;
		} 

		if(timeToHak < Time.time) {
			hak.SetActive(true);
			klik.SetActive(false);
			isHacking = true;
			return;
		}

		if(timeToKlik < Time.time) {
			klik.SetActive(true);
			hak.SetActive(false);
			isHacking = false;
			return;
		}
	}
}
