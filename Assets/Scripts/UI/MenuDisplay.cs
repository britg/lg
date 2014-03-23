using UnityEngine;
using System.Collections;

public class MenuDisplay : DisplayBehaviour {

	BlurEffect _blur;
	BlurEffect blur {
		get {
			if (_blur == null) {
				_blur = Camera.main.GetComponent<BlurEffect>();
			}
			return _blur;
		}
	}


	public void ActivateMenu () {
		gameObject.SetActive(true);
		blur.enabled = true;
	}

	public void DeactivateMenu () {
		gameObject.SetActive(false);
		blur.enabled = false;
	}


}
