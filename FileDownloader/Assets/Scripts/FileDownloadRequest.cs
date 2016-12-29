using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloadRequest
{
    private FileDownloadInfo fileDownloadInfo;
    public UnityWebRequest unityWebRequest { get; private set; }

    public FileDownloadRequest(FileDownloadInfo fileDownloadInfo)
    {
        this.fileDownloadInfo = fileDownloadInfo;
        this.fileDownloadInfo.buffer = new byte[this.fileDownloadInfo.bufferSize];

        this.fileDownloadInfo.receivedBytes = 0;
        this.fileDownloadInfo.totalBytes = 0;

        this.fileDownloadInfo.idleTime = 0f;
        this.fileDownloadInfo.retryCnt = 0;

        this.fileDownloadInfo.fileStream = new FileStream(this.fileDownloadInfo.savePath + "/" + this.fileDownloadInfo.fileName, FileMode.Create, FileAccess.Write);

        unityWebRequest = UnityWebRequest.Get(this.fileDownloadInfo.url);
        unityWebRequest.downloadHandler = new FileDownloadHandler(this.fileDownloadInfo);
    }

    public void Release()
    {
        fileDownloadInfo.Release();

        if (unityWebRequest != null)
        {            
            unityWebRequest.Abort();
            unityWebRequest.Dispose();            
            unityWebRequest = null;
        }
    }
}
