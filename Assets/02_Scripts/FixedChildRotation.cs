using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedChildRotation : MonoBehaviour
{
    Quaternion noRot;
    float noMovY;

    void Start()
    {
        // �ڽ� ������Ʈ�� �ʱ� ȸ����,Y�� ����
        noRot = transform.rotation;
        noMovY = transform.position.y;
    }


    void LateUpdate()
    {
        // �ڽ� ������Ʈ�� ȸ���� �ʱ� ȸ�������� ����
        transform.rotation = noRot;

        // �ڽ� ������Ʈ�� ��ġ�� �ʱ� y ��ġ�� ����
        Vector2 pos = transform.position;
        pos.y = noMovY;
        transform.position = pos;
    }
}
