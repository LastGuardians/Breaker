using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //Debug.Log(GPGSManager.instance.userdata);
	}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void CharacterSelect()
    {
        SceneManager.LoadScene("Character");
    }
}
