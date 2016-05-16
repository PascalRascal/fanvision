using UnityEngine;
using System.Collections;
using GeoUtil;
using fanVision;
using System;


public class bullet_shoot : MonoBehaviour {
    public GameObject townHall;
    public GameObject cam;
    public GameObject bullet;

    GameObject town;
    GeoUTMConverter UTMFactory;
    //Actually up
    Vector3 up = new Vector3(0, 1);

    //Actually north
    Vector3 north = new Vector3(-1, 0);


    //Actually east
    Vector3 east = new Vector3(0, 0, 1);

    float townHallEasting = 462975.30f;
    float townHallNorthing = 4557416.43f;

    float homeEasting = 462704.71f;
    float homeNorthing = 4557309.34f;
    LongLatSpawner lls;

   

    // Use this for initialization
    void Start()
    {
        //Instantiate(y, cam.transform.position + up, cam.transform.rotation);

        float renderEasting = townHallEasting - homeEasting;
        float renderNorthing = townHallNorthing - homeNorthing;
        Input.location.Start();
        lls = new LongLatSpawner(Input.location.lastData.latitude, Input.location.lastData.longitude, cam, bullet);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector3 RenderVector = lls.spawnModel(41.167274f, -81.441114f);
                Instantiate(bullet, cam.transform.position + RenderVector, cam.transform.rotation);

            }
        }
    }


   


}

