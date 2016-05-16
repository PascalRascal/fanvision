using UnityEngine;
using GeoUtil;
using System.Collections;
using System;

namespace fanVision
{
    public class LongLatSpawner
    {
        private float latitude;
        private float longitude;
        private GameObject playerCam;
        private GameObject render;
        float brng;
        float y, x;
        private float playerLong;
        private float playerLat;

        private float playerEasting;
        private float playerNorthing;
        private float objectEasting;
        private float objectNorthing;

        GeoUTMConverter UTMFactory;

        //Actually north
        Vector3 north = new Vector3(-1, 0, 0);


        //Actually east
        Vector3 east = new Vector3(0, 0, 1);

        private Vector3 objectVector;
        private Vector3 personVector;

        
        public LongLatSpawner(float playerLati, float playerLongi, GameObject cam, GameObject gpsModel)
        {
            playerLat = playerLati;
            playerLong = playerLongi;
            playerCam = cam;
            render = gpsModel;
            Debug.Log("HelloWorld");
            
            UTMFactory = new GeoUTMConverter();
            UTMFactory.Hemi = GeoUTMConverter.Hemisphere.Northern;
            UTMFactory.ToUTM(playerLat, playerLong);

            playerEasting = (float) UTMFactory.X;
            playerNorthing = (float) UTMFactory.Y;

            Debug.Log(playerEasting + ", " + playerNorthing);

            //Coords for city hall
            float lat2 = 41.167436f;
            float long2 = -81.441500f;


            //Coords for my house
            float lat1 = 41.16632f;
            float long1 = -81.4468f;

        }


        public Vector3 spawnModel(float objectLatitude, float objectLongitude)
        {
            UTMFactory.ToUTM(objectLatitude, objectLongitude);
            objectEasting = (float) UTMFactory.X;
            objectNorthing = (float) UTMFactory.Y;

            Debug.Log(objectEasting + ", " + objectNorthing);

            float renderEasting = objectEasting - playerEasting;
            float renderNorthing = objectNorthing - playerNorthing;

            Vector3 renderVectorEasting = east * renderEasting;
            Vector3 renderVectorNorthing = north * renderNorthing;
            Vector3 renderVector = renderVectorEasting + renderVectorNorthing;
            renderVector.Normalize();
            return renderVector;
        }


        public float geteBrng()
        {
            return brng;
        }



        static double DegreeBearing(
    double lat1, double lon1,
    double lat2, double lon2)
        {
            const double R = 6371; //earth’s radius (mean radius = 6,371km)
            var dLon = ToRad(lon2 - lon1);
            var dPhi = Math.Log(
                Math.Tan(ToRad(lat2) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat1) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }




    }
}
