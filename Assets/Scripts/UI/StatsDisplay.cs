using UnityEngine;
using System.Collections;

public class StatsDisplay : LGMonoBehaviour {

	public UILabel pos;
	public UILabel ammo;
	public UILabel shields;
	public UILabel hull;
	public UILabel fuel;
	public UILabel elements;

	void Awake () {
		NotificationCenter.AddObserver(this, LG.n_playerLoaded);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	}

	void OnPlayerLoaded () {
		AssignPlayer();
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
		ammo.text = "Ammo: " + player.weaponAttributes.ammo;
	}

	void UpdateShields () {
		shields.text = "Shields: " + player.shipAttributes.shields;
	}

	void UpdateHull () {
		hull.text = "Hull: " + player.shipAttributes.hull;
	}

	void UpdateFuel () {
		fuel.text = "Fuel: " + player.shipAttributes.fuel.ToString("F2");
	}

	void UpdateElements () {
	}
}
