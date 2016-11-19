using UnityEngine;
using System.Collections;

public class Container : MonoSingleton<Container> {

		Vector3 mMouseDownPos;
	Vector3 mMouseUpPos;
	public float speed = .1f;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	void OnMouseDown() 
	{
		mMouseDownPos = Input.mousePosition;
		Debug.Log( "the mouse down pos is " + mMouseDownPos.y.ToString() );
		mMouseDownPos = Input.mousePosition;
		Debug.Log( "the mouse down pos is " + mMouseDownPos.z.ToString() );
		mMouseDownPos.z = 0;
	}
	
	void OnMouseUp() 
	{
		mMouseUpPos = Input.mousePosition;
		mMouseUpPos = Input.mousePosition;
		mMouseUpPos.z = 0;
		var direction = mMouseDownPos - mMouseUpPos;
		direction.Normalize();
		GetComponent<Rigidbody>().AddForce (direction * speed);
		Debug.Log( "the mouse up pos is " + mMouseUpPos.ToString() );
	}
}
