using UnityEngine;
using System.Collections;

public class PanController : LGMonoBehaviour {

	struct PanDir {
		public static int Left = -1;
		public static int Right = 1;
	}

	public Transform lookTarget;
	public float panPercent;
	public float panTime;

	void Start () {
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
	}

	void OnPlayerStatsLoaded () {
		lookTarget = player.transform;
	}
	
	void Update () {
		if (lookTarget != null) {
			DetectInput();
		}
	}

	void DetectInput () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			Pan(PanDir.Left);
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			Pan(PanDir.Right);
		}
	}

	void Pan (int dir) {
		iTween.RotateBy(gameObject, new Vector3(0f, 0f, dir * panPercent), panTime);
	}

}
