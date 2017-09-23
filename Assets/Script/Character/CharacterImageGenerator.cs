using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterImageGenerator: MonoBehaviour
{
	public GameObject Character;
	public GameObject Coin;
	public GameObject Key;
	public GameObject Window;
	public GameObject YesButton;
	public GameObject WindowText;

	public Sprite UnveiledCharacterSprite;

	public BoxCollider CharacterButtonCollider;
	public BoxCollider CancelButtonCollider;

	public string CharacterButtonImageName;

	public static int CoinAmount = 100000000;
	public static int KeyAmount = 500;
	public static int TargetCharacterIndex;

	public Text CoinText;
	public Text KeyText;

	public Font MainFont;

	public Sprite[] OpenedCharacterSpriteArray = new Sprite[12];

	public static string[] CharacterArray = new string[12] { "Cat", "Frog", "Deer", "Monkey", "Bear", "Alpaca", "Unveiled", "Unveiled", "Unveiled", "Unveiled", "Unveiled", "Unveiled" };
	public static string[] CharacterStatusArray = new string[12] { "Opened", "Opened", "Opened", "Opened", "Opened", "Locked", "Unveiled", "Unveiled", "Unveiled", "Unveiled", "Unveiled", "Unveiled" };
	public static int[] CharacterPriceArray = new int[12] { 100, 100, 100, 100, 100, 100, 0, 0, 0, 0, 0, 0 };

	public GameObject[] CharacterButtonArray = new GameObject[12];


	void Start()
    {
		Window = GameObject.Find("Window");
		YesButton = GameObject.Find("YesButton");
		WindowText = GameObject.Find("WindowText");

		Window.SetActive(false);

		GenerateImage();
		GenerateLabel();
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
				if (CharacterStatusArray[CharacterIndex] == "Locked")
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
				else if(CharacterStatusArray[CharacterIndex] == "Unveiled")
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
				Debug.Log(CharacterButtonArray[CharacterIndex]);
			}
		}
	}

	public void LoadWindow(int characterIndex)
	{
		Window.SetActive(true);

		YesButton.GetComponent<Button>().onClick.AddListener(() => UnlockCharacter(characterIndex));
		CharacterButtonController(false);
	}

	public void CancelWindow()
	{
		Window.SetActive(false);
		CharacterButtonController(true);
		WindowText.GetComponent<Text>().text = "Would you like to unlock the character?";
		YesButton.SetActive(true);
	}

	public void UnlockCharacter(int characterIndex)
	{
		if(KeyAmount >= CharacterPriceArray[characterIndex])
		{
			CancelWindow();

			CharacterStatusArray[characterIndex] = CharacterArray[characterIndex];
			GameObject TargetCharacterButton = CharacterButtonArray[characterIndex];
			Color TempColor = TargetCharacterButton.GetComponent<Image>().color;
			TempColor.a = 1.0f;
			TargetCharacterButton.GetComponent<Image>().color = TempColor;

			TargetCharacterButton.GetComponent<Button>().onClick.RemoveAllListeners();
			TargetCharacterButton.GetComponent<Button>().onClick.AddListener(() => LoadSelection(characterIndex));

			KeyAmount -= CharacterPriceArray[characterIndex];
			Key.GetComponent<Text>().text = KeyAmount.ToString();
		}
		else
		{
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
		KeyAmount = 10;
		CoinAmount = 100;

		//코인 보유량 라벨 생성
		Coin = new GameObject("CoinAmountText");
		Coin.transform.parent = GameObject.Find("Canvas").transform;
		Coin.transform.localPosition = new Vector3(-20, 540, 0);

		CoinText = Coin.AddComponent<Text>();
		CoinText.font = MainFont;
		CoinText.fontSize = 40;
		CoinText.text = CoinAmount.ToString();
		CoinText.alignment = TextAnchor.UpperCenter;

		Coin.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

		//키 보유량 라벨 생성
		Key = new GameObject("KeyAmountText");
		Key.transform.parent = GameObject.Find("Canvas").transform;
		Key.transform.localPosition = new Vector3(270, 540, 0);

		KeyText = Key.AddComponent<Text>();
		KeyText.font = MainFont;
		KeyText.fontSize = 40;
		KeyText.text = KeyAmount.ToString();
		KeyText.alignment = TextAnchor.UpperCenter;

		Key.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
	}
}


