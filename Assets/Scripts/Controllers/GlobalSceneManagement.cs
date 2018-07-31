/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// The scene manager handles transitions between scenes.
/// 
/// It can be called via the single instance (manager), by using manager.ChangeScene(newSceneName).
/// </summary>
public class GlobalSceneManagement : MonoBehaviour {

	/// <summary>
	/// The single scene manager.
	/// </summary>
	public static GlobalSceneManagement manager;
	/// <summary>
	/// Stores the name of the scene to change to
	/// </summary>
	public string newScene = "";
	/// <summary>
	/// The load mode. (single or additive)
	/// </summary>
	public LoadSceneMode loadMode = LoadSceneMode.Single;

	public bool useKinect = false;

	/// <summary>
	/// Holds if a scene is currently being handled, to avoid overflow/multiple clicks
	/// </summary>
	private bool hasScene = false;

	void Awake () {
		// ensure only one instance of the canvas
		if (manager == null) {
			DontDestroyOnLoad (gameObject);
			manager = this;
		} else if (manager != this) {
			Destroy(gameObject);
		}
	}
	/// <summary>
	/// Changes the scene without loading screen.
	/// </summary>
	/// <param name="newSceneName">New scene name.</param>
	public void ChangeSceneWithoutLoadingScreen(string newSceneName) {
		// load the requested scene
		SceneManager.LoadSceneAsync (newSceneName, loadMode);
	}
	/// <summary>
	/// Called by a script to change the scene.
	/// </summary>
	/// <param name="newSceneName">New scene name.</param>
	public void ChangeScene(string newSceneName) {
		if (hasScene == false) {
			hasScene = true;
			newScene = newSceneName;
			StartCoroutine (loadScreen ());
		}
	}
	/// <summary>
	/// Loads the loading screen without allowing activation (to avoid lag). Then calls a coroutine to load the actual
	/// scene requested
	/// </summary>
	/// <returns>The screen.</returns>
	private IEnumerator loadScreen() {
		// load the loading scene.

		string level = "Loading" + (useKinect ? "Kinect" : "VR");

		AsyncOperation ao = SceneManager.LoadSceneAsync (level, LoadSceneMode.Single);
		ao.allowSceneActivation = false;
		StartCoroutine (loadNext (ao));
		yield break;
	}
	/// <summary>
	/// Loads the next scene after at least 1 second on the loading screen.
	/// </summary>
	/// <returns>The next.</returns>
	private IEnumerator loadNext(AsyncOperation ao) {
		yield return new WaitForSeconds (1);
		ao.allowSceneActivation = true;
		yield return new WaitForSeconds (2);
		SceneManager.LoadSceneAsync (newScene);
		hasScene = false;
		yield break;
	}
}
