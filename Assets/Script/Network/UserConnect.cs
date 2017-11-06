using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class UserConnect : MonoBehaviour
{
	public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";
	private string userId;
	public static int CoinAmount = 100000;
	public static int KeyAmount = 100;
	public static int CurrentCharacterIndex;
	public static int CurrentWeaponIndex;

	void Start()
	{
		DontDestroyOnLoad(this);
		userId = GPGSManager.mainId;
		CreateUser();
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
		string weaponId = "Weapon" + (UserConnect.CurrentWeaponIndex + 1).ToString();
		string characterId = "Character" + (UserConnect.CurrentCharacterIndex + 1).ToString();
		StartCoroutine(_SetUser(userId, UserConnect.CoinAmount, UserConnect.KeyAmount, weaponId, characterId));
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
		GetUser();
	}

	/* 유저 조회 */
	public IEnumerator _GetUser(string userId)
	{
		Debug.Log(userId);
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

		CoinAmount = int.Parse(json["coin"].ToString());
		KeyAmount = int.Parse(json["prisonKey"].ToString());

		try
		{
			CurrentWeaponIndex = int.Parse(json["weaponId"].ToString().Substring(6)) - 1;
			CurrentCharacterIndex = int.Parse(json["characterId"].ToString().Substring(9)) - 1;
		}
		catch
		{
			CurrentWeaponIndex = 0;
			CurrentCharacterIndex = 0;
		}
	}

	/* 유저 갱신 */
	public IEnumerator _SetUser(string userId, int coin, int prisonKey, string weaponId, string characterId)
	{
		string url = baseUrl + "/user/" + userId + "/update";
		WWWForm form = new WWWForm();
		form.headers["content-type"] = "application/json";
		form.AddField("coin", coin);
		form.AddField("prisonKey", prisonKey);
		form.AddField("weaponId", weaponId);
		form.AddField("characterId", characterId);

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
