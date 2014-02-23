using UnityEngine;
using System;
using System.Collections;

public enum ElementYieldType {
	oxygen,
	hydrogen,
	nitrogen,
	carbon,
	trace
}

[Serializable]
public class ElementYield {

	public int oxygen = 0;
	public int hydrogen = 0;
	public int nitrogen = 0;
	public int carbon = 0;
	public int trace = 1;

	public ElementYield () {}

	public ElementYield (ElementYieldType t, int amount) {
		foreach(ElementYieldType type in Enum.GetValues(typeof(ElementYieldType))) {
			if (t == type) {
				Add (t, amount);
				break;
			}
		}
	}

	public override string ToString ()
	{
		string s = "(O: " + oxygen;
		s += " H: " + hydrogen;
		s += " N: " + nitrogen;
		s += " C: " + carbon;
		s += " T: " + trace;
		s += ")";
		return s;
	}

	public int of (string elementName) {
		Debug.Log ("element name is " + elementName);
		Debug.Log (this.GetType());
		Debug.Log (this.GetType().GetField(elementName));
		return (int)this.GetType().GetField(elementName).GetValue(this);
	}

	public int of (ElementYieldType t) {
		int amount = 0;
		foreach(ElementYieldType type in Enum.GetValues(typeof(ElementYieldType))) {
			if (t == type) {
				amount = of(type.ToString());
				break;
			}
		}

		return amount;
	}


	public ElementYield Add(ElementYield other) {
		foreach(ElementYieldType type in Enum.GetValues(typeof(ElementYieldType))) {
			Add (type, other.of (type));
		}

		return this;
	}

	public int Add (ElementYieldType t, int amount) {
		int currentAmount = of(t);
		this.GetType().GetField(t.ToString()).SetValue(this, currentAmount + amount);
		return of(t);
	}

	public void Add (IDictionary props) {
		int amount = 0;
		foreach(ElementYieldType type in Enum.GetValues(typeof(ElementYieldType))) {
			object prop = props[type.ToString()];
			if (prop != null) {
				int.TryParse(prop.ToString (), out amount);
				Add (type, amount);
			}
		}
	}

}
