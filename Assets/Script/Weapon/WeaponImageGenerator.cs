using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponImageGenerator : MonoBehaviour
{
	public GameObject Canvas;
	public GameObject AbilityGauge;
	public GameObject RedAbilityGauge;
	public GameObject Coin;
	public GameObject Key;
	public GameObject Plus;
	public GameObject Minus;
	public GameObject SaveButton;
	public GameObject CancelButton;
	public GameObject Lock;

	public Image AbilityGaugeImage;

	public Sprite SaveButtonSprite;
	public Sprite CancelButtonSprite;

	public Text CoinText;
	public Text KeyText;

	public int CoinAmount;
	public int KeyAmount;

	public Font MainFont;
	
	public Sprite[] AbilityGaugeArray = new Sprite[3];
	public Sprite[] UpgradeButtonArray = new Sprite[2];

	public string[] WeaponStatusArray = new string[3] { "", "", "Locked" };

	public GameObject[] LockWeaponArray = new GameObject[3];
	public GameObject[,,] WeaponObjectArray = new GameObject[3, 3, 10];

	public int[,] WeaponAbilityArray = new int[3, 3] {{4, 2, 7}, {3, 9, 4}, {2, 5, 7}};
	public int[,] TempAbilityArray = new int[3, 3] {{ 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }};
	

	private void Start()
	{
		Canvas = GameObject.Find("Canvas");
		LockWeapon();
		GenerateGauge();
		GenerateLabel();
		GenerateAbilityButton();
		GenerateDecisionButton();
	}

	public void LockWeapon()
	{
		for(int i = 1; i <= 3; i++)
		{
			if(WeaponStatusArray[i - 1] == "Locked")
			{
				Lock = new GameObject("Lock" + i.ToString());
				Lock.transform.parent = Canvas.transform;
				Lock.AddComponent<Image>();
				Lock.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/lockedWeapon");

				Lock.GetComponent<RectTransform>().sizeDelta = new Vector2(750, 300);
				Lock.transform.localPosition = new Vector3(0, -360 * i + 660);

				LockWeaponArray[i - 1] = Lock;
				SetLockLast();
			}
		}
	}

	public void GenerateGauge()
	{
		AbilityGaugeArray = Resources.LoadAll<Sprite>("UI/Gauge");
		for (int i = 1; i <= 3; i++)
		{
			for (int j = 1; j <= 3; j++)
			{
				for (int k = 1; k <= 10; k++)
				{
					AbilityGauge = new GameObject("AbilityGauge");
					AbilityGauge.transform.parent = Canvas.transform; // canvas의 자식으로 이동
					AbilityGaugeImage = AbilityGauge.AddComponent<Image>();

					AbilityGauge.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 40);
					AbilityGauge.transform.localPosition = new Vector3(20 * k + 80, - 360 * i - 70 * j + 830, 0);

					AbilityGaugeImage.sprite = AbilityGaugeArray[0];

					WeaponObjectArray[i - 1, j - 1, k - 1] = AbilityGauge;
				}
			}
		}
		StartCoroutine(AbilityAnimation());
	}

	IEnumerator AbilityAnimation()
	{
		for (int k = 1; k <= 10; k++)
		{
			yield return new WaitForSeconds(0.1f);
			for (int j = 1; j <= 3; j++)
			{
				for (int i = 1; i <= 3; i++)
				{
					if (k <= WeaponAbilityArray[i - 1, j - 1])
					{
						WeaponObjectArray[i - 1, j - 1, k - 1].GetComponent<Image>().sprite = AbilityGaugeArray[1];
					}
				}
			}
		}
	}

	public void GenerateLabel()
	{
		CoinAmount = 100;
		KeyAmount = 10;
		
		//코인 보유량 라벨 생성
		Coin = new GameObject("CoinAmountText");
		Coin.transform.parent = Canvas.transform;
		Coin.transform.localPosition = new Vector3(-120, 550, 0);

		CoinText = Coin.AddComponent<Text>();
		CoinText.font = MainFont;
		CoinText.fontSize = 40;
		CoinText.text = "COIN\n" + CoinAmount.ToString();
		CoinText.alignment = TextAnchor.UpperCenter;

		Coin.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

		//키 보유량 라벨 생성
		Key = new GameObject("KeyAmountText");
		Key.transform.parent = Canvas.transform;
		Key.transform.localPosition = new Vector3(120, 550, 0);

		KeyText = Key.AddComponent<Text>();
		KeyText.font = MainFont;
		KeyText.fontSize = 40;
		KeyText.text = "KEY\n" + KeyAmount.ToString();
		KeyText.alignment = TextAnchor.UpperCenter;

		Key.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
	}

	public void GenerateAbilityButton()
	{
		UpgradeButtonArray = Resources.LoadAll<Sprite>("UI/UpgradeButtons");
		for(int i = 1; i <= 3; i++)
		{
			for(int j = 1; j <= 3; j++)
			{
				int TempI = i;
				int TempJ = j;

				//마이너스 버튼 생성
				Minus = new GameObject("MinusButton" + i.ToString() + j.ToString());
				Minus.transform.parent = Canvas.transform;

				Minus.AddComponent<Image>();
				Minus.GetComponent<Image>().sprite = UpgradeButtonArray[0];

				Minus.AddComponent<Button>();
				Minus.GetComponent<Button>().onClick.AddListener(() => Subtract(TempI, TempJ));

				Minus.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
				Minus.transform.localPosition = new Vector3(60, -360 * i - 70 * j + 830, 0);

				//플러스 버튼 생성
				Plus = new GameObject("PlusButton" + i.ToString() + j.ToString());
				Plus.transform.parent = Canvas.transform;

				Plus.AddComponent<Image>();
				Plus.GetComponent<Image>().sprite = UpgradeButtonArray[1];

				Plus.AddComponent<Button>();
				Plus.GetComponent<Button>().onClick.AddListener(() => Add(TempI, TempJ));

				Plus.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
				Plus.transform.localPosition = new Vector3(320, -360 * i - 70 * j + 830, 0);
				SetLockLast();
			}
		}
	}

	public void GenerateDecisionButton()
	{
		SaveButtonSprite = Resources.Load<Sprite>("UI/saveButton");
		CancelButtonSprite = Resources.Load<Sprite>("UI/cancelButton");
		for (int i = 1; i <= 3; i++)
		{
			//저장 버튼 생성
			SaveButton = new GameObject("SaveButton" + i.ToString());
			SaveButton.transform.parent = Canvas.transform;

			SaveButton.AddComponent<Image>();
			SaveButton.GetComponent<Image>().sprite = SaveButtonSprite;

			SaveButton.AddComponent<Button>();
			SaveButton.GetComponent<Button>().onClick.AddListener(() => SaveStatus());

			SaveButton.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 40);
			SaveButton.transform.localPosition = new Vector3(280, -360 * i + 560);

			//취소 버튼 생성
			CancelButton = new GameObject("CancelButton" + i.ToString());
			CancelButton.transform.parent = Canvas.transform;

			CancelButton.AddComponent<Image>();
			CancelButton.GetComponent<Image>().sprite = CancelButtonSprite;

			CancelButton.AddComponent<Button>();
			CancelButton.GetComponent<Button>().onClick.AddListener(() => CancelStatus());

			CancelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 40);
			CancelButton.transform.localPosition = new Vector3(30, -360 * i + 560);
			SetLockLast();
		}
	}

	public void Subtract(int i, int j)
	{
		if(TempAbilityArray[i - 1, j - 1] > 0)
		{
			TempAbilityArray[i - 1, j - 1]--;
			WeaponObjectArray[i - 1, j - 1, WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1]].GetComponent<Image>().sprite = AbilityGaugeArray[0];
		}
	}

	public void Add(int i, int j)
	{
		if(TempAbilityArray[i - 1, j - 1] < 10 - WeaponAbilityArray[i - 1, j - 1])
		{
			TempAbilityArray[i - 1, j - 1]++;
			WeaponObjectArray[i - 1, j - 1, WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1] - 1].GetComponent<Image>().sprite = AbilityGaugeArray[2];
		}
	}

	public void SaveStatus()
	{
		for(int i = 1; i <= 3; i++)
		{
			for(int j = 1; j <= 3; j++)
			{
				for(int k = WeaponAbilityArray[i - 1, j - 1] + 1; k <= WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1]; k++)
				{
					WeaponObjectArray[i - 1, j - 1, k - 1].GetComponent<Image>().sprite = AbilityGaugeArray[1];
				}
				WeaponAbilityArray[i - 1, j - 1] += TempAbilityArray[i - 1, j - 1];
			}
		}

		TempAbilityArray = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
	}

	public void CancelStatus()
	{
		for (int i = 1; i <= 3; i++)
		{
			for (int j = 1; j <= 3; j++)
			{
				for (int k = WeaponAbilityArray[i - 1, j - 1] + 1; k <= WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1]; k++)
				{
					WeaponObjectArray[i - 1, j - 1, k - 1].GetComponent<Image>().sprite = AbilityGaugeArray[0];
				}
			}
		}
		TempAbilityArray = new int[3, 3] {{0, 0, 0}, {0, 0, 0}, {0, 0, 0}};
	}

	public void SetLockLast()
	{
		foreach(GameObject Locked in LockWeaponArray)
		{
			if(Locked != null)
			{
				Locked.transform.SetAsLastSibling();
			}
		}
	}
}

