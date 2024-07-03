using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedChildRotation : MonoBehaviour
{
    Quaternion noRot;
    float noMovY;

    void Start()
    {
        // 자식 오브젝트의 초기 회전값,Y값 저장
        noRot = transform.rotation;
        noMovY = transform.position.y;
    }


    void LateUpdate()
    {
        // 자식 오브젝트의 회전을 초기 회전값으로 복원
        transform.rotation = noRot;

        // 자식 오브젝트의 위치를 초기 y 위치로 복원
        Vector2 pos = transform.position;
        pos.y = noMovY;
        transform.position = pos;
    }
}
