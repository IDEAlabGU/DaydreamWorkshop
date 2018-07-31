/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 * 
 * Edited by X.Hunt
 *  - No longer crashes when there's no active navmesh
 *  - Fish no longer move to the camera when selected
 * 
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Used to handle the logic for an interactive environment
/// 
/// It is designed to control an environment containing navigating objects and static objects. It requires access to an
/// InformationCanvasController, which displays information to the user. And requires access to a 
/// FindTheFishGameController as this script acts as the controller for the games environment.
/// </summary>
public class QuicksilverRootController : MonoBehaviour {
	/// <summary>
	/// Holds the single instance of a QuicksilverRoot scene
	/// </summary>
	public static QuicksilverRootController control;

	/// <summary>
	/// The status of the environment (normal environment, or a particular game environment)
	/// </summary>
	public string status;

	/// <summary>
	/// The previous object navigation wander reference.
	/// </summary>
	private NavWander prevObjectWander;
	/// <summary>
	/// The previous object navigation agent reference.
	/// </summary>
	private UnityEngine.AI.NavMeshAgent prevObjectAgent;
	/// <summary>
	/// The type of the previous game object that was clicked on.
	/// </summary>
	private string prevType = "";

	// Temporary fake object
	private GameObject fakeObject;
	public float rotSpeed = 50;

	void Awake () {
		// ensure only one instance of the canvas
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy(gameObject);
		}
	}
		
	/// <summary>
	/// Called when any object located inside the environment is selected.
	/// </summary>
	/// <param name="type">the type of object clicked</param>
	/// <param name="name">the formatted name of the clicked object</param>
	/// <param name="clickedObject">a reference to the actual object</param>
	public void ObjectClicked(string type, string name, EnvironmentObject clickedObject) {
		// check the status of the application and respond to the environment object respectively.
		if (status == "Environment") {
			// locate foot menu and store in temp vec. Used to identify the new position of the fish and the information
			// popup.
			Vector3 menuPos = new Vector3 (Camera.main.transform.forward.x, 0.5f, Camera.main.transform.forward.z); //GameObject.FindGameObjectWithTag ("FootMenu");


			// if the previous type was a fish
			if (prevType == "Fish") {
				// update the fish that was clicked previously so that it will continue to wander.

				/*
				if (prevObjectWander && prevObjectAgent) {
					prevObjectWander.wandering = true;
					prevObjectAgent.speed = 0.5f;
				}
				*/
			}

			// if the current type is a fish
			if (type == "Fish") {
				// get new fish's information
				NavWander tempWander = clickedObject.GetComponent<NavWander> ();
				UnityEngine.AI.NavMeshAgent tempAgent = clickedObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();

				/*
				if (tempAgent && tempWander) {
					//stop fish from roaming
					tempWander.wandering = false;

					//calculate the new position for the fish (near the player)
					Vector3 vec = Camera.main.transform.position + (Camera.main.transform.forward * 2.0f);
					vec.y = 0.5f;


					//set the destination of the fish to the calculated position.
					tempAgent.SetDestination(vec);
					tempAgent.speed = 1.5f;
				*/

				//store the new fish's information into temp storage
				prevObjectWander = tempWander;
				prevObjectAgent = tempAgent;
				prevType = type;

			} else if (type == "Coral" || type == "Other") {
				//complete coral specific tasks
			}

			// call controller to update and configure with the name and new position.
			//InformationCanvasController.control.UpdateCanvas (name, menuPos, clickedObject);

			// -------- X start here -------- //
			// Cleanup the old fakeObject
			deleteFakeObject();

			// Spawn a copy of the object clicked, and move it to the correct location
			fakeObject = Instantiate((GameObject)Resources.Load("Prefabs/" + clickedObject.formattedName));     // Shoot me

			Transform fakeLocation = GameObject.FindGameObjectWithTag("UIModel").transform;
			//fakeObject.transform.parent = fakeLocation;
			fakeObject.transform.position = fakeLocation.position;
			if (fakeObject.GetComponent<UIScale>()) {
				fakeObject.transform.localScale *= fakeObject.GetComponent<UIScale>().Scale;
			}
			// -------- X end here -------- //

		}
		else if (status == "FindTheFish") {
			//call game controller and send through the name of the object clicked.
			FindTheFishGameController.control.WasClicked(name);
		
		} else {
		// add more: status == "game/SceneHere") {
		}

	}

	// -------- X start here -------- //
	void Update() {

		if(fakeObject) {
			fakeObject.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);
		}

	}

	public void deleteFakeObject() {
		if (fakeObject)
			Destroy(fakeObject);
	}

	// -------- X end here -------- //

}
