using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OnButton : MonoBehaviour
{
    public UISprite TargetButton;
    public string TargetCharacter;
    public UISprite TargetSprite;
    private UIAtlas TargetAtlas;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                TargetSprite.atlas = Resources.Load("Atlas", typeof(UIAtlas)) as UIAtlas;
                TargetSprite.spriteName = "select1";
                Debug.Log(TargetSprite.spriteName);

                GameObject CharacterButton = new GameObject(string.Format("Selection{0}", i));
                CharacterButton.transform.position = new Vector3((j + 1) - 2, i - 1, 0);
            }
        }
    }

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
            TargetCharacter = TargetButton.name.Substring(7, TargetButton.name.Length);
        }
    }
}


