using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    RaycastHit hit_0; // RaycastHit 선언.

    void Update()
    {

        Vector3 ray_position = transform.position; //현재 위치.
        //Debug.Log("ray_position.y : " + ray_position.y);

        Vector3 ray_direction_R = transform.TransformDirection(0, 5, 0); //x값이 1, y값이 0,  z값이 0 방향.

        //Vector2 pos = transform.position;
        //Ray2D ray = new Ray2D(pos, Vector2.zero);
        //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        Ray ray = new Ray(transform.position, Vector3.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        
        if(hit.collider.tag == "block1")
        {
            Debug.Log("블럭과 충돌");
        }

        if (Physics.Raycast(ray_position, ray_direction_R, out hit_0, 1f)) // 0.5거리가 떨어져있는 위치에 닿는 다면.{
        {
            if (hit_0.collider.tag == "block1")
            { //Block_R tag라면 -0.75 위치로 이동한다. (접근을 제한한 것.).
                Debug.Log("무기와 충돌");   
            }
        }
    }
}