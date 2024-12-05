using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;


public class ThrusterCallibrationNode : MonoBehaviour
{
    ROSConnection ros;
    public string topicNameCalibration = "/unity/calibrate/thruster";
    private BoolMsg calibrateThruster;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<BoolMsg>("/esp/switch0", CalibrateThruster);
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<BoolMsg>(topicNameCalibration);
        
    }

    public void CalibrateThruster(BoolMsg msg)
    { 
        if (msg.data == true)
        {
            calibrateThruster = new BoolMsg(true);
            Debug.Log("Initiate Thruster Calibration");
            ros.Publish(topicNameCalibration, calibrateThruster);
        } else if (msg.data == false)
        {
            calibrateThruster = new BoolMsg(false);
            Debug.Log("Thruster Calibration complete");
            ros.Publish(topicNameCalibration, calibrateThruster);
        }
    }
}
