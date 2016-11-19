using UnityEngine;
using System.Collections;

public class DragCatapultMovement : MonoBehaviour {

	private Vector2 mouseDownPos;
	private Vector2 mouseUpPos;
	private Rigidbody2D myRigidbody;

	[SerializeField]
	private float speed = 500f;

	void Awake(){
		myRigidbody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	void OnMouseDown() 
	{
		mouseDownPos = Input.mousePosition;
	}
	
	void OnMouseUp() 
	{
		mouseUpPos = Input.mousePosition;
		var direction = mouseDownPos - mouseUpPos;
		direction.Normalize();
		myRigidbody.AddForce (direction * speed);
	}
}
