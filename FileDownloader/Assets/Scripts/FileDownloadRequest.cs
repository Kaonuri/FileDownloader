using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloadRequest
{
    public string url;
    public string fileName;
    public string savePath;

    public int bufferSize;
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

        bufferSize = 256*1024;
        buffer = new byte[bufferSize];

        receivedBytes = 0;
        totalBytes = 0;

        idleTime = 0f;
        retryCnt = 0;
        
        Debug.Log(savePath);
        fileStream = new FileStream(savePath + "/" + fileName, FileMode.Create, FileAccess.Write);
        unityWebRequest = UnityWebRequest.Get(url);
        unityWebRequest.downloadHandler = new FileDownloadHandler(this);
    }

    public void Release()
    {
        if (fileStream != null)
        {
            fileStream.Flush();
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
