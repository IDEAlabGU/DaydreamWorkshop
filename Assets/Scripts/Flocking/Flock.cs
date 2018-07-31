using UnityEngine;
using System.Collections;

//https://www.youtube.com/watch?v=eMpI1eCsIyM

public class Flock : MonoBehaviour {

	public float speed = 0.1f;
	public float maxSpeed = 0.5f;

	public float rotSpeed = 4.0f;
	public float neighbourDistance = 2.0f;
	public float neighbourPadding = 1f;

	public float chanceToAI = 1;

	public string controllerTag = "Flock - Clownfish";      // This is bad, but whatever :P

	private Vector3 averageHeading;
	private Vector3 averagePosition;

	private GlobalFlock gf;

	// For avoiding things
	private bool hitObject;
	private Vector3 tempPos;

	// Use this for initialization
	void Start () {
		gf = GameObject.FindWithTag(controllerTag).GetComponent<GlobalFlock>();
		speed = Random.Range(0.1f, maxSpeed);
	}

	void OnTriggerEnter(Collider col) {

		if(!hitObject) {
			tempPos = transform.position - col.transform.position;
		}

		hitObject = true;
	}

	void OnTriggerExit(Collider col) {
		hitObject = false;
	}

	// Update is called once per frame
	void Update () {

		// Avoid things
		if(hitObject) {
			Vector3 dir = tempPos - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
		}

		else {

			// Force the fish back to the centre
			float dist = Vector3.Distance(transform.position, gf.targetPostion);
			if (dist > gf.flockRadius) {
				Vector3 dir = gf.targetPostion - transform.position;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
				speed = maxSpeed + (maxSpeed * (dist / 5f));
			}

			else {
				// Apply flock rules sometimes
				if (Random.Range(0, 100) < chanceToAI) {
					ApplyRules();
				}
			}
		}

		// Finally, move
		transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));

	}

	void ApplyRules() {

		GameObject[] allFish = gf.allFish;

		Vector3 centre = Vector3.zero;
		Vector3 avoid = Vector3.zero;
		float gSpeed = 0.1f;
		float dist;

		Vector3 targetPos = gf.targetPostion;

		int groupSize = 0;

		foreach (GameObject go in allFish) {

			// Only apply to all other fish in this flock
			if(go != gameObject) {

				dist = Vector3.Distance(go.transform.position, transform.position);
				if(dist <= neighbourDistance) {

					centre += go.transform.position;
					groupSize++;

					if(dist < neighbourPadding) {
						avoid += (this.transform.position - go.transform.position);
					}

					Flock otherFlock = go.GetComponent<Flock>();
					gSpeed += otherFlock.speed;

				}

			}

			if(groupSize > 0) {

				centre = centre / groupSize + (targetPos - transform.position);
				speed = gSpeed / groupSize;

				Vector3 dir = (centre + avoid) - transform.position;
				if(dir != Vector3.zero) {
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
				}

			}

		}

	}

	public void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawRay(new Ray(transform.position, transform.forward));
	}

}
