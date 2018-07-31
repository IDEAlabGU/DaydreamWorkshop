using UnityEngine;
using System.Collections;

public class GlobalFlock : MonoBehaviour {

	public GameObject fish;
	public int sizeOfFlock = 10;

	public GameObject[] allFish;

	public float flockRadius = 10f;
	public float flockHeight = 3f;

	public Vector3 targetPostion;

	// Use this for initialization
	void Start () {

		allFish = new GameObject[sizeOfFlock];

		// Instatiate each fish. This might be taxing
		for(int i = 0; i < sizeOfFlock; i++) {

			// Each fish is given a random location inside the flockDimensions, centered on this GO position
			Vector3 pos = new Vector3(
				(transform.position.x - flockRadius / 2) + Random.Range(0, flockRadius),
				 transform.position.y,
				(transform.position.z - flockRadius / 2) + Random.Range(0, flockRadius)
				);

			allFish[i] = (GameObject) Instantiate(fish, pos, Quaternion.identity);
			//allFish[i].transform.SetParent(transform);

		}

	}

	// Update is called once per frame
	void Update() {
		targetPostion = transform.position;
	}

	void OnDrawGizmos() {

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position, 0.2f);
		Gizmos.DrawWireSphere(transform.position, flockRadius);

	}
}
