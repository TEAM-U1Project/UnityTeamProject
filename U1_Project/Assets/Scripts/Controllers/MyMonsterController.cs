﻿using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    class MyMonsterController : MonsterController
    {
        protected override void Init()
        {
            base.Init();
            Managers.Input.KeyAction -= GetDirOrder;
            Managers.Input.KeyAction += GetDirOrder;
        }

        protected override void UpdateController()
        {
            switch (_state)
            {
                case State.Dead:
                    break;
                case State.Moving:
                    GetDirOrder();
                    break;
                case State.Idle:
                    GetDirOrder();
                    break;
            }

            base.UpdateController();
        }

        protected override void UpdateIdle()
        {
            // 이동 상태로 갈지 확인
            if (Dir != MoveDir.None)
            {
                State = State.Moving;
                return;
            }

            if (_coSkillColltime == null && Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Skill! Space 클릭");

                C_Skill skill = new C_Skill() { Info = new SkillInfo() };
                skill.Info.SkillId = 1;
                Managers.Network.Send(skill);

                _coSkillColltime = StartCoroutine("ColInputCooltime", 0.2f);
            }
        }

        Coroutine _coSkillColltime;
        IEnumerator ColInputCooltime(float time)
        {
            yield return new WaitForSeconds(time);
            _coSkillColltime = null;
        }

        void GetDirOrder()
        {
            // 싸울 타겟을 지정해야함
            if (Input.GetKey(KeyCode.W))
            {
                Dir = MoveDir.Up;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Dir = MoveDir.Left;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Dir = MoveDir.Down;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Dir = MoveDir.Right;
            }
            else
            {
                Dir = MoveDir.None;
            }
        }

        protected override void MoveToNextPos()
        {
            if (Dir == MoveDir.None)
            {
                State = State.Idle;
                CheckUpdatedFlag();
                return;
            }

            Vector3Int destPos = CellPos;

            switch (Dir)
            {
                case MoveDir.Up:
                    destPos += new Vector3Int(0, 0, 1);
                    break;
                case MoveDir.Left:
                    destPos += Vector3Int.left;
                    break;
                case MoveDir.Right:
                    destPos += Vector3Int.right;
                    break;
                case MoveDir.Down:
                    destPos += new Vector3Int(0, 0, -1);
                    break;
            }

            if (Managers.Object.Find(destPos) == null)
            {
                CellPos = destPos;
            }

            CheckUpdatedFlag();

        }

        protected override void CheckUpdatedFlag()
        {
            if (_updated)
            {
                C_Move movePacket = new C_Move();
                movePacket.PosInfo = PosInfo;
                Managers.Network.Send(movePacket);
                _updated = false;
            }
        }

    }
}