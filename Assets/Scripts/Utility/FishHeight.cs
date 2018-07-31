using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class FishHeight : MonoBehaviour {

	public float minHeight = 0.1f;
	public float maxHeight = 0.01f;
	public float speed = 1f;

	private UnityEngine.AI.NavMeshAgent agent;

	void Start() {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	void Update() {
		if(Random.Range(0, 100) < 5) {
			setHeight(Random.Range(minHeight, maxHeight));
		}
	}

	public void setHeight(float newHeight) {
		StartCoroutine(doSetHeight(newHeight));
	}

	private IEnumerator doSetHeight(float newHeight) {

		float h = newHeight - agent.baseOffset;
		int dir = h > 0 ? 1 : -1;
		h = Mathf.Abs(h);

		while (h > 0) {

			agent.baseOffset = Mathf.Lerp(agent.baseOffset, h, Time.deltaTime * speed * dir);
			h -= Time.deltaTime * speed;

			yield return new WaitForEndOfFrame();
		}
	}

}
