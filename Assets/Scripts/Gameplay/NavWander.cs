/*
 * Copyright (C) 2016 Team To Be Created - All Rights Reserved
 * Unauthorized copying or modifying of this file, via any medium is strictly prohibited
 * For licensing or use contact the author of this file.
 * 
 * Written by Tyrone Ranatunga <tyrone.ranatunga@griffithuni.edu.au>, October 2016
 * 
 * Edited by X.Hunt
 *	- Shows target position as a gizmo
 *	- Doesnt complain when there's a navmeshLink
 *	- Adjusts speed when on a link
 * 
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// This script allows an object on a navigation mesh to wander around.
/// 
/// It requires an object to be placed on an object and can be configured by the length of each wander, and the distance
/// of each wander.
/// </summary>
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NavWander : MonoBehaviour {
	/// <summary>
	/// The radius from the objects current position where it can wander to.
	/// </summary>
	public float wanderRadius;
	/// <summary>
	/// The time it can wander to its new position.
	/// </summary>
	public float wanderTimer;
	/// <summary>
	/// Is the object currently wandering. Can be set by other scripts to stop the object.
	/// </summary>
	public bool wandering = true;

	/// <summary>
	/// The target position for the object
	/// </summary>
	private Transform target;
	/// <summary>
	/// The navigation agent attatched to the object
	/// </summary>
	private UnityEngine.AI.NavMeshAgent agent;
	/// <summary>
	/// The current time
	/// </summary>
	private float timer;

	// Dynamic navlink speed things
	private float navLinkSpeedMod = 3f;		// 3 works nicely, but isn't perfect
	private float realSpeed;
	private float navSpeed;

	// Use this for initialization
	void OnEnable () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		timer = wanderTimer;
		navSpeed = agent.speed / navLinkSpeedMod;
		realSpeed = agent.speed;
	}

	// Update is called once per frame
	void Update () {
		if (wandering == true) {
			// update timer
			timer += Time.deltaTime;

			// X: Adjust the speed if we're on a navmesh. Unity is weird sometimes...
			if (agent.isOnOffMeshLink) {
				UnityEngine.AI.OffMeshLinkData link = agent.currentOffMeshLinkData;
				float dist = Vector3.Distance(link.startPos, link.endPos);
				agent.speed = navSpeed * dist;
			}
			else
				agent.speed = realSpeed;

			// check if the object needs a path and provide it with one if need be.
			if (!agent.hasPath && !agent.isOnOffMeshLink) {
				Vector3 newPos = RandomNavSphere (transform.position, wanderRadius, -1);
				agent.SetDestination (newPos);
				timer = 0;
			}
			if (timer >= wanderTimer && !agent.isOnOffMeshLink) {
				Vector3 newPos = RandomNavSphere (transform.position, wanderRadius, -1);
				agent.SetDestination (newPos);
				timer = 0;
			}
		}
	}
	/// <summary>
	/// Generates a random position within the wandering radius for the object.
	/// </summary>
	/// <returns>A random position within the radius</returns>
	/// <param name="origin">The position of the object</param>
	/// <param name="dist">The distance the object can travel</param>
	/// <param name="layermask">The layermask</param>
	private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {

		// get a random position
		Vector3 randDirection = Random.insideUnitSphere * dist;
		// add it to its current position
		//randDirection += origin;

		// configure the navigation
		UnityEngine.AI.NavMeshHit navHit;
		UnityEngine.AI.NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
		// return the configured navigation
		return navHit.position;
	}

	void OnDrawGizmosSelected() {
		if (agent) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(agent.destination, 0.1f);
		}
	}

}