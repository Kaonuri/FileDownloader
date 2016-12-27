using UnityEngine;
using UnityEngine.Networking;
using System.IO;


class FileDownloadHandler : DownloadHandlerScript
{
    private FileDownloadRequest fileDownloadRequest;

    FileStream fileStream;
    int offset = 0;
    int length = 0;

    public FileDownloadHandler(FileDownloadRequest fileDownloadRequest) : base(fileDownloadRequest.buffer)
    {
        this.fileDownloadRequest = fileDownloadRequest;
    }

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (data == null || data.Length < 1)
        {
            return false;
        }

        fileDownloadRequest.fileStream.Write(data, 0, dataLength);
        fileDownloadRequest.receivedBytes += dataLength;
        return true;
    }

    protected override void ReceiveContentLength(int contentLength)
    {
        fileDownloadRequest.totalBytes = contentLength;
    }

    protected override float GetProgress()
    {
        if (length == 0)
            return 0.0f;

        return (float)offset / length;
    }

    protected override void CompleteContent()
    {
        fileStream.Flush();
        fileStream.Close();
    }
}