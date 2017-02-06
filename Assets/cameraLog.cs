using UnityEngine;
using System;
using System.Collections;

public class cameraLog : MonoBehaviour {

	public string baseFilename = "/home/$USER/";
	int i = 0;

	// Use this for initialization

	 public void captureImage()
     	{
	     // capture the virtuCam and save it as a square PNG.

	     int sqr = 512;
	     i++;

	     GetComponent<Camera>().aspect = 1.0f;
	     // recall that the height is now the "actual" size from now on

	     RenderTexture tempRT = new RenderTexture(2*sqr,sqr, 24 );
	     // the 24 can be 0,16,24, formats like
	     // RenderTextureFormat.Default, ARGB32 etc.

	     GetComponent<Camera>().targetTexture = tempRT;
	     GetComponent<Camera>().Render();

	     RenderTexture.active = tempRT;
	     Texture2D virtualPhoto = new Texture2D(sqr,sqr, TextureFormat.RGB24, false);
	     // false, meaning no need for mipmaps
	     virtualPhoto.ReadPixels( new Rect(0, 0, sqr,sqr), 0, 0);

	     RenderTexture.active = null; //can help avoid errors
	     GetComponent<Camera>().targetTexture = null;
	     // consider ... Destroy(tempRT);

	     byte[] bytes;
	     bytes = virtualPhoto.EncodeToPNG();

	     System.IO.File.WriteAllBytes(imageLocation(), bytes );

	     Debug.Log("Saved PNG");
	     // virtualCam.SetActive(false); ... no great need for this.

	     // now use the image,
	     //UseFileImageAt( OurTempSquareImageLocation() );
     	}

	 private string imageLocation()
     	{
	     string r = baseFilename + i+".png";
	     return r;
     	}

	void Start () {

	}

	// Update is called once per frame
	void Update () {
		captureImage();
	}
}
