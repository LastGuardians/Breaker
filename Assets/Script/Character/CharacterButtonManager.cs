using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButtonManager : MonoBehaviour
{

    public UIAtlas TargetAtlas;
	
    void Start()
    {
		for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                GameObject CharacterButton = new GameObject(string.Format("SelectButton{0}", 4 * (i - 1) + j)); // 버튼 오브젝트 생성
				CharacterButton.transform.parent = GameObject.Find("UI Root (2D)").transform; // UI Root의 자식으로 이동
				CharacterButton.layer = 9; //레이어를 GUI로 바꿈

				UISprite TargetSprite = CharacterButton.AddComponent<UISprite>();
				BoxCollider TargetCollider = CharacterButton.AddComponent<BoxCollider>();

				System.Type ButtonScript = System.Type.GetType("OnButton");
				CharacterButton.AddComponent(ButtonScript);

				TargetSprite.atlas = TargetAtlas;
				TargetSprite.spriteName = "select" + (4 * (i - 1) + j).ToString();
				TargetSprite.depth = 1;

				CharacterButton.transform.localScale = new Vector3(80, 120, 0);
				CharacterButton.transform.localPosition = new Vector3(100 * j - 250 , 130 * (5 - i) - 300, 0);
			}
		}
    }
}


