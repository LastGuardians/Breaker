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
		if (this.name == "LeftButton")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterSelectionImageGenerator>().ChangeCharacter(TargetButton);
		}
		else if (this.name == "RightButton")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterSelectionImageGenerator>().ChangeCharacter(TargetButton);
		}
		else if (this.name == "ExitButton")
		{
			SceneManager.LoadScene("Character");
		}
	}
}
