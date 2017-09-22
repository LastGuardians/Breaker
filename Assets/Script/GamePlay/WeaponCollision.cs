using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    RaycastHit hit_0; // RaycastHit 선언.

    void Update()
    {

        Vector3 ray_position = transform.position; //현재 위치.

        Vector3 ray_direction_R = transform.TransformDirection(0, 1, 0); //x값이 1, y값이 0,  z값이 0 방향.

        if (Physics.Raycast(ray_position, transform.forward, out hit_0, 0.51f)) // 0.5거리가 떨어져있는 위치에 닿는 다면.{
        {
            if (hit_0.collider.tag == "block1")
            { //Block_R tag라면 -0.75 위치로 이동한다. (접근을 제한한 것.).
                Debug.Log("무기와 충돌");   
            }
        }
    }
}