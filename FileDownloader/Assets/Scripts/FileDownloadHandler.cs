using UnityEngine;
using UnityEngine.Networking;
using System.IO;


class FileDownloadHandler : DownloadHandlerScript
{
    private FileDownloadInfo fileDownloadInfo;

    public FileDownloadHandler(FileDownloadInfo fileDownloadInfo) : base(fileDownloadInfo.buffer)
    {
        this.fileDownloadInfo = fileDownloadInfo;
    }

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (data == null || data.Length < 1)
        {
            return false;
        }

        fileDownloadInfo.fileStream.Write(data, 0, dataLength);
        fileDownloadInfo.receivedBytes += dataLength;
        return true;
    }

    protected override void ReceiveContentLength(int contentLength)
    {
        fileDownloadInfo.totalBytes = contentLength;
    }

    protected override float GetProgress()
    {
        if (fileDownloadInfo.totalBytes == 0)
            return 0.0f;

        return (float)fileDownloadInfo.receivedBytes / fileDownloadInfo.totalBytes;
    }

    protected override void CompleteContent()
    {
        FileDownloadManager.downloadState = FileDownloadManager.DownloadState.Complete;
    }
}