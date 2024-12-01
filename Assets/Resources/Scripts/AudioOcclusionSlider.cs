using System.Security.Cryptography;
using UnityEngine;

public class AudioOcclusionSlider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Tooltip("Distance between an object and the audio source in cm based on the distance sensor HC SR04. Occlusion distance with min distance fully covering the output")]
    [Range(0f, 50f)]
    public float distance = 50f;


    public float minDistance = 3f;
    public float maxDistance = 20f;
    [Tooltip("Enables or disables the lowpass filter")]
    public bool enableLowPass = true;
    [Tooltip("The minimum cutoff frequency of the lowpass filter. The smaller the min, the stronger the muffled effect becomes with decreasing distance.")]
    public float minLowPassCutOffFreq = 1000f;
    [Tooltip("The initial value of the cutoff frequency defined by the lowpass filter")]
    private float maxLowPassCutOffFreq = 10000f;

    [Tooltip("The factor of the exponential curve for lowpass filter that controls the rate of muffled sound with decreasing distance\nSince high frequencies are attenuated faster than low freq, a lower value should be chosen than expHighPassFactor.")]
    [Range(1f, 6f)]
    public float expLowPassFactor = 3f;

    [Tooltip("Enables or disables the highpass filter")]
    public bool enableHighPass = true;
    [Tooltip("The initial value of the cutoff frequency defined by the highpass filter")]
    private float minHighPassCutOffFreq = 10f;
    [Tooltip("The maximum cutoff frequency of the highpass filter. The larger the max, the stronger the muffled effect becomes with decreasing distance.")]
    public float maxHighPassCuttOffFreq = 500f;


    [Tooltip("The factor of the exponential curve for highpass filter that controls the rate of muffled sound with decreasing distance\nSince low frequencies are attenuated slower than high freq, a higher value should be chosen than expLowPassFactor.")]
    [Range(1f, 6f)]
    public float expHighPassFactor = 4.5f;

    [Tooltip("Decreasing the distance, increases the gain, as sound becomes more cetralized")]
    [Range(0f, 10f)]
    public float addedGainDb = 5f;
    [Tooltip("The initial value of the gainDb defined by the MetaXRAudioSource")]
    private float initialGainDb;
    [Tooltip("Decreasing the distance, decreases the volume, as sound becomes less audible")]
    [Range(0.1f, 1f)]
    public float minVolume = 0.6f;
    [Tooltip("Initial volume from the AudioSource")]
    [Range(0.1f, 1f)]
    public float maxVolume = 1.0f;

    public bool enableReverb;
    [Range(-10000f, 0f)]
    public float minDryLevel = -250f;
    private float maxDryLevel = 0f;
    [Range(-10000f, 0f)]
    public float maxRoomReverb = -2000f;
    private float minRoomReverb = -10000f;

    [Range(-10000f, 1000f)]
    public float maxReflectionLevel = 250f;
    private float minReflectionLevel = -10000f;


    private float lowPassExpCurve = 0f;
    private float highPassExpCurve = 0f;

    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;
    private AudioHighPassFilter highPassFilter;
    private AudioReverbFilter reverbFilter;
    private MetaXRAudioSource MetaSpatializer;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        highPassFilter = GetComponent<AudioHighPassFilter>();
        reverbFilter = GetComponent<AudioReverbFilter>();
        MetaSpatializer = GetComponent<MetaXRAudioSource>();
        maxLowPassCutOffFreq = lowPassFilter.cutoffFrequency;
        maxVolume = audioSource.volume;
        initialGainDb = MetaSpatializer.GainBoostDb;
        maxDryLevel = reverbFilter.dryLevel;
        minRoomReverb = reverbFilter.room;
        minReflectionLevel = reverbFilter.reflectionsLevel;
    }

    // Update is called once per frame
    void Update()
    {
        float normalizedDistance = Mathf.Clamp((distance - minDistance) / (maxDistance - minDistance), 0f, 1f); // Normalize distance to a 0-1 range

        if (normalizedDistance < 1f)
        {
            lowPassExpCurve = Mathf.Exp(-normalizedDistance * expLowPassFactor); // Exponential decay factor
            highPassExpCurve = Mathf.Exp(-normalizedDistance * expHighPassFactor);
        }
        else if (highPassExpCurve != 0 && lowPassExpCurve != 0) 
        {
            highPassExpCurve = 0;
            lowPassExpCurve = 0;
        }

        if (enableLowPass)
        {
            lowPassFilter.cutoffFrequency = Mathf.Lerp(maxLowPassCutOffFreq, minLowPassCutOffFreq, lowPassExpCurve);
            
        }

        if (enableHighPass)
        {
            highPassFilter.cutoffFrequency = Mathf.Lerp(minHighPassCutOffFreq, maxHighPassCuttOffFreq, highPassExpCurve);
        }

        if (enableReverb)
        {
            reverbFilter.dryLevel = Mathf.Lerp(maxDryLevel, minDryLevel, highPassExpCurve);
            reverbFilter.room = Mathf.Lerp(minRoomReverb, maxRoomReverb, highPassExpCurve);
            reverbFilter.reflectionsLevel = Mathf.Lerp(minReflectionLevel, maxReflectionLevel, highPassExpCurve);
        }

        audioSource.volume = Mathf.Lerp(maxVolume, minVolume, highPassExpCurve);
        MetaSpatializer.GainBoostDb = Mathf.Lerp(initialGainDb, initialGainDb + addedGainDb, highPassExpCurve);


    }
}
