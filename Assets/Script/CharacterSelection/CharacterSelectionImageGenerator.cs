using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionImageGenerator: MonoBehaviour
{
	public GameObject CharacterImage;
	public UIAtlas CharacterImageAtlas;
	public UISprite CharacterImageSprite;
	public Animation CharacterImageAnimation;
	public AnimationClip LeftAnimationClip;
	public AnimationClip RightAnimationClip;

	public void Start()
	{
		if(CharacterButtonManager.TargetCharacter == null)
		{
			CharacterButtonManager.TargetCharacter = "select9";
		}


		// 캐릭터 이미지 오브젝트 생성
		CharacterImage = new GameObject("CharacterImage");
		CharacterImage.transform.parent = GameObject.Find("UI Root (2D)").transform; // UI Root의 자식으로 이동
		CharacterImage.layer = 9; //레이어를 GUI로 바꿈

		CharacterImageSprite = CharacterImage.AddComponent<UISprite>();

		CharacterImageAnimation = CharacterImage.AddComponent<Animation>();
		CharacterImageAnimation.playAutomatically = false;
		LeftAnimationClip.legacy = true;
		RightAnimationClip.legacy = true;
		CharacterImageAnimation.AddClip(LeftAnimationClip, "LeftAnimation");
		CharacterImageAnimation.AddClip(RightAnimationClip, "RightAnimation");
		
		CharacterImageSprite.atlas = CharacterImageAtlas;
		CharacterImageSprite.spriteName = CharacterButtonManager.TargetCharacter;
		CharacterImageSprite.depth = 1;

		CharacterImage.transform.localScale = new Vector3(360, 480, 0);
		CharacterImage.transform.localPosition = new Vector3(0, 0, 0);
	}

	public void ChangeCharacter(GameObject TargetButton)
	{
		int TargetCharacterIndex = int.Parse(CharacterButtonManager.TargetCharacter.Substring(6)) - 1;
		if (TargetButton.name == "LeftButton")
		{
			CharacterImageAnimation.Play("LeftAnimation");
			do
			{
				TargetCharacterIndex = ((TargetCharacterIndex - 1) % 16 + 16) % 16;
			}
			while (CharacterImageGenerator.CharacterStatusArray[TargetCharacterIndex] == "Locked");
		}
		else if (TargetButton.name == "RightButton")
		{
			CharacterImageAnimation.Play("RightAnimation");
			do
			{
				TargetCharacterIndex = ((TargetCharacterIndex + 1) % 16 + 16) % 16;
			}
			while (CharacterImageGenerator.CharacterStatusArray[TargetCharacterIndex] == "Locked");
		}

		CharacterButtonManager.TargetCharacter = "select" + (TargetCharacterIndex + 1).ToString();
		StartCoroutine(ChangeImage());
	}

	public IEnumerator ChangeImage()
	{
		yield return new WaitForSeconds(0.5f);
		CharacterImageSprite.spriteName = CharacterButtonManager.TargetCharacter;
	}
}


