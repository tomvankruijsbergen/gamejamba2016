using UnityEngine;
using System.Collections;

public class Container : MonoSingleton<Container> {

		
	public delegate void _OnTest(bool b);
	public _OnTest OnTest;

	//private AudioManager audioManager;

	void Start () {
		//this.OnTest += (bool b) => {};
		this.OnTest(true);
		this.Update();
	}


	void Update () {
		
	}
}
