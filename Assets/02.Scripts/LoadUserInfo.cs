﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class LoadUserInfo : MonoBehaviour {

    public TextAsset jsonData = null;
    public string strJsonData = null;



	// Use this for initialization
	void Start () {
        jsonData = Resources.Load<TextAsset>("user_info");

        strJsonData = jsonData.text;

        var N = JSON.Parse(strJsonData);

        string user_name = N["이름"].ToString();
        int level = N["능력치"]["레벨"].AsInt;

        Debug.Log(user_name);
        Debug.Log(level.ToString());

        for (int i = 0; i < N["보유스킬"].Count;i++)
        {
            Debug.Log(N["보유스킬"][i].ToString());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
