using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerControl : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the Audio Mixer


    // Sets the overall volume of the audio group
    public void SetVolumeNormalized(float volume)
    {
        // Clamp input
        volume = Mathf.Clamp01(volume);

        // Map 0-1 logarithmically to the range -80 dB to 20 dB
        float minDb = -80f;
        float maxDb = 20f;

        // Logarithmic curve (scale the range exponentially)
        float dB = minDb + (Mathf.Log10(volume * 9 + 1) * (maxDb - minDb));
        // Set the calculated dB value to the Audio Mixer
        audioMixer.SetFloat("MasterVolume", dB);
    }

    // should be number between -80 and + 20
    public void SetVoumeDirectDB(float dB)
    {
        audioMixer.SetFloat("MasterVolume", dB); // Adjust the exposed parameter
    }
}
