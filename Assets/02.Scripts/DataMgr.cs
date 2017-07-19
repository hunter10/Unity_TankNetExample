using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class DataMgr : MonoBehaviour {

    public static DataMgr instance = null;

    // MySQL 데이터베이스 사용을 위해 부여된 고유번호
    private const string seqNo = "3201411232";

    // 점수 저장 PHP 주소
    private string urlSave = "http://wwww.Unity3dStudy.com/TankWark/save_score.php";

    // 랭킹 정보를 요청하기 위한 php주
    private string urlScoreList = "http://wwww.Unity3dStudy.com/TankWark/get_score_list.php";

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator SaveScore(string user_name, int killCount)
    {
        // POST방식으로 인자를 전달하기 위한 FORM선언 
        WWWForm form = new WWWForm();

        // 전달할 파라미터 설정 
        form.AddField("user_name", user_name);
        form.AddField("kill_count", killCount);
        form.AddField("seq_no", seqNo);

        // URL호출
        var www = new WWW(urlSave, form);

        yield return www;

        if(string.IsNullOrEmpty(www.error)){
            Debug.Log(www.text);
        } else {
            Debug.Log("Error : " + www.error);
        }

        // 점수 저장 후 랭킹 정보 요청을 위한 코루틴 함수 호출
        StartCoroutine(this.GetScoreList());
    }

    public IEnumerator GetScoreList()
    {
		// POST방식으로 인자를 전달하기 위한 FORM선언 
		WWWForm form = new WWWForm();

        form.AddField("seq_no", seqNo);

		// URL호출
        var www = new WWW(urlScoreList, form);

		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			Debug.Log(www.text);
            DispScoreList(www.text);
		}
		else
		{
			Debug.Log("Error : " + www.error);
		}
    }

    // JSON파일을 파싱한 후 점수를 표시하는 함수
    void DispScoreList(string strJsonData)
    {
        var N = JSON.Parse(strJsonData);

        for (int i = 0; i < N.Count; i++)
        {
            int ranking = N[i]["ranking"].AsInt;
            string userName = N[i]["user_name"].ToString();
            int killCount = N[i]["kill_count"].AsInt;

            // 결과값을 Console뷰에 표시
            Debug.Log(ranking.ToString() + userName + killCount.ToString());
        }
    }
}
