﻿using System.Collections;
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

    public static UnityEngine.SocialPlatforms.ILocalUser mainplayeruserdata;

	public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";
    public int iResultScore = 0;
    public int iMaxScore = 0;
    public int iResultCoin = 0;
    public int iResultkey = 0;
    public bool bGameEnd = false;

    System.Random r = new System.Random();
    int idRange = 0;

    public static string mainId;

    // Use this for initialization
    void Start ()
	{
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
        bGameEnd = false;

        StartCoroutine(ResultCheck());
        StartCoroutine(SetUserStatus());

        Login();
	}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        SceneManager.LoadScene("Main");
    //    }
    //}

    public IEnumerator ResultCheck()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if (GameObject.Find("ResultManager") != null)
            {
                ResultManager.instance.ResultScore(iResultScore);
                ResultManager.instance.MaxScore(iMaxScore);
                ResultManager.instance.ResultCoin(iResultCoin);
                ResultManager.instance.ResultKey(iResultkey);
            }
        }
    }

    public IEnumerator SetUserStatus()
    {
        while(true)
        {
            yield return new WaitUntil(() => bGameEnd);
            //Debug.Log("bGameEnd: " + bGameEnd);
            StartCoroutine(Rank.instance._CreateRank(mainId, iMaxScore));
            StartCoroutine(UserConnect.instance._SetUser(mainId, (int)UserConnect.CoinAmount, UserConnect.KeyAmount,
                UserConnect.CurrentWeaponIndex.ToString(), UserConnect.CurrentCharacterIndex.ToString()));

        }
    }

    IEnumerator TestUserLogin()
    {
        int time = 0;
        while(true)
        {
            yield return new WaitForSeconds(1f);
            time += 1;

            if(time >= 3)
            {
                SceneManager.LoadScene("Main");
                yield break;
            }
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
				//StartCoroutine(GameObject.Find("NetworkManager").GetComponent<UserConnect>()._GetUser(mainplayeruserdata.userName));
				//StartCoroutine(_GetUser(mainplayeruserdata.userName));    // 로그인한 유저 조회
				while (true)
				{
					if (Social.localUser.id != "")
					{
						mainId = Social.localUser.id;
						GameObject NetworkManagerPrefab = Resources.Load("Prefabs/NetworkManager") as GameObject;
						GameObject NetworkManager = MonoBehaviour.Instantiate(NetworkManagerPrefab) as GameObject;
						SceneManager.LoadScene("Main");// 메인 화면으로 전환
						break;
					}
				}
			}
            else
            {
                // 로그인 실패 처리
                Debug.Log("Login Fail...");
                idRange = r.Next(0, 100000);
                mainId = idRange.ToString();
                //Debug.Log("mainId: " + mainId);
                GameObject NetworkManagerPrefab = Resources.Load("Prefabs/NetworkManager") as GameObject;
				GameObject NetworkManager = MonoBehaviour.Instantiate(NetworkManagerPrefab) as GameObject;
                StartCoroutine(TestUserLogin());
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
                //Debug.Log("총 score : " + score);
                // 그에 따른 처리
            }
            else
            {
                // Report 실패
                // 그에 따른 처리
            }
        });

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
