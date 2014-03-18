using UnityEngine;
using System.Collections;

public class MobClient : LGMonoBehaviour {

	DamageDisplay damageDisplay;
	LabelDisplay labelDisplay;


	public enum State {
		Alive,
		Dead
	}

	State _currentState = State.Alive;
	public State currentState {
		get {
			return _currentState;
		}
		set {
			_currentState = value;
			StateChange();
		}
	}

	void StateChange () {
		if (currentState == State.Dead) {
			labelDisplay.label.text = "Debris";
		}
	}

	void Start () {
		damageDisplay = GetComponent<DamageDisplay>();
		labelDisplay = GetComponent<LabelDisplay>();
	}

	public static string Client_TakeDamage = "TakeDamage";
	[RPC] void TakeDamage (float amount) {
		damageDisplay.DisplayDamage(amount);
	}

	public static string Client_Die = "Die";
	[RPC] void Die () {
		currentState = State.Dead;
		damageDisplay.DisplayDeath();

		WeaponTarget weaponTarget = GetComponent<WeaponTarget>();
		Destroy (weaponTarget);
	}

	void OnDestroy () {

	}

}
