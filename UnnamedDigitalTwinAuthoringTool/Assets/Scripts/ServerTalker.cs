using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerTalker : MonoBehaviour
{
    public Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        //make a web request to get info from the server
        //this will be a text response.

        //StartCoroutine(GetWebData("http://localhost:8000/user"));
        StartCoroutine(GetWebData("https://aubreysimonson.gitlab.io/page/"));//<--this also works--can you print this on the headset?
    }

    IEnumerator GetWebData(string address){
      UnityWebRequest www = UnityWebRequest.Get(address);
      yield return www.SendWebRequest();

      if(www.result != UnityWebRequest.Result.Success){
        Debug.LogError("That wasn't supposed to happen: " + www.error);
      }
      else{
        Debug.Log(www.downloadHandler.text);
        debugText.text = www.downloadHandler.text;
      }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
