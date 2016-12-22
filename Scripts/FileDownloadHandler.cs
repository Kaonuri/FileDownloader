using UnityEngine;
using UnityEngine.Networking;
using System.IO;


class FileDownloadHandler : DownloadHandlerScript
{    
    FileStream fs;
    int offset = 0;
    int length = 0;

    public FileDownloadHandler(string path, byte[] buffer): base(buffer)
    {
        fs = new FileStream(path, FileMode.Create, FileAccess.Write);
    }

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        fs.Write(data, 0, dataLength);
        offset += dataLength;
        return true;
    }

    protected override void CompleteContent()
    {
        fs.Flush();
        fs.Close();
    }

    protected override void ReceiveContentLength(int contentLength)
    {
        length = contentLength;
        Debug.Log("ContentLengt : " + length);
    }

    protected override float GetProgress()
    {
        if (length == 0)
            return 0.0f;

        return (float)offset / length;
    }
}