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
        }

        public GameObject getPOI()
        {
            return poi;
        }

    }
}
