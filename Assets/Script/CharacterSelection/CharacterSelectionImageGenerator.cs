using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionImageGenerator: MonoBehaviour
{
	public GameObject Character;
	public GameObject Canvas;

	public RuntimeAnimatorController CharacterAnimatorController;

	public Sprite[] CharacterImageArray = new Sprite[12];

	public string AnimatorPath;

	public void Start()
	{
		Canvas = GameObject.Find("Canvas");
		GenerateSelectionImage();
	}

	public void GenerateSelectionImage()
	{
		// 캐릭터 이미지 오브젝트 생성
		Character = new GameObject("Character");
		Character.transform.parent = Canvas.transform; // Canvas의 자식으로 이동
		Character.AddComponent<Image>();

		Rescale(Character, CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex]);
		
		Character.AddComponent<Animator>();
		AnimatorPath = "Animation/Animator/" + CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex] + "Animator";
		Character.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(AnimatorPath);

		Character.transform.SetSiblingIndex(2);
	}

	public void Rescale(GameObject TargetObject, string TargetCharacter)
	{
		if(TargetCharacter == "Deer")
		{
			TargetObject.transform.localScale = new Vector3(10, 9, 0);
			TargetObject.transform.localPosition = new Vector3(0, 190, 0);
		}
		else if(TargetCharacter == "Monkey")
		{
			TargetObject.transform.localScale = new Vector3(12, 18, 0);
			TargetObject.transform.localPosition = new Vector3(0, 120, 0);
		}
		else if(TargetCharacter == "Bear")
		{
			TargetObject.transform.localScale = new Vector3(9, 11, 0);
			TargetObject.transform.localPosition = new Vector3(0, 260, 0);
		}
		else
		{
			TargetObject.transform.localScale = new Vector3(10, 9, 0);
			TargetObject.transform.localPosition = new Vector3(0, 50, 0);
		}
	}

	public void ChangeIndex(string TargetButtonName)
	{
		if (TargetButtonName == "Left")
		{
			do
			{
				CharacterImageGenerator.TargetCharacterIndex = ((CharacterImageGenerator.TargetCharacterIndex - 1) % 12 + 12) % 12;
			}
			while (CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex] == "Locked" || CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex] == "Unveiled");
		}
		else if (TargetButtonName == "Right")
		{
			do
			{
				CharacterImageGenerator.TargetCharacterIndex = ((CharacterImageGenerator.TargetCharacterIndex + 1) % 12 + 12) % 12;
			}
			while (CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex] == "Locked" || CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex] == "Unveiled");
		}
		ChangeImage(TargetButtonName);
	}

	public void ChangeImage(string ClickedButton)
	{
		Character.GetComponent<Animator>().SetBool(ClickedButton, true);
		AnimatorPath = "Animation/Animator/" + CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex] + "Animator";
		Character.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(AnimatorPath);
		Rescale(Character, CharacterImageGenerator.CharacterArray[CharacterImageGenerator.TargetCharacterIndex]);
	}
}


