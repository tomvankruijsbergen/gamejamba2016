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

	private LineRenderer lineRenderer;
	[SerializeField]
	private GameObject headBandEnd;

	private Rigidbody2D myRigidBody;
	private Vector2 lastPosition = Vector2.zero;

	void Awake(){
		plane = new Plane(Vector3.forward, Vector3.zero);
		catapultForce = Container.instance.config.catapultForce;

		Container.instance.OnDragStart += this.DragStart;
		Container.instance.OnDragEnd += this.DragEnd;
		Container.instance.OnDragUpdate += this.DragChanged;

		lineRenderer = gameObject.GetComponent<LineRenderer>();

		myRigidBody = gameObject.GetComponent<Rigidbody2D>();
	}

	void DragStart(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		if(Container.instance.GetJumpsLeft() > 0){
			weDraggin = true;
		}
	}

	void Update(){
		if(!weDraggin){
			if(lastPosition == Vector2.zero) {
				lastPosition = transform.position;
				return;
			}
			var dir = (Vector2)transform.position - lastPosition;
			if(dir.magnitude < 0.05f) {
				if(spriteRenderer.transform.localScale.y < 0) {
					transform.rotation = Quaternion.AngleAxis(-180f, Vector3.forward);
				} else {
					transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
				}
			} else {
				var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				if(angle < -90 || angle > 90){
					// spriteRenderer.flipY =true;
					spriteRenderer.transform.localScale = new Vector2(spriteRenderer.transform.localScale.x, -1);
				}else{
					// spriteRenderer.flipY =false;
					spriteRenderer.transform.localScale = new Vector2(spriteRenderer.transform.localScale.x, 1);
				}
			}
			lastPosition = transform.position;
			lineRenderer.SetPosition(1,transform.position  - transform.right * 2.5f);
			headBandEnd.transform.position = transform.position  - transform.right * 2.5f;
		} else {
			lastPosition = Vector2.zero;
			
		}
		lineRenderer.SetPosition(0,transform.position  - transform.right * 2.5f);
		
	}

	void DragEnd(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		if(weDraggin){
			lineRenderer.SetPosition(1,transform.position  - transform.right * 2.5f);
			headBandEnd.transform.position = transform.position  - transform.right * 2.5f;
			weDraggin = false;
		}
		// Container.instance
	}

	void DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		if(weDraggin){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(plane.Raycast(ray, out distance)) {
				point = ray.GetPoint(distance);
				worldMousePos = point;
				lineRenderer.SetPosition(1,point);
				headBandEnd.transform.position = worldMousePos;
			}

			Vector2 diff = worldMousePos - (Vector2)transform.position;

			
			Vector2 diffNotNormalised = diff;
			
			diff.Normalize();
			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			if(rot_z < -90 || rot_z > 90){
				// spriteRenderer.flipY =false;
				spriteRenderer.transform.localScale = new Vector2(spriteRenderer.transform.localScale.x, 1);
			}else{
				// spriteRenderer.flipY =true;
				spriteRenderer.transform.localScale = new Vector2(spriteRenderer.transform.localScale.x, -1);
			}
			transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);
		}
	}
}
