using UnityEngine;
using System.Collections;

[System.Serializable]
public class Inventory {

	public Item[] slots;

	public Inventory (int numSlots) {
		slots = new Item[numSlots];
	}
}
