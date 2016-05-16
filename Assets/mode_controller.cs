using UnityEngine;
using System.Collections;

public class mode_controller : MonoBehaviour {


    float TRIGGER_DURATION = 3;
    float touchTimer;
    bool triggered;
    Quaternion previousOrientation;
    gyro_CameraRotation gyroCam;
    Canvas HUD;

    // Use this for initialization
    void Start () {
        gyroCam = GetComponent<gyro_CameraRotation>();
        HUD = GetComponentInChildren<Canvas>();


    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchTimer = TRIGGER_DURATION;
                triggered = true;

            }

            if (triggered)
            {
                touchTimer -= Time.deltaTime;
                if (touchTimer < 0)
                {
                    Handheld.Vibrate();
                    if (gyroCam.enabled)
                    {
                        gyroCam.enabled = false;
                        //HUD.enabled = false;
                    }else
                    {
                        gyroCam.enabled = true;
                        HUD.enabled = true;
                    }
                    triggered = false;
                }
            }

            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {

            }
        }
    }
}
