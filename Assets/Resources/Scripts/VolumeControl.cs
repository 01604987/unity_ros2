using UnityEngine;

public class VolumeControl : MonoBehaviour
{

    public float vol;
    private float preVol;
    public AudioMixerControl mixerControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vol = 1;
        preVol = vol;
        mixerControl.SetVolumeNormalized(vol);
    }

    // Update is called once per frame
    void Update()
    {
        if (vol != preVol)
        {
            mixerControl.SetVolumeNormalized(vol);
            preVol = vol;
        }
 
    }
}
