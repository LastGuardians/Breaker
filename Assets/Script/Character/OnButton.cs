using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnButton : MonoBehaviour
{
	public UISprite TargetButton;
	public static string TargetCharacter;

	void OnClick()
	{
		TargetButton = this.GetComponent<UISprite>();
		if (TargetButton.name == "HomeButton")
		{
			TargetButton.enabled = false;
			SceneManager.LoadScene("Main");
		}
		else
		{
			TargetCharacter = TargetButton.name;
			SceneManager.LoadScene("CharacterSpecific");
		}
	}
}
