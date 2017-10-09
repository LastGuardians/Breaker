using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    Vector3 cameraPos;

    public float xMargin = 1f;      // Distance in the x axis the player can move before the camera follows.

    public float yMargin = 1f;      // Distance in the y axis the player can move before the camera follows.

    public float xSmooth = 8f;      // How smoothly the camera catches up with it's target movement in the x axis.

    public float ySmooth = 8f;      // How smoothly the camera catches up with it's target movement in the y axis.

    public Vector2 maxXAndY;        // The maximum x and y coordinates the camera can have.

    public Vector2 minXAndY;        // The minimum x and y coordinates the camera can have.


	void Start () {
        this.player = GameObject.Find("Player");
	}

    bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.transform.position.x) > xMargin;
    }
    
    bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.transform.position.y) > yMargin;
    }
    

    void FixedUpdate()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;
        
        //if (CheckXMargin())
        //    targetX = Mathf.Lerp(transform.position.x, player.transform.position.x, xSmooth * Time.deltaTime);
        

        if (CheckYMargin())
            targetY = Mathf.Lerp(transform.position.y, player.transform.position.y, ySmooth * Time.deltaTime);


        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
        transform.position = new Vector3(targetX, targetY, transform.position.z);

    }
    
    private void LateUpdate()
    {
        //TrackPlayer();
        //cameraPos.x = player.transform.position.x;
        //cameraPos.y = player.transform.position.y + 3;
        //cameraPos.z = transform.position.z;

        // 카메라 이동 보간
        //transform.position = Vector3.Lerp(
        //   transform.position, cameraPos, 7);

        //if (player.transform.position.y > 2)
        //{
        //    //transform.position = new Vector3(
        //    //   transform.position.x, cameraPos.y, transform.position.z);

        //    transform.position = Vector3.Lerp(
        //  transform.position, cameraPos, 7);
        //}

    }

    void Update()
    {
        //cameraPos.x = player.transform.position.x;
        //cameraPos.y = player.transform.position.y + 3;
        //cameraPos.z = transform.position.z;

        //transform.position = Vector3.Lerp(
        //   transform.position, cameraPos, 9 * Time.deltaTime);

    }
}
