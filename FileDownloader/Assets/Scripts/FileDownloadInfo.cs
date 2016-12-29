using System.IO;

public class FileDownloadInfo
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

    public FileDownloadInfo(string url, string fileName, string savePath, int bufferSize = 256 * 1024)
    {
        this.url = url;
        this.fileName = fileName;
        this.savePath = savePath;
        this.bufferSize = bufferSize;
    }

    public void Release()
    {
        if (fileStream != null)
        {
            fileStream.Flush();
            fileStream.Dispose();
            fileStream.Close();
            fileStream = null;
        }
    }
}
