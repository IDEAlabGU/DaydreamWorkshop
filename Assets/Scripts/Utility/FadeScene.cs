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
/// Used to fade in/out the scene/screen.
/// </summary>
public class FadeScene : MonoBehaviour {
	/// <summary>
	/// The picture shown when fading
	/// </summary>
	public Texture2D picture;
	/// <summary>
	/// The fade speed.
	/// </summary>
	public float fadeSpeed = 0.1f;
	/// <summary>
	/// The fade direction. -1 to fade in, 1 to fade out
	/// </summary>
	public int fadeDir = -1;

	/// <summary>
	/// The draw priority. lower priority means it will be drawn last
	/// </summary>
	private int draw = 1000;
	/// <summary>
	/// The alpha value of the texture (from 0 to 1)
	/// </summary>
	private float alpha = 1.0f;

	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// </summary>
	private void OnGUI() {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;

		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = draw;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), picture);
	}
}
