using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;


public class ThrustControl : MonoBehaviour
{

    private float position = 0;
    private float normalized_pos = 0;
    public bool reverse = false;


    [Range(0.1f, 500.0f)]
    [SerializeField]
    [Tooltip("Size of the thruster lever in mm")]
    private float thrusterSize;

    [SerializeField]
    private Transform shaftTransform;
    private Vector3 localPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Float32Msg>("/esp/thruster_position", SetPositionRos);

        localPos = shaftTransform.localPosition;
    }



    // position received in mm
    // need to translate to m for unity
    public void SetPositionRos(Float32Msg msg)
    {
        if (reverse)
        {
            msg.data = 1 - msg.data;
        }

        normalized_pos = msg.data / 1000f;

        position = normalized_pos * thrusterSize;
        localPos.z = position;
        shaftTransform.localPosition = localPos;
    }

    // position received in mm
    public void SetPositionUnity(float pos)
    {

        if (reverse)
        {
            pos = 1 - pos;
        }
        normalized_pos = pos / 1000f;

        position = normalized_pos * thrusterSize;
        localPos.z = position;
        shaftTransform.localPosition = localPos;
    }
}
