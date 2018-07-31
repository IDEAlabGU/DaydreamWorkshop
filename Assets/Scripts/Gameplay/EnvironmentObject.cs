/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 * 
 * Edited by X.Hunt:
 *	- Objects now have a UIScale field, used to scale the object when it is a FakeObject being displayed
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
/// <summary>
/// This class holds the information relating to an object that would be placed in an environment.
/// 
/// It contains a type, name, and various other 
/// </summary>
public class EnvironmentObject : MonoBehaviour {
	/// <summary>
	/// Holds value of the type of environment object e.g. Fish, Coral, or Other.
	/// </summary>
	public string type;
	/// <summary>
	/// The name of the Fish, Coral or Other. e.g. "Angel Fish" *note this is case and space sensitive
	/// </summary>
	public string formattedName;
	/// <summary>
	/// Should this object be highlighted when the user is hovering over it
	/// </summary>
	public bool highlightOnHover = true;

	/// <summary>
	/// The original shader of the object (Mobile/VertexLit)
	/// </summary>
	private Shader originalShader;
	/// <summary>
	/// The shader to switch to when the user is hovering over the object (Outlined/Silhouetted Diffuse)
	/// </summary>
	private Shader newShader;
	/// <summary>
	/// References to each of the renderer components attatched to the object and its children
	/// </summary>
	private Renderer[] rendChildArr;
	/// <summary>
	/// The navigation agent attatched to this script
	/// </summary>
	private UnityEngine.AI.NavMeshAgent nav;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		// get renderers
		rendChildArr = GetComponentsInChildren<Renderer> ();
		// get navigation agent
		if (type == "Fish") {
			nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		}
		// setup shaders
		originalShader = Shader.Find("Mobile/VertexLit");
		newShader = Shader.Find ("Outlined/Silhouetted Diffuse");
	}

	/// <summary>
	/// Send the type, formattedName and a reference to this object to the QuicksilverRootController
	/// </summary>
	public void OnClick() {
		//callquicksilver root and send through the type and name of the object, and a reference to the object
		QuicksilverRootController.control.ObjectClicked (type, formattedName, this);
	}

	/// <summary>
	/// Toggls the shader of the object from the originalShader to the newShader
	/// </summary>
	public void TogglShader() {
		// check if this object should be highlighted
		if (highlightOnHover == true) {
			if (originalShader == null || newShader == null) {
				Debug.LogError ("No shaders assigned to EnvironmentObject");
			} else {

				// Update parent & children renderers
				foreach (Renderer r in rendChildArr) {
					if (r.material.shader == newShader) {
						r.material.shader = originalShader;
					} else {
						r.material.shader = newShader;
					}
				}
			}
		}
	}
		
	/// <summary>
	/// *note Currently Unused
	/// Can be called to rotate the 3D oject this script is attached to.
	/// </summary>
	private IEnumerator Rotate(float duration) {
		if (nav.hasPath == false) {
			float startRotation = transform.eulerAngles.y;
			float endRotation = startRotation + 360.0f;
			float t = 0.0f;
			while (t < duration) {
				t += Time.deltaTime;
				float yRotation = Mathf.Lerp (startRotation, endRotation, t / duration) % 360.0f;
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

				yield return null;
			}
		}
	}
}