using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour {

    public Button displayButton;
    public Text displayButtonText;
    string[] displayStr = new string[5];
    int range = 0;
    System.Random r = new System.Random();

    // Use this for initialization
    void Start () {    
        range = r.Next(0, 5);
        //Debug.Log("range : " + range);

        displayStr[0] = "Rating!";  // 별점주기
        displayStr[1] = "Watching Ad";  // 영상 광고
        displayStr[2] = "Get password"; // 홈페이지(캐릭터 패스워드 게시글)
        displayStr[3] = "More games";   // 페이지 광고
        displayStr[4] = "About BandalSoft"; // 홈페이지 메인

        displayButtonText.GetComponent<Text>().text = displayStr[range];

    }
	
    public void ButtonStatus()
    {
        if(range.Equals(0))
        {
            GameObject.Find("ResultManager").GetComponent<RankingManager>().ShowLeaderboardUI();
        }
        else if(range.Equals(1))
        {
            GameObject.Find("AdManager").GetComponent<UnityAdsHelper>().ShowRewardedAd();
        }
        else if (range.Equals(2))
        {

        }
        else if (range.Equals(3))
        {

        }
        else if (range.Equals(4))
        {

        }
    }
}
