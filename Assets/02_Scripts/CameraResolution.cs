using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    public float movSpd = 10f;
    // Canvas�� Rendermode�� Screen Space - Camera�� �����ؾ� �۵�
    //ī�޶��� ����Ʈ ������ ������ ������ ���� ���� �κ��� �������� ������ڵ�
    private void Awake()
    {
        // ���� GameObject�� ������ Camera ������Ʈ�� �������� �ڵ�
        Camera cam = GetComponent<Camera>();

        // ���� ī�޶��� ����Ʈ ������ �������� �ڵ�
        Rect viewportRect = cam.rect;

        // ���ϴ� ���� ���� ������ ����ϴ� �ڵ�
        float screenAspectRatio = (float)Screen.width / (float)Screen.height;
        float targetAspectRatio = 16f / 9f; // ���ϴ� ���� ���� ���� (��: 16:9)

        // ȭ�� ���� ���� ������ ���� ����Ʈ ������ �����ϴ� �ڵ�
        if (screenAspectRatio < targetAspectRatio)
        {
            // ȭ���� �� '����'�� (���ΰ� �� ��ٸ�) ���θ� �����ϴ� �ڵ�
            viewportRect.height = screenAspectRatio / targetAspectRatio;
            viewportRect.y = (1f - viewportRect.height) / 2f;
        }
        else
        {
            // ȭ���� �� '�д�'�� (���ΰ� �� ��ٸ�) ���θ� �����ϴ� �ڵ�.
            viewportRect.width = targetAspectRatio / screenAspectRatio;
            viewportRect.x = (1f - viewportRect.width) / 2f;
        }

        // ������ ����Ʈ ������ ī�޶� �����ϴ� �ڵ�
        cam.rect = viewportRect;
    }

    private void FixedUpdate()
    {
        CameraMov();
    }

    public void CameraMov()
    {
        //���콺�� ��ġ �Է��� ����� ī�޶� ������ ���� �̵�
        float moveSpeed = -0.1f; //ī�޶� �̵� �ӵ�
        Vector3 camPos = Camera.main.transform.position;
        float movX = 0f;

        //���콺 ������ ��ư�� ���� ��
        if (Input.GetMouseButton(1))
        {
            movX = Input.GetAxis("Mouse X") * movSpd;
        }
        //Ű���� �¿�Ű�� ad�� ���� ��
        else
        {
            movX = -Input.GetAxis("Horizontal");
        }
        
        //�Է��� ���� ���� ī�޶� �̵�
        if (movX != 0f)
        {
            camPos.x = Mathf.Clamp(camPos.x + movX * moveSpeed, 0f, 12.2f);
            Camera.main.transform.position = camPos;
        }
    }
}
