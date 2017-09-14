using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image fade;
    float fades = 1.0f;
    float time = 0;

    public static Fade instance = null;

    private void Update()
    {
        time += Time.deltaTime;
        if (fades > 0.0f && time >= 0.1f)
        {
            fades -= 0.1f;
            fade.color = new Color(0, 0, 0, fades);
            time = 0;
        }
        else if(fades <= 0.0f)
        {

        }
    }
}