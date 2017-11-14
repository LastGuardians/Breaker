using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider slider;
    private bool IsLoadGame = false;
    private float timer = 0.0f;
    public Text tips;
    string[] tipStr = new string[6];

    int range = 0;
    System.Random r = new System.Random();

    private AsyncOperation async;

    // Use this for initialization
    void Start()
    {
        //가비지 콜렉터 실행
        Resources.UnloadUnusedAssets();
        range = r.Next(0, 6);

        tipStr[0] = "Check 'Tutorial' on the Setting panel";
        tipStr[1] = "Contact with bombs will cost you 2 lives";
        tipStr[2] = "You can break more buildings during fever times";
        tipStr[3] = "Whenever alert alarm rings, watch out for falling obstacles.";
        tipStr[4] = "You can share ranking with others by pressing ranking button";
        tipStr[5] = "You can get 10 keys after watching advertisement by pressing AD button";

        tips.GetComponent<Text>().text = tipStr[range];      

        StartCoroutine(StartLoad("GamePlay"));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        slider.value = timer;
        if (timer >= 5)
        {
            async.allowSceneActivation = true;
            //타이머가 어느정도 로딩시간을 벌어줌
        }
    }

    public IEnumerator StartLoad(string Scene)
    {
        async = SceneManager.LoadSceneAsync(Scene);
        //장면이 준비된 즉시 장면이 활성화되는 것을 허용하지 않습니다.
        async.allowSceneActivation = false;

        if (!IsLoadGame)
        {
            IsLoadGame = true;

            while (async.progress < 0.9f)
            {
                slider.value = async.progress;
                yield return true;
            }
        }
    }
}
