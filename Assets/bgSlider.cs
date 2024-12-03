using UnityEngine;
using UnityEngine.UI;

public class bgSlider : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // Set the slider's initial value to the current music volume.
        if (BackgroundMusic.Instance != null)
        {
            volumeSlider.value = BackgroundMusic.Instance.GetComponent<AudioSource>().volume;
        }

        // Add a listener to handle value changes.
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        if (BackgroundMusic.Instance != null)
        {
            BackgroundMusic.Instance.SetVolume(value);
        }
    }
}
