using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    Vector3 cameraPos;

	void Start () {
        this.player = GameObject.Find("Player");
	}

    private void LateUpdate()
    {
        cameraPos.x = player.transform.position.x;
        cameraPos.y = player.transform.position.y + 3;
        cameraPos.z = transform.position.z;

        // 카메라 이동 보간
        transform.position = Vector3.Lerp(
           transform.position, cameraPos, 7 * Time.deltaTime);
    }

 //   void Update () {
 //       Vector3 playerPos = this.player.transform.position;
 //       transform.position = Vector3.Lerp(
 //           transform.position, cameraPos, 1);

	//}
}
