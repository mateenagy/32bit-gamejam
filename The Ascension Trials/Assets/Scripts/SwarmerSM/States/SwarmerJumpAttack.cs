using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SwarmerJumpAttack : SwarmerState
{
    public SwarmerJumpAttack(SwarmerSM stateMachine, SwarmerFactory factory) : base(stateMachine, factory)
    {
        IsRoot = true;
    }

    public override void Enter()
    {
        base.Enter();
        Ctx.SwarmerTransform.DOJump(Ctx.Player.transform.position, 2, 1, Ctx.JumpSpeed).OnComplete(() =>
        {
            Ctx.Agent.isStopped = true;
            Ctx.StartCoroutine(WaitAndMove());
        });
    }

    IEnumerator WaitAndMove()
    {
        float waitTime = Random.Range(.5f, 1f);
        yield return new WaitForSeconds(waitTime);
        Ctx.Agent.isStopped = false;
        SwitchState(Factory.States[SwarmerStates.Idle]);
    }

    public override void Update()
    {
        base.Update();
        CheckSwitchState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.GetComponent<PlayerSM>())
        {
            // other.gameObject.GetComponent<PlayerSM>().life -= Ctx.Damage;
            other.gameObject.GetComponent<PlayerSM>().TakeDamage(Ctx.Damage);
        }
    }

    public override void CheckSwitchState()
    {
        base.CheckSwitchState();
    }
}
