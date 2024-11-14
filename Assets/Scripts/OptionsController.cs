using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Slider volumeSlider;
    public Dropdown qualityDropdown;

    private void Start()
    {
        
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);

        
        ApplyVolume();
        ApplyQuality();
    }

    public void ApplyVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value); 
    }

    public void ApplyQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
        PlayerPrefs.SetInt("Quality", qualityDropdown.value); 
    }
}
