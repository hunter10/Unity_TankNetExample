using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public float speed = 6000.0f;

    public GameObject expEffect;

    private CapsuleCollider _collider;
    private Rigidbody _rigidbody;

    // 포탄을 발사한 플레이어의 ID 저장
    public int playerId = -1;

	// Use this for initialization
	void Start () {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().AddForce(transform.forward * speed);

        StartCoroutine(this.ExplosionCannon(3.0f));
	}

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(this.ExplosionCannon(0.0f));
    }

    IEnumerator ExplosionCannon(float tm)
    {
        yield return new WaitForSeconds(tm);

        _collider.enabled = false;

        _rigidbody.isKinematic = true;

        GameObject obj = (GameObject)Instantiate(expEffect, transform.position, Quaternion.identity);

        Destroy(obj, 1.0f);

        Destroy(this.gameObject, 1.0f);
    }
}
