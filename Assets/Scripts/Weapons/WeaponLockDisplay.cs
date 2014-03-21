using UnityEngine;
using System.Collections;

public class WeaponLockDisplay : DisplayBehaviour {
	
	GameObject labelObj;
	UILabel label;
	UIFollowTarget labelFollow;
	
	public void ConnectDisplay () {
		labelObj = labeler.weaponLockLabel;
		label = GetLabel(labelObj);
		labelFollow = GetFollow(labelObj);
	}
	
	public void StartDisplay (GameObject target) {
		if (labelObj == null) {
			ConnectDisplay();
		}
		labelFollow.target = target.transform;
		UpdateDisplay(0);
		labelObj.SetActive(true);
	}
	
	public void UpdateDisplay (int lockPercentage) {
		label.text = "Locking " + lockPercentage + "%";
	}
	
	public void BreakDisplay () {
		//		weaponLockLabel.text = "Broken!";
		if (labelObj != null) {
			labelObj.SetActive(false);
		}
	}
	
	public void CompleteDisplay () {
		label.text = "Locked!";
	}
}
