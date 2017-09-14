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
	public GameObject YesOrNoFrame;
	public GameObject YesOrNoWindow;
	public GameObject CancelButton;
	public GameObject Message;

	public Image CharacterImage;
	public Image YesOrNoWindowImage;
	public Image CancelButtonImage;

	public Button CharacterButton;

	public BoxCollider CharacterButtonCollider;
	public BoxCollider CancelButtonCollider;

	public string CharacterButtonImageName;

	public static int CoinAmount;
	public static int KeyAmount;
	public static int TargetCharacterIndex;

	public Text CoinText;
	public Text KeyText;
	public Text WindowMessage;

	public Font MainFont;

	public Sprite[] OpenedCharacterSpriteArray = new Sprite[12];
	public Sprite[] LockedCharacterSpriteArray = new Sprite[12];
	public static string[] CharacterStatusArray = new string[12] { "Cat", "Frog", "Deer", "Monkey", "Locked", "Locked", "Locked", "Locked", "Locked", "Locked", "Locked", "Locked" };
	public GameObject[] CharacterButtonArray = new GameObject[12];


	void Start()
    {
		GenerateImage();
		GenerateLabel();
	}

	public void GenerateImage()
	{
		OpenedCharacterSpriteArray = Resources.LoadAll<Sprite>("Character/Wanted/Opened");
		LockedCharacterSpriteArray = Resources.LoadAll<Sprite>("Character/Wanted/Locked");

		for (int i = 1; i <= 4; i++)
		{
			for (int j = 1; j <= 3; j++)
			{
				int CharacterIndex = 3 * (i - 1) + j - 1;
				if (CharacterStatusArray[CharacterIndex] == "Locked")
				{
					Character = new GameObject(string.Format("SelectButton{0}{1}", 3 * (i - 1) + j, "Locked")); // 버튼 오브젝트 생성
					Character.transform.parent = GameObject.Find("Content").transform; // Content의 자식으로 이동

					CharacterImage = Character.AddComponent<Image>();
					CharacterButton = Character.AddComponent<Button>();

					CharacterButton.onClick.AddListener(() => LoadWindow());

					CharacterImage.sprite = LockedCharacterSpriteArray[CharacterIndex];
				}
				else
				{
					Character = new GameObject(string.Format("SelectButton{0}", 3 * (i - 1) + j)); // 버튼 오브젝트 생성
					Character.transform.parent = GameObject.Find("Content").transform; // Content의 자식으로 이동

					CharacterImage = Character.AddComponent<Image>();
					CharacterButton = Character.AddComponent<Button>();

					CharacterButton.onClick.AddListener(() => LoadSelection(CharacterIndex));

					CharacterImage.sprite = OpenedCharacterSpriteArray[CharacterIndex];
				}

				Character.transform.localScale = new Vector3(2.2f, 3.3f, 0);
				Character.transform.localPosition = new Vector3(230 * j - 450, 340 * (5 - i) - 1550, 0);

				CharacterButtonArray[3 * (i - 1) + j - 1] = Character;
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
		Coin.transform.localPosition = new Vector3(-120, 550, 0);

		CoinText = Coin.AddComponent<Text>();
		CoinText.font = MainFont;
		CoinText.fontSize = 40;
		CoinText.text = "COIN\n" + CoinAmount.ToString();
		CoinText.alignment = TextAnchor.UpperCenter;

		Coin.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

		//키 보유량 라벨 생성
		Key = new GameObject("KeyAmountText");
		Key.transform.parent = GameObject.Find("Canvas").transform;
		Key.transform.localPosition = new Vector3(120, 550, 0);

		KeyText = Key.AddComponent<Text>();
		KeyText.font = MainFont;
		KeyText.fontSize = 40;
		KeyText.text = "KEY\n" + KeyAmount.ToString();
		KeyText.alignment = TextAnchor.UpperCenter;

		Key.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
	}

	public void GenerateWindow()
	{
		if (YesOrNoFrame == null)
		{
			//선택창의 부모 생성
			YesOrNoFrame = new GameObject("YesOrNoFrame");
			YesOrNoFrame.transform.parent = GameObject.Find("Canvas").transform;

			YesOrNoFrame.transform.localPosition = new Vector3(0, 0, 0);

			// 선택창 생성
			YesOrNoWindow = new GameObject("YesOrNoWindow");
			YesOrNoWindow.transform.parent = YesOrNoFrame.transform;
			YesOrNoWindowImage = YesOrNoWindow.AddComponent<Image>();
			YesOrNoWindowImage.sprite = Resources.Load<Sprite>("UI/yesOrNo");


			YesOrNoWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 300);
			YesOrNoWindow.transform.localPosition = new Vector3(0, 0, 0);

			// 닫기 버튼 생성
			CancelButton = new GameObject("CancelButton");
			CancelButton.transform.parent = YesOrNoFrame.transform;

			CancelButtonImage = CancelButton.AddComponent<Image>();
			CancelButtonImage.sprite = Resources.Load<Sprite>("UI/cancelButton");

			CancelButton.AddComponent<Button>();
			CancelButton.GetComponent<Button>().onClick.AddListener(() => CancelWindow(YesOrNoFrame));

			CancelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 60);
			CancelButton.transform.localPosition = new Vector3(150, -70, 0);

			YesOrNoFrame.SetActive(true);
			CharacterButtonController(false);

			// 메시지 생성
			Message = new GameObject("MessageText");
			Message.transform.parent = YesOrNoFrame.transform;
			WindowMessage = Message.AddComponent<Text>();
			WindowMessage.text = "DO YOU WANT TO UNLOCK THE CHARACTER?";
			WindowMessage.font = MainFont;
			WindowMessage.fontSize = 35;

			Message.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 100);
			Message.transform.localPosition = new Vector3(20, 40, 0);
		}
		else
		{
			YesOrNoFrame.SetActive(true);
			CharacterButtonController(false);
		}
	}

	public void LoadWindow()
	{
		GenerateWindow();
		CharacterButtonController(false);
	}

	public void CancelWindow(GameObject TargetObject)
	{
		TargetObject.SetActive(false);
		CharacterButtonController(true);
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
}


