using UnityEngine;
using System.IO;
using System.IO.Ports;
using System;
using DG.Tweening;


public class ArduinoLEDController : MonoBehaviour {

    private string portName = "COM4";
    private SerialPort serialPort;


    [SerializeField]
    private float triggerDelay;

    private HotSpot[] hotSpots;

    public class HotSpot {
        public bool isOn { get; set; }
        public string[] Commands { get; set; }
        public Tween delayedCaller;

        public HotSpot(string on, string off) {
            Commands = new string[2];
            Commands[0] = on;
            Commands[1] = off;
        }
    }

    void Start() {
        Debug.Log("ArduinoLEDSetup");
        hotSpots = new HotSpot[3];
        hotSpots[0] = new HotSpot("A", "a");
        hotSpots[1] = new HotSpot("B", "b");
        hotSpots[2] = new HotSpot("C", "c");
    }

    public void Trigger(int index) {
        Debug.Log("LEDFX Trigger: " + index);
        hotSpots[index].delayedCaller = DOVirtual.DelayedCall(triggerDelay, () => ActivateSpot(hotSpots[index]));
    }

    public void Kill(int index) {
        Debug.Log("LEDFX Kill: " + index);
        ResetSpot(index);
    }

    private void ActivateSpot(HotSpot spot) {
        spot.isOn = true;
    }

    private void ResetSpot(int index) {
        hotSpots[index].isOn = false;
        hotSpots[index].delayedCaller.Kill();
    }

    void OnDestroy() {
        if (serialPort != null) {
            Debug.Log("Close on Destroy!");

            for (int i = 0; i < hotSpots.Length; i++) {
                HotSpot hotSpot = hotSpots[i];

                string command = hotSpot.Commands[1];
                serialPort.WriteLine(command);
            }

            serialPort.Close();
            serialPort = null;
        }
    }

    void Update() {

#if UNITY_EDITOR

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1)) Trigger(0);
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2)) Trigger(1);
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3)) Trigger(2);


        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.Alpha1)) Kill(0);
        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.Alpha2)) Kill(1);
        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.Alpha3)) Kill(2);

#endif

        try {
            if (serialPort == null) serialPort = new SerialPort(portName, 9600);
            if (serialPort.IsOpen == false) serialPort.Open();

            for (int i = 0; i < hotSpots.Length; i++) {
                HotSpot hotSpot = hotSpots[i];

                string command = hotSpot.isOn ? hotSpot.Commands[0] : hotSpot.Commands[1];
                serialPort.WriteLine(command);

                //Debug.Log(hotSpot.Pin+"|"+command);
            }

            //serialPort.BaseStream.Flush();
            //flatArray = "";
        }
        catch (IOException e) {
            if (serialPort != null) {
                serialPort.Close();
                serialPort = null;
            }
            Debug.LogWarning(portName + ": " + e.ToString());
        }
        catch (Exception e) {
            Debug.LogWarning(portName + ": " + e.ToString());
        }
    }
}