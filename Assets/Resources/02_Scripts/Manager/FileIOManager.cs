using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Exception = System.Exception;

public class FileIOManager : MonoBehaviour
{
    public static FileIOManager Instance;

    public int Clear;
    private string path = "SoulStealer";
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);

        try
        {
            if (File.Exists(path))
            {
                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    byte[] buffer = new byte[4];
                    fs.Read(buffer, 0, 4);

                    Clear = BitConverter.ToInt32(buffer, 0);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
            Clear = 0;
        }
    }

    public void Save()
    {
        Clear++;

        try
        {
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                byte[] buffer = new byte[4];
                Buffer.BlockCopy(BitConverter.GetBytes(Clear), 0, buffer, 0, 4);
                fs.Write(buffer, 0, 4);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}
