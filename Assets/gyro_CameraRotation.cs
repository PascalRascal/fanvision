using UnityEngine;
using System.Collections;

public class gyro_CameraRotation : MonoBehaviour {


    static bool gyroBool;
    private Gyroscope gyro;
    private Quaternion quatMult;
    private Quaternion quatMap;
    private Quaternion previousRotation;
    private Quaternion newquatMap;
    private GameObject camGrandparent;
    private float heading = 0;

    Transform currentParent;
    GameObject camParent;

    private Vector2 screenSize;
    private Vector2 mouseStartPoint;
    private float headingAtTouchStart = 0;

    //Called when the script instance is being loaded! xd
    void Awake()
    {

        currentParent = transform.parent;
        camParent = new GameObject("camParent");
        camParent.transform.position = transform.position;
        transform.parent = camParent.transform;

        camGrandparent = new GameObject("camGrandParent");
        camGrandparent.transform.position = transform.position;
        camParent.transform.parent = camGrandparent.transform;
        camGrandparent.transform.parent = currentParent;

        gyroBool = SystemInfo.supportsGyroscope;

    }
    // Use this for initialization
    void Start()
    {

        if (gyroBool)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            camParent.transform.eulerAngles = new Vector3(90, 90, 0);


            if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            {

                quatMult = new Quaternion(0, 0, 1, 0); //**

            }
            else if (Screen.orientation == ScreenOrientation.LandscapeRight)
            {

                quatMult = new Quaternion(0, 0, 1, 0); //**

            }
            else if (Screen.orientation == ScreenOrientation.Portrait)
            {

                quatMult = new Quaternion(0, 0, 1, 0); //**

            }
            else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {

                quatMult = new Quaternion(0, 0, 1, 0); // Unable to build package on upsidedown

            }
            quatMap = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
            previousRotation = quatMap;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gyroBool)
        {
            newquatMap = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
        }

        //Ideally cuts down on the shape stuttering around
        if (Mathf.Abs(Quaternion.Angle(newquatMap, previousRotation)) > 0.1)
        {
            transform.localRotation = newquatMap * quatMult;
            previousRotation = newquatMap;
        }

    }

    void Disable()
    {
        camParent.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}


