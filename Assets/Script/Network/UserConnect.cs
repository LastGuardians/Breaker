using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class UserConnect : MonoBehaviour
{
    public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";
    public string userId = "TestUser";
    public int coin = 0;
    public int prisonKey = 0;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void CreateUser()
    {
        StartCoroutine(_CreateUser(userId));
    }

    public void GetUser()
    {
        StartCoroutine(_GetUser(userId));
    }

    public void SetUser()
    {
        StartCoroutine(_SetUser(userId, coin, prisonKey));
    }

    public void DeleteUser()
    {
        StartCoroutine(_DeleteUser(userId));
    }

    /* 유저 생성 */
    public IEnumerator _CreateUser(string userId)
    {
        string url = baseUrl + "/user/create";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("userId", userId);

        WWW www = new WWW(url, form);
        yield return www;

        Debug.Log(www.isDone);
        PrintLog(www.error);
    }

    /* 유저 조회 */
    public IEnumerator _GetUser(string userId)
    {
        string url = baseUrl + "/user/" + userId;
        WWW www = new WWW(url);
        yield return www;

        PrintLog(www.error);

        if (www.error != null)   // 조회한 유저가 없음.
        {
            Debug.Log("조회한 유저가 없음");
            StartCoroutine(_CreateUser(userId));
            //StartCoroutine(GameObject.Find("NetworkManager").GetComponent<UserWeapon>()
            //    ._CreateUserWeapon(userId, "defaultWeapon"));
        }
        else if (www.isDone)    // 조회한 유저가 있음.
        {

        }

        JsonData json = JsonMapper.ToObject(www.text);
        JsonData name = json["userId"];
        Debug.Log("name : " + name);
        PrintLog(json["userId"] + " " + json["coin"] + " " + json["prisonKey"]);
    }

    /* 유저 갱신 */
    public IEnumerator _SetUser(string userId, int coin, int prisonKey)
    {
        string url = baseUrl + "/user/" + userId + "/update";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("coin", coin);
        form.AddField("prisonKey", prisonKey);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }


    /* 유저 삭제 */
    public IEnumerator _DeleteUser(string userId)
    {
        string url = baseUrl + "/user/" + userId + "/delete";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("padding", 0);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }

    void PrintLog(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Debug.Log(message);
        }
    }
}
