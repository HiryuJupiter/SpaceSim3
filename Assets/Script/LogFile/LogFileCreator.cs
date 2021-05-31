using UnityEngine;
using System.Collections;
using System.IO;

namespace HiryuTK.AsteroidsTopDownController
{
    public class LogFileCreator : MonoBehaviour
    {
        public void SaveLog(string text)
        {
            string path = Application.persistentDataPath + "/GameLogs.txt";
            StreamWriter writer = new StreamWriter(path, true);
            writer.Write(text);
            writer.Flush();
            Debug.Log("log created at location " + path);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                SaveLog("Test123");
        }
    }
}