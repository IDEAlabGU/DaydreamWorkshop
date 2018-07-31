/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Startup is designed to be called by the first scene in a project. 
/// 
/// It is used to perform any universal modifications like locking the device rotation or setting a targed frame rate.
/// Global lighting, and other tweaks can be set using this script by adding the respective Unity functions.
/// </summary>
public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// lock the device to landscape
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		// set the framerate (works for iOS and Android)
		Application.targetFrameRate = 60;
	}	


}
