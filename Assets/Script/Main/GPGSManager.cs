using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using GooglePlayGames.BasicApi;
using System.Net;
using System.IO;
using System.Text;

public class GPGSManager : MonoBehaviour {
    
    string LoginUrl;

    public static GPGSManager instance = null;

    Network nw = new Network();

    string userdata;

    private float timer = 0.0f;

    public UnityEngine.SocialPlatforms.ILocalUser mainplayeruserdata;


    void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
            AudioSource splash_bgm = GameObject.Find("SoundManager").GetComponent<AudioSource>();
            Destroy(splash_bgm);
            //instance = new GameObject("GPGSManager").AddComponent<GPGSManager>();
        }
        else if (instance != this)
        {
            //잘못된 인스턴스를 가르키고 있을 경우
            Destroy(gameObject);
        }

        LoginUrl = "tozha31@tozha31.woobi.co.kr/mobile_php_connect.php";

        //StartCoroutine(LoginCor());
        Login();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene("Main");
        }
    }


    public void Login()
    {
        //#if UNITY_ANDROID

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();

//#elif UNITY_IOS
 
//        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
 
//#endif

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                // 로그인 성공 처리
                Debug.Log("Login Success!");

                mainplayeruserdata = GetUserInfoData();  // 유저 데이터 가져옴

                if (mainplayeruserdata == null)
                {
                    Debug.LogErrorFormat("!! SignInCallback Error Null MainPlayer Data : ");
                }
                else
                {
                    userdata = string.Format("UserData : {0}, {1}, {2}, {3}"
                        , mainplayeruserdata.id
                        , mainplayeruserdata.userName
                        , mainplayeruserdata.image
                        , mainplayeruserdata.friends
                        );
                    Debug.LogFormat("SignInCallback MainPlayer Data : {0} ", userdata);
                }

                //StartCoroutine(LoginCor());
                SceneManager.LoadScene("Main");     // 메인 화면으로 전환
              
            }
            else
            {
                // 로그인 실패 처리
                Debug.Log("Login Fail...");
            }
        });
    }

    public void LogOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    public bool Authenticated
    {
        get
        {
            return Social.Active.localUser.authenticated;
        }
    }

    public UnityEngine.SocialPlatforms.ILocalUser GetUserInfoData()
    {
        if (Authenticated)
        {
            // Active 가 null일수 있음
            //return Social.Active.localUser;
            return Social.localUser;
        }
        else
        {
            return null;
        }

    }

    public void UnlockAchievement(int score)        // 업적 잠금 해제
    {
        if (score >= 100)
        {
//#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_score, 100f, null);
//#elif UNITY_IOS
//            Social.ReportProgress("Score_100", 100f, null);
//#endif
        }
    }

    public void ReportScore(int score)      // 리더보드에 점수 기록
    {
        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_ranking, (bool success) =>
        {
            if (success)
            {
                // Report 성공
                Debug.Log("총 score : " + score);
                // 그에 따른 처리
            }
            else
            {
                // Report 실패
                // 그에 따른 처리
            }
        });

    }

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


    IEnumerator LoginCor()
    {
        Debug.Log("LoginCor 생성");
        // 폼 필드 생성
        //WWWForm loginData = new WWWForm();
        ////현재 id 값을 php의 id로 대입
        //loginData.AddField("id", "test2");
        //loginData.AddField("password", "1234");

        byte[] dummy = new byte[10];
        dummy[0] = 100;
        //폼필드 데이터 전송
        WWW login = new WWW(LoginUrl, dummy);

        yield return login;

        Debug.Log(login.text);


    }

  

}
