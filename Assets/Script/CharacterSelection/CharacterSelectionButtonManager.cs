using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionButtonManager : MonoBehaviour
{
	public void Left()
	{
		GameObject.Find("CharacterSelectionImageGenerator").GetComponent<CharacterSelectionImageGenerator>().ChangeIndex("Left");
	}
	public void Right()
	{
		GameObject.Find("CharacterSelectionImageGenerator").GetComponent<CharacterSelectionImageGenerator>().ChangeIndex("Right");
	}

	public void LoadCharacter()
	{
		SceneManager.LoadScene("Character");
	}
			
}
