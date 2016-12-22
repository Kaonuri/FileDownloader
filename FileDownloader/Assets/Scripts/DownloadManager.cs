using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DownloadManager : MonoBehaviour
{
    // http://answers.unity3d.com/questions/147209/www-downloadmanager-c.html

    [SerializeField]
    private string url;

    [SerializeField]
    private string filename;

    [SerializeField]
    private string path;

    [SerializeField]
    private int bufferSize = 256;

    [SerializeField]
    private float progress;

    private static bool isDownloading = false;

    public delegate void DownloadCallback();
    private static DownloadManager instance = null;

    private static Queue<Downloadable> queue;

    // public bool failedFileEnqueue = false;

    private class Downloadable
    {
        public string url { get; set; }
        public string filename { get; set; }
        public string path { get; set; }
        public DownloadCallback callback { get; set; }

        public Downloadable(string url, string filename, string path , DownloadCallback callback)
        {
            this.url = url;
            this.filename = filename;
            this.path = path;
            this.callback = callback;
        }
    }

    void Awake()
    {
        if (DownloadManager.instance == null)
        {
            DownloadManager.instance = FindObjectOfType(typeof(DownloadManager)) as DownloadManager;
        }
        queue = new Queue<Downloadable>();
        AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV1.mp4", "N_S5_LV1.mp4", Application.dataPath);
        AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV2.mp4", "N_S5_LV2.mp4", Application.dataPath);
        AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV3.mp4", "N_S5_LV3.mp4", Application.dataPath);
        StartCoroutine(Download());
    }

    void OnApplicationQuit()
    {
        DownloadManager.instance = null;
        StopCoroutine(Download());
    }

    private IEnumerator Download()
    {
        isDownloading = true;

        Debug.Log("Download " + queue.Count + " Contents");
        while (queue.Count != 0)
        {
            Downloadable toDownload = queue.Dequeue();

            using (UnityWebRequest request = UnityWebRequest.Get(toDownload.url))
            {
                byte[] buffer = new byte[bufferSize * 1024];
                request.downloadHandler = new FileDownloadHandler(toDownload.path + "/" + toDownload.filename, buffer);
                request.Send();

                Debug.Log("[" + toDownload.filename + "] Download Start...");

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
                    Debug.Log("[" + toDownload.filename + "] Download Complete!");
                }
                buffer = null;
            }
            yield return null;
        }
        Debug.Log("All Contents Download Complete!");
        isDownloading = false;
    }

    public static void AddDownload(string url, string filename, string path, DownloadCallback callback = null)
    {
        queue.Enqueue(new Downloadable(url, filename, path, callback));
    }
}
