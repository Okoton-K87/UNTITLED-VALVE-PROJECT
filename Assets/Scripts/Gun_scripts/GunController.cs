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

    [Header("UI References")]
    public TextMeshProUGUI gunMessage; // UI message for player
    public TextMeshProUGUI ammoText; // Ammo count display

    [Header("References")]
    public Transform firePoint; // Where the raycast starts
    public Camera playerCamera; // Reference to the player's camera

    private int currentAmmo; // Current ammo in the magazine
    private bool isUsingGun = false; // Is the gun active
    private bool isAiming = false; // Is the player in ADS mode
    private float nextFireTime = 0f; // Time until the next shot
    private float messageTimer = 0f; // Timer for hiding messages
    private bool isReloading = false; // Is the player reloading

    private void Start()
    {
        currentAmmo = maxAmmo;
        hipFireGun.SetActive(false);
        adsGun.SetActive(false);
        UpdateAmmoUI();
    }

    private void Update()
    {
        HandleMessageTimer();

        if (InventoryManager.Instance.InventoryContains("M1A1"))
        {
            if (!isUsingGun)
            {
                gunMessage.text = "Press 1 to use the Gun.";
                gunMessage.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && !isReloading)
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

        if (isUsingGun)
        {
            gunMessage.gameObject.SetActive(false); // Hide the message when the gun is active
        }
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
            else if (!isReloading)
            {
                gunMessage.text = "Out of ammo! Press R to reload.";
                gunMessage.gameObject.SetActive(true);
                messageTimer = 3f; // Hide after 3 seconds
            }
        }
    }

    private void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        Vector3 shootDirection = playerCamera.transform.forward;
        if (!isAiming)
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

            // Damage the enemy
            var health = hit.collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.TakeDamage(bulletDamage);
            }

            // Add a bullet tracer
            Debug.DrawLine(firePoint.position, hit.point, Color.red, 0.1f);
        }
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            if (InventoryManager.Instance.InventoryContains("M1A1_mag"))
            {
                isReloading = true;
                gunMessage.text = "Reloading...";
                gunMessage.gameObject.SetActive(true);

                // Update message timer for reload
                StartCoroutine(ReloadTimer());
            }
            else
            {
                gunMessage.text = "No magazines left!";
                gunMessage.gameObject.SetActive(true);
                messageTimer = 3f; // Hide after 3 seconds
            }
        }
    }

    private System.Collections.IEnumerator ReloadTimer()
    {
        float reloadTime = 2f;
        while (reloadTime > 0)
        {
            gunMessage.text = $"Reloading... {reloadTime:F1}s";
            yield return new WaitForSeconds(0.1f);
            reloadTime -= 0.1f;
        }

        InventoryManager.Instance.RemoveItemByName("M1A1_mag");
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        gunMessage.text = "Reloaded!";
        messageTimer = 3f; // Hide after 3 seconds
        isReloading = false;
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
    }

    private void HandleMessageTimer()
    {
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
            {
                gunMessage.gameObject.SetActive(false);
            }
        }
    }
}
