using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonManager : MonoBehaviour
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
		else if (TargetButton.name == "BackButton")
		{
			SceneManager.LoadScene("Character");
		}
		else if (TargetButton.name == "LeftButton")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterSelectionManager>().ChangeCharacter(TargetButton);
		}
		else if (TargetButton.name == "RightButton")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterSelectionManager>().ChangeCharacter(TargetButton);
		}
		else
		{
			TargetCharacter = TargetButton.spriteName;
			SceneManager.LoadScene("SpecificCharacter");
		}
	}
}
