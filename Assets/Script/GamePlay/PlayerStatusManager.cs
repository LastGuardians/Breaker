using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatusManager : MonoBehaviour
{
    //public User user_data;

    public int score = 0;       // 획득 점수
    public int best_score = 0;  // 획득한 점수 중 최고 점수
    public int coin = 0;        // 코인 수
    public int key = 0;         // 열쇠 수


    public static PlayerStatusManager instance = null;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //잘못된 인스턴스를 가르키고 있을 경우
            Destroy(gameObject);
        }
    }
}