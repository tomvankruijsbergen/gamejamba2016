﻿using UnityEngine;
using System.Collections;

public class CatapultGraphics : MonoBehaviour {

	private float catapultForce;
	[SerializeField]
	private SpriteRenderer spriteRenderer; 

	private Plane plane;
	private Ray ray;
	private float distance;
	private Vector3 point;

	private float maxDragDistance = 5f;

	private bool weDraggin = false;

	private Vector2 dragPositionHeadBand;

	private Rigidbody2D myRigidBody;

	void Awake(){
		plane = new Plane(Vector3.forward, Vector3.zero);
		catapultForce = Container.instance.config.catapultForce;

		Container.instance.OnDragStart += this.DragStart;
		Container.instance.OnDragEnd += this.DragEnd;
		Container.instance.OnDragUpdate += this.DragChanged;

		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
	}

	void DragStart(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		weDraggin = true;
	}

	void Update(){
		if(!weDraggin){

			var dir = myRigidBody.velocity - (Vector2)transform.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			if(angle < -90 || angle > 90){
				spriteRenderer.flipY =true;
			}else{
				spriteRenderer.flipY =false;
			}
		}
	}

	void DragEnd(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		
		weDraggin = false;
	}

	void DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(plane.Raycast(ray, out distance)) {
			point = ray.GetPoint(distance);
			dragPositionHeadBand = point;
		}

		Vector2 diff = dragPositionHeadBand -  (Vector2)transform.position;

		diff.Normalize();
     	float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		if(rot_z < -90 || rot_z > 90){
			spriteRenderer.flipY =false;
		}else{
			spriteRenderer.flipY =true;
		}
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);

	}
}
