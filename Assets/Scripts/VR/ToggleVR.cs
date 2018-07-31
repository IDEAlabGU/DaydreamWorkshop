/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This script can be used to toggle the VR mode of a cardboard camera.
/// 
/// It can be attached to any game object, and toggled via a cthe ToggleVRMode() function.
/// </summary>
public class ToggleVR : MonoBehaviour {

	/// <summary>
	/// The sprite to update the button to represent turning on VR mode
	/// </summary>
	public Sprite onVR;
	/// <summary>
	/// The sprite to update the button to represent turning off VR mode
	/// </summary>
	public Sprite offVR;

	/// <summary>
	/// If it has been clicked.
	/// </summary>
	private bool modeVR = true;

	/// <summary>
	/// Toggles the VR mode.
	/// Accessed Cardboard.SDK.
	/// </summary>
	public void ToggleVRMode() {
		// toggle, and update images
		if (modeVR == false) {
			modeVR = true;

			GetComponent<Text>().text = "Normal Mode";
			transform.parent.GetComponent<Image> ().sprite = offVR;
			//FIX: GVR.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
		} else {
			modeVR = false;
			GetComponent<Text>().text = "VR Mode";
			transform.parent.GetComponent<Image> ().sprite = onVR;
			//FIX : Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
		}
	}
}
