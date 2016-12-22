//using System;
//using UnityEngine;
//using UnityEditor;
//
//
//[CustomEditor(typeof(DownloadManager))]
//public class DownloadManagerEditor : Editor {
//
//    private DownloadManager myScript;
//
//    private string[] formats = new string[] {"mp4", "MP4"};
//
//    private SerializedProperty urlProp;
//    private SerializedProperty urlsProp;
//    private SerializedProperty bufferSizeProp;
//    private SerializedProperty pathProp;
//    private SerializedProperty filenameProp;
//    private SerializedProperty progressProp;
//
//    private void OnEnable()
//    {
//        // myScript = serializedObject.targetObject as DownloadManager;
//        myScript = target as DownloadManager;
//
//        urlProp = serializedObject.FindProperty("url");
//        urlsProp = serializedObject.FindProperty("urls");
//        bufferSizeProp = serializedObject.FindProperty("bufferSize");        
//        pathProp = serializedObject.FindProperty("path");        
//        filenameProp = serializedObject.FindProperty("filename");
//        progressProp = serializedObject.FindProperty("progress");
//    }
//
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//
//        #region URL
//        urlProp.stringValue = EditorGUILayout.TextField("URL", urlProp.stringValue);
//        // EditorGUILayout.PropertyField(urlsProp, true);
//        #endregion
//
//        #region filename
//        filenameProp.stringValue = EditorGUILayout.TextField("File Name", filenameProp.stringValue);
//        CheckFileName(filenameProp.stringValue);
//
//        #endregion
//
//        #region Path
//        GUILayout.BeginHorizontal();
//        pathProp.stringValue = EditorGUILayout.TextField("Path", pathProp.stringValue);
//        if (GUILayout.Button("Select Folder", GUILayout.Width(110)))
//        {
//            string path = EditorUtility.OpenFolderPanel("Set The Save Path", "", "");
//            if (path != String.Empty)
//                pathProp.stringValue = path;
//        }
//        GUILayout.EndHorizontal();
//        #endregion
//
//        #region BufferSize
//        bufferSizeProp.intValue = EditorGUILayout.IntSlider(new GUIContent("Buffer Size (KB)", "Buffer size is size of download/save file each frame"), bufferSizeProp.intValue, 0, 1024);
//        // EditorGUILayout.HelpBox("Buffer size is size of download/save file each time", MessageType.Info);
//        bufferSizeProp.intValue = CalculateBufferSize(bufferSizeProp.intValue);        
//        #endregion
//
//        if (Application.isPlaying)
//        {
//            if (GUILayout.Button("Start Download"))
//            {
//                myScript.StartDownload();
//            }
//            EditorGUILayout.LabelField("Progress", progressProp.floatValue.ToString("P"));
//        }
//
//        serializedObject.ApplyModifiedProperties();
//    }
//
//    private void CheckFileName(string filename)
//    {
//        if (String.IsNullOrEmpty(filename))
//        {
//
//        }
//
//        else
//        {
//            bool hasFileFormat = false;
//            foreach (var format in formats)
//            {
//                if (filename.Contains(format))
//                {
//                    hasFileFormat = true;
//                }
//            }
//            if (hasFileFormat == false)
//                EditorGUILayout.HelpBox("Write filename with file name extension", MessageType.Warning);
//        }
//    }
//
//    private int CalculateBufferSize(int bufferSize)
//    {
//        if (bufferSize <= 128)
//            return 128;
//
//        if (bufferSize <= 256)
//            return 256;
//
//        if (bufferSize <= 512)
//            return 512;
//
//        return 1024;       
//    }
//}
