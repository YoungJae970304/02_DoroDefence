using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    public float movSpd = 10f;
    // Canvas의 Rendermode를 Screen Space - Camera로 설정해야 작동
    //카메라의 뷰포트 영역을 조정해 비율에 맞지 않은 부분을 여백으로 만드는코드
    private void Awake()
    {
        // 현재 GameObject에 부착된 Camera 컴포넌트를 가져오는 코드
        Camera cam = GetComponent<Camera>();

        // 현재 카메라의 뷰포트 영역을 가져오는 코드
        Rect viewportRect = cam.rect;

        // 원하는 가로 세로 비율을 계산하는 코드
        float screenAspectRatio = (float)Screen.width / (float)Screen.height;
        float targetAspectRatio = 16f / 9f; // 원하는 고정 비율 설정 (예: 16:9)

        // 화면 가로 세로 비율에 따라 뷰포트 영역을 조정하는 코드
        if (screenAspectRatio < targetAspectRatio)
        {
            // 화면이 더 '높다'면 (세로가 더 길다면) 세로를 조절하는 코드
            viewportRect.height = screenAspectRatio / targetAspectRatio;
            viewportRect.y = (1f - viewportRect.height) / 2f;
        }
        else
        {
            // 화면이 더 '넓다'면 (가로가 더 길다면) 가로를 조절하는 코드.
            viewportRect.width = targetAspectRatio / screenAspectRatio;
            viewportRect.x = (1f - viewportRect.width) / 2f;
        }

        // 조정된 뷰포트 영역을 카메라에 설정하는 코드
        cam.rect = viewportRect;
    }

    private void FixedUpdate()
    {
        CameraMov();
    }

    public void CameraMov()
    {
        //마우스나 터치 입력을 사용해 카메라를 유저가 직접 이동
        float moveSpeed = -0.1f; //카메라 이동 속도
        Vector3 camPos = Camera.main.transform.position;
        float movX = 0f;

        //마우스 오른쪽 버튼을 누를 때
        if (Input.GetMouseButton(1))
        {
            movX = Input.GetAxis("Mouse X") * movSpd;
        }
        //키보드 좌우키나 ad를 누를 때
        else
        {
            movX = -Input.GetAxis("Horizontal");
        }
        
        //입력이 있을 때만 카메라 이동
        if (movX != 0f)
        {
            camPos.x = Mathf.Clamp(camPos.x + movX * moveSpeed, 0f, 12.2f);
            Camera.main.transform.position = camPos;
        }
    }
}
