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
/// SceneTimerSimple handles running a scene for a pre-determined length of time.
/// 
/// It should be attached to any instanced game object and will automatically change the scene after a specified time
/// and name.
/// 
/// It requires access to GlobalSceneManagement
/// </summary>
public class SceneTimerSimple : MonoBehaviour {
	public string newScene;
	public float timeOfScene;

	// Use this for initialization
	void Start() {
		StartCoroutine(Timer(timeOfScene));
	}
	/// <summary>
	/// A simple timer that waits for x seconds, and then changes to a new scene
	/// </summary>
	IEnumerator Timer(float time) {
		yield return new WaitForSeconds(time);
		GlobalSceneManagement.manager.ChangeSceneWithoutLoadingScreen (newScene);

	}
}
