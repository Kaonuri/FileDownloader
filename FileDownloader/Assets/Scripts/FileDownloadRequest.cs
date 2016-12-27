using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloadRequest
{
    public string url;
    public string fileName;
    public string savePath;

    public int bufferSize = 1024;
    public byte[] buffer;

    public long receivedBytes;
    public long totalBytes;

    public float idleTime;
    public int retryCnt;

    public FileStream fileStream;
    public UnityWebRequest unityWebRequest;

    public FileDownloadRequest(string url, string fileName, string savePath)
    {
        this.url = url;
        this.fileName = fileName;
        this.savePath = savePath;

        buffer = new byte[bufferSize];

        receivedBytes = 0;
        totalBytes = 0;

        idleTime = 0f;
        retryCnt = 0;

        fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        unityWebRequest = UnityWebRequest.Get(url);
        unityWebRequest.downloadHandler = new FileDownloadHandler(toFileDownload.savePath + "/" + toFileDownload.fileName, buffer);
    }

    public void Release()
    {
        if (fileStream != null)
        {
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
        }

        if (unityWebRequest != null)
        {            
            unityWebRequest.Abort();
            unityWebRequest.Dispose();
            unityWebRequest = null;
        }
    }
}
