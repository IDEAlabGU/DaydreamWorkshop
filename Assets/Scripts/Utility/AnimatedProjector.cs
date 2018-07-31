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
/// Projects a series of provided images onto a scene.
/// 
/// This script can be used to animate things such as underwater caustics in a scene.
/// </summary>
[RequireComponent (typeof (Projector))]
public class AnimatedProjector : MonoBehaviour {
	/// <summary>
	/// The desired fps
	/// </summary>
	public float fps = 60.0f;
	/// <summary>
	/// The selection of images to cycle through. 
	/// </summary>
	public Texture2D[] frames;

	/// <summary>
	/// The index of the frame.
	/// </summary>
	private int frameIndex;
	/// <summary>
	/// The projector projecting the images.
	/// </summary>
	private Projector projector;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		projector = GetComponent<Projector> ();
		NextFrame ();
		InvokeRepeating("NextFrame", 1 / fps, 1/ fps);
	}
	/// <summary>
	/// Handles the next frame of the projector
	/// </summary>
	private void NextFrame() {
		projector.material.SetTexture ("_ShadowTex", frames[frameIndex]);
		//projector.material.SetTesture ("_ShadowText", frames[frameIndex]);
		frameIndex = (frameIndex + 1) % frames.Length;
	}
}