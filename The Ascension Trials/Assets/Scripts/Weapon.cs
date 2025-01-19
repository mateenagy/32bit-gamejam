using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public float hitRadius = 0.5f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private float nextTimeToFire = 0f;
    public InputActionReference fire;
    public PlayerSM playerSM;
    public bool canShootJump = true;
    public bool groudned = true;
    [Header("Shake options")]
    public float duration = 0.2f;
    public Vector3 strength = new(0, 0, 0);
    public int vibrato = 10;
    public float randomness = 90f;
    public float weaponDuration = 0.2f;
    public Vector3 weaponStrength = new(0, 0, 0);
    public int weaponVibrato = 10;
    public float weaponRandomness = 90f;

    void Update()
    {
        groudned = playerSM.IsGrounded;
        if (fire.action.triggered)
        {
            if (playerSM.playerCamera.transform.eulerAngles.x > 45f &&
                playerSM.playerCamera.transform.eulerAngles.x < 270f &&
                canShootJump)
            {
                canShootJump = false;
                playerSM.Velocity = new(playerSM.Velocity.x, Mathf.Sqrt(3f * -2f * playerSM.Gravity), playerSM.Velocity.z);
            }
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }

        if (playerSM.IsGrounded && playerSM.Velocity.y < 0)
        {
            canShootJump = true;
        }
    }

    void Shoot()
    {
        // muzzleFlash.Play();
        // if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range))
        // {
        //     EnemySM target = hit.transform.GetComponent<EnemySM>();
        //     if (target != null)
        //     {
        //         target.TakeDamage(damage);
        //     }
        //     if (hit.rigidbody != null)
        //     {
        //         hit.rigidbody.AddForce(-hit.normal * impactForce);
        //     }
        //     GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        //     Destroy(impactGO, 2f);
        // }
        playerSM.playerCamera.transform.DOShakePosition(duration, strength, vibrato, randomness);
        transform.DOShakePosition(weaponDuration, weaponStrength, weaponVibrato, weaponRandomness);
        RaycastHit[] hits = Physics.SphereCastAll(fpsCam.transform.position, hitRadius, fpsCam.transform.forward, range);

        foreach (RaycastHit hit in hits)
        {
            EnemySM target = hit.transform.GetComponent<EnemySM>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fpsCam.transform.position + fpsCam.transform.forward * range, hitRadius);
    }
}
