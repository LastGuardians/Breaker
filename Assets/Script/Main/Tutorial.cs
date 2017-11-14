using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

    Sprite[] tutorial = new Sprite[10];
    public GameObject TutoImage;
    public GameObject RightCollider;    

    public int current = 0;

    private void Start()
    {
        for (int i = 0; i < 10; ++i) 
        {
            tutorial[i] = Resources.Load<Sprite>("UI/Tutorial/tutorial" + (i + 1).ToString());
        }
        
        current = 0;
    }

    public void LeftButton()
    {
        if(current > 0)
        {
            TutoImage.GetComponent<SpriteRenderer>().sprite = tutorial[current - 1];
            --current;
            RightCollider.SetActive(false);
        }
        else if(current.Equals(0))
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void RightButton()
    {
        if (current < 9)
        {
            TutoImage.GetComponent<SpriteRenderer>().sprite = tutorial[current + 1];
            ++current;

            if(current.Equals(9))
                RightCollider.SetActive(true);
        }
        else
        {            
            return;
        }
    }
}
