using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterButtonManager : MonoBehaviour
{
	public UISprite TargetButton;
	public static string TargetCharacter;

	void OnClick()
	{
		TargetButton = this.GetComponent<UISprite>();

		if (this.name == "HomeButton")
		{
			SceneManager.LoadScene("Main");
		}
		else if (this.name == "ShopButton")
		{
			SceneManager.LoadScene("Main");
		}
		else if (TargetButton.spriteName.Substring(TargetButton.spriteName.Length - 6) == "Locked")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterImageGenerator>().GenerateWindow();
		}
		else if (this.name == "CancelButton")
		{
			this.transform.parent.gameObject.SetActive(false);
			GameObject.Find("UI Root (2D)").GetComponent<CharacterImageGenerator>().CharacterButtonController(true);
		}
		else
		{
			TargetCharacter = TargetButton.spriteName;
			SceneManager.LoadScene("SpecificCharacter");
		}
	}

}
