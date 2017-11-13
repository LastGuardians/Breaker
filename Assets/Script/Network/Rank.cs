using UnityEngine;
using System.Collections;
using LitJson;

public class Rank : MonoBehaviour
{
    public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";
    private string userId;
    public int score = 0;
    public int offset = 0;
    public int limit = 5;

    public static Rank instance = null;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //잘못된 인스턴스를 가르키고 있을 경우
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
		userId = GPGSManager.mainId;
	}
	public void CreateRank()
    {
        StartCoroutine(_CreateRank(userId, score));
    }

    public void GetRank()
    {
        StartCoroutine(_GetRank(offset, limit));
    }

    /* 랭킹 생성 */
    public IEnumerator _CreateRank(string userId, int score)
    {
        string url = baseUrl + "/rank/create";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("userId", userId);
        form.AddField("score", score);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }

    /* 랭킹 조회 */
    public IEnumerator _GetRank(int offset, int limit)
    {
        string url = baseUrl + "/rank?offset=" + offset + "&limit=" + limit;
        WWW www = new WWW(url);
        yield return www;

        PrintLog(www.error);
        PrintLog(www.text);

        JsonData json = JsonMapper.ToObject(www.text);
        for (int i = 0; i < json.Count; i++)
        {
            PrintLog(i + 1 + " " + json[i]["score"] + " " + json[i]["userId"]);
        }
    }

    void PrintLog(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Debug.Log(message);
        }
    }
}
