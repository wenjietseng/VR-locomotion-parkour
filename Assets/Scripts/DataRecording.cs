using System.Collections.Generic;
using UnityEngine;

public class DataRecording : MonoBehaviour
{
    // ParkourCounter parkourCounter;
    // SelectionTaskMeasure selectionTaskMeasure;

    public struct ObjectInteractionData
    {
        public string round;
        public int number;
        public float taskTime;
        public Vector3 error; // error is calculated by the distance between three cubes (e.g., red to red, green to green, and blue to blue)

        public ObjectInteractionData(string round, int number, float taskTime, Vector3 error)
        {
            this.round = round;
            this.number = number;
            this.taskTime = taskTime;
            this.error = error;
        }
    }
    
    public List<ObjectInteractionData> dataList = new();


    public void AddOneData(string round, int number, float taskTime, Vector3 error)
    {
        dataList.Add(new ObjectInteractionData(round, number, taskTime, error));
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
