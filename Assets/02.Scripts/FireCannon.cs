﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour {

    public GameObject cannon = null;
    public Transform firepos;

    private AudioClip fireSfx = null;
    private AudioSource sfx = null;

    private PhotonView pv = null;

    private void Awake()
    {
        cannon = (GameObject)Resources.Load("Cannon");

        fireSfx = Resources.Load<AudioClip>("CannonFire");

        sfx = GetComponent<AudioSource>();
        sfx.volume = 0.1f;
        pv = GetComponent<PhotonView>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // 현재 마우스 커서가 Canvas UI 항목 위에 있으면 Updata 함수를 빠져나감
        //if (MouseHover.instance.isUIHover) return;

		if(pv.isMine && Input.GetMouseButtonDown(0))
        {
            fire();

            pv.RPC("Fire", PhotonTargets.Others, null);
        }
	}

    [PunRPC]
    void fire()
    {
        sfx.PlayOneShot(fireSfx, 1.0f);
        GameObject _cannon = (GameObject)Instantiate(cannon, firepos.position, firepos.rotation);
        _cannon.GetComponent<Cannon>().playerId = pv.ownerId;
    }
}
