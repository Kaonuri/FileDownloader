using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngineInternal;

public class FileDownloadManager : MonoBehaviour
{
    // private string baseUrl = "https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/";
    // private string filename = "N_S5_LV2.mp4";

    [SerializeField]
    private string url;

    [SerializeField]
    private string[] urls;

    [SerializeField]
    private int bufferSize;

    [SerializeField]
    private string path;

    [SerializeField]
    private string filename;

    [SerializeField]
    private float progress;
    
    void Awake()
    {
        progress = 0.0f;
    }

    public void StartDownload()
    {
        StartCoroutine(Download(url + filename));
    }

    IEnumerator Download(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            byte[] buffer = new byte[bufferSize];
            request.downloadHandler = new FileDownloadHandler(Application.dataPath + "/" + filename, buffer);
            request.Send();

            Debug.Log("[" + filename + "] Download Start");
            while (!request.isDone)
            {
                progress = request.downloadProgress;
                yield return null;
            }

            if (request.isError)
            {
                Debug.LogError(request.error);
            }

            else
            {
                Debug.Log("[" + filename + "] Download Complete");
            }
            buffer = null;
        }
    }
}
