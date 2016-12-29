using System;
using UnityEngine;
using System.Collections.Generic;

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

    public static DownloadState downloadState = DownloadState.None;

    public Action<string, long, long> OnDownLoadProgress;

    public Action<string, string> OnDownLoadCompleted;

    public Action<string, Exception> OnDownLoadFailed;

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

    private static Queue<FileDownloadInfo> queue;
    private FileDownloadRequest fileDownloadRequest = null;
    private FileDownloadInfo fileDownloadInfo = null;

    // public bool failedFileEnqueue = false;    

    private void Awake()
    {
        if (FileDownloadManager.instance == null)
        {
            FileDownloadManager.instance = FindObjectOfType(typeof(FileDownloadManager)) as FileDownloadManager;
        }

        queue = new Queue<FileDownloadInfo>();        
    }

    private void Update()
    {
        switch (downloadState)
        {
            case DownloadState.None:
                {
                    Release();
                }
                break;

            case DownloadState.Prepare:
                {
                    if (queue.Count != 0)
                    {
                        fileDownloadInfo = queue.Dequeue();
                        fileDownloadRequest = new FileDownloadRequest(fileDownloadInfo);
                        fileDownloadRequest.unityWebRequest.Send();

                        if (fileDownloadRequest.unityWebRequest.isError)
                        {
                            Debug.LogError(fileDownloadRequest.unityWebRequest.error);
                            downloadState = DownloadState.Failed;
                            break;
                        }

                        Debug.Log("[" + fileDownloadInfo.fileName + "] Download Start...");
                        downloadState = DownloadState.DownLoading;
                    }

                    else
                    {
                        Debug.LogWarning("Download Queue is empty.");
                        downloadState = DownloadState.None;
                    }
                }
                break;

            case DownloadState.DownLoading:
                {
                    if (fileDownloadInfo.totalBytes > 0)
                    {
                        progress = fileDownloadRequest.unityWebRequest.downloadProgress;                  
                    }

                    if (fileDownloadRequest.unityWebRequest.isError)
                    {
                        Debug.LogError(fileDownloadRequest.unityWebRequest.error);
                        downloadState = DownloadState.Failed;
                    }
                }
                break;

            case DownloadState.Complete:
                {
                    Debug.Log("[" + fileDownloadInfo.fileName + "] Download Complete!");
                    Release();
                    downloadState = DownloadState.Prepare;
                }
                break;

            case DownloadState.Failed:
                {
                    Release();
                    downloadState = DownloadState.None;
                }
                break;
        }        
    }

    private void OnApplicationQuit()
    {
        downloadState = DownloadState.None;
        FileDownloadManager.instance = null;
    }

    private void Release()
    {
        if (fileDownloadRequest != null)
        {
            fileDownloadRequest.Release();
            fileDownloadRequest = null;
        }

        if (fileDownloadInfo != null)
        {
            fileDownloadInfo.Release();
            fileDownloadInfo = null;
        }
    }

    public static void AddDownload(string url, string filename, string path)
    {
        queue.Enqueue(new FileDownloadInfo(url, filename, path));
    }

    public static void StartDownload()
    {
        downloadState = DownloadState.Prepare;
    }

    public static void StopDownload()
    {
        downloadState = DownloadState.None;
    }

    public static void PauseDownload()
    {

    }

    public static void ResumeDownload()
    {
        
    }
}
