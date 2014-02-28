using UnityEngine;
using System.Collections;

public class StatsDisplay : LGMonoBehaviour {

	public UILabel pos;
	public UILabel ammo;
	public UILabel shields;
	public UILabel hull;
	public UILabel fuel;
	public UILabel elements;

	private GameObject player;

	void Awake () {
		NotificationCenter.AddObserver(this, LG.n_playerLoaded);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	}

	void OnPlayerLoaded (Notification note) {
		Hashtable data = note.data;
		player = (GameObject)data[LG.n_playerKey];
		playerAttributes = player.GetComponent<PlayerAttributes>();
		InvokeRepeating("UpdateDisplay", 0.1f, 0.1f);
	}

	void UpdateDisplay () {
		UpdatePos ();
		UpdateAmmo ();
		UpdateShields();
		UpdateHull ();
		UpdateFuel();
		UpdateElements();
	}

	void uLink_OnDisconnectedFromServer () {
		CancelInvoke();
	}

	void UpdatePos () {
		pos.text = "Pos: " + player.transform.position.ToString();
	}

	void UpdateAmmo () {
		ammo.text = "Ammo: " + playerAttributes.weaponAttributes.ammo;
	}

	void UpdateShields () {
		shields.text = "Shields: " + playerAttributes.shipAttributes.shields;
	}

	void UpdateHull () {
		hull.text = "Hull: " + playerAttributes.shipAttributes.hull;
	}

	void UpdateFuel () {
		fuel.text = "Fuel: " + playerAttributes.shipAttributes.fuel.ToString("F2");
	}

	void UpdateElements () {
	}
}
