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
/// Handles instanced logic of the Find The Fish game.
/// 
/// Used to change the scene & reset the game.
/// </summary>
public class FindTheFish : MonoBehaviour {

	/// <summary>
	/// Changes to the specified scene.
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	public void ChangeScene(string sceneName) {
		GlobalSceneManagement.manager.ChangeScene (sceneName);
	}

	/// <summary>
	/// Reset the game to its initial state
	/// </summary>
	public void Reset () {
		FindTheFishGameController.control.Reset ();
	}
}
