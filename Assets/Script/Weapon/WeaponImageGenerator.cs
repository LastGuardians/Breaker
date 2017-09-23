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
	public GameObject Window;
	public GameObject YesButton;
	public GameObject WindowText;

	public GameObject Light1;
	public GameObject Light2;
	public GameObject Light3;

	public GameObject Lock1;
	public GameObject Lock2;
	public GameObject Lock3;

	public Image AbilityGaugeImage;

	public Sprite SaveButtonSprite;
	public Sprite CancelButtonSprite;

	public Text CoinText;
	public Text KeyText;

	public int WeaponIndex;

	public Font MainFont;
	
	public Sprite[] AbilityGaugeArray = new Sprite[3];
	public Sprite[] UpgradeButtonArray = new Sprite[2];

	public string[] WeaponStatusArray = new string[3] { "Opened", "Opened", "Locked" };

	public GameObject[] LockArray = new GameObject[3];
	public GameObject[,,] WeaponObjectArray = new GameObject[3, 3, 10];
	public GameObject[] LightArray = new GameObject[3];
	public GameObject[,,] AbilityButtonArray = new GameObject[3, 3, 2];
	public GameObject[,] DecisionButtonArray = new GameObject[3, 2];

	public int[,] WeaponAbilityArray = new int[3, 3] { {4, 2, 7}, {3, 9, 4}, {2, 5, 7} };
	public int[,] TempAbilityArray = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
	public int[] StatusLocationArray = new int[3] { 50, -280, 50 };
	public int[] WeaponPriceArray = new int[3] { 0, 100000, 200000 };
	

	private void Start()
	{
		Window = GameObject.Find("Window");
		YesButton = GameObject.Find("YesButton");
		WindowText = GameObject.Find("WindowText");

		Window.SetActive(false);

		LightArray[0] = Light1;
		LightArray[1] = Light2;
		LightArray[2] = Light3;

		Canvas = GameObject.Find("Canvas");
		ShowSelected(WeaponIndex);
		LockWeapon();
		GenerateGauge();
		GenerateLabel();
		GenerateAbilityButton();
		GenerateDecisionButton();
		LockButton(true);

		Window.transform.SetAsLastSibling();
	}

	public void LoadWindow(int weaponIndex)
	{
		Window.SetActive(true);

		YesButton.GetComponent<Button>().onClick.AddListener(() => UnlockWeapon(weaponIndex));
		LockButton(false);
	}

	public void UnlockWeapon(int weaponIndex)
	{
		if (CharacterImageGenerator.CoinAmount >= WeaponPriceArray[weaponIndex])
		{
			CancelWindow();

			LockArray[weaponIndex].GetComponent<Animator>().SetBool("Unlock", true);
			CharacterImageGenerator.CoinAmount -= WeaponPriceArray[weaponIndex];
			Coin.GetComponent<Text>().text = CharacterImageGenerator.CoinAmount.ToString();
		}
		else
		{
			WindowText.GetComponent<Text>().text = "Not enough key! It requires " + WeaponPriceArray[weaponIndex].ToString() + " coins";
			YesButton.SetActive(false);
		}
	}

	public void CancelWindow()
	{
		Window.SetActive(false);
		LockButton(true);
		WindowText.GetComponent<Text>().text = "Would you like to unlock the weapon?";
		YesButton.SetActive(true);
	}

	public void ShowSelected(int weaponIndex)
	{
		Light1.SetActive(false);
		Light2.SetActive(false);
		Light3.SetActive(false);

		LightArray[weaponIndex].SetActive(true);
		WeaponIndex = weaponIndex;
	}

	public void LockWeapon()
	{
		LockArray[0] = Lock1;
		LockArray[1] = Lock2;
		LockArray[2] = Lock3;

		Lock1.SetActive(false);
		Lock2.SetActive(false);
		Lock3.SetActive(false);

		for(int i = 0; i < 3; i++)
		{
			if(WeaponStatusArray[i] == "Locked")
			{
				LockArray[i].SetActive(true);
			}
		}
	}

	public void LockButton(bool status)
	{
		for (int i = 0; i < 3; i++)
		{
			if (WeaponStatusArray[i] == "Locked")
			{
				for(int j = 0; j < 3; j++)
				{
					for(int k = 0; k < 2; k ++)
					{
						AbilityButtonArray[i, j, k].GetComponent<Button>().interactable = false;
						DecisionButtonArray[i, k].GetComponent<Button>().interactable = false;
					}
				}
			}
			else
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						AbilityButtonArray[i, j, k].GetComponent<Button>().interactable = status;
						DecisionButtonArray[i, k].GetComponent<Button>().interactable = status;
					}
				}
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
					AbilityGauge.transform.localPosition = new Vector3(20 * k + 80 + StatusLocationArray[i - 1], - 330 * i - 70 * j + 690, 0);

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
		//코인 보유량 라벨 생성
		Coin = new GameObject("CoinAmountText");
		Coin.transform.parent = Canvas.transform;
		Coin.transform.localPosition = new Vector3(-20, 550, 0);

		CoinText = Coin.AddComponent<Text>();
		CoinText.font = MainFont;
		CoinText.fontSize = 40;
		CoinText.text = CharacterImageGenerator.CoinAmount.ToString();
		CoinText.alignment = TextAnchor.UpperCenter;

		Coin.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

		//키 보유량 라벨 생성
		Key = new GameObject("KeyAmountText");
		Key.transform.parent = Canvas.transform;
		Key.transform.localPosition = new Vector3(270, 550, 0);

		KeyText = Key.AddComponent<Text>();
		KeyText.font = MainFont;
		KeyText.fontSize = 40;
		KeyText.text = CharacterImageGenerator.KeyAmount.ToString();
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
				Minus.transform.localPosition = new Vector3(60 + StatusLocationArray[i - 1], -330 * i - 70 * j + 690, 0);
				AbilityButtonArray[i - 1, j - 1, 0] = Minus;

				//플러스 버튼 생성
				Plus = new GameObject("PlusButton" + i.ToString() + j.ToString());
				Plus.transform.parent = Canvas.transform;

				Plus.AddComponent<Image>();
				Plus.GetComponent<Image>().sprite = UpgradeButtonArray[1];

				Plus.AddComponent<Button>();
				Plus.GetComponent<Button>().onClick.AddListener(() => Add(TempI, TempJ));

				Plus.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
				Plus.transform.localPosition = new Vector3(320 + StatusLocationArray[i - 1], -330 * i - 70 * j + 690, 0);
				AbilityButtonArray[i - 1, j - 1, 1] = Plus;

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
			SaveButton.transform.localPosition = new Vector3(280 + StatusLocationArray[i - 1], -330 * i + 390);

			DecisionButtonArray[i - 1, 0] = SaveButton;

			//취소 버튼 생성
			CancelButton = new GameObject("CancelButton" + i.ToString());
			CancelButton.transform.parent = Canvas.transform;

			CancelButton.AddComponent<Image>();
			CancelButton.GetComponent<Image>().sprite = CancelButtonSprite;

			CancelButton.AddComponent<Button>();
			CancelButton.GetComponent<Button>().onClick.AddListener(() => CancelStatus());

			CancelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 40);
			CancelButton.transform.localPosition = new Vector3(30 + StatusLocationArray[i - 1], -330 * i + 390);

			DecisionButtonArray[i - 1, 1] = CancelButton;

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
		foreach(GameObject Locked in LockArray)
		{
			if(Locked != null)
			{
				Locked.transform.SetAsLastSibling();
			}
		}
	}
}

