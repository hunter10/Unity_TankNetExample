using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour {

    private Transform tr;
    private Transform mainCameraTr;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
        mainCameraTr = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        tr.LookAt(mainCameraTr);
	}
}
