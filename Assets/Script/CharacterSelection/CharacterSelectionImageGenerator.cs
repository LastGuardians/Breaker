using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionImageGenerator: MonoBehaviour
{
	private GameObject CharacterImage;
	public UIAtlas CharacterImageAtlas;
	private UISprite CharacterImageSprite;

	private void Start()
	{
		// 캐릭터 이미지 오브젝트 생성
		CharacterImage = new GameObject("CharacterImage");
		CharacterImage.transform.parent = GameObject.Find("UI Root (2D)").transform; // UI Root의 자식으로 이동
		CharacterImage.layer = 9; //레이어를 GUI로 바꿈

		CharacterImageSprite = CharacterImage.AddComponent<UISprite>();

		CharacterImageSprite.atlas = CharacterImageAtlas;
		CharacterImageSprite.spriteName = CharacterButtonManager.TargetCharacter;
		CharacterImageSprite.depth = 1;

		CharacterImage.transform.localScale = new Vector3(360, 480, 0);
		CharacterImage.transform.localPosition = new Vector3(0, 0, 0);
	}

	public void ChangeCharacter(UISprite TargetButton)
	{
		int TargetCharacterIndex = int.Parse(CharacterButtonManager.TargetCharacter.Substring(6)) - 1;
		if (TargetButton.name == "LeftButton")
		{
			do
			{
				TargetCharacterIndex = ((TargetCharacterIndex - 1) % 16 + 16) % 16;
			}
			while (CharacterImageGenerator.CharacterStatusArray[TargetCharacterIndex] == "Locked");
		}
		else if (TargetButton.name == "RightButton")
		{
			do
			{
				TargetCharacterIndex = ((TargetCharacterIndex + 1) % 16 + 16) % 16;
			}
			while (CharacterImageGenerator.CharacterStatusArray[TargetCharacterIndex] == "Locked");

		}
		CharacterButtonManager.TargetCharacter = "select" + (TargetCharacterIndex + 1).ToString();
		CharacterImageSprite.spriteName = CharacterButtonManager.TargetCharacter;
	}
}
