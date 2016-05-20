using UnityEngine;
using System.Collections;

namespace fanVision
{
    public class POIFactory
    {
        GameObject poi;
        TextMesh[] txt;
        string title, desc;
        public POIFactory(GameObject obj, string POItitle, string POIdesc)
        {
            poi = obj;
            title = POItitle;
            desc = POIdesc;
            poi.GetComponentsInChildren<TextMesh>()[0].text = POItitle;
            poi.GetComponentsInChildren<MeshRenderer>()[2].material.color = Color.blue;
        }
        public POIFactory(GameObject obj, string POItitle, string POIdesc, string color)
        {
            poi = obj;
            title = POItitle;
            desc = POIdesc;
            poi.GetComponentsInChildren<TextMesh>()[0].text = POItitle;

            if (color.Equals("red"))
            {
                poi.GetComponentsInChildren<MeshRenderer>()[2].material.color = Color.red;

            }else if (color.Equals("green"))
            {
                poi.GetComponentsInChildren<MeshRenderer>()[2].material.color = Color.green;

            }else if (color.Equals("blue"))
            {
                poi.GetComponentsInChildren<MeshRenderer>()[2].material.color = Color.blue;
            }
        }

        public GameObject getPOI()
        {
            return poi;
        }

    }
}
