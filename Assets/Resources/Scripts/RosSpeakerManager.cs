using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
public class RosSpeakerManager: MonoBehaviour
{
    [SerializeField]
    private AudioOcclusionSlider distanceSlider;
    [SerializeField]
    private AudioMixerControl volumeSlider;


    [SerializeField]
    private bool enableHcsr04 = false;

    [SerializeField]
    private bool enableFilteredHcsr04 = true;

    [SerializeField]
    private bool enableLinearPot = false;

    [SerializeField]
    private bool enableFilteredLinearPot = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Float32Msg>("/esp/hcsr04", distanceToSpeaker);
        ROSConnection.GetOrCreateInstance().Subscribe<Float32Msg>("/esp/filtered/hcsr04", distanceToSpeakerFiltered);
        ROSConnection.GetOrCreateInstance().Subscribe<Float32Msg>("/esp/linear_pot", speakerVolumeControl);
        ROSConnection.GetOrCreateInstance().Subscribe<Float32Msg>("/esp/filtered/linear_pot", speakerVolumeControlFiltered);

    }

    private void distanceToSpeaker(Float32Msg msg)
    {
        if (enableHcsr04)
        {
            distanceSlider.distance = msg.data;
        }
    }

    private void distanceToSpeakerFiltered(Float32Msg msg)
    {
        if (enableFilteredHcsr04)
        {
            distanceSlider.distance = msg.data;
        }
    }
    private void speakerVolumeControl(Float32Msg msg)
    {
        if (enableLinearPot)
        {
            volumeSlider.SetVolumeNormalized(msg.data);
        }
    }
    private void speakerVolumeControlFiltered(Float32Msg msg)
    {
        if (enableFilteredLinearPot)
        {
            volumeSlider.SetVolumeNormalized(msg.data);
        }
    }

}
