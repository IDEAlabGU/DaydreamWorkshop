/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This script keeps an object infront of the users camera at all times with a slight lag, to make the object appear
/// as if it is in the environment.
/// 
/// It can be attached to a unity worldpace object and will update that objects position with smoothing to make it feel
/// like the object is in the environment.
/// Values can be changed using the Unity editor, but defaults are provided.
/// </summary>
public class MenuPosition : MonoBehaviour {

	public GameObject GameController;
	public GameObject environment;
	public GameObject staghornCoralButton;
	public GameObject plateCoralButton;
	public GameObject giantClamButton;
	public GameObject plateCoral2Button;
	public GameObject brainCoralButton;
	public GameObject undoButton;
	public float smoothSpeed = 3f;
	public GameObject footmenu;
	private bool backToCenter = true;
	private Vector3 screenPoint;

	// Use this for initialization
	void Start() {
		SetupFunctionsToRun();

	}

	// Update is called once per frame
	void Update() {
		// in front of the user
		//Vector3 forward = Camera.main.transform.forward;
		// ensuring menu doesnt get too close to the player
		//float hypo = (Mathf.Abs (forward.x) * Mathf.Abs (forward.x)) + (Mathf.Abs (forward.z) * Mathf.Abs (forward.z));
		//TODO: this now triggers all the time, so 

		try {
			screenPoint = Camera.main.WorldToViewportPoint(footmenu.transform.position);
			bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
			if (onScreen && backToCenter) {
			} else {
				transform.rotation = /*Quaternion.Lerp(transform.rotation, Camera.main.transform.rotation, smoothSpeed * Time.deltaTime);*/
				Quaternion.Slerp(Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + 180, 0), smoothSpeed * Time.deltaTime);
				if (Mathf.Abs(Camera.main.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y) - 180 < 2f)
					backToCenter = true;
				else
					backToCenter = false;
			}
		} catch { Debug.Log("Please turn Scene Management and GVRMain on"); };
	}

	private void SetupFunctionsToRun() {
		//Fix the on click buttons first cause they are easier
		staghornCoralButton.GetComponent<Button>().onClick.AddListener(delegate () {
			GameController.GetComponent<CreateEnvironmentGameController>().SetSelectedObjectType(staghornCoralButton.name);
		});
		plateCoralButton.GetComponent<Button>().onClick.AddListener(delegate () {
			GameController.GetComponent<CreateEnvironmentGameController>().SetSelectedObjectType(plateCoralButton.name);
		});
		giantClamButton.GetComponent<Button>().onClick.AddListener(delegate () {
			GameController.GetComponent<CreateEnvironmentGameController>().SetSelectedObjectType(giantClamButton.name);
		});
		brainCoralButton.GetComponent<Button>().onClick.AddListener(delegate () {
			GameController.GetComponent<CreateEnvironmentGameController>().SetSelectedObjectType(brainCoralButton.name);
		});
		plateCoral2Button.GetComponent<Button>().onClick.AddListener(delegate () {
			GameController.GetComponent<CreateEnvironmentGameController>().SetSelectedObjectType(plateCoral2Button.name);
		});
		undoButton.GetComponent<Button>().onClick.AddListener(delegate () {
			GameController.GetComponent<CreateEnvironmentGameController>().Undo();
		});

		EventTrigger.Entry clickEventStag = new EventTrigger.Entry();
		clickEventStag.eventID = EventTriggerType.PointerClick;
		EventTrigger.Entry enterEventStag = new EventTrigger.Entry();
		enterEventStag.eventID = EventTriggerType.PointerEnter;
		EventTrigger.Entry exitEventStag = new EventTrigger.Entry();
		exitEventStag.eventID = EventTriggerType.PointerExit;

		clickEventStag.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonClick(staghornCoralButton); });
		staghornCoralButton.GetComponent<EventTrigger>().triggers.Add(clickEventStag);
		enterEventStag.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonEnter(staghornCoralButton); });
		staghornCoralButton.GetComponent<EventTrigger>().triggers.Add(enterEventStag);
		exitEventStag.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonExit(staghornCoralButton); });
		staghornCoralButton.GetComponent<EventTrigger>().triggers.Add(exitEventStag);


		EventTrigger.Entry clickEventPlate = new EventTrigger.Entry();
		clickEventPlate.eventID = EventTriggerType.PointerClick;
		EventTrigger.Entry enterEventPlate = new EventTrigger.Entry();
		enterEventPlate.eventID = EventTriggerType.PointerEnter;
		EventTrigger.Entry exitEventPlate = new EventTrigger.Entry();
		exitEventPlate.eventID = EventTriggerType.PointerExit;

		clickEventPlate.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonClick(plateCoralButton); });
		plateCoralButton.GetComponent<EventTrigger>().triggers.Add(clickEventPlate);
		enterEventPlate.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonEnter(plateCoralButton); });
		plateCoralButton.GetComponent<EventTrigger>().triggers.Add(enterEventPlate);
		exitEventPlate.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonExit(plateCoralButton); });
		plateCoralButton.GetComponent<EventTrigger>().triggers.Add(exitEventPlate);


		EventTrigger.Entry clickEventClam = new EventTrigger.Entry();
		clickEventClam.eventID = EventTriggerType.PointerClick;
		EventTrigger.Entry enterEventClam = new EventTrigger.Entry();
		enterEventClam.eventID = EventTriggerType.PointerEnter;
		EventTrigger.Entry exitEventClam = new EventTrigger.Entry();
		exitEventClam.eventID = EventTriggerType.PointerExit;

		clickEventClam.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonClick(giantClamButton); });
		giantClamButton.GetComponent<EventTrigger>().triggers.Add(clickEventClam);
		enterEventClam.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonEnter(giantClamButton); });
		giantClamButton.GetComponent<EventTrigger>().triggers.Add(enterEventClam);
		exitEventClam.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonExit(giantClamButton); });
		giantClamButton.GetComponent<EventTrigger>().triggers.Add(exitEventClam);

		EventTrigger.Entry clickEventBrain = new EventTrigger.Entry();
		clickEventBrain.eventID = EventTriggerType.PointerClick;
		EventTrigger.Entry enterEventBrain = new EventTrigger.Entry();
		enterEventBrain.eventID = EventTriggerType.PointerEnter;
		EventTrigger.Entry exitEventBrain = new EventTrigger.Entry();
		exitEventBrain.eventID = EventTriggerType.PointerExit;

		clickEventBrain.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonClick(brainCoralButton); });
		brainCoralButton.GetComponent<EventTrigger>().triggers.Add(clickEventBrain);
		enterEventBrain.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonEnter(brainCoralButton); });
		brainCoralButton.GetComponent<EventTrigger>().triggers.Add(enterEventBrain);
		exitEventBrain.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonExit(brainCoralButton); });
		brainCoralButton.GetComponent<EventTrigger>().triggers.Add(exitEventBrain);

		EventTrigger.Entry clickEventPlate2 = new EventTrigger.Entry();
		clickEventPlate2.eventID = EventTriggerType.PointerClick;
		EventTrigger.Entry enterEventPlate2 = new EventTrigger.Entry();
		enterEventPlate2.eventID = EventTriggerType.PointerEnter;
		EventTrigger.Entry exitEventPlate2 = new EventTrigger.Entry();
		exitEventPlate2.eventID = EventTriggerType.PointerExit;

		clickEventPlate2.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonClick(plateCoral2Button); });
		plateCoral2Button.GetComponent<EventTrigger>().triggers.Add(clickEventPlate2);
		enterEventPlate2.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonEnter(plateCoral2Button); });
		plateCoral2Button.GetComponent<EventTrigger>().triggers.Add(enterEventPlate2);
		exitEventPlate2.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().OnButtonExit(plateCoral2Button); });
		plateCoral2Button.GetComponent<EventTrigger>().triggers.Add(exitEventPlate2);

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((eventData) => { GameController.GetComponent<CreateEnvironmentGameController>().PlaceCurrentObject(); });

		foreach (Transform child in environment.transform) {
			if(child.name == "Environment") {
				foreach(Transform grandChild in child) {
					try {
						grandChild.GetComponent<EventTrigger>().triggers.Add(entry);
					} catch { }
				}
			}
		}
	}
}
