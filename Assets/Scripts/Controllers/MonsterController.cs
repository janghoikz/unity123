using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
	[SerializeField]
	protected Vector3 _destPos;

	[SerializeField]
	protected Define.State _state = Define.State.Idle;

	[SerializeField]
	protected GameObject _lockTarget;

	Stat _stat;

	[SerializeField]
	float _scanRange = 10;

	[SerializeField]
	float _attackRange = 2.5f;

    private void Start()
    {
        _stat = GetComponent<Stat>();
    }

    void Update()
	{
		switch (State)
		{
			case Define.State.Die:
				UpdateDie();
				break;
			case Define.State.Moving:
				UpdateMoving();
				break;
			case Define.State.Idle:
				UpdateIdle();
				break;
			case Define.State.Skill:
				UpdateSkill();
				break;
		}
	}

	public Define.State State
	{
		get { return _state; }
		set
		{
			_state = value;

			Animator anim = GetComponent<Animator>();
			switch (_state)
			{
				case Define.State.Die:
					break;
				case Define.State.Idle:
					anim.SetInteger("speed", 0);
					anim.SetBool("attack", false);
					break;
				case Define.State.Moving:
					anim.SetInteger("speed", 5);
					anim.SetBool("attack", false);
					break;
				case Define.State.Skill:
					anim.SetBool("attack", true);
					break;
			}
		}
	}

	void UpdateDie()
	{ 
		Destroy(this.gameObject);
	}

	protected void UpdateIdle()
	{
		if (_stat.Hp <= 0)
		{
			State = Define.State.Die;
		}
		_destPos = _lockTarget.transform.position;
		GameObject player = _lockTarget;
		if (player == null)
			return;

		float distance = (player.transform.position - transform.position).magnitude;
		if (distance <= _attackRange)
		{
			State = Define.State.Skill;
			return;
		}
		if (distance <= _scanRange)
		{
			State = Define.State.Moving;
			return;
		}
	}

	protected void UpdateMoving()
	{
		if (_stat.Hp <= 0)
		{
			State = Define.State.Die;
		}

		_destPos = _lockTarget.transform.position;
		float distance = (_destPos - transform.position).magnitude;
		if (distance <= _attackRange)
		{
			State = Define.State.Skill;
			return;
		}

		// 이동
		Vector3 dir = _destPos - transform.position;
		if (dir.magnitude < 1.5f)
		{
			State = Define.State.Idle;
		}
		else
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
			transform.position += transform.TransformDirection(Vector3.forward * Time.deltaTime * _stat.Speed);
		}
	}

	protected void UpdateSkill()
	{
		if (_stat.Hp <= 0)
		{
			State = Define.State.Die;
		}
		if (_lockTarget != null)
		{
			Vector3 dir = _lockTarget.transform.position - transform.position;
			Quaternion quat = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
		}
		State = Define.State.Idle;
	}

	void OnHitEvent()
	{
		if (_stat.Hp <= 0)
		{
			State = Define.State.Die;
		}
		// 체력
		Stat targetStat = _lockTarget.GetComponent<Stat>();
		if (_lockTarget != null)
		{
			Stat myStat = gameObject.GetComponent<Stat>();
			int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
			targetStat.Hp -= damage;
		}

		if (targetStat.Hp > 0)
		{
			float distance = (_lockTarget.transform.position - transform.position).magnitude;
			if (distance <= _attackRange)
				State = Define.State.Skill;
			else
				State = Define.State.Moving;
		}
		else
		{
			State = Define.State.Idle;
		}
	}
}
