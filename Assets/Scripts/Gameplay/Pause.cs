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
/// Pause the environment around the user.
/// </summary>
public class Pause : MonoBehaviour {
	/// <summary>
	/// is the game paused.
	/// </summary>
	public bool paused = false;
	/// <summary>
	/// The sprite to switch to when the game is paused
	/// </summary>
	public Sprite playSprite;
	/// <summary>
	/// The sprite to switch to when the game is unpaused
	/// </summary>
	public Sprite pauseSprite;

	/// <summary>
	/// When the pause button is clicked this function can be called
	/// </summary>
	public void OnClick() {
		// if the game is paused, unpause, and vice versa
		if (paused == false) {
			paused = true;
			GetComponent<Text>().text = "Resume";
			transform.parent.GetComponent<Image> ().sprite = playSprite;
			Time.timeScale = 0;
		} else if (paused == true) {
			paused = false;
			GetComponent<Text>().text = "Pause";
			transform.parent.GetComponent<Image> ().sprite = pauseSprite;
			Time.timeScale = 1;
		}
	}
}
