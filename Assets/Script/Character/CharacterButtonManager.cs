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
		if (TargetButton.name == "HomeButton")
		{
			SceneManager.LoadScene("Main");
		}
		else if (TargetButton.name == "ShopButton")
		{
			SceneManager.LoadScene("Main");
		}
		else
		{
			TargetCharacter = TargetButton.spriteName;
			SceneManager.LoadScene("SpecificCharacter");
		}
	}
}
