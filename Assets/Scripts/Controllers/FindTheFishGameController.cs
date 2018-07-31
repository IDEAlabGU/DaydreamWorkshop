/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 * 
 * Edited by X.Hunt
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// Used to handle the game logic for the find the fish game.
/// 
/// It is designed to be used with a text field to show users information.
/// </summary>
public class FindTheFishGameController : MonoBehaviour {


	/// <summary>
	/// The path to load prefabs from. Should NOT end with a slash
	/// </summary>
	public string prefabPath = "Prefabs";
	public float rotSpeed = 50f;
	public float UIScaleMod = 0.5f;

	/// <summary>
	/// A single instance of the game controller
	/// </summary>
	public static FindTheFishGameController control;
	/// <summary>
	/// A reference to the text field that has instructions as to what the user should find
	/// </summary>
	private GameObject gameInstructionText;
	private GameObject gameImage;

	private GameObject fakeObject;

	void Start () {
		// ensure only one instance of the canvas
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy(gameObject);
		}

		// setup the references & scene status (if quicksilver scene exists - doesnt exist if loading direct through unity editor)
		if (QuicksilverRootController.control != null) {
			QuicksilverRootController.control.status = "FindTheFish";
			gameInstructionText = GameObject.FindWithTag ("FindTheGameText");
			gameImage = GameObject.FindWithTag ("FindTheFishImage");
		}		
		// If new fish/coral have been added to find. then need to compare the "Data.database.FTFData.original" with the
		// Data.database.dictionary and force an update to it.
		if (System.IO.File.Exists (Application.persistentDataPath + "/findTheFish.dat")) {
			//Data.database.LoadFTF ();
		} else {
			Reset ();
		}

		// setup the users game information
		if (Data.database.dataFTF.toFind.Length == 0 || Data.database.dataFTF.toFind == null) {
			gameInstructionText.GetComponent<Text> ().text = "To replay, tap the reset button";
			UpdateReferenceImage ("Blank");

		} else {
			gameInstructionText.GetComponent<Text> ().text = ("Find the: " + Data.database.dataFTF.toFind [0]);
			UpdateReferenceImage (Data.database.dataFTF.toFind [0]);
		}
	}

	void Update() {
		if (fakeObject) {
			Transform fakeLocation = GameObject.FindGameObjectWithTag("UIModel").transform;
			fakeObject.transform.position = fakeLocation.position;

			fakeObject.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);
		}
	}

	/// <summary>
	/// Called when an object has been clicked.
	/// </summary>
	/// <param name="name">the name of the object</param>
	public void WasClicked(string name) {
		// start coroutine so delays can be used
		StartCoroutine (UpdateText (name));
	}
	/// <summary>
	/// Checks if the object found was the correct obeject, informs the user and and then updates
	/// </summary>
	/// <param name="name">The name of the object</param>
	private IEnumerator UpdateText(string name) {
		// find text field with the instructions
		gameInstructionText = GameObject.FindWithTag ("FindTheGameText");
		// check if the object found is the correct object
		if (name == Data.database.dataFTF.toFind [0]) {
			StartCoroutine(WasFound ());
			yield break;
		} else {
			gameInstructionText.GetComponent<Text> ().text = "Try again";
			yield return new WaitForSeconds(2);
			gameInstructionText.GetComponent<Text> ().text = "Find The: " + Data.database.dataFTF.toFind[0];
			yield break;
		}
	}

	/// <summary>
	/// Informs the user that they found the correct object, wait 2 seconda and update the users instructions 
	/// </summary>
	private IEnumerator WasFound() {
		gameInstructionText.GetComponent<Text> ().text = "Nice!";
		Data.database.dataFTF.toFind = Data.database.dataFTF.toFind.Skip (1).ToArray ();
		yield return new WaitForSeconds(2);
		Data.database.SaveFTF ();
		ToFind ();
		yield break;
	}
	/// <summary>
	/// Updates the games instructions after an object has successfully been found
	/// </summary>
	private void ToFind() {
		if (Data.database.dataFTF.toFind.Length == 0 || Data.database.dataFTF.toFind == null) {
			//display on screen what to find
			gameInstructionText.GetComponent<Text> ().text = "To Play again, click the reset button";
			UpdateReferenceImage ("Blank");

		} else {
			//display on screen what to find
			gameInstructionText.GetComponent<Text> ().text = "Find The: " + Data.database.dataFTF.toFind[0];
			UpdateReferenceImage (Data.database.dataFTF.toFind [0]); 
		}
	}

	/// <summary>
	/// Resets the game.
	/// </summary>
	public void Reset() {
		//set temp file of names to find, to the template one.
		Data.database.dataFTF.toFind = Data.database.dataFTF.original;
		// save the file
		Data.database.SaveFTF ();
		StartCoroutine (ResetDelay ());
	}
	/// <summary>
	/// Delays between updating the information provided to the user.
	/// </summary>
	private IEnumerator ResetDelay() {
		// indicate to user that the app is resetting
		gameInstructionText = GameObject.FindWithTag ("FindTheGameText");
		gameImage = GameObject.FindWithTag ("FindTheFishImage");
		gameInstructionText.GetComponent<Text> ().text = "Resetting...";
		UpdateReferenceImage ("Blank");
		yield return new WaitForSeconds (1);
		gameInstructionText.GetComponent<Text> ().text = "Find The: " + Data.database.dataFTF.toFind[0];
		UpdateReferenceImage (Data.database.dataFTF.toFind [0]);
		yield break;
	}

	/// <summary>
	/// Updates the reference image to an image using the same name as the object, stored in 
	/// "~/Resources/ReferenceImages/name")
	/// </summary>
	/// <param name="name">Name.</param>
	private void UpdateReferenceImage(string name) {

		Sprite newSprite = Resources.Load<Sprite>("ReferenceImages/" + name);

		deleteFakeObject();

		if (newSprite){
			gameImage.GetComponent<Image>().sprite = newSprite;

		} else {
			newSprite =  Resources.Load <Sprite>("ReferenceImages/" + "Blank");
			gameImage.GetComponent<Image> ().sprite = newSprite;
			Debug.LogError("Sprite not found", this);
		}

		if (!name.ToLower().Equals("blank")) {

			// Only add a new model if there's an image
			fakeObject = Instantiate((GameObject)Resources.Load("Prefabs/" + name));
			Transform fakeLocation = GameObject.FindGameObjectWithTag("UIModel").transform;
			fakeObject.transform.position = fakeLocation.position;

			if (fakeObject.GetComponent<UIScale>()) {
				fakeObject.transform.localScale *= fakeObject.GetComponent<UIScale>().Scale * UIScaleMod;
			}
		}
	}

	public void deleteFakeObject() {
		if (fakeObject)
			Destroy(fakeObject);
	}

	// X: Really dunno where this is being disabled, but I need it enabled
	void OnDisable() {
		FindTheFishGameController.control.enabled = true;
	}

    /// <summary>
    /// Changes the scene to the scene name specified.
    /// </summary>
    /// <param name="sceneName">the scene name to change to</param>
    public void ChangeScene(string sceneName)
    {
        GlobalSceneManagement.manager.ChangeScene(sceneName);
    }
}
