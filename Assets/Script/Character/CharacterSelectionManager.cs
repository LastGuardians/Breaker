using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
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
		CharacterImageSprite.spriteName = SceneButtonManager.TargetCharacter;
		CharacterImageSprite.depth = 1;

		CharacterImage.transform.localScale = new Vector3(180, 240, 0);
		CharacterImage.transform.localPosition = new Vector3(0, 0, 0);
	}

	public void ChangeCharacter(UISprite TargetButton)
	{
		int TargetCharacterIndex = int.Parse(SceneButtonManager.TargetCharacter.Substring(6));
		if (TargetButton.name == "LeftButton")
		{
			if(TargetCharacterIndex == 1)
			{
				TargetCharacterIndex = 16;
			}
			else
			{
				TargetCharacterIndex--;
			}
		}
		else if (TargetButton.name == "RightButton")
		{
			if (TargetCharacterIndex == 16)
			{
				TargetCharacterIndex = 1;
			}
			else
			{
				TargetCharacterIndex++;
			}
		}
		SceneButtonManager.TargetCharacter = "select" + TargetCharacterIndex.ToString();
		CharacterImageSprite.spriteName = SceneButtonManager.TargetCharacter;
	}
}
