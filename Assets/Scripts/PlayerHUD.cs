using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Image healthBarFill;
    public TextMeshProUGUI ammoText;

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    public void UpdateAmmo(int currentAmmo)
    {
        ammoText.text = "Ammo: " + currentAmmo;
    }
}
