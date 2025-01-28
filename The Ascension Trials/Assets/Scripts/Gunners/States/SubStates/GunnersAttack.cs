using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnersAttack : GunnersState
{
    public float timeToNextAttack = 1f;
    public GunnersAttack(GunnersSM stateMachine, GunnersFactory factory) : base(stateMachine, factory)
    {
        IsRoot = false;
    }

    public override void Enter()
    {
        base.Enter();
        Ctx.StartCoroutine(Attack());
    }

    public override void Update()
    {
        base.Update();
        Ctx.transform.LookAt(new Vector3(Ctx.Player.transform.position.x, Ctx.transform.position.y, Ctx.Player.transform.position.z));
        if (!Ctx.CheckPlayerInSight())
        {
            SwitchState(Factory.States[GunnersStates.Patrol]);
        }
        if (Time.time >= timeToNextAttack)
        {
            Ctx.StartCoroutine(Shoot());
            timeToNextAttack = Time.time + Ctx.TimeToNextShoot;
        }
        CheckSwitchState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(Ctx.ShootingTime);
        SwitchState(Factory.States[GunnersStates.Patrol]);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CheckSwitchState()
    {
        base.CheckSwitchState();
    }

    IEnumerator Shoot()
    {
        Vector3 direction = Ctx.Player.transform.position - Ctx.transform.position;

        float x = Random.Range(-Ctx.Spread, Ctx.Spread);
        float y = Random.Range(-Ctx.Spread, Ctx.Spread);
        float z = Random.Range(-Ctx.Spread, Ctx.Spread);
        Vector3 directionWithSpread = direction + new Vector3(x, y, z);

        GameObject bullet = Object.Instantiate(Ctx.Bullet, Ctx.transform.position, Quaternion.identity);
        bullet.transform.forward = directionWithSpread.normalized;
        bullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * Ctx.ShootForce, ForceMode.Impulse);
        yield return null;
    }
}
