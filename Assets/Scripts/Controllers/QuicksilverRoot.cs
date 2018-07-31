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
/// Quicksilver root should be attached to the GameRoot of a scene.
/// 
/// It can be used to handle game logic that requires references from instanced objects.
/// </summary>
public class QuicksilverRoot : MonoBehaviour {
	/// <summary>
	/// Changes the scene to the specified scene
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	public void ChangeScene(string sceneName) {
		GlobalSceneManagement.manager.ChangeScene (sceneName);
	}
}
