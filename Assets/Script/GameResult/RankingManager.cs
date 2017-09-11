using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;

public class RankingManager : MonoBehaviour
{
    public void ShowLeaderboardUI()     // 리더보드 표시 
    {
        // Sign In 이 되어있지 않은 상태라면
        // Sign In 후 리더보드 UI 표시 요청할 것
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                    // 바로 리더보드 UI 표시 요청
                    Social.ShowLeaderboardUI();
                    return;
                }
                else
                {
                    // Sign In 실패 
                    // 그에 따른 처리
                    return;
                }
            });
        }
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI4L-S66kTEAIQAw");
    }

}