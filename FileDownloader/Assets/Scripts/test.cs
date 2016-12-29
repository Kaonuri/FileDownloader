using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	void Start () {
        FileDownloadManager.AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV1.mp4", "N_S5_LV1.mp4", Application.dataPath);
        FileDownloadManager.AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV2.mp4", "N_S5_LV2.mp4", Application.dataPath);
        FileDownloadManager.AddDownload("https://d1afzz5wrpeqg9.cloudfront.net/resources/FOH_VER2/N_S3_LV3.mp4", "N_S5_LV3.mp4", Application.dataPath);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            FileDownloadManager.downloadState == FileDownloadManager.DownloadState.None)
        {
            FileDownloadManager.StartDownload();
        }
    }
}
