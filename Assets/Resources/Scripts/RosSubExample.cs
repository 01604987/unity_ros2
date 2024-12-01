using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
public class RosSubExample : MonoBehaviour
{
    private Vector3 currentAngle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Int32Msg>("/esp/optical_rotary_encoder", rotateY);
        ROSConnection.GetOrCreateInstance().Subscribe<Int16Msg>("/esp/rotary_encoder", rotateZ);
        currentAngle = this.transform.eulerAngles;
    }

    private void rotateY(Int32Msg msg)
    {
        if (this != null) 
        {
            currentAngle.y = msg.data / 10f;
            this.transform.eulerAngles = currentAngle;
        }
    }
    private void rotateZ(Int16Msg msg)
    {
        if(this != null)
        {
            currentAngle.z = msg.data * 3f;
            this.transform.eulerAngles = currentAngle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
