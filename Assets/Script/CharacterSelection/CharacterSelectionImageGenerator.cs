using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionImageGenerator : MonoBehaviour
{
	public GameObject Character;
	public GameObject Canvas;
	public GameObject Explanation;
	GameObject CheckeButton;

	public Sprite onButton;
	public Sprite offButton;

	public RuntimeAnimatorController CharacterAnimatorController;

	//public Sprite[] CharacterImageArray = new Sprite[12];
	public Sprite[] ExplanationArray = new Sprite[6];

	public string AnimatorPath;
	public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";

	public string userId;

	public void Start()
	{
		userId = GPGSManager.mainId;
		//userId = "TestUser";
		ExplanationArray = Resources.LoadAll<Sprite>("Character/Explanation");
		onButton = Resources.Load<Sprite>("UI/onButton");
		offButton = Resources.Load<Sprite>("UI/offButton");
		Canvas = GameObject.Find("Canvas");
		GenerateSelectionImage();
		SetExplanation();
		GenerateCheckButton();
		ChangeCheckButton(CharacterImageGenerator.TargetCharacterIndex);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			SceneManager.LoadScene("Character");
		}
	}

	public void GenerateSelectionImage()
	{
		// 캐릭터 이미지 오브젝트 생성
		Character = new GameObject("Character");
		Character.transform.parent = Canvas.transform; // Canvas의 자식으로 이동
		Character.AddComponent<Image>();

		Rescale(Character, CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex]);

		Character.AddComponent<Animator>();
		AnimatorPath = "Animation/Animator/" + CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex] + "Animator";
		Character.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(AnimatorPath);

		Character.transform.SetSiblingIndex(2);
	}

	public void Rescale(GameObject TargetObject, string TargetCharacter)
	{
		if (TargetCharacter == "Deer")
		{
			TargetObject.transform.localScale = new Vector3(10, 9, 0);
			TargetObject.transform.localPosition = new Vector3(0, 190, 0);
		}
		else if (TargetCharacter == "Monkey")
		{
			TargetObject.transform.localScale = new Vector3(12, 18, 0);
			TargetObject.transform.localPosition = new Vector3(0, 120, 0);
		}
		else if (TargetCharacter == "Bear")
		{
			TargetObject.transform.localScale = new Vector3(9, 11, 0);
			TargetObject.transform.localPosition = new Vector3(0, 260, 0);
		}
		else
		{
			TargetObject.transform.localScale = new Vector3(10, 9, 0);
			TargetObject.transform.localPosition = new Vector3(0, 50, 0);
		}
	}

	public void ChangeIndex(string TargetButtonName)
	{
		if (TargetButtonName == "Left")
		{
			do
			{
				CharacterImageGenerator.TargetCharacterIndex = ((CharacterImageGenerator.TargetCharacterIndex - 1) % 12 + 12) % 12;
			}
			while (UserCharacter.CharacterStatusArray[CharacterImageGenerator.TargetCharacterIndex] == "Locked" || UserCharacter.CharacterStatusArray[CharacterImageGenerator.TargetCharacterIndex] == "Unveiled");
			Debug.Log(CharacterImageGenerator.TargetCharacterIndex);
		}
		else if (TargetButtonName == "Right")
		{
			do
			{
				CharacterImageGenerator.TargetCharacterIndex = ((CharacterImageGenerator.TargetCharacterIndex + 1) % 12 + 12) % 12;
			}
			while (UserCharacter.CharacterStatusArray[CharacterImageGenerator.TargetCharacterIndex] == "Locked" || UserCharacter.CharacterStatusArray[CharacterImageGenerator.TargetCharacterIndex] == "Unveiled");
		}
		ChangeImage(TargetButtonName);
		ChangeCheckButton(CharacterImageGenerator.TargetCharacterIndex);
	}

	public void ChangeImage(string ClickedButton)
	{
		Character.GetComponent<Animator>().SetBool(ClickedButton, true);
		AnimatorPath = "Animation/Animator/" + CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex] + "Animator";
		Character.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(AnimatorPath);
		Rescale(Character, CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex]);
		SetExplanation();

	}

	public void SetExplanation()
	{
		Explanation.GetComponent<Image>().sprite = ExplanationArray[CharacterImageGenerator.TargetCharacterIndex];
	}

	public void GenerateCheckButton()
	{
		CheckeButton = new GameObject("CheckButton");
		CheckeButton.transform.parent = Canvas.transform;

		CheckeButton.transform.localScale = new Vector3(1.0f, 1.0f, 0);
		CheckeButton.transform.localPosition = new Vector3(200, -150, 0);

		CheckeButton.AddComponent<Button>();
		CheckeButton.AddComponent<Image>();
	}

	public void ChangeCheckButton(int targetcharacterIndex)
	{
		CheckeButton.GetComponent<Button>().onClick.RemoveAllListeners();
		CheckeButton.GetComponent<Button>().onClick.AddListener(() => ChangeCurrentIndex(targetcharacterIndex));
		if (targetcharacterIndex == UserConnect.CurrentCharacterIndex)
		{
			CheckeButton.GetComponent<Image>().sprite = onButton;
		}
		else
		{
			CheckeButton.GetComponent<Image>().sprite = offButton;
		}
	}

	public void ChangeCurrentIndex(int targetcharacterIndex)
	{
		UserConnect.CurrentCharacterIndex = targetcharacterIndex;
		CheckeButton.GetComponent<Image>().sprite = onButton;
		SetUser();
	}

	//서버파트
	public void SetUser()
	{
		string weaponId = "Weapon" + (UserConnect.CurrentWeaponIndex + 1).ToString();
		string characterId = "Character" + (UserConnect.CurrentCharacterIndex + 1).ToString();
		StartCoroutine(_SetUser(userId, UserConnect.CoinAmount, UserConnect.KeyAmount, weaponId, characterId));
	}

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

	public void PrintLog(string message)
	{
		if (!string.IsNullOrEmpty(message))
		{
			Debug.Log(message);
		}
	}
}


