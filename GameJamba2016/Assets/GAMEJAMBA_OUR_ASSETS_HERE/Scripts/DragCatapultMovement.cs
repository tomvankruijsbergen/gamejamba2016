﻿using UnityEngine;
using System.Collections;

public class DragCatapultMovement : MonoBehaviour {

	private Vector2 mouseDownPos;
	private Vector2 mouseUpPos;
	private Rigidbody2D myRigidbody;

	private float catapultForce;

	private Plane plane;
	private Ray ray;
	private float distance;
	private Vector3 point;

	private float maxDragDistance = 2f;

	void Awake(){
		plane = new Plane(Vector3.forward, Vector3.zero);
		catapultForce = Container.instance.config.catapultForce;
		myRigidbody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	void OnMouseDown() 
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(plane.Raycast(ray, out distance)) {
			point = ray.GetPoint(distance);
			if(Vector2.Distance(point, transform.position) <= maxDragDistance){
				mouseDownPos = Input.mousePosition;
				Container.instance.DragStart(mouseDownPos);
			}
		}
	}

	void Update(){
		if(mouseDownPos != Vector2.zero){
			Container.instance.DragUpdate(Input.mousePosition);
		}
	}
	
	void OnMouseUp() 
	{
		if(mouseDownPos != Vector2.zero){
			mouseUpPos = Input.mousePosition;
			var direction = mouseDownPos - mouseUpPos;
			direction.Normalize();
			myRigidbody.AddForce (direction * force);
		}

		mouseDownPos = Vector2.zero;
		Container.instance.DragRelease(mouseUpPos);
	}
}
