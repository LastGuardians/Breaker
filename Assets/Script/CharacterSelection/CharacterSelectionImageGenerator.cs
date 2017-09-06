using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionImageGenerator: MonoBehaviour
{
	public GameObject Character;
	public GameObject Canvas;

	public Animator CharacterAnimator;

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
		CharacterImageGenerator.TargetCharacterIndex = 0;

		// 캐릭터 이미지 오브젝트 생성
		Character = new GameObject("Character");
		Character.transform.parent = Canvas.transform; // Canvas의 자식으로 이동
		Character.AddComponent<SpriteRenderer>();
		
		Character.transform.localScale = new Vector3(100f, 100f, 0);
		Character.transform.localPosition = new Vector3(-1000, 0, 0);

		Character.AddComponent<Animator>();
		AnimatorPath = "Animation/Animator/" + CharacterImageGenerator.CharacterStatusArray[CharacterImageGenerator.TargetCharacterIndex] + "Animator";
		Character.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(AnimatorPath);

		Character.transform.SetAsLastSibling();
	}

	public void ChangeIndex(string TargetButtonName)
	{
		if (TargetButtonName == "Left")
		{
			do
			{
				CharacterImageGenerator.TargetCharacterIndex = ((CharacterImageGenerator.TargetCharacterIndex - 1) % 12 + 12) % 12;
			}
			while (CharacterImageGenerator.CharacterStatusArray[CharacterImageGenerator.TargetCharacterIndex] == "Locked");
		}
		else if (TargetButtonName == "Right")
		{
			do
			{
				CharacterImageGenerator.TargetCharacterIndex = ((CharacterImageGenerator.TargetCharacterIndex + 1) % 12 + 12) % 12;
			}
			while (CharacterImageGenerator.CharacterStatusArray[CharacterImageGenerator.TargetCharacterIndex] == "Locked");
		}
		StartCoroutine(ChangeImage(TargetButtonName));
	}

	IEnumerator ChangeImage(string ClickedButton)
	{
		CharacterAnimator.SetBool(ClickedButton, true);

		yield return new WaitForSeconds(0.5f);

		Character.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Resources/Animation/Animator/" + CharacterImageGenerator.CharacterStatusArray[CharacterImageGenerator.TargetCharacterIndex] + "Animator");
		CharacterAnimator.SetBool(ClickedButton, false);
	}
}


