using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankDamage : MonoBehaviour {

    private MeshRenderer[] renderers;

    private GameObject expEffect = null;

    private int initHp = 100;

    private int currHp = 0;

    public Canvas hudCanvas;

    public Image hpBar;

    private PhotonView pv = null;

    public int playerId = -1;

    public int killCount = 0;

    public Text txtKillCount;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();

        currHp = initHp;

        expEffect = Resources.Load<GameObject>("Large Explosion");

        hpBar.color = Color.green;

        pv = GetComponent<PhotonView>();

        playerId = pv.ownerId;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(currHp > 0 && coll.GetComponent<Collider>().tag == "CANNON")
        {
            currHp -= 20;

            // 현재 생명치 백분율 = (현재 생명치) / (초기 생명치)
            hpBar.fillAmount = (float)currHp / (float)initHp;

            // 생명 수치에 따라 filled 이미지의 색상을 변경
            if (hpBar.fillAmount <= 0.4f)
                hpBar.color = Color.red;
            else if (hpBar.fillAmount <= 0.6f)
                hpBar.color = Color.yellow;
            
            if(currHp <= 0)
            {
                // 자신을 파괴시킨 적 탱크를 검색해 스코어를 증가시키는 함수
                SaveKillCount(coll.GetComponent<Cannon>().playerId);
                StartCoroutine(this.ExplosionTank());
            }
        }
    }

    IEnumerator ExplosionTank()
    {
        Object effect = GameObject.Instantiate(expEffect, transform.position, Quaternion.identity);

        Destroy(effect, 3.0f);

        hudCanvas.enabled = false;

        SetTankVisible(false);

        yield return new WaitForSeconds(3.0f);

        hpBar.fillAmount = 1.0f;

        hpBar.color = Color.green;

        hudCanvas.enabled = true;

        currHp = initHp;

        SetTankVisible(true);
    }

    void SetTankVisible(bool isVisible)
    {
        foreach(MeshRenderer _renderer in renderers)
        {
            _renderer.enabled = isVisible;
        }
    }

	// 자신을 파괴시킨 적 탱크를 검색해 스코어를 증가시키는 함수
	void SaveKillCount(int firePlayerId)
    {
        // Tank 태그로 지정된 모든 탱크를 가져와 배열에 저장
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");

        foreach(GameObject tank in tanks)
        {
            var tankDamage = tank.GetComponent<TankDamage>();

            if (tankDamage != null && tankDamage.playerId == firePlayerId)
            {
                tankDamage.IncKillCount();
                break;
            }
        }
    }

    void IncKillCount()
    {
        ++killCount;
        txtKillCount.text = killCount.ToString();

        if(pv.isMine){
            PhotonNetwork.player.AddScore(1);

            StartCoroutine(DataMgr.instance.SaveScore(PhotonNetwork.player.name, 1));
        }
    }
}
