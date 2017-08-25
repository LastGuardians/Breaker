using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterImageGenerator: MonoBehaviour
{
	private GameObject CharacterButton;
	private GameObject CoinAmountLabel;
	private GameObject KeyAmountLabel;
	public UIAtlas TargetAtlas;
	private UISprite TargetSprite;
	private BoxCollider TargetCollider;
	public UILabel CoinLabel;
	public UILabel KeyLabel;
	public UIFont MainFont;
	public static int CoinAmount;
	public static int KeyAmount;
	public static string[] CharacterStatusArray = new string[16] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "Locked" };

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

				TargetSprite = CharacterButton.AddComponent<UISprite>();
				TargetCollider = CharacterButton.AddComponent<BoxCollider>();

				TargetSprite.atlas = TargetAtlas;
				TargetSprite.spriteName = "select" + (4 * (i - 1) + j).ToString() + CharacterStatusArray[4 * (i - 1) + j - 1];
				TargetSprite.depth = 1;

				System.Type ButtonScript = System.Type.GetType("CharacterButtonManager");
				CharacterButton.AddComponent(ButtonScript);

				CharacterButton.transform.localScale = new Vector3(80, 120, 0);
				CharacterButton.transform.localPosition = new Vector3(100 * j - 250 , 130 * (5 - i) - 300, 0);
			}
		}

		//코인 보유량 라벨 생성
		CoinAmountLabel = new GameObject("CoinAmountLabel");
		CoinAmountLabel.transform.parent = GameObject.Find("UI Root (2D)").transform;
		CoinAmountLabel.layer = 9;
		CoinLabel = CoinAmountLabel.AddComponent<UILabel>();
		CoinLabel.depth = 1;
		CoinLabel.font = MainFont;

		CoinLabel.text = "Coin " + CoinAmount.ToString();
		CoinLabel.transform.localScale = new Vector3(30, 30, 0);
		CoinLabel.transform.localPosition = new Vector3(- 60, 340, 0);

		//키 보유량 라벨 생성
		KeyAmountLabel = new GameObject("KeyAmountLabel");
		KeyAmountLabel.transform.parent = GameObject.Find("UI Root (2D)").transform;
		KeyAmountLabel.layer = 9;
		KeyLabel = KeyAmountLabel.AddComponent<UILabel>();
		KeyLabel.depth = 1;
		KeyLabel.font = MainFont;

		KeyLabel.text = "Key " + KeyAmount.ToString();
		KeyLabel.transform.localScale = new Vector3(30, 30, 0);
		KeyLabel.transform.localPosition = new Vector3(80, 340, 0);
	}
}


