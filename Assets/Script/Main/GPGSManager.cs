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
using LitJson;

public class GPGSManager : MonoBehaviour {
    
    string LoginUrl;

    public static GPGSManager instance = null;

    Network nw = new Network();

    string userdata;

    private float timer = 0.0f;

    public UnityEngine.SocialPlatforms.ILocalUser mainplayeruserdata;


    public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";
    public string userId = "TestUser";
    public int coin = 0;
    public int prisonKey = 0;


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

        //LoginUrl = "tozha31@tozha31.woobi.co.kr/mobile_php_connect.php";

        //userId = mainplayeruserdata.id;
        //StartCoroutine(LoginCor());
        Login();
        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SceneManager.LoadScene("Main");
        //}
    }

    // 서버와 통신할 코루틴 생성
    public void CreateUser()
    {
        StartCoroutine(_CreateUser(userId));
    }

    public void GetUser()
    {
        StartCoroutine(_GetUser(userId));
    }

    public void SetUser()
    {
        StartCoroutine(_SetUser(userId, coin, prisonKey));
    }

    public void DeleteUser()
    {
        StartCoroutine(_DeleteUser(userId));
    }


    /* 유저 생성 */
    public IEnumerator _CreateUser(string userId)
    {
        Debug.Log("_CreateUser 코루틴 시작");
        string url = baseUrl + "/user/create";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("userId", userId);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }

    /* 유저 조회 */
    public IEnumerator _GetUser(string userId)
    {
        Debug.Log("_GetUser 코루틴 시작");
        Debug.Log("userId: " + userId);
        string url = baseUrl + "/user/" + userId;
        WWW www = new WWW(url);
        yield return www;

        PrintLog(www.error);
        try
        {
            if (www.error != null)   // 조회한 유저가 없음.
            {
                StartCoroutine(_CreateUser(mainplayeruserdata.userName));
            }
        }
        catch(KeyNotFoundException ex)
        {

        }
             

        JsonData json = JsonMapper.ToObject(www.text);
        PrintLog(json["userId"] + " " + json["coin"] + " " + json["prisonKey"]);
        
    }

    /* 유저 갱신 */
    public IEnumerator _SetUser(string userId, int coin, int prisonKey)
    {
        string url = baseUrl + "/user/" + userId + "/update";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("coin", coin);
        form.AddField("prisonKey", prisonKey);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }


    /* 유저 삭제 */
    public IEnumerator _DeleteUser(string userId)
    {
        string url = baseUrl + "/user/" + userId + "/delete";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("padding", 0);

        WWW www = new WWW(url, form);
        yield return www;

        PrintLog(www.error);
    }

    void PrintLog(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            Debug.Log(message);
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
                SceneManager.LoadScene("Main");     // 메인 화면으로 전환
                
                StartCoroutine(_GetUser(mainplayeruserdata.userName));    // 로그인한 유저 조회
              
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
