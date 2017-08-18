using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Click : MonoBehaviour
{
    void OnClick()
    {
        SceneManager.LoadScene("Main");
    }
}
