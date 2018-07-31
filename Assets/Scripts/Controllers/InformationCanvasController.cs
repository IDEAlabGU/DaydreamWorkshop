/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 * 
 * Edited by X.Hunt
 *	- No longer crashes when there's no navmesh on a fish
 *	- Properly hides fish
 *	- Loads Kinect scenes
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This script implements a Singleton class design to control an information canvas in Unity worldspace. 
/// It is to be attached to a Unity worldspace Canvas that contains two UIText elements and a Unity Animator.
/// </summary>
public class InformationCanvasController : MonoBehaviour {
	/// <summary>
	/// Holds the single instance of an InformationCanvas
	/// </summary>
	public static InformationCanvasController control;

	/// <summary>
	/// The title text field of the canvas
	/// </summary>
	public GameObject title;
	/// <summary>
	/// The information text field of the canvas
	/// </summary>
	public GameObject infoText;
	/// <summary>
	/// The reference image.
	/// </summary>
	public GameObject referenceImage;
	/// <summary>
	/// The touch image containing a dont touch icon (will be visible if the user cant touch, and invisible if they can)
	/// </summary>
	public GameObject touchImage;
	/// <summary>
	/// The play button, used to configure text/image
	/// </summary>
	public GameObject playButton;
	/// <summary>
	/// The animator with a controller.
	/// </summary>
	public Animator animator;

	/// <summary>
	/// The selected environment object that the canvas is displaying information for.
	/// </summary>
	private EnvironmentObject selectedEnvironmentObject;

	// Use this for initialization
	void Start() {
		QuicksilverRootController.control.status = "Environment";
		animator.SetTrigger ("StartUp");
	}

	void Awake () {
		// ensure only one instance of the canvas
		if (control == null) {
			control = this;
		} else if (control != this) {
			Destroy(gameObject);
		}
	}
		

	/// <summary>
	/// Hides the information canvas. (fades the information canvas out).
	/// </summary>
	public void Hide() {
		// ensure no fish is already being "looked at", but if so, let it roam
		if (selectedEnvironmentObject == null) {
			Debug.LogError ("No reference to object");
		} else {
			if (selectedEnvironmentObject.type == "Fish") {
				// Only do wander stuff if the fish is a wanderin'

				// X: Dont need this any more
				//if (selectedEnvironmentObject.GetComponent<NavWander>()) {
				//	selectedEnvironmentObject.GetComponent<NavWander>().wandering = true;
				//	selectedEnvironmentObject.GetComponent<NavMeshAgent>().speed = 0.5f;
				//}
			}
		}

		QuicksilverRootController.control.deleteFakeObject();

		animator.SetTrigger ("FadeOut");
		transform.position = new Vector3 (0, -2, 0);
	}

	/// <summary>
	/// Called if the user looks away from the canvas.
	/// </summary>
	public void LookedAway() {
		animator.SetTrigger ("FadeOut");
		transform.position = new Vector3 (0, -2, 0);
	}

	/// <summary>
	/// Update the specified newPosition, newTitle and newInfo. Called from an environment object.
	/// </summary>
	/// <param name="newPosition">New position</param>
	/// <param name="newTitle">New title of the object</param>
	/// <param name="newInfo">The object that was clicked</param>
	public void UpdateCanvas(string newTitle, Vector3 newPosition, EnvironmentObject clickedObject) {
		//title = key
		//search table for information keyed with title.
		string[] arr = Data.database.GetObjectQS(newTitle);
	
		// update reference
		selectedEnvironmentObject = clickedObject;

		// call private updates
		UpdateTitle (newTitle);
		UpdatePosition (newPosition);
		UpdateInformation (arr[0]);
		UpdateReferenceImage (newTitle);
		UpdateCanTouch (arr[2]);

		UpdatePlayButton ();

		//* NOTE
		// if multiple games have been added. configure canvas with arr[1] for gameName/canvasName. if "" empty then 
		// dont display button/dont configure tied to a game or explore. if has name, search for game and link acordingly.
	}

	/// <summary>
	/// Change the scene to a game scene. Currently coral = CreateEnvironment, Fish = FindTheFish
	/// </summary>
	public void PlayGame() {
		//* if above NOTE has been implemented, configure here

		// X: If we're using a kinect, load the kinect version
		string levelAppend = GlobalSceneManagement.manager.useKinect ? "Kinect" : "";

		// check which game to change to
		if (selectedEnvironmentObject.type == "Fish") {
			GlobalSceneManagement.manager.ChangeScene ("FindTheFish" + levelAppend);
		} else if (selectedEnvironmentObject.type == "Coral" || selectedEnvironmentObject.type == "Other") {
			GlobalSceneManagement.manager.ChangeScene ("CreateEnvironmentGame" + levelAppend);
		}
	}

    public void PlayGame(int level)
    {
        //* if above NOTE has been implemented, configure here

        // X: If we're using a kinect, load the kinect version
        string levelAppend = GlobalSceneManagement.manager.useKinect ? "Kinect" : "";

        // check which game to change to
        if (level == 0)
        {
            GlobalSceneManagement.manager.ChangeScene("FindTheFish" + levelAppend);
        }
        else if (level == 1)
        {
            GlobalSceneManagement.manager.ChangeScene("CreateEnvironmentGame" + levelAppend);
        }
    }
    /// <summary>
    /// Updates the reference image to an image using the same name as the object, stored in 
    /// "~/Resources/ReferenceImages/name")
    /// </summary>
    /// <param name="name">Name of the object in string form</param>
    private void UpdateReferenceImage(string name) {
		
		Sprite newSprite =  Resources.Load <Sprite>("ReferenceImages/" + name);
		if (newSprite){
			referenceImage.GetComponent<Image> ().sprite = newSprite;
		} else {
			newSprite =  Resources.Load <Sprite>("ReferenceImages/" + "Blank");
			referenceImage.GetComponent<Image> ().sprite = newSprite;
			Debug.LogError("Sprite not found", this);
		}
	}
	/// <summary>
	/// Updates the title of the canvas. Requires a string containing the new title.
	/// </summary>
	/// <param name="newTitle">New title in string form</param>
	private void UpdateTitle(string newTitle) {
		title.GetComponent<Text> ().text = newTitle;
	}
	/// <summary>
	/// Updates the information of the canvas. Requires a string containing the new information.
	/// </summary>
	/// <param name="newInfo">New info to update in string form</param>
	private void UpdateInformation(string newInfo) {
		infoText.GetComponent<Text> ().text = newInfo;
	}
	/// <summary>
	/// Updates the image informing the user if they can or cant touch the type of creature they are looking at
	/// </summary>
	/// <param name="canTouch">Can touch string</param>
	private void UpdateCanTouch(string canTouch) {
		if (canTouch == "no") {
			// update the alpha of the image to 1/visible
			Image image = touchImage.GetComponent<Image> ();
			Color c = image.color;
			c.a = 1;
			image.color = c;
		} else {
			//update the alpha of the image to 0/invisible
			Image image = touchImage.GetComponent<Image> ();
			Color c = image.color;
			c.a = 0;
			image.color = c;
		}
	}
	/// <summary>
	/// Updates the position of the canvas. Moves canvas respective to the object that calls this function. 
	/// Requires the Vector3 position of the object that called it.
	/// </summary>
	/// <param name="objectPosition">Object position in vector 3 form</param>
	private void UpdatePosition(Vector3 newPosition) {
		Vector3 pos = Camera.main.transform.position + (Camera.main.transform.forward * 2.0f);

		if (pos.y < 1.4f) {
			pos.y = 1.4f;
		} else if (pos.y > 2f) {
			pos.y = 2f;
		}
		transform.position = pos;
		transform.LookAt (Camera.main.transform.position);

		//if (visible == false) {
		animator.SetTrigger ("FadeIn");
		//}
		//visible = true;
	}
	/// <summary>
	/// Updates the text of the play button. Can be expanded to allow changes of the play button icon etc
	/// </summary>
	private void UpdatePlayButton() {
		// check which game to change to
		if (selectedEnvironmentObject.type == "Coral" || selectedEnvironmentObject.type == "Other" ) {
			Text text = playButton.GetComponent<Text> ();
			text.text = "Play! \nReef Builder";
		} else {
			Text text = playButton.GetComponent<Text> ();
			text.text = "Play! \nFind the Fish";
		}
	}


}
