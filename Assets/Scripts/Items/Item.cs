using UnityEngine;
using System.Collections;
using System;

public class Item {

	public string displayName;
	public int quantity;

	public static Item FromString (string itemName) {
		return FromString(itemName, 1);
	}

	public static Item FromString (string itemName, int quantity) {
		Type itemType = Type.GetType(itemName);
		Item item = null;

		if (itemType != null) {
			item = Activator.CreateInstance(itemType) as Item;
			if (item != null) {
				item.quantity = quantity;
			}
		}

		return item;
	}

	public override string ToString () {
		return string.Format ("[Item] " + this.GetType().Name + " (" + quantity + ")");
	}

}
