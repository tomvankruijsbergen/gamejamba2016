using UnityEngine;
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

	private Vector2 worldMousePos;

	[SerializeField]
	GameObject headBandForStretch; 

	[SerializeField]
	GameObject headBandEnd;

	private Rigidbody2D myRigidBody;
	private Vector2 lastPosition = Vector2.zero;

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
			if(lastPosition == Vector2.zero) {
				lastPosition = transform.position;
				return;
			}
			var dir = (Vector2)transform.position - lastPosition;
			if(dir.magnitude < 0.03f) {
				if(spriteRenderer.flipY) {
					transform.rotation = Quaternion.AngleAxis(-180f, Vector3.forward);
				} else {
					transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
				}
			} else {
				var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				if(angle < -90 || angle > 90){
					spriteRenderer.flipY =true;
				}else{
					spriteRenderer.flipY =false;
				}
			}
			lastPosition = transform.position;
		} else {
			lastPosition = Vector2.zero;
		}
	}

	void DragEnd(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		// headBandForStretch.transform.localScale = Vector3.zero;
		iTween.ScaleTo(headBandForStretch, iTween.Hash(
			"x", 0,
            "y", 1,
            "z", 1,
            "easetype", "easeInQuint",
			"time",0.005f
        ));

		headBandEnd.transform.localPosition = Vector3.zero;
		
		weDraggin = false;
	}

	void DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(plane.Raycast(ray, out distance)) {
			point = ray.GetPoint(distance);
			worldMousePos = point;
			headBandEnd.transform.position = point;

		}

		Vector2 diff = worldMousePos - (Vector2)transform.position;
		
		Vector2 diffNotNormalised = diff;
		
		diff.Normalize();
     	float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		if(rot_z < -90 || rot_z > 90){
			spriteRenderer.flipY =false;
			headBandForStretch.transform.localScale = new Vector2(-diffNotNormalised.x,1);
			// headBandEnd.transform.localPosition = new Vector2(diffNotNormalised.x,1);
			
		}else{
			spriteRenderer.flipY =true;
			headBandForStretch.transform.localScale = new Vector2(diffNotNormalised.x,1);
			// headBandEnd.transform.localPosition = new Vector2(-diffNotNormalised.x,1);
		}
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);
	}
}
