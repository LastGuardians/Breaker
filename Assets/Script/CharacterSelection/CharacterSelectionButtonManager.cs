using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionButtonManager : MonoBehaviour
{
	void OnClick()
	{
		if (this.name == "LeftButton")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterSelectionImageGenerator>().ChangeCharacter(gameObject);
		}
		else if (this.name == "RightButton")
		{
			GameObject.Find("UI Root (2D)").GetComponent<CharacterSelectionImageGenerator>().ChangeCharacter(gameObject);
		}
		else if (this.name == "ExitButton")
		{
			SceneManager.LoadScene("Character");
		}
	}
}
