using UnityEngine;
using System.Collections;
using LitJson;

public class UserWeapon : MonoBehaviour
{
    public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";
    private string userId = "TestUser";
    private string weaponId = "TestWeapon";
    int damageLevel = 1;
    int criticalLevel = 1;
    int probabilityLevel = 1;

    void Start()
    {
        DontDestroyOnLoad(this);
		DeleteUserWeapon();
		GetUserWeapons();
    }

    public void CreateUserWeapon()
    {
        StartCoroutine(_CreateUserWeapon(userId, weaponId));
    }

    public void GetUserWeapons()
    {
        StartCoroutine(_GetUserWeapons(userId));
    }

    public void SetUserWeapon()
    {
        StartCoroutine(_SetUserWeapon(userId, weaponId, damageLevel, criticalLevel, probabilityLevel));
    }

    public void DeleteUserWeapon()
    {
        StartCoroutine(_DeleteUserWeapon(userId, weaponId));
    }

    /* 무기 생성 */
    public IEnumerator _CreateUserWeapon(string userId, string weaponId)
    {
        string url = baseUrl + "/user/" + userId + "/weapons/create";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("weaponId", weaponId);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }

    /* 무기 조회 */
    public IEnumerator _GetUserWeapons(string userId)
    {
        string url = baseUrl + "/user/" + userId + "/weapons";
        WWW www = new WWW(url);
        yield return www;

        PrintLog(www.error);
        PrintLog(www.text);

        JsonData json = JsonMapper.ToObject(www.text);
        for (int i = 0; i < json.Count; i++)
        {
            PrintLog(json[i]["userId"] + " " + json[i]["weaponId"] + " " + json[i]["damageLevel"] + " " + json[i]["criticalLevel"] + " " + json[i]["probabilityLevel"]);
        }
    }

    /* 무기 갱신 */
    public IEnumerator _SetUserWeapon(string userId, string weaponId, int damageLevel, int criticalLevel, int probabilityLevel)
    {
        string url = baseUrl + "/user/" + userId + "/weapons/" + weaponId + "/update";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("damageLevel", damageLevel);
        form.AddField("criticalLevel", criticalLevel);
        form.AddField("probabilityLevel", probabilityLevel);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }


    /* 무기 삭제 */
    public IEnumerator _DeleteUserWeapon(string userId, string weaponId)
    {
        string url = baseUrl + "/user/" + userId + "/weapons/" + weaponId + "/delete";
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
