using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using Oculus.Interaction;
using System.Collections;
using Unity.XR.CoreUtils;


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

    [SerializeField]
    public Transform rightController_transform;

    public Transform shaft;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Float32Msg>("/esp/thruster_position", SetPositionRos);

        localPos = shaftTransform.localPosition;


        // On start of the scene, only track controllers. Determine the location of the controllers.
        // Spawn a handle on location of the controller.
        // Disable simultaneous hand and disable controller tracking.
        // 
        // On startup move spawn prefab on location of the controller
        // 
        StartCoroutine(InitializeWhenControllerActive());
    }

    IEnumerator InitializeWhenControllerActive()
    {
        // Wait until any controller is active
        while ((OVRInput.GetActiveController() & OVRInput.Controller.RTouch) == 0)
        {
            yield return null;
        }

        if(rightController_transform != null)
        {
            shaft.position = rightController_transform.position;
        }


        // Optional: disable simultaneous hand tracking
        //OVRPlugin.SetSimultaneousHandsAndControllersEnabled(false);
        //Debug.Log("Disabled Multimodal Control");
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
