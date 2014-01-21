using UnityEngine;
using System.Collections;

public class Framerate : MonoBehaviour {

	public int framerate = 60;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = framerate;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
