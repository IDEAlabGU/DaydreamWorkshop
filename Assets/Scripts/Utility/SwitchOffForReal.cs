using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOffForReal : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
        //keep on
#else
        gameObject.SetActive(false);
        //switch off
#endif

    }

    // Update is called once per frame
    void Update () {
		
	}
}
