using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterImageGenerator: MonoBehaviour
{
	private GameObject CharacterButton;
	private GameObject Coin;
	private GameObject Key;
	private GameObject YesOrNoFrame;
	private GameObject YesOrNoWindow;
	private GameObject CancelButton;
	private GameObject Message;

	public UIAtlas CharacterButtonAtlas;
	public UIAtlas WindowAtlas;

	private UISprite CharacterButtonSprite;
	private UISprite YesOrNoWindowSprite;
	private UISprite CancelButtonSprite;

	private BoxCollider TargetCollider;
	private BoxCollider CancelButtonCollider;

	public UILabel CoinLabel;
	public UILabel KeyLabel;
	public UILabel MessageLabel;

	public UIFont MainFont;

	public static int CoinAmount;
	public static int KeyAmount;
	public static string[] CharacterStatusArray = new string[16] { "", "", "", "", "", "", "", "", "", "", "", "", "Locked", "", "", "Locked" };
	private GameObject[] CharacterButtonArray = new GameObject[16];

	void Start()
    {
		KeyAmount = 10;
		CoinAmount = 100;
		for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                CharacterButton = new GameObject(string.Format("SelectButton{0}", 4 * (i - 1) + j)); // 버튼 오브젝트 생성
				CharacterButton.transform.parent = GameObject.Find("UI Root (2D)").transform; // UI Root의 자식으로 이동
				CharacterButton.layer = 9; //레이어를 GUI로 바꿈

				CharacterButtonSprite = CharacterButton.AddComponent<UISprite>();
				TargetCollider = CharacterButton.AddComponent<BoxCollider>();

				CharacterButtonSprite.atlas = CharacterButtonAtlas;
				CharacterButtonSprite.spriteName = "select" + (4 * (i - 1) + j).ToString() + CharacterStatusArray[4 * (i - 1) + j - 1];
				CharacterButtonSprite.depth = 1;

				System.Type ButtonScript = System.Type.GetType("CharacterButtonManager");
				CharacterButton.AddComponent(ButtonScript);

				CharacterButton.transform.localScale = new Vector3(80, 120, 0);
				CharacterButton.transform.localPosition = new Vector3(100 * j - 250 , 130 * (5 - i) - 300, 0);

				CharacterButtonArray[4 * (i - 1) + j - 1] = CharacterButton;
			}
		}

		//코인 보유량 라벨 생성
		Coin = new GameObject("CoinAmountLabel");
		Coin.transform.parent = GameObject.Find("UI Root (2D)").transform;
		Coin.layer = 9;

		CoinLabel = Coin.AddComponent<UILabel>();
		CoinLabel.depth = 1;
		CoinLabel.font = MainFont;

		CoinLabel.text = "Coin " + CoinAmount.ToString();
		Coin.transform.localScale = new Vector3(30, 30, 0);
		Coin.transform.localPosition = new Vector3(- 60, 340, 0);

		//키 보유량 라벨 생성
		Key = new GameObject("KeyAmountLabel");
		Key.transform.parent = GameObject.Find("UI Root (2D)").transform;
		Key.layer = 9;
		KeyLabel = Key.AddComponent<UILabel>();
		KeyLabel.depth = 1;
		KeyLabel.font = MainFont;

		KeyLabel.text = "Key " + KeyAmount.ToString();
		Key.transform.localScale = new Vector3(30, 30, 0);
		Key.transform.localPosition = new Vector3(80, 340, 0);
	}

	public void GenerateWindow()
	{
		if (YesOrNoFrame == null)
		{
			//선택창의 부모 생성
			YesOrNoFrame = new GameObject("YesOrNoWindow");
			YesOrNoFrame.transform.parent = GameObject.Find("UI Root (2D)").transform;
			YesOrNoFrame.layer = 9;

			System.Type WindowScript = System.Type.GetType("UIAnimation");

			YesOrNoFrame.transform.localScale = new Vector3(400, 200, 0);
			YesOrNoFrame.AddComponent(WindowScript);

			// 선택창 생성
			YesOrNoWindow = new GameObject("YesOrNoWindow");
			YesOrNoWindow.transform.parent = YesOrNoFrame.transform; // UI Root의 자식으로 이동
			YesOrNoWindow.layer = 9; //레이어를 GUI로 바꿈

			YesOrNoWindowSprite = YesOrNoWindow.AddComponent<UISprite>();

			YesOrNoWindowSprite.atlas = WindowAtlas;
			YesOrNoWindowSprite.spriteName = "YesOrNo";
			YesOrNoWindowSprite.depth = 3;

			YesOrNoWindow.transform.localScale = new Vector3(1f, 1f, 0);
			YesOrNoWindow.transform.localPosition = new Vector3(0, 0, 0);

			// 닫기 버튼 생성
			CancelButton = new GameObject("CancelButton");
			CancelButton.transform.parent = YesOrNoFrame.transform;
			CancelButton.layer = 9; //레이어를 GUI로 바꿈

			CancelButtonSprite = CancelButton.AddComponent<UISprite>();
			CancelButtonCollider = CancelButton.AddComponent<BoxCollider>();

			CancelButtonSprite.atlas = WindowAtlas;
			CancelButtonSprite.spriteName = "cancel";
			CancelButtonSprite.depth = 4;

			System.Type CancelButtonScript = System.Type.GetType("CharacterButtonManager");
			CancelButton.AddComponent(CancelButtonScript);

			CancelButton.transform.localScale = new Vector3(0.4f, 0.3f, 0);
			CancelButton.transform.localPosition = new Vector3(0.25f, -0.2f, 0);

			YesOrNoFrame.SetActive(true);
			CharacterButtonController(false);

			// 메시지 생성
			Message = new GameObject("MessageLabel");
			Message.transform.parent = YesOrNoFrame.transform;
			Message.layer = 9;
			MessageLabel = Message.AddComponent<UILabel>();
			MessageLabel.depth = 5;
			MessageLabel.font = MainFont;

			MessageLabel.text = "잠금을 해제하시겠\n습니까?";
			Message.transform.localScale = new Vector3(0.1f, 0.2f, 0);
			Message.transform.localPosition = new Vector3(0f, 0.2f, 0f);
		}
		else
		{
			YesOrNoFrame.SetActive(true);
			CharacterButtonController(false);
		}
	}

	public void CharacterButtonController(bool enable)
	{
		if (enable)
		{
			foreach (GameObject button in CharacterButtonArray)
			{
				button.AddComponent<BoxCollider>();
			}
		}
		else
		{
			foreach (GameObject button in CharacterButtonArray)
			{
				Destroy(button.GetComponent<BoxCollider>());
			}
		}
	}
}


