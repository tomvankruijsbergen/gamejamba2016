using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour {

	public void DOIT() {
		Destroy(Container.instance);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
