using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class FileDownloadManager : MonoBehaviour
{
    // http://answers.unity3d.com/questions/147209/www-downloadmanager-c.html

    public enum DownloadState
    {
        None,
        Prepare,
        DownLoading,
        Failed,
        Complete
    }

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
    private static FileDownloadManager instance = null;

    private static Queue<FileDownloadRequest> queue;
    private FileDownloadRequest fileDownloadRequest = null;

    // public bool failedFileEnqueue = false;    

    void Awake()
    {
        if (FileDownloadManager.instance == null)
        {
            FileDownloadManager.instance = FindObjectOfType(typeof(FileDownloadManager)) as FileDownloadManager;
        }

        queue = new Queue<FileDownloadRequest>();        
    }

    void Start()
    {
        AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV1.mp4", "N_S5_LV1.mp4", Application.dataPath);
        AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV2.mp4", "N_S5_LV2.mp4", Application.dataPath);
        AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV3.mp4", "N_S5_LV3.mp4", Application.dataPath);
        StartCoroutine(Download());
    }

    void OnApplicationQuit()
    {        
        fileDownloadRequest.Release();
        fileDownloadRequest = null;
        StopCoroutine(Download());
        FileDownloadManager.instance = null;
    }

    private IEnumerator Download()
    {
        isDownloading = true;

        Debug.Log("Download " + queue.Count + " Contents");
        while (queue.Count != 0)
        {
            fileDownloadRequest = queue.Dequeue();

            fileDownloadRequest.unityWebRequest.Send();

            if (fileDownloadRequest.unityWebRequest.isError)
            {
                Debug.LogError(fileDownloadRequest.unityWebRequest.error);
                yield break;
            }

            else
            {
                Debug.Log("[" + fileDownloadRequest.fileName + "] Download Start...");
                while (!fileDownloadRequest.unityWebRequest.isDone)
                {                    
                    progress = fileDownloadRequest.unityWebRequest.downloadProgress;
                    yield return null;
                }
                fileDownloadRequest.Release();
                Debug.Log("[" + fileDownloadRequest.fileName + "] Download Complete!");
            }
            yield return null;            
        }
        Debug.Log("All Contents Download Complete!");
        isDownloading = false;
    }

    public static void AddDownload(string url, string filename, string path)
    {
        queue.Enqueue(new FileDownloadRequest(url, filename, path));
    }
}
