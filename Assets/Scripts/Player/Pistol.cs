using UnityEngine;
using System.Collections;

public class Pistol : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform muzzle;
    public Light muzzleFlash;

    [Header("Shooting")]
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.2f;

    [Header("Ammo")]
    public int magazineSize = 12;
    public int currentAmmo = 12;
    public float reloadTime = 1.2f;

    [Header("Recoil")]
    public float recoilAmount = 3f;
    public float recoilRecovery = 10f;

    private float nextFireTime;
    private bool isReloading;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }

        RecoverRecoil();
    }

    void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        nextFireTime = Time.time + fireRate;

        Shoot();
    }

    void Shoot()
    {
        currentAmmo--;

        FireRecoil();

        StartCoroutine(MuzzleFlash());

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            range))
        {
            Debug.Log(
                "Pistol hit: " +
                hit.collider.name
            );

            EnemyHealth enemy =
                hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                Vector3 hitDirection =
                    ray.direction;

                enemy.TakeDamage(
                    damage,
                    hitDirection
                );
            }

            Debug.DrawLine(
                ray.origin,
                hit.point,
                Color.red,
                0.2f
            );
        }
        else
        {
            Debug.Log("Pistol missed");
        }
    }

    void FireRecoil()
    {
        transform.localPosition =
            originalPosition +
            Vector3.back * 0.08f;

        transform.localRotation =
            originalRotation *
            Quaternion.Euler(
                -recoilAmount,
                0f,
                0f
            );
    }

    void RecoverRecoil()
    {
        transform.localPosition =
            Vector3.Lerp(
                transform.localPosition,
                originalPosition,
                recoilRecovery * Time.deltaTime
            );

        transform.localRotation =
            Quaternion.Slerp(
                transform.localRotation,
                originalRotation,
                recoilRecovery * Time.deltaTime
            );
    }

    IEnumerator MuzzleFlash()
    {
        if (muzzleFlash == null)
            yield break;

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.05f);

        muzzleFlash.enabled = false;
    }

    void StartReload()
    {
        if (currentAmmo == magazineSize)
            return;

        if (isReloading)
            return;

        isReloading = true;

        Debug.Log("Reloading...");

        Invoke(
            nameof(FinishReload),
            reloadTime
        );
    }

    void FinishReload()
    {
        currentAmmo = magazineSize;

        isReloading = false;

        Debug.Log("Reload complete!");
    }
}