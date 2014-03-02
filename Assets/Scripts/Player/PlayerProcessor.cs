using UnityEngine;
using System.Collections;

public class PlayerProcessor : APIBehaviour {

	public StatCollection stats;

	public void SetStats (Hashtable apiStats) {
		stats.Set (apiStats);
	}

}
