using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEditor;
using System.Xml.Serialization;
using System.IO;

public class ServerTalker : MonoBehaviour
{
    public Text debugText;
    public bool recording;
    public string lastFrameData;
    public string recordedData;
    public MTConnectParser parser;

    //You should make a system of recording data, then making an interface from the data you've recorded
	  //But to do it, you actually have to change how you represent data under the hood a little
    // Start is called before the first frame update
    void Start()
    {
        //make a web request to get info from the server
        //this will be a text response.

        //StartCoroutine(GetWebData("http://localhost:8000/user"));
        //StartCoroutine(GetWebData("https://smstestbed.nist.gov/vds/"));//<--this works.
    }

    IEnumerator GetWebData(string address){
      UnityWebRequest www = UnityWebRequest.Get(address);
      yield return www.SendWebRequest();

      if(www.result != UnityWebRequest.Result.Success){
        Debug.LogError("That wasn't supposed to happen: " + www.error);
      }
      else{
        //Debug.Log(" we got: " + www.downloadHandler.text);
        lastFrameData = www.downloadHandler.text;
        debugText.text = lastFrameData;
        parser.SetAndReadWebData(lastFrameData);
      }
    }



    public void GetDataSnapshot(string url){//there's a technical term for this, and this is probably not the right one
      //get the data from the fucking internet
      StartCoroutine(GetWebData(url));
    }
}
