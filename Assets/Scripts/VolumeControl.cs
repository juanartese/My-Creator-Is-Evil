using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [Header("UI References")]
    public Slider volumeSlider;

    private void Start()
    {
        // 1. Check if we saved a volume previously. If not, default to 1 (100%)
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 1f);
        }
        
        // 2. Load the saved volume and apply it to the game
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
        AudioListener.volume = savedVolume;
        
        // 3. Make sure the slider matches the saved volume visually
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }
    }

    // This is the method the Slider will call when you drag it
    public void ChangeVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat("MasterVolume", newVolume); // Save it for next time!
    }
}