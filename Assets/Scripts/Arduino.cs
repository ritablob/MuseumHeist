using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Arduino : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM4", 9600);
    bool isStreaming = false;
    bool ledOn = false;
    void OpenCOnnection()
    {
        isStreaming = true;
        sp.ReadTimeout = 100;
        sp.Open();
    }
    void CloseConnection()
    {
        sp.Close();
    }
    string ReadSerialPort(int timeout = 50)
    {
        string message;
        sp.ReadTimeout = timeout;
        try
        {
            message = sp.ReadLine();
            return message;
        }
        catch (System.TimeoutException)
        {
            return null;
        }
    }
    void SwitchLedState()
    {
        ledOn = !ledOn;
        sp.WriteLine("L" + (ledOn ? "1" : "0"));
        Debug.Log("L" + (ledOn ? "1" : "0"));
    }
    // Start is called before the first frame update
    void Start()
    {
        OpenCOnnection();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStreaming)
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                Debug.LogWarning("LEd switch");
                SwitchLedState();
            }

            string value = ReadSerialPort();
            if (value != null)
            {
                Debug.Log(value);

                //CloseConnection();
            }
            //Debug.Log(WriteSerialPort());
        }
    }
}

