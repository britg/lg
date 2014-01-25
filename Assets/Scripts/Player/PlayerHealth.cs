using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public TextMesh display;
	private int currentCount = 100;

	// Use this for initialization
	void Start () {
		StartTimer();
	}
	
	// Update is called once per frame
	void Update () {
		display.text = "" + currentCount;
	}

	void StartTimer () {
		InvokeRepeating("CountDown", 1f, 1f);
	}

	[RPC] void CountDown () {
		currentCount -= 1;
	}
}
