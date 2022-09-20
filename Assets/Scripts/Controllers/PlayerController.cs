using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	int _mask = (1 << (int)Define.Layer.Wall) | (1 << (int)Define.Layer.Monster);
	PlayerStat _stat;

	GameObject _lockTarget;

	Vector3 _destPos;

    void Start()
    {
		_stat = GetComponent<PlayerStat>();

		Managers.Input.KeyAction -= OnKeyboard;	//	실수로 다른부분에서 키액션을 입력할 경우를 방지(취소후)
		Managers.Input.KeyAction += OnKeyboard;

		Managers.Input.MouseAction -= OnMouseClicked;
		Managers.Input.MouseAction += OnMouseClicked;


		//임시
		//Managers.UI.ShowSceneUI<UI_Inven>();
	}

	public enum PlayerState
	{
		Die,
		Moving,
		Attack,
		Idle,
	}

	[SerializeField]
	PlayerState _state = PlayerState.Idle;

	public PlayerState State
	{ 
		get { return _state; }
		set 
		{ 
			_state = value; 
			Animator anim = GetComponent<Animator>();
			switch (_state)
			{
				case PlayerState.Die:
					anim.SetBool("attack", false);
					break;
				case PlayerState.Idle:
					anim.SetFloat("speed", 0);
					anim.SetBool("attack", false);
					break;
				case PlayerState.Moving:
					anim.SetBool("attack", false);
					anim.SetFloat("speed", _stat.Speed);
					break;
				case PlayerState.Attack:
					anim.SetBool("attack", true);
					break;
			}
		}
	}

	void UpdateDie()
	{
		// 아무것도 못함

	}

	void UpdateMoving()
	{
		//Vector3 dir = _destPos - transform.position;
		//if (dir.magnitude < 0.0001f)
		//{
		//	_state = PlayerState.Idle;
		//}
		//else
		//{
		//	float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
		//	transform.position += dir.normalized * moveDist;
		//	transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
		//}
		// 애니메이션
		//Animator anim = GetComponent<Animator>();
		// 현재 게임 상태에 대한 정보를 넘겨준다
		//anim.SetFloat("speed", _speed);
	}
	void Update()
	{
		if (Input.anyKey != true)
		{
			_state = PlayerState.Idle;
		}
		switch (State)
		{
			case PlayerState.Die:
				UpdateDie();
				break;
			case PlayerState.Moving:
				UpdateMoving();
				break;
			case PlayerState.Idle:
				UpdateIdle();
				break;
			case PlayerState.Attack:
				UpdateAttack();
				break;
		}
	}

	void UpdateIdle()
	{
		Animator anim = GetComponent<Animator>();
		anim.SetFloat("speed", 0);
	}

    void OnKeyboard()
    {
		#region 이동메모
		//  Local -> World(지역좌표에서 월드좌표로 변환) == transform.TransformDirection
		//  World -> Local(월드좌표에서 지역좌표로 변환) == transform.InverseTransformDirection

		//  transform.position += transform.TransformDirection(Vector3.forward * Time.deltaTime * _speed);
		//  transform.Translate(Vector3.forward * Time.deltaTime * _speed)
		//  위의 둘은 같은 문법이다.
		#endregion
		#region 회전메모
		//  Euler Angle : x,y,z 3개의 축을 기준으로 0~360도만큼 회전시키는 좌표계
		//  하지만 '짐벌락'이라는 문제점이 존재해 x,y,z,w를 이용하는 Quaternion(사원수)를 이용해야한다.
		//  유니티의 모든 회전에는 Quaternion이 사용되며 우리는 함수를 이용해 Euler를 그대로 사용할수 있다.

		//  transform.rotation == 게임 오브젝트를 회전시키기 위한 함수 (Quaternion을 기반으로 함)
		//  사용예제 : transform.rotation = Quaternion.Euler(x,y,z);

		//  Quaternion.Euler == rotation에 수치를 대입하기 위해 사용되는 함수
		//  사용예제 : 위와같음

		//  Quaternion.Slerp == 오브젝트 회전을 부드럽게 하기위해 사용
		//  사용예제 : Quaternion.Slerp( a, b, t);
		//  a == 회전시작값 즉, 현재 오브젝트의 rotate값
		//  b == 회전목표값, 오브젝트의 최종 rotate값
		//  t == 보간비율(회전속도), float형이며 0에 가까울수록 출력이 a와 가깝고 1에 가까울수록 출력이 b와 가깝다.

		//  Quaternion.LookRotation == target을 바라보는 함수
		//  사용예제 : Quaternion.LookRotation(Vector3.foward, Vector3.up) 또는 Quaternion.LookRotation(Vector3.foward)
		//  Vector3.foward == 오브젝트는 현재위치를 기준으로 (0,0,1) 을 바라본다.
		//  Vector3.up == 오브젝트의 머리(상단)는 (0,1,0)을 바라본다. 생략할경우 자동으로 Vector3.up이 적용된다.
		#endregion
		//  Time.deltaTime == 프레임이 완료되기까지 걸린 시간(성능에 따라 한프레임에 나오는 결과값 보정용)
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) 
			|| Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
		{
			State = PlayerState.Moving;
		}
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _stat.Speed;
		}
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _stat.Speed;
		}
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _stat.Speed;
		}
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _stat.Speed;
		}
		if (Input.GetKey(KeyCode.Mouse0))
		{
			State = PlayerState.Attack;
		}
		//_moveToDest = false;
	}
	void UpdateAttack()
	{
		
	}

	void OnHitEvent()
	{
		
		if (_lockTarget != null)
		{
			Stat targetStat = _lockTarget.GetComponent<Stat>();
			PlayerStat myStat = gameObject.GetComponent<PlayerStat>();
			int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
			targetStat.Hp -= damage;
		}
		State = PlayerState.Moving;
	}

    void OnMouseClicked(Define.MouseEvent evt)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
		Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

		if (raycastHit)
		{
			if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
				_lockTarget = hit.collider.gameObject;
			else
				_lockTarget = null;
		}

		//if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
		//{
		//	_destPos = hit.point;
		//	_state = PlayerState.Moving;
		//}
	}
}
