using UnityEngine;
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

	private float maxDragDistance = 5f;

	public int jumpCount = 2;

	void Awake(){
		plane = new Plane(Vector3.forward, Vector3.zero);
		catapultForce = Container.instance.config.catapultForce;
		myRigidbody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	void OnMouseDown() 
	{
		if(jumpCount>0){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(plane.Raycast(ray, out distance)) {
				point = ray.GetPoint(distance);
				if(Vector2.Distance(point, transform.position) <= maxDragDistance){
					mouseDownPos = Input.mousePosition;
					Container.instance.DragStart(mouseDownPos);
				}
			}
		}
	}

	void Update(){
		if(mouseDownPos != Vector2.zero && jumpCount >0){
			Container.instance.DragUpdate(Input.mousePosition);
		}
	}
	
	void OnMouseUp() 
	{
		
		if(jumpCount>0){
			StartCoroutine(Launch());
			jumpCount --;
		}
	}

	void OnCollisionEnter2D(){
		ResetJumpCount();
	}

	public void ResetJumpCount(){
		jumpCount = 2;
	}

	private IEnumerator Launch(){
		yield return new WaitForSeconds(0.002f);

		if(mouseDownPos != Vector2.zero){
			mouseUpPos = Input.mousePosition;
			var direction = mouseDownPos - mouseUpPos;
			// Debug.Log(direction.magnitude);
			Vector3.ClampMagnitude(direction, Container.instance.config.maxPullMagnitude);
			myRigidbody.velocity = Vector3.zero;
			myRigidbody.AddForce(direction * catapultForce);
		}

		mouseDownPos = Vector2.zero;
		Container.instance.DragRelease(mouseUpPos);
	}
}
