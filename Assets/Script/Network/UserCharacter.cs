using UnityEngine;
using System.Collections;
using LitJson;

public class UserCharacter : MonoBehaviour
{
    private string userId = "TestUser";
    private string characterId = "Character1";
    private string skillId = "TestSkill";
    public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";

	public static string[] CharacterStatusArray = new string[12] { "Locked", "Locked", "Locked", "Locked", "Locked", "Locked", "Unveiled", "Unveiled", "Unveiled", "Unveiled", "Unveiled", "Unveiled" };

	void Start()
    {
        DontDestroyOnLoad(this);
		CreateUserCharacter();
		//DeleteUserCharacter();
		//GetUserCharacters();
    }

    public void CreateUserCharacter()
    {
        StartCoroutine(_CreateUserCharacter(userId, characterId));
    }

    public void GetUserCharacters()
    {
        StartCoroutine(_GetUserCharacters(userId));
    }

    public void CreateUserCharacterSkill()
    {
        StartCoroutine(_CreateUserCharacterSkill(userId, characterId, skillId));
    }

    public void DeleteUserCharacterSkill()
    {
        StartCoroutine(_DeleteUserCharacterSkill(userId, characterId, skillId));
    }

    public void DeleteUserCharacter()
    {
        StartCoroutine(_DeleteUserCharacter(userId, characterId));
    }

    /* 캐릭터 생성 */
    public IEnumerator _CreateUserCharacter(string userId, string characterId)
    {
        string url = baseUrl + "/user/" + userId + "/characters/create";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("characterId", characterId);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);

		GetUserCharacters();
	}

    /* 캐릭터 조회 */
    public IEnumerator _GetUserCharacters(string userId)
    {
        string url = baseUrl + "/user/" + userId + "/characters";
        WWW www = new WWW(url);
        yield return www;

        PrintLog(www.error);
        PrintLog(www.text);

        JsonData json = JsonMapper.ToObject(www.text);
        for (int i = 0; i < json.Count; i++)
        {
            int OpenedCharacterIndex = int.Parse(json[i]["characterId"].ToString().Substring(9)) - 1;
			CharacterStatusArray[OpenedCharacterIndex] = "Opened";
        }
    }

    /* 캐릭터 스킬 추가 */
    public IEnumerator _CreateUserCharacterSkill(string userId, string characterId, string skillId)
    {
        string url = baseUrl + "/user/" + userId + "/characters/" + characterId + "/skill/create";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("skillId", skillId);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }

    /* 캐릭터 스킬 삭제 */
    public IEnumerator _DeleteUserCharacterSkill(string userId, string characterId, string skillId)
    {
        string url = baseUrl + "/user/" + userId + "/characters/" + characterId + "/skills/" + skillId + "/delete";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("padding", 0);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }


    /* 캐릭터 삭제 */
    public IEnumerator _DeleteUserCharacter(string userId, string characterId)
    {
        string url = baseUrl + "/user/" + userId + "/characters/" + characterId + "/delete";
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
