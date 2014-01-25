using UnityEngine;
using System.Collections;

public class PlayerAttachCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TopFollow topFollow = Camera.main.GetComponent<TopFollow>();
		topFollow.player = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
