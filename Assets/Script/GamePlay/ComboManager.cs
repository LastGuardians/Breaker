using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour {

    Sprite[] comboNum = new Sprite[10];
    int blockCnt = 0;
    int comboTime = 0;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < 10; ++i)
        {
            comboNum[i] = Resources.Load<Sprite>("UI/Number/digit" + i.ToString());

        }
    }
	
    // 블럭 파괴되면 코루틴 실행
	public IEnumerator ComboCheck()
    {
        blockCnt = PlayerManager.instance.blockCnt;
        while (true)
        {
            // 3초 전 카운트와 3초 후 카운트가 같으면 콤보 끊김.            
            yield return new WaitForSeconds(1f);
            comboTime += 1;

            if(comboTime >= 3)
            {
                if(blockCnt == PlayerManager.instance.blockCnt)
                {
                    PlayerManager.instance.blockCnt = 0;
                    comboTime = 0;
                }
            }
        }

        if (blockCnt != PlayerManager.instance.blockCnt)
        {

        }
    }

}
