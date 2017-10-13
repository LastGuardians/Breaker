using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

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
	public GameObject UpgradeLabel;

	public GameObject Light1;
	public GameObject Light2;
	public GameObject Light3;

	public GameObject Lock1;
	public GameObject Lock2;
	public GameObject Lock3;

	public Image AbilityGaugeImage;

	public Text CoinText;
	public Text KeyText;

	public int WeaponIndex;
	public int UpgradePrice;

	public Font MainFont;

	public Sprite[] AbilityGaugeArray = new Sprite[3];
	public Sprite[] UpgradeButtonArray = new Sprite[2];

	public GameObject[] LockArray = new GameObject[3];
	public GameObject[,,] WeaponObjectArray = new GameObject[3, 3, 10];
	public GameObject[] LightArray = new GameObject[3];
	public GameObject[,,] AbilityButtonArray = new GameObject[3, 3, 2];
	public GameObject[] DecisionButtonArray = new GameObject[2];

	public int[,] TempAbilityArray = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
	public int[] StatusLocationArray = new int[3] { 50, -280, 50 };
	public int[] WeaponPriceArray = new int[3] { 0, 100000, 200000 };
	public int[,] UpgradePriceArray = new int[3, 10]
	{ 
		{200, 400, 800, 1500, 3000, 6000, 12000, 20000, 30000, 40000},
		{400, 800, 1500, 3000, 6000, 12000, 20000, 30000, 40000, 60000 },
		{1500, 3000, 6000, 12000, 20000, 30000, 40000, 60000, 80000, 100000 }
	};

	public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";

	private string userId;
	private string weaponId;
	private int OpenedWeaponIndex;
	private int damageLevel;
	private int criticalLevel;
	private int probabilityLevel;


	private void Start()
	{
		userId = GPGSManager.mainplayeruserdata.id;
		Window.SetActive(false);

		LightArray[0] = Light1;
		LightArray[1] = Light2;
		LightArray[2] = Light3;

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
		WindowText.GetComponent<Text>().text = "\t\t\tX\t" + WeaponPriceArray[weaponIndex].ToString();
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
			UserWeapon.WeaponStatusArray[weaponIndex] = "Opened";

			weaponId = "Weapon" + (weaponIndex + 1).ToString();
			CreateUserWeapon();

			LockButton(true);
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

		for (int i = 0; i < 3; i++)
		{
			if (UserWeapon.WeaponStatusArray[i] == "Locked")
			{
				LockArray[i].SetActive(true);
			}
		}
	}

	public void LockButton(bool status)
	{
		for (int i = 0; i < 3; i++)
		{
			if (UserWeapon.WeaponStatusArray[i] == "Locked")
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						AbilityButtonArray[i, j, k].GetComponent<Button>().interactable = false;
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
					AbilityGauge.transform.localPosition = new Vector3(20 * k + 80 + StatusLocationArray[i - 1], -330 * i - 70 * j + 690, 0);

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
					if (k <= UserWeapon.WeaponAbilityArray[i - 1, j - 1])
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
		Coin.transform.localPosition = new Vector3(-20, 620, 0);

		CoinText = Coin.AddComponent<Text>();
		CoinText.font = MainFont;
		CoinText.fontSize = 40;
		CoinText.text = CharacterImageGenerator.CoinAmount.ToString();
		CoinText.alignment = TextAnchor.UpperCenter;

		Coin.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

		//키 보유량 라벨 생성
		Key = new GameObject("KeyAmountText");
		Key.transform.parent = Canvas.transform;
		Key.transform.localPosition = new Vector3(270, 620, 0);

		KeyText = Key.AddComponent<Text>();
		KeyText.font = MainFont;
		KeyText.fontSize = 40;
		KeyText.text = CharacterImageGenerator.KeyAmount.ToString();
		KeyText.alignment = TextAnchor.UpperCenter;

		Key.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

		//업그레이드 금액 라벨 생성

		UpgradeLabel = new GameObject("UpgradeLabel");
		UpgradeLabel.transform.parent = Canvas.transform;
		UpgradeLabel.transform.localPosition = new Vector3(-200, -700, 0);

		UpgradeLabel.AddComponent<Text>();
		UpgradeLabel.GetComponent<Text>().font = MainFont;
		UpgradeLabel.GetComponent<Text>().fontSize = 40;
		UpgradeLabel.GetComponent<Text>().text = UpgradePrice.ToString();
		UpgradeLabel.GetComponent<Text>().alignment = TextAnchor.UpperCenter;

		UpgradeLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
	}

	public void GenerateAbilityButton()
	{
		UpgradeButtonArray = Resources.LoadAll<Sprite>("UI/UpgradeButtons");
		for (int i = 1; i <= 3; i++)
		{
			for (int j = 1; j <= 3; j++)
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
		//저장 버튼 생성
		SaveButton = new GameObject("SaveButton");
		SaveButton.transform.parent = Canvas.transform;

		SaveButton.AddComponent<Text>();
		SaveButton.GetComponent<Text>().text = "SAVE";
		SaveButton.GetComponent<Text>().font = MainFont;
		SaveButton.GetComponent<Text>().fontSize = 30;

		SaveButton.AddComponent<Button>();
		SaveButton.GetComponent<Button>().onClick.AddListener(() => SaveStatus());

		SaveButton.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 40);
		SaveButton.transform.localPosition = new Vector3(100, -680);

		DecisionButtonArray[0] = SaveButton;

		//취소 버튼 생성
		CancelButton = new GameObject("CancelButton");
		CancelButton.transform.parent = Canvas.transform;

		CancelButton.AddComponent<Text>();
		CancelButton.GetComponent<Text>().text = "CANCEL";
		CancelButton.GetComponent<Text>().font = MainFont;
		CancelButton.GetComponent<Text>().fontSize = 30;

		CancelButton.AddComponent<Button>();
		CancelButton.GetComponent<Button>().onClick.AddListener(() => CancelStatus());

		CancelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 40);
		CancelButton.transform.localPosition = new Vector3(300, - 680);

		DecisionButtonArray[1] = CancelButton;

		SetLockLast();
	}

	public void Subtract(int i, int j)
	{
		if (TempAbilityArray[i - 1, j - 1] > 0)
		{
			TempAbilityArray[i - 1, j - 1]--;
			WeaponObjectArray[i - 1, j - 1, UserWeapon.WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1]].GetComponent<Image>().sprite = AbilityGaugeArray[0];
			UpgradePrice -= UpgradePriceArray[i - 1, UserWeapon.WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1]];
		}
		UpgradeLabel.GetComponent<Text>().text = UpgradePrice.ToString();
	}

	public void Add(int i, int j)
	{
		if (TempAbilityArray[i - 1, j - 1] < 10 - UserWeapon.WeaponAbilityArray[i - 1, j - 1])
		{
			TempAbilityArray[i - 1, j - 1]++;
			WeaponObjectArray[i - 1, j - 1, UserWeapon.WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1] - 1].GetComponent<Image>().sprite = AbilityGaugeArray[2];
			UpgradePrice += UpgradePriceArray[i - 1, UserWeapon.WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1] - 1];
		}
		UpgradeLabel.GetComponent<Text>().text = UpgradePrice.ToString();
	}

	public void SaveStatus()
	{
		if(UpgradePrice <= CharacterImageGenerator.CoinAmount)
		{
			for (int i = 1; i <= 3; i++)
			{
				for (int j = 1; j <= 3; j++)
				{
					for (int k = UserWeapon.WeaponAbilityArray[i - 1, j - 1] + 1; k <= UserWeapon.WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1]; k++)
					{
						WeaponObjectArray[i - 1, j - 1, k - 1].GetComponent<Image>().sprite = AbilityGaugeArray[1];
					}
					UserWeapon.WeaponAbilityArray[i - 1, j - 1] += TempAbilityArray[i - 1, j - 1];
				}
			}
			CharacterImageGenerator.CoinAmount -= UpgradePrice;
			CoinText.text = CharacterImageGenerator.CoinAmount.ToString();
			TempAbilityArray = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
			SetStatus();
			UpgradePrice = 0;
			UpgradeLabel.GetComponent<Text>().text = UpgradePrice.ToString();
		}
	}

	public void CancelStatus()
	{
		for (int i = 1; i <= 3; i++)
		{
			for (int j = 1; j <= 3; j++)
			{
				for (int k = UserWeapon.WeaponAbilityArray[i - 1, j - 1] + 1; k <= UserWeapon.WeaponAbilityArray[i - 1, j - 1] + TempAbilityArray[i - 1, j - 1]; k++)
				{
					WeaponObjectArray[i - 1, j - 1, k - 1].GetComponent<Image>().sprite = AbilityGaugeArray[0];
				}
			}
		}
		TempAbilityArray = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
		UpgradePrice = 0;
		UpgradeLabel.GetComponent<Text>().text = UpgradePrice.ToString();
		
	}

	public void SetStatus()
	{
		for (int i = 0; i < 3; i++)
		{
			if (UserWeapon.WeaponStatusArray[i] == "Opened")
			{
				weaponId = "Weapon" + (i + 1).ToString();
				damageLevel = UserWeapon.WeaponAbilityArray[i, 0];
				criticalLevel = UserWeapon.WeaponAbilityArray[i, 1];
				probabilityLevel = UserWeapon.WeaponAbilityArray[i, 2];
				SetUserWeapon();
			}
		}
	}

	public void SetLockLast()
	{
		foreach (GameObject Locked in LockArray)
		{
			if (Locked != null)
			{
				Locked.transform.SetAsLastSibling();
			}
		}
	}

	//서버 함수
	public void CreateUserWeapon()
	{
		StartCoroutine(_CreateUserWeapon(userId, weaponId));
	}

	public void GetUserWeapons()
	{
		StartCoroutine(_GetUserWeapons(userId));
	}

	public void SetUserWeapon()
	{
		StartCoroutine(_SetUserWeapon(userId, weaponId, damageLevel, criticalLevel, probabilityLevel));
	}

	public IEnumerator _CreateUserWeapon(string userId, string weaponId)
	{
		string url = baseUrl + "/user/" + userId + "/weapons/create";
		WWWForm form = new WWWForm();
		form.headers["content-type"] = "application/json";
		form.AddField("weaponId", weaponId);

		WWW www = new WWW(url, form);
		yield return www;

		PrintLog(www.error);
	}

	public IEnumerator _GetUserWeapons(string userId)
	{
		string url = baseUrl + "/user/" + userId + "/weapons";
		WWW www = new WWW(url);
		yield return www;

		PrintLog(www.error);
		PrintLog(www.text);

		JsonData json = JsonMapper.ToObject(www.text);
		for (int i = 0; i < json.Count; i++)
		{
			OpenedWeaponIndex = int.Parse(json[i]["weaponId"].ToString().Substring(6)) - 1;
			damageLevel = int.Parse(json[i]["damageLevel"].ToString());
			criticalLevel = int.Parse(json[i]["criticalLevel"].ToString());
			probabilityLevel = int.Parse(json[i]["probabilityLevel"].ToString());

			UserWeapon.WeaponStatusArray[OpenedWeaponIndex] = "Opened";
			UserWeapon.WeaponAbilityArray[OpenedWeaponIndex, 0] = damageLevel;
			UserWeapon.WeaponAbilityArray[OpenedWeaponIndex, 1] = criticalLevel;
			UserWeapon.WeaponAbilityArray[OpenedWeaponIndex, 2] = probabilityLevel;
		}
	}

	public IEnumerator _SetUserWeapon(string userId, string weaponId, int damageLevel, int criticalLevel, int probabilityLevel)
	{
		Debug.Log(userId);
		Debug.Log(weaponId);
		Debug.Log(damageLevel);
		Debug.Log(criticalLevel);
		Debug.Log(probabilityLevel);
		string url = baseUrl + "/user/" + userId + "/weapons/" + weaponId + "/update";
		WWWForm form = new WWWForm();
		form.headers["content-type"] = "application/json";
		form.AddField("damageLevel", damageLevel);
		form.AddField("criticalLevel", criticalLevel);
		form.AddField("probabilityLevel", probabilityLevel);

		WWW www = new WWW(url, form);
		yield return www;

		PrintLog(www.error);
		Debug.Log("Set");
	}

	void PrintLog(string message)
	{
		if (!string.IsNullOrEmpty(message))
		{
			Debug.Log(message);
		}
	}
}