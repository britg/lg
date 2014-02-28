using UnityEngine;
using System.Collections;

public class PlayerProcessor : PersistenceRequest {

	public StatCollection stats;

	public void SetStats (Hashtable apiStats) {
		stats.Set (apiStats);
	}

}
