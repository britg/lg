using UnityEngine;
using System.Collections;

public class Framerate : MonoBehaviour {

	public int framerate = 60;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = framerate;
		Screen.SetResolution(1440, 900, false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
