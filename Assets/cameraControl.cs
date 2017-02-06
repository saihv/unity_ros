using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class cameraControl : MonoBehaviour {

	public static cameraControl instance;

	public string baseFilename = "/home/sai/";
	int i = 0;

	private Vector3 pos;
	private float X, Y, Z;
	private string[] values;

    Thread readThread;
    UdpClient client;

    private int port = 5005;
	private string lastReceivedPacket = "";

	public void moveCamera() {
		pos.x = X;
		pos.y = Y;
		pos.z = Z;
		GetComponent<Camera>().transform.position = pos;
	}

	private void ReceiveData() {

        client = new UdpClient(port);
        while (true)
        {
            try
            {
                // receive bytes
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                // encode UTF8-coded bytes to text format
                string posData = Encoding.UTF8.GetString(data);

                // show received message
                print("Current position >> " + posData);

				values = posData.Split(',');

				X = float.Parse(values[0]);
				Y = float.Parse(values[1]);
				Z = float.Parse(values[2]);

				for(int i = 0; i < values.Length; i++)
					values[i] = values[i].Trim();

				print("Current position >> " + values[0] + "," + values[1] + "," + values[2]);

                // store new massage as latest message
                lastReceivedPacket = posData;

                // update received messages
                // allReceivedPackets = allReceivedPackets + posData;

            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

	private string SetImageLocation() {
		string r =  baseFilename + i +".png";
		return r;
	}

	// Use this for initialization
	void Start () {
		readThread = new Thread(new ThreadStart(ReceiveData));
    	readThread.IsBackground = true;
    	readThread.Start();
	}

	// Update is called once per frame
	void Update () {
		int sqr = 512;
		i++;
		GetComponent<Camera>().aspect = 1.0f;
		RenderTexture tempRT = new RenderTexture(2*sqr,sqr, 24 );

		GetComponent<Camera>().targetTexture = tempRT;

		GetComponent<Camera>().Render();
		RenderTexture.active = tempRT;
		Texture2D virtualPhoto = new Texture2D(sqr,sqr, TextureFormat.RGB24, false);

		virtualPhoto.ReadPixels( new Rect(0, 0, sqr,sqr), 0, 0);

		RenderTexture.active = null; //can help avoid errors
		GetComponent<Camera>().targetTexture = null;
		// consider ... Destroy(tempRT);

		byte[] bytes;
		bytes = virtualPhoto.EncodeToPNG();

		System.IO.File.WriteAllBytes(SetImageLocation(), bytes );

		Debug.Log("Saved PNG");
		moveCamera();
	}
}
