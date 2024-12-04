using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject hipFireGun; // Hip-fire gun model
    public GameObject adsGun; // ADS gun model
    public int maxAmmo = 40; // Max ammo per magazine
    public float fireRate = 0.1f; // Time between shots
    public int bulletDamage = 20; // Damage per bullet
    public float spreadRange = 0.05f; // Spread range for hip-fire
    public TextMeshProUGUI gunMessage; // UI message for player

    [Header("References")]
    public Transform firePoint; // Where the raycast starts
    public Camera playerCamera; // Reference to the player's camera

    private int currentAmmo; // Current ammo in the magazine
    private bool isUsingGun = false; // Is the gun active
    private bool isAiming = false; // Is the player in ADS mode
    private float nextFireTime = 0f; // Time until the next shot

    private void Start()
    {
        // Initialize ammo and hide the gun models
        currentAmmo = maxAmmo;
        hipFireGun.SetActive(false);
        adsGun.SetActive(false);
    }

    private void Update()
    {
        if (InventoryManager.Instance.InventoryContains("M1A1"))
        {
            gunMessage.text = "Press 1 to use the Gun.";
            gunMessage.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToggleGun();
            }
        }
        else
        {
            gunMessage.gameObject.SetActive(false);
        }

        if (isUsingGun)
        {
            HandleADS();
            HandleShooting();
            HandleReload();
        }
    }

    private void ToggleGun()
    {
        isUsingGun = !isUsingGun;
        hipFireGun.SetActive(isUsingGun && !isAiming);
        adsGun.SetActive(isUsingGun && isAiming);
        gunMessage.text = isUsingGun ? "Press 1 to disable the Gun." : "Press 1 to use the Gun.";
    }

    private void HandleADS()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            isAiming = true;
            hipFireGun.SetActive(false);
            adsGun.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            hipFireGun.SetActive(true);
            adsGun.SetActive(false);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                gunMessage.text = "Out of ammo! Press R to reload.";
            }
        }
    }

    private void Shoot()
    {
        currentAmmo--;

        Vector3 shootDirection = playerCamera.transform.forward;
        if (!isAiming) // Apply spread for hip-fire
        {
            shootDirection += new Vector3(
                Random.Range(-spreadRange, spreadRange),
                Random.Range(-spreadRange, spreadRange),
                Random.Range(-spreadRange, spreadRange)
            );
        }

        if (Physics.Raycast(firePoint.position, shootDirection, out RaycastHit hit, 100f))
        {
            Debug.Log($"Hit {hit.collider.name}");
            // Apply damage if the hit object has a health component
            var health = hit.collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.TakeDamage(bulletDamage);
            }
        }
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (InventoryManager.Instance.InventoryContains("M1A1_mag"))
            {
                InventoryManager.Instance.RemoveItemByName("M1A1_mag");
                currentAmmo = maxAmmo;
                gunMessage.text = "Reloaded!";
                Debug.Log("Reloaded!");
            }
            else
            {
                gunMessage.text = "No magazines left!";
                Debug.Log("No magazines left!");
            }
        }
    }
}
