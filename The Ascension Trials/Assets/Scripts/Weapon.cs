using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    public UIDocument skillUI;
    private VisualElement root;
    private Animator animator;
    [Header("Weapon options")]
    public int damage = 10;
    public float range = 100f;
    public float fireRate = 15f;
    public float fireRateBase = 15f;
    public float fireRateWhenSkillActive = 15f;
    public float impactForce = 30f;
    public float hitRadius = 0.5f;
    public AudioSource shootSoundSource;
    public AudioClip shootSoundClip;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private float nextTimeToFire = 0f;
    public InputActionReference fire;
    public PlayerSM playerSM;
    private bool canShootJump = true;
    [Header("Shake options")]
    public float duration = 0.2f;
    public Vector3 strength = new(0, 0, 0);
    public int vibrato = 10;
    public float randomness = 90f;
    public float weaponDuration = 0.2f;
    public Vector3 weaponStrength = new(0, 0, 0);
    public int weaponVibrato = 10;
    public float weaponRandomness = 90f;

    [Header("Rate skill options")]
    public InputActionReference rateSkillInput;
    public float rateSkillCooldown = 1f;
    public float rateSkillTime = 2f;
    private float nextCoolDownTime = 0f;


    void Start()
    {
        root = skillUI.rootVisualElement;
        fireRateBase = fireRate;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (SkillManager.Instance && SkillManager.Instance.skills.BinarySearch(Skill.FireRate) >= 0)
        {
            if (rateSkillInput.action.triggered)
            {
                if (Time.time >= nextCoolDownTime)
                {
                    var fireRateSkillOverlayUI = root.Q<VisualElement>("fire-rate").Q<VisualElement>("overlay");
                    fireRateSkillOverlayUI.style.scale = new StyleScale(new Vector2(1, 1));
                    StartCoroutine(FireRateSkill());
                    nextCoolDownTime = Time.time + (rateSkillCooldown + rateSkillTime);
                }
            }
        }
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
                animator.SetTrigger("IsShooting");
                nextTimeToFire = Time.time + 1f / fireRateBase;
                Shoot();
            }
        }

        if (playerSM.IsGrounded && playerSM.Velocity.y < 0)
        {
            canShootJump = true;
        }
    }

    IEnumerator FireRateSkill()
    {
        // fireRateBase = fireRateWhenSkillActive;
        damage *= 2;
        float value = 1f;
        var fireRateSkillOverlayUI = root.Q<VisualElement>("fire-rate").Q<VisualElement>("overlay");
        yield return new WaitForSeconds(rateSkillTime);
        DOTween.To(() => value, x => value = x, 0f, rateSkillCooldown).OnUpdate(() =>
        {
            fireRateSkillOverlayUI.style.scale = new StyleScale(new Vector2(1, value));
        });
        // fireRateBase = fireRate;
        damage /= 2;
    }

    void Shoot()
    {
        // muzzleFlash.Play();
        shootSoundSource.clip = shootSoundClip;
        shootSoundSource.Play();
        playerSM.playerCamera.transform.DOShakePosition(duration, strength, vibrato, randomness);
        // transform.DOShakePosition(weaponDuration, weaponStrength, weaponVibrato, weaponRandomness);
        RaycastHit[] hits = Physics.SphereCastAll(fpsCam.transform.position, hitRadius, fpsCam.transform.forward, range);

        foreach (RaycastHit hit in hits)
        {
            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                ;
                target.TakeDamage(damage, hit.transform.position);
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
