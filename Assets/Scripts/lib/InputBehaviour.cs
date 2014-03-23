using UnityEngine;
using System.Collections;

public class InputBehaviour : uLink.MonoBehaviour {

	protected enum InputContext {
		Game,
		Menu
	}

	protected InputContext context = InputContext.Game;

	protected bool acceptInput = true;
	protected bool panLeftDown {
		get {
			return (Input.GetKeyDown(KeyCode.Q));
		}
	}
	
	protected bool panRightDown {
		get {
			return (Input.GetKeyDown(KeyCode.E));
		}
	}

	protected bool menuDown {
		get {
			return Input.GetKeyDown(KeyCode.Escape);
		}
	}


	void Update () {

		if (context == InputContext.Game) {
			AnnounceInput();
			DetectInput();
		} else if (context == InputContext.Menu) {
			AnnounceMenuInput();
			DetectMenuInput();
		}

		InputUpdate();
	}

	protected virtual void DetectInput () {}
	protected virtual void DetectMenuInput () {}
	protected virtual void InputUpdate() {}

	protected virtual void OnPanLeftDown () {}
	protected virtual void OnPanRightDown () {}
	protected virtual void OnMenuDown () {}

	protected virtual void OnMenuPanLeftDown () {}
	protected virtual void OnMenuPanRightDown () {}
	protected virtual void OnMenuMenuDown () {}

	void AnnounceInput () {
		if (panLeftDown) {
			OnPanLeftDown();
		}

		if (panRightDown) {
			OnPanRightDown();
		}

		if (menuDown) {
			context = InputContext.Menu;
			OnMenuDown();
		}
	}

	void AnnounceMenuInput () {
		if (panLeftDown) {
			OnMenuPanLeftDown();
		}

		if (panRightDown) {
			OnMenuPanRightDown();
		}

		if (menuDown) {
			context = InputContext.Game;
			OnMenuMenuDown();
		}
	}

}
