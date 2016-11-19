using UnityEngine;
using System.Collections;

public class CatapultGraphics : MonoBehaviour {

	private float catapultForce;

	// [SerializeField]
	// private GameObject HeadBandGraphics;

	private SpriteRenderer headBandSprite;

	private LineRenderer lineRenderer;

	[SerializeField]
	private Transform headBandStartPosition;

	[SerializeField]
	private Transform headBandEndPosition;

	private Plane plane;
	private Ray ray;
	private float distance;
	private Vector3 point;

	private float maxDragDistance = 5f;

	private bool weDraggin = false;

	private Vector2 dragPositionHeadBand;

	void Awake(){
		plane = new Plane(Vector3.forward, Vector3.zero);
		catapultForce = Container.instance.config.catapultForce;

		Container.instance.OnDragStart += this.DragStart;
		Container.instance.OnDragEnd += this.DragEnd;
		Container.instance.OnDragUpdate += this.DragChanged;

		lineRenderer = gameObject.GetComponent<LineRenderer>();
		
		lineRenderer.SetPosition(0, headBandStartPosition.position);
		lineRenderer.SetPosition(1, headBandEndPosition.position);
	}

	void DragStart(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		weDraggin = true;
	}

	void Update(){
		if(weDraggin){
			lineRenderer.SetPosition(0, headBandStartPosition.position);
			
		}else{
			lineRenderer.SetPosition(0, headBandStartPosition.position);
			lineRenderer.SetPosition(1, headBandEndPosition.position);
		}
	}

	void DragEnd(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		
		weDraggin = false;
		lineRenderer.SetPosition(0, headBandStartPosition.position);
		lineRenderer.SetPosition(1, headBandEndPosition.position);
	}

	void DragChanged(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition){
		
		


		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(plane.Raycast(ray, out distance)) {
			point = ray.GetPoint(distance);

			dragPositionHeadBand = point;
			lineRenderer.SetPosition(1, point);
		}

		Vector2 diff = dragPositionHeadBand -  (Vector2)headBandStartPosition.position;

		diff.Normalize();
 
     	float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);

	}
}
