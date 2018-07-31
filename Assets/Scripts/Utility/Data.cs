/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// The object containing all of the persistent data of the app.
/// 
/// This data class handles data for the QuicksilverRoot, FindTheFishGameController, and CreateEnvironmentGameController.
/// </summary>
public class Data : MonoBehaviour {
	/// <summary>
	/// Holds the single instance of the database
	/// </summary>
	public static Data database;

	/// <summary>
	/// Quicksilver Root dictionary reference
	/// </summary>
	public Dictionary<string, string[]> dictionary;

	/// <summary>
	/// FindTheFish data reference
	/// </summary>
	public FindTheFishData dataFTF;
	/// <summary>
	/// The create environment data reference
	/// </summary>
	public CreateEnvironmentData dataCE;

	/// <summary>
	/// The name of the file (stored in ~/Resources) containing formatted information about the fish & coral.
	/// </summary>
	public TextAsset fileName;

	/// Start this instance.
	/// Any pre initialised data be setup here.
	void Start() {

	}
	/// Awake this instance.
	void Awake () {
		// ensure only one instance of the canvas
		if (database == null) {
			//DontDestroyOnLoad (gameObject);
			database = this;
		} else if (database != this) {
			Destroy(gameObject);
		}

		dictionary = new Dictionary<string, string[]>();
		dataFTF = new FindTheFishData();

		// reinitialise each time as nothing can change at the current state.
		InitialiseQSRoot();

		// QUICKSILVER ROOT SCENE
		if (File.Exists(Application.persistentDataPath + "/quicksilverRootData.dat")) {
			// load the data file
			LoadQS();
		}
		/// FIND THE FISH SCENE
		// if a save file already exists, load it otherwise create and initialise a save file
		//if (File.Exists (Application.persistentDataPath + "/findTheFish.dat")) {
		// load the data file
		//LoadFTF ();
		//} else {
		// initialise the data file (first launch)
		InitialiseFTF();
		//}

	}

	//**********************************************QUICKSILVER ROOT**************************************************//
	/// <summary>
	/// Gets the object from the Quicksilver dictionary
	/// </summary>
	/// <returns>The a string[] containing the information keyed.</returns>
	/// <param name="key">The dictionary key.</param>
	public string[] GetObjectQS(string key) {
		string[] arr = dictionary [key];
		return arr;
	}
	/// <summary>
	/// Puts the object in the Quicksilver dictionary
	/// </summary>
	/// <param name="key">The dictionary key to be stored under.</param>
	/// <param name="strArray">The String array containing the information.</param>
	public void PutObjectQS(string key, string[] strArray) {
		dictionary.Add (key, strArray);
	}

	/// <summary>
	/// Saves the Quicksilver Data to file
	/// </summary>
	public void SaveQS() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/quicksilverRootData.dat");

		bf.Serialize (file, dictionary);
		file.Close ();
	}
	/// <summary>
	/// Loads the Quicksilver Data from file
	/// </summary>
	public void LoadQS() {
		if (File.Exists(Application.persistentDataPath + "/quicksilverRootData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/quicksilverRootData.dat", FileMode.Open);

			dictionary = (Dictionary<string, string[]>)bf.Deserialize(file);
			file.Close();
		}
	}
	/// <summary>
	/// Initialises the QS scenes data.
	/// </summary>
	/// <returns>The QS root.</returns>
	private void InitialiseQSRoot() {
		// split file by lines
		string[] fileLines = fileName.text.Split ('\n');

		// create new array length = to the num of lines
		string[][] fileSplit = new string[fileLines.Length][];
		// temp count for the number of entries for later use
		int numberOfEntries = 0;
		// go through each line, and check if its a comment or an actual line and split respectively
		foreach (string line in fileLines) {
			if (line.StartsWith ("#")) {
				//comments
			} else {
				fileSplit [numberOfEntries] = line.Split ('%');
				numberOfEntries++;
			}
		}
		for (int i = 1; i < numberOfEntries; i++) {
			// create temp array to be stored in the dictionary
			string[] x = { fileSplit [i] [1], fileSplit [i] [2], fileSplit [i] [3] };
			// store the information, using the key from position 0, and the new array from the rest of the information.
			PutObjectQS (fileSplit [i] [0], x);
		}
		SaveQS ();
	}

	//***********************************************FIND THE FISH****************************************************//
	/// <summary>
	/// Initialises the find the fish data, called during the first launch
	/// </summary>
	private void InitialiseFTF() {
		// NOTE 
		// If every fish in the data file = in the game, then implement a setup using the dictionary keys instead
		// of the hard coded names.
		//string[] original =	dictionary.Keys;
		//Dictionary<string, string[]>.KeyCollection keyColl = dictionary.Keys;

		string[] original = {"Sixbar Angelfish", "Bullethead Parrotfish", "Bluespot Butterflyfish", "Clownfish", "Surgeonfish"};
		dataFTF.original = original;
		dataFTF.toFind = original;
		SaveFTF ();
	}
	/// <summary>
	/// Saves the current state of the find the fish data
	/// </summary>
	public void SaveFTF() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/findTheFish.dat");

		bf.Serialize (file, dataFTF);
		file.Close ();
	}
		
	/// <summary>
	/// Loads the find the fish data from file
	/// </summary>
	/// <returns>Returns 0 if error, 1 if successful</returns>
	public int LoadFTF() {
		if (File.Exists (Application.persistentDataPath + "/findTheFish.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/findTheFish.dat", FileMode.Open);

			dataFTF = (FindTheFishData)bf.Deserialize (file);
			file.Close ();
			return 1;
		} else {
			return 0;
		}
	}

	//*********************************************CREATE ENVIRONMENT*************************************************//
	/// <summary>
	/// Saves the current state of the create environment data
	/// </summary>
	public void SaveCE() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/createEnvironment.dat");

		bf.Serialize (file, dataCE);
		file.Close ();
	}

	/// <summary>
	/// Loads the create enviroment data
	/// </summary>
	/// <returns>Returns 0 if error, 1 if successful</returns>
	public int LoadCE() {
		if (File.Exists (Application.persistentDataPath + "/createEnvironment.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/createEnvironment.dat", FileMode.Open);

			dataCE = (CreateEnvironmentData)bf.Deserialize (file);
			file.Close ();

			return 1;
		} else {
			return 0;
		}
	}
}
	

/// <summary>
/// Find the fish game data class.
/// </summary>
[Serializable]
public class FindTheFishData {
	public string[] original;

	public string[] found;
	public string[] toFind;

}
/// <summary>
/// Create Environment game data class
/// </summary>
[Serializable]	
public class CreateEnvironmentData {
	public List<string> type;
	public List<Coordinate> position;
	public List<Rotation> rotation;

}

/// <summary>
/// A seralizable type used to represent a coordinate in a three axis environment.
/// </summary>
[Serializable]
public struct Coordinate {
	public float x;
	public float y;
	public float z;
}
/// <summary>
/// A seralizable type used to represent a rotation in a three axis environment.
/// </summary>
[Serializable]
public struct Rotation {
	public float x;
	public float y;
	public float z;
}