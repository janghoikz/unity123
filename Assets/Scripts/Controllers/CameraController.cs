using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;    //기본설정 BackView

    [SerializeField]
    public Transform _target;          //타겟등록
    [SerializeField]
    public Transform cameraArm;        //카메라 암 등록
    [SerializeField]
    Vector3 cameraPos = new Vector3(0.0f, 10f, -8.0f);  //기본 카메라 위치


    private float scrollSpeed = 5.0f;
    private float minScroll = 20.0f;
    private float maxScroll = 80.0f;

    void Start()
    {
     
    }

    void LateUpdate()
    {
        Camera.main.fieldOfView += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;   //카메라 줌인 줌아웃
        if (Camera.main.fieldOfView < minScroll)
        {
            Camera.main.fieldOfView = minScroll;
        }
        else if (Camera.main.fieldOfView > maxScroll)
        {
            Camera.main.fieldOfView = maxScroll;
        }

        if (Input.GetKeyDown(KeyCode.Tab))   //탭눌러서 화면모드 전환
        {
            switch (_mode)
            {
                case Define.CameraMode.BackView:
                    _mode++;
                    break;
                case Define.CameraMode.QuarterView:
                    _mode--;
                    break;
            }
        }

        switch (_mode)  //카메라 모드에따라 실행함수 변경
        {
            case Define.CameraMode.BackView:
                LookAround();
                break;
            case Define.CameraMode.QuarterView:
                LookQuarterView();
                break;
        }
        if (_mode == Define.CameraMode.QuarterView)
        {
            RaycastHit hit;
            Ray ray = new();
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red);
            if (Physics.Raycast(_target.transform.position, cameraPos, out hit, cameraPos.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - _target.transform.position).magnitude * 0.8f;
                transform.position = _target.transform.position + cameraPos.normalized * dist;
            }
            else
            {
                transform.LookAt(_target.transform);
            }
        }
    }

    private void LookAround()
    {
        cameraArm.transform.position = new Vector3(0f, 2f, -3f) + _target.position; //카메라암 기본 위치설정
        cameraArm.rotation = Quaternion.Euler(0f, 0f, 0f);
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));   //(임시) 마우스입력 Vector2로 받아오기
        Vector3 camAngle = cameraArm.rotation.eulerAngles;  //카메라 기존 Rotate값

        float x = camAngle.x - mouseDelta.y;    // 카메라 상하회전 제한기준값

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -40f, 60f);  // 위쪽방향 rotate 제한
        }
        else
        {
            x = Mathf.Clamp(x, 300f, 361f); // 아랫방향 rotate 제한
        }

        if (Input.GetMouseButton(1)) //마우스 우측 버튼이 눌려있을시, 카메라 회전갱신
        {
            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        }
    }

    private void LookQuarterView()
    {
        cameraArm.transform.position = new Vector3(0.0f, 10.0f, -8.0f) + _target.position;   // 쿼터뷰 벡터 초기값 = 플레이어 좌표 + 카메라 기준 벡터값
        cameraArm.rotation = Quaternion.Euler(40f, 0f, 0f); // 카메라 회전값, 아랫방향 40도 회전고정
    }
}
