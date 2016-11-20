using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiSlave : MonoBehaviour {

	[SerializeField] private Text uiText;
	[SerializeField] private float duration;
	
	private Vector2 movementOverTime;
	private Vector3 target;
	private Vector3 moveVelocity;

	public void InitWithText(string text) {
		uiText.text = text;
		// init blood effects and stuff here
		this.Invoke("Remove", duration);
		this.target = transform.position + new Vector3(0,10,0);
	}

	void Update() {
		transform.position = Vector3.SmoothDamp(transform.position, target, ref moveVelocity, duration);
	}

	void Remove() {
		Destroy(gameObject);
	}
}
