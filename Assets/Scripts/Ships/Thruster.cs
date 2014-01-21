using UnityEngine;
using System.Collections;

public class Thruster : MonoBehaviour {

	public ParticleSystem particles;
	public PlayerMove playerMovement;

	// Use this for initialization
	void Start () {
		particles = gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
