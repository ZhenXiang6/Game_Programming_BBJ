using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // Reference to the slider
    [SerializeField] private AudioSource[] audioSources; // List of AudioSources to control

    private const string VolumePrefKey = "VolumeLevel";

    private void Start()
    {
        // Load and apply saved volume immediately (even if slider is inactive)
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.1f); // Default to 1 (full volume)
        SetVolume(savedVolume);

        // If the slider is active, synchronize it immediately
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    private void SetVolume(float value)
    {
        // Update all AudioSources' volume
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = value;
        }

        // Save the volume setting
        PlayerPrefs.SetFloat(VolumePrefKey, value);
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }
}
