using UnityEngine;

public class PlayerInitialise : MonoBehaviour {

	void Awake(){
		Container.instance.AssignPlayer(transform);
	}
}
