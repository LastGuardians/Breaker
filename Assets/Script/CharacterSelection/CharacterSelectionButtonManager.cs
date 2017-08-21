using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionButtonManager : MonoBehaviour
{
	public UISprite TargetButton;
	void OnClick()
	{
		TargetButton = this.GetComponent<UISprite>();
		if (TargetButton.name == "LeftButton")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterSelectionImageGenerator>().ChangeCharacter(TargetButton);
		}
		else if (TargetButton.name == "RightButton")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterSelectionImageGenerator>().ChangeCharacter(TargetButton);
		}
		else if (TargetButton.name == "BackButton")
		{
			SceneManager.LoadScene("Character");
		}
		else if (TargetButton.name == "ShopButton")
		{
			SceneManager.LoadScene("Main");
		}
	}
}
