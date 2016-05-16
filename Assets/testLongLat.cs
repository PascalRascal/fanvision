using UnityEngine;
using UnityEngine.UI;
using fanVision;
using GeoUtil;
using System.Collections;

public class testLongLat : MonoBehaviour {

    public GameObject townHall;
    public GameObject cam;
    public GameObject bullet;
    GameObject poi;
    Text txt;

    GameObject town;
    GameObject lookAt;
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

    float longi;
    float lati;
    LongLatSpawner lls;

    POIFactory poif;



    // Use this for initialization
    void Start()
    {
        //Instantiate(y, cam.transform.position + up, cam.transform.rotation);
        txt = GetComponent<Text>();
        Input.location.Start();
        lls = new LongLatSpawner(41.16632f, -81.4468f, cam, bullet);
        Debug.Log(lookAt.transform.position);
        Debug.Log(cam.transform.position);
        Debug.Log(this.transform.position);
        //lookAt = (GameObject)Instantiate(townHall, cam.transform.position + Vector3.forward, cam.transform.rotation);





        // txt.text = RenderVector.x.ToString() + "\n" + RenderVector.y.ToString() + "\n" + RenderVector.z.ToString() + "\n" + lati + "\n" + longi + "\n" + lls.geteBrng().ToString();





    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                lati = Input.location.lastData.latitude;
                longi = Input.location.lastData.longitude;
                lls = new LongLatSpawner(lati, longi, cam, bullet);
                //Instantiate(townHall, cam.transform.position, cam.transform.rotation);


                createPOI(41.167436f, -81.441500f, "Stow City Hall");
                createPOI(41.155937f, -81.405905f, "Chipotle");




            }
        }
    }

    private void createPOI(float lat, float longi, string title)
    {
        Vector3 RenderVector = lls.spawnModel(lat, longi);
        poif = new POIFactory(townHall, title, title);
        GameObject POI = (GameObject)Instantiate(poif.getPOI(), cam.transform.position + RenderVector, cam.transform.rotation);
        POI.transform.LookAt(cam.transform);
    }
}
