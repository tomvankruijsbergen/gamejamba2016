using UnityEngine;
using System.Collections;

public class EnemySwordControl : MonoBehaviour {

	[SerializeField] private GameObject klik;
	[SerializeField] private GameObject hak;
	[SerializeField] private float delay = .2f;
	[SerializeField] private bool hideKlik = true;

	// Use this for initialization
	void Awake () {
		
	}

	void OnTriggerStay2D(Collider2D other) {

	}
	
	public void DoIt() {

	}
}
