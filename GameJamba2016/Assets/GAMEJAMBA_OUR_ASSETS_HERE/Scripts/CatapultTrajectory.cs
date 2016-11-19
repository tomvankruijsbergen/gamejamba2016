using UnityEngine;
using System.Collections;



public class CatapultTrajectory : MonoBehaviour {

	private float catapultForce;

	void Awake(){
		Container.instance.OnDragStart += this.DragStart;
		Container.instance.OnDragEnd += this.DragEnd;
		Container.instance.OnDragUpdate += this.DragChanged;
		this.catapultForce = Container.instance.config.catapultForce;
	}

	void CalculateTraject(float aimAngle){
		// Make estimation of trajectory with LineRenderer
		
		var vSquared = (catapultForce * catapultForce);
		var sinAngle = Mathf.Sin(2 * aimAngle);
		
		//var distanceTraveled = vSquared * sinAngle * (1 / Physics.gravity);
	}

	void DragStart(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		// Make trajectory visible
		
		
		 var angle = Vector2.Angle(dragPosition, playerPosition);
		 this.CalculateTraject(angle);
		// //direction.Normalize();
		// myRigidbody.DrawForce (direction * force);
	}

	void DragEnd(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		// Hide trajectory
	}

	void DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		// Adjust Trajectory
		var angle = dragPosition - playerPosition;
	}
}
