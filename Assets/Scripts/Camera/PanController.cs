using UnityEngine;
using System.Collections;

public class PanController : ControllerBehaviour {

	struct PanDir {
		public static int Left = -1;
		public static int Right = 1;
	}

	public Transform lookTarget;
	public float panPercent;
	public float panTime;

	protected override void ControllerStart () {
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
	}

	void OnPlayerStatsLoaded () {
		lookTarget = thePlayer().transform;
	}

	protected override void OnPanLeftDown () {
		Pan(PanDir.Left);
	}

	protected override void OnPanRightDown () {
		Pan(PanDir.Right);
	}

	void Pan (int dir) {
		iTween.RotateBy(gameObject, new Vector3(0f, 0f, dir * panPercent), panTime);
	}

}
