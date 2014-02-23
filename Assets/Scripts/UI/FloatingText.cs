using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

	public float lifeTime = 1f;

	// Use this for initialization
	void Start () {
		Invoke("DestroySelf", lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DestroySelf () {
		Destroy (gameObject);
	}
}
