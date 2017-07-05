using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour {

    public GameObject cannon = null;
    public Transform firepos;

    private AudioClip fireSfx = null;
    private AudioSource sfx = null;

    private void Awake()
    {
        cannon = (GameObject)Resources.Load("Cannon");

        fireSfx = Resources.Load<AudioClip>("CannonFire");

        sfx = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            fire();
        }
	}

    void fire()
    {
        sfx.PlayOneShot(fireSfx, 1.0f);
        Instantiate(cannon, firepos.position, firepos.rotation);
    }
}
