using UnityEngine;
using System.Collections;

public class FlockTarget : MonoBehaviour {

	public Transform cameraTransform;
	public float targetDist = 5f;
	public Vector3 target;
	public GlobalFlock gf;

	void Start() {
		gf = GetComponent<GlobalFlock>();
	}

	// Update is called once per frame
	void Update () {
		gf.targetPostion = transform.position;

	}
}
