using UnityEngine;
using System.Collections;
using LitJson;

public class UserWeapon : MonoBehaviour
{
    public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";
    private string userId;
	private string weaponId = "Weapon1";
    private int damageLevel = 0;
    private int criticalLevel = 0;
    private int probabilityLevel = 0;

	public static string[] WeaponStatusArray = new string[3] { "Locked", "Locked", "Locked" };
	public static int[,] WeaponAbilityArray = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

	void Start()
    {
		DontDestroyOnLoad(this);
		userId = GPGSManager.mainId;
		CreateUserWeapon();
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
		GetUserWeapons();
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
			int OpenedWeaponIndex = int.Parse(json[i]["weaponId"].ToString().Substring(6)) - 1;
			damageLevel = int.Parse(json[i]["damageLevel"].ToString());
			criticalLevel = int.Parse(json[i]["criticalLevel"].ToString());
			probabilityLevel = int.Parse(json[i]["probabilityLevel"].ToString());

			WeaponStatusArray[OpenedWeaponIndex] = "Opened";
			WeaponAbilityArray[OpenedWeaponIndex, 0] = damageLevel;
			WeaponAbilityArray[OpenedWeaponIndex, 1] = criticalLevel;
			WeaponAbilityArray[OpenedWeaponIndex, 2] = probabilityLevel;
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
		GetUserWeapons();
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
