using UnityEngine;
using System.Collections;

public class ServerConnectionGUI : MonoBehaviour {

	public GameObject UIRoot;

	void Awake () {
		NotificationCenter.AddObserver(this, LG.n_showServerConnectionUI);
		NotificationCenter.AddObserver(this, LG.n_hideServerConnectionUI);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnShowServerConnectionUI () {
		UIRoot.SetActive(true);
	}

	void OnHideServerConnectionUI () {
		UIRoot.SetActive(false);
	}
}
