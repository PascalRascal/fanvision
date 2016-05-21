using UnityEngine;
using System.Collections;
using WebSocketSharp;

namespace fanVision
{
    public class Network_Socket
    {
        public string msg { get; set; }
        WebSocket ws;
        public Network_Socket()
        {
            msg = "NO DATA =(";
            ws = new WebSocket("ws://stoh.io:9009");

            ws.OnMessage += (sender, e) =>
                msg = e.Data;
            ws.ConnectAsync();

        }
    }
}
