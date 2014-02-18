using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	public UILabel versionLabel;

	void Start () {
		versionLabel.text = LG.version;
	}

	// Update is called once per frame
	public void OnStartGameButton () {
		Application.LoadLevel(1);
	}
}
