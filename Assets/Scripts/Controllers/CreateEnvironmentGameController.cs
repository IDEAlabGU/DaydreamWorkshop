using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * Re-wrote the old Controller
 * 
 * Written by X.Hunt, based on Tyrone's CreateEnvironmentGameController (now called CreateEnvironmentGameController__OLD)
 * 
 * As well as doing everything the old controller did, this controller also:
 * - Allows an object to be placed multiple times
 * - UI elements remain highlighted when selected
 * - If using Kinect, move the object to under the hand
 */

public class CreateEnvironmentGameController : MonoBehaviour {

	public Color canPlaceHighlightColor = Color.blue;
	public Color canNotPlaceHighlightColor = Color.red;
	/// <summary>
	/// The rotation speed of the placed coral
	/// </summary>
	public float rotationSpeed = 50f;
	private float playerRadius = 0.65f;

	/// <summary>
	/// The path to load prefabs from. Should NOT end with a slash
	/// </summary>
	private string prefabPath = "Prefabs/Coral";

    private LayerMask layerMask = -1;

	private string currentSelectedType = "";
	private GameObject currentSelectedGO;

	// References to shaders
	private Shader originalShader;
	private Shader outlinedShader;

	// States
	//     None: No object is selected
	// Selected: Object has been chosen, but not placed
	//  Placing: The single frame that an object is being spawned into the world
	public enum PlacingState {None, Selected, Placing };
	private PlacingState placingState = PlacingState.None;

	private List<Bounds> boundaries = new List<Bounds>();
	private List<GameObject> objects = new List<GameObject>();

	private GameObject currentButton;
	private float alphaStart;
	private float alphaSelected = 1;

	// Use this for initialization
	void Start () {

		originalShader = Shader.Find("Mobile/VertexLit");
		outlinedShader = Shader.Find("Outlined/Silhouetted Diffuse");

	}
	
	// Update is called once per frame
	void Update () {
	
		// Do the thing
		switch(placingState) {

			case PlacingState.None:
				break;

			case PlacingState.Selected:
				updateSelected();
				break;

			case PlacingState.Placing:
				doPlaceObject();
				break;

		}
	}

	#region Select

	/// <summary>
	/// Sets the object toPlace as the object inputted to this function
	/// </summary>
	/// <returns>The selected object type.</returns>
	/// <param name="type">the formatted name of the object</param>
	public void SetSelectedObjectType(string type) {
		
		// If there's currently a selected object, destroy it and set the state
		if(currentSelectedGO) {
			unselect();
		}

		// Get a new object
		currentSelectedType = type;
		currentSelectedGO = Instantiate(SpawnNewObject());
		currentSelectedGO.GetComponent<Collider>().enabled = false;

		// Update state
		placingState = PlacingState.Selected;

		// Update shader
		currentSelectedGO.GetComponent<Renderer>().material.shader = outlinedShader;

	}

	private void updateSelected() {

		Ray ray = new Ray();

		//for daydream, lets let google handle this.
		ray = Camera.main.GetComponent<GvrBasePointerRaycaster>().GetLastRay().ray;

		RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {

				// ensuring menu doesnt get too close to the player
				float hypo = (Mathf.Abs(hit.point.x) * Mathf.Abs(hit.point.x)) + (Mathf.Abs(hit.point.z) * Mathf.Abs(hit.point.z));

				// if outside of menu range/area
				if (hypo <= playerRadius) {
					currentSelectedGO.GetComponent<Renderer>().enabled = false;
				} else if (hit.transform.gameObject.layer == 5 /*UI Layer*/) {
					currentSelectedGO.GetComponent<Renderer>().enabled = false;
				} else {

					currentSelectedGO.GetComponent<Renderer>().enabled = true;

					// Update shader
					if (isValid() && hit.transform.gameObject.layer != layerMask)
						currentSelectedGO.GetComponent<Renderer>().material.SetColor("_OutlineColor", canPlaceHighlightColor);
					else
						currentSelectedGO.GetComponent<Renderer>().material.SetColor("_OutlineColor", canNotPlaceHighlightColor);

					// Update position of object to the reticle position
					currentSelectedGO.transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);

					// Set the rotation of the current object
					var targetPosition = Camera.main.transform.position;
					targetPosition.y = currentSelectedGO.transform.position.y + 10;

					// Rotate over time when selected
					currentSelectedGO.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

					// TODO: Objects need to not clip into things
				}
			} else {
				currentSelectedGO.GetComponent<Renderer>().enabled = false;
			}
	}

#endregion

#region Placing

	/// <summary>
	/// Places the current object the user is interacting with.
	/// </summary>
	/// <returns>The current object.</returns>
	public void PlaceCurrentObject() {
		placingState = PlacingState.Placing;
	}

	// Actually places the object
	private void doPlaceObject() {

		if (currentSelectedGO && isValid()) {

			GameObject toSpawn = (GameObject)Instantiate(currentSelectedGO, currentSelectedGO.transform.position, currentSelectedGO.transform.rotation);

			Collider col = toSpawn.GetComponent<Collider>();
			Renderer ren = toSpawn.GetComponent<Renderer>();

			col.enabled = true;
			ren.material.shader = originalShader;

			objects.Add(toSpawn);
			boundaries.Add(col.bounds);
		}

		placingState = PlacingState.Selected;
	}

	private bool isValid() {
		Collider col = currentSelectedGO.GetComponent<Collider>();
		if (boundaries.Count > 0) {
			foreach (Bounds bound in boundaries) {
				if (bound.Intersects(col.bounds))
					return false;
			}
		}

		return true;
	}

#endregion

#region Helpers

	// Returns a new copy of the specified gameObject
	private GameObject SpawnNewObject(string type) {
		return (GameObject)Resources.Load(prefabPath + "/" + type);
	}

	// No type param means just use the current selected one
	private GameObject SpawnNewObject() {
		return SpawnNewObject(currentSelectedType);
	}

	// Deletes the current object, and set the state back to None
	private void unselect() {
		if (currentSelectedGO) {
			Destroy(currentSelectedGO);
			currentSelectedGO = null;
		}
		placingState = PlacingState.None;
	}

#endregion

	/// <summary>
	/// Undo the last placed item in the scene.
	/// </summary>
	public void Undo() {
		if (objects.Count > 0) {
			Destroy(objects[objects.Count - 1]);
			objects.RemoveAt(objects.Count - 1);
			boundaries.RemoveAt(boundaries.Count - 1);
		}
	}

	/// <summary>
	/// Changes the scene to the scene name specified.
	/// </summary>
	/// <param name="sceneName">the scene name to change to</param>
	public void ChangeScene(string sceneName) {
		GlobalSceneManagement.manager.ChangeScene(sceneName);
	}

#region Buttons

	public void OnButtonClick(GameObject go) {

		Color c;

		// Set the alpha back to unselected first
		if (currentButton) {
			c = currentButton.GetComponent<Image>().color;
			currentButton.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alphaStart);
		}

		// Now apply the new alpha
		currentButton = go;
		alphaStart = currentButton.GetComponent<Image>().color.a;
		c = currentButton.GetComponent<Image>().color;
		currentButton.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alphaSelected);

	}

	public void OnButtonEnter(GameObject go) {
		go.GetComponentInChildren<Text>().enabled = true;
	}

	public void OnButtonExit(GameObject go) {
		go.GetComponentInChildren<Text>().enabled = false;
	}

#endregion
}
