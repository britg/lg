using UnityEngine;
using System.Collections;

public class ServerConnectionUI : MonoBehaviour {

	public GameObject panel;

	void Awake () {
		NotificationCenter.AddObserver(this, LG.n_showServerConnectionUI);
		NotificationCenter.AddObserver(this, LG.n_hideServerConnectionUI);
	}

	// Use this for initialization
	void Start () {

	}

	void OnConnectedToServer () {
		panel.SetActive(false);
	}

	void OnServerInitialized () {
		panel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnShowServerConnectionUI () {
		panel.SetActive(true);
	}

	void OnHideServerConnectionUI () {
		panel.SetActive(false);
	}
}
