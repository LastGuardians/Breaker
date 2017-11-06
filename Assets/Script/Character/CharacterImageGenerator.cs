using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;

public class CharacterImageGenerator: MonoBehaviour
{
	public GameObject Character;
	public GameObject Coin;
	public GameObject Key;
	public GameObject Window;
	public GameObject YesButton;
	public GameObject WindowText;
	public GameObject Canvas;
	public GameObject Key2;
	public GameObject Panel;

	public Sprite[] DigitArray = new Sprite[10];
	public GameObject[] CoinArray;
	public GameObject[] KeyArray;

	public Sprite UnveiledCharacterSprite;

	public BoxCollider CharacterButtonCollider;
	public BoxCollider CancelButtonCollider;

	public string CharacterButtonImageName;

	public static int TargetCharacterIndex;

	public Text CoinText;
	public Text KeyText;

	public Font MainFont;

	public Sprite[] OpenedCharacterSpriteArray = new Sprite[12];

	public static string[] CharacterArray = new string[12] { "Cat", "Frog", "Deer", "Monkey", "Bear", "Alpaca", "Unveiled", "Unveiled", "Unveiled", "Unveiled", "Unveiled", "Unveiled" };
	public static int[] CharacterPriceArray = new int[12] { 100, 100, 100, 100, 100, 100, 0, 0, 0, 0, 0, 0 };

	public GameObject[] CharacterButtonArray = new GameObject[12];
	
	//서버 변수
	public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";
	private string userId;
	private string characterId;

	private void Start()
    {
		DigitArray = Resources.LoadAll<Sprite>("UI/Number/Digits");
		CoinArray = new GameObject[15];
		KeyArray = new GameObject[15];

		userId = GPGSManager.mainId;
		//userId = "TestUser";
		Window.SetActive(false);
		GenerateImage();
		GenerateLabel();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			SceneManager.LoadScene("Main");
		}
	}

	public void GenerateImage()
	{
		OpenedCharacterSpriteArray = Resources.LoadAll<Sprite>("Character/Wanted/Opened");
		UnveiledCharacterSprite = Resources.Load<Sprite>("Character/Wanted/Unveiled/selectUnveiled");

		for (int i = 1; i <= 4; i++)
		{
			for (int j = 1; j <= 3; j++)
			{
				int CharacterIndex = 3 * (i - 1) + j - 1;
				if (UserCharacter.CharacterStatusArray[CharacterIndex] == "Locked")
				{
					Character = new GameObject(string.Format("SelectButton{0}{1}", 3 * (i - 1) + j, "Locked"));
					Character.transform.parent = GameObject.Find("Content").transform;

					Character.AddComponent<Image>();
					Character.GetComponent<Image>().sprite = OpenedCharacterSpriteArray[CharacterIndex];

					Color TempColor = Character.GetComponent<Image>().color;
					TempColor.a = 0.6f;
					Character.GetComponent<Image>().color = TempColor;

					Character.AddComponent<Button>();
					Character.GetComponent<Button>().onClick.AddListener(() => LoadWindow(CharacterIndex));
				}
				else if(UserCharacter.CharacterStatusArray[CharacterIndex] == "Unveiled")
				{
					Character = new GameObject(string.Format("SelectButton{0}{1}", 3 * (i - 1) + j, "Unveiled")); // 버튼 오브젝트 생성
					Character.transform.parent = GameObject.Find("Content").transform; // Content의 자식으로 이동

					Character.AddComponent<Image>();
					Character.GetComponent<Image>().sprite = UnveiledCharacterSprite;

					Character.AddComponent<Button>();
				}
				else
				{
					Character = new GameObject(string.Format("SelectButton{0}", 3 * (i - 1) + j)); // 버튼 오브젝트 생성
					Character.transform.parent = GameObject.Find("Content").transform; // Content의 자식으로 이동

					Character.AddComponent<Image>();
					Character.AddComponent<Button>();

					Character.GetComponent<Button>().onClick.AddListener(() => LoadSelection(CharacterIndex));

					Character.GetComponent<Image>().sprite = OpenedCharacterSpriteArray[CharacterIndex];
				}

				Character.transform.localScale = new Vector3(2.2f, 3.3f, 0);
				Character.transform.localPosition = new Vector3(230 * j - 450, 340 * (5 - i) - 1550, 0);

				CharacterButtonArray[CharacterIndex] = Character;
			}
		}
	}

	public void LoadWindow(int characterIndex)
	{
		Window.SetActive(true);
		WindowText.GetComponent<Text>().text = "\t\t\tX\t" + CharacterPriceArray[characterIndex].ToString();
		YesButton.GetComponent<Button>().onClick.AddListener(() => UnlockCharacter(characterIndex));
		CharacterButtonController(false);
		Window.transform.SetAsLastSibling();
	}

	public void CancelWindow()
	{
		Window.SetActive(false);
		CharacterButtonController(true);
		YesButton.SetActive(true);
		Key2.SetActive(true);
	}

	public void UnlockCharacter(int characterIndex)
	{
		if(UserConnect.KeyAmount >= CharacterPriceArray[characterIndex])
		{
			CancelWindow();

			UserCharacter.CharacterStatusArray[characterIndex] = CharacterArray[characterIndex];
			GameObject TargetCharacterButton = CharacterButtonArray[characterIndex];
			Color TempColor = TargetCharacterButton.GetComponent<Image>().color;
			TempColor.a = 1.0f;
			TargetCharacterButton.GetComponent<Image>().color = TempColor;

			TargetCharacterButton.GetComponent<Button>().onClick.RemoveAllListeners();
			TargetCharacterButton.GetComponent<Button>().onClick.AddListener(() => LoadSelection(characterIndex));

			UserConnect.KeyAmount -= CharacterPriceArray[characterIndex];
			DeleteText(KeyArray);
			GenerateText(UserConnect.KeyAmount.ToString(), 240, 640, KeyArray);

			characterId = "Character" + (characterIndex + 1).ToString();
			CreateUserCharacter();
		}
		else
		{
			Key2.SetActive(false);
			WindowText.GetComponent<Text>().text = "Not enough key! It requires " + CharacterPriceArray[characterIndex].ToString() + " keys";
			YesButton.SetActive(false);
		}
	}

	public void LoadSelection(int CharacterIndex)
	{
		TargetCharacterIndex = CharacterIndex;
		SceneManager.LoadScene("SpecificCharacter");
	}

	public void CharacterButtonController(bool enable)
	{
		if (enable)
		{
			foreach (GameObject button in CharacterButtonArray)
			{
				button.GetComponent<Button>().interactable = true;
			}
		}
		else
		{
			foreach (GameObject button in CharacterButtonArray)
			{
				button.GetComponent<Button>().interactable = false;
			}
		}
	}

	public void GenerateLabel()
	{
		//코인 보유량 라벨 생성
		GenerateText(UserConnect.CoinAmount.ToString(), -30, 640, CoinArray);

		//키 보유량 라벨 생성
		GenerateText(UserConnect.KeyAmount.ToString(), 290, 640, KeyArray);
	}

	public void GenerateText(string targetText, int x, int y, GameObject[] objectList)
	{
		GameObject TargetString = new GameObject("Digits");
		for (int i = 0; i < targetText.Length; i++)
		{
			int TargetDigit = int.Parse(targetText[i].ToString());

			GameObject TargetChar = new GameObject("Digit" + TargetDigit.ToString());
			TargetChar.transform.parent = TargetString.transform;

			TargetChar.transform.localPosition = new Vector3(i * 60, 0, 0);
			TargetChar.AddComponent<Image>();
			TargetChar.GetComponent<Image>().sprite = DigitArray[TargetDigit];
			TargetChar.transform.localScale = new Vector3(0.8f, 0.8f, 0);

			objectList[i] = TargetChar;
		}
		TargetString.transform.parent = Panel.transform;
		TargetString.transform.localScale = new Vector3(0.5f, 0.5f, 0);
		TargetString.GetComponent<Transform>().localPosition = new Vector3(x, y, 0);
	}

	public void DeleteText(GameObject[] objectList)
	{
		foreach (GameObject g in objectList)
		{
			Destroy(g);
		}
	}

	//서버 함수
	public void CreateUserCharacter()
	{
		StartCoroutine(_CreateUserCharacter(userId, characterId));
	}

	public void GetUserCharacters()
	{
		StartCoroutine(_GetUserCharacters(userId));
	}

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
			UserCharacter.CharacterStatusArray[OpenedCharacterIndex] = "Opened";
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


