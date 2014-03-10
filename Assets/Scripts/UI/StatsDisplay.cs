using UnityEngine;
using System.Collections;

public class StatsDisplay : DisplayBehaviour {

	public float refreshRate = 0.2f;

	public UILabel pos;
	public UILabel ammo;
	public UILabel shields;
	public UILabel hull;
	public UILabel fuel;

	public UILabel tier1;
	public UILabel tier2;
	public UILabel tier3;
	public UILabel tier4;
	public UILabel tier5;
	public UILabel tier6;
	public UILabel tier7;

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
		UpdateResources();
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

	void UpdateResources () {
		tier1.text = player.resource(Resource.tier1).ToString();
		tier2.text = player.resource(Resource.tier2).ToString();
		tier3.text = player.resource(Resource.tier3).ToString();
		tier4.text = player.resource(Resource.tier4).ToString();
		tier5.text = player.resource(Resource.tier5).ToString();
		tier6.text = player.resource(Resource.tier6).ToString();
	}
}
