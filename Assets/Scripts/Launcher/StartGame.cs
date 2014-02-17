using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Update is called once per frame
	public void OnStartGameButton () {
		Application.LoadLevel(1);
	}
}
