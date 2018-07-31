using UnityEngine;
using System.Collections;

// Links all children together with a red line. Works in Editor.
// Used for showing where navlinks go
//
// Written by X.Hunt

public class NavLinkDisplay : MonoBehaviour {

	void OnDrawGizmos() {
		if(transform.childCount > 1) {

			Gizmos.color = Color.red;

			for (int i = 0; i < transform.childCount - 1; i++) {
				Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
			}
		}
	}
}
