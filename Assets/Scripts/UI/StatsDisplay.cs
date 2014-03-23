using UnityEngine;
using System.Collections;

public class StatsDisplay : DisplayBehaviour {

	public float refreshRate = 0.2f;

	public UILabel pos;
	public UILabel ammo;
	public UILabel shields;
	public UILabel hull;
	public UILabel fuel;
	
	void Awake () {
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	}

	void OnPlayerStatsLoaded () {
		InvokeRepeating("UpdateDisplay", refreshRate, refreshRate);
	}

	void uLink_OnDisconnectedFromServer () {
		CancelInvoke();
	}

	void UpdateDisplay () {
		UpdatePos ();
		UpdateAmmo ();
		UpdateShields();
		UpdateHull ();
		UpdateFuel();
	}

	void UpdatePos () {
		pos.text = "Pos: " + player.transform.position.ToString();
	}

	void UpdateAmmo () {
		ammo.text = "Ammo: " + player.stat (Stat.ammo);
	}

	void UpdateShields () {
		shields.text = "Shields: " + player.stat (Stat.shields);
	}

	void UpdateHull () {
		hull.text = "Hull: " + player.stat(Stat.hull);
	}

	void UpdateFuel () {
		fuel.text = "Fuel: " + player.stat(Stat.fuel).ToString("F2");
	}
}
