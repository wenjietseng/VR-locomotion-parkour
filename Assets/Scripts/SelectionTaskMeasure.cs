using System.Collections;
using UnityEngine;
using TMPro;
public class SelectionTaskMeasure : MonoBehaviour
{
    public GameObject targetT;
    public GameObject targetTPrefab;
    Vector3 targetTStartingPos;
    public GameObject objectT;
    public GameObject objectTPrefab;
    Vector3 objectTStartingPos;

    public GameObject taskStartPanel;
    public GameObject donePanel;
    public TMP_Text startPanelText;
    public TMP_Text scoreText;
    public int completeCount;
    public bool isTaskStart;
    public bool isTaskEnd;
    public bool isCountdown;
    public Vector3 manipulationError;
    public float taskTime;
    public GameObject taskUI;
    public ParkourCounter parkourCounter;
    public DataRecording dataRecording;
    private int part;
    public float partSumTime;
    public float partSumErr;


    // Start is called before the first frame update
    void Start()
    {
        parkourCounter = GetComponent<ParkourCounter>();
        dataRecording = GetComponent<DataRecording>();
        part = 1;
        donePanel.SetActive(false);
        scoreText.text = "Part" + part.ToString();
        taskStartPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTaskStart)
        {
            // recording time
            taskTime += Time.deltaTime;
        }

        if (isCountdown)
        {
            taskTime += Time.deltaTime;
            startPanelText.text = (3.0 - taskTime).ToString("F1");
        }
    }

    public void StartOneTask()
    {
        taskTime = 0f;
        taskStartPanel.SetActive(false);
        donePanel.SetActive(true);
        objectTStartingPos = taskUI.transform.position + taskUI.transform.forward * 0.5f + taskUI.transform.up * 0.75f;
        targetTStartingPos = taskUI.transform.position + taskUI.transform.forward * 0.75f + taskUI.transform.up * 1.2f;
        objectT = Instantiate(objectTPrefab, objectTStartingPos, new Quaternion(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
        targetT = Instantiate(targetTPrefab, targetTStartingPos, new Quaternion(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
    }

    public void EndOneTask()
    {
        donePanel.SetActive(false);
        
        // release
        isTaskEnd = true;
        isTaskStart = false;
        
        // distance error
        manipulationError = Vector3.zero;
        for (int i = 0; i < targetT.transform.childCount; i++)
        {
            manipulationError += targetT.transform.GetChild(i).transform.position - objectT.transform.GetChild(i).transform.position;
        }
        scoreText.text = scoreText.text + "Time: " + taskTime.ToString("F1") + ", offset: " + manipulationError.magnitude.ToString("F2") + "\n";
        partSumErr += manipulationError.magnitude;
        partSumTime += taskTime;
        dataRecording.AddOneData(parkourCounter.locomotionTech.stage.ToString(), completeCount, taskTime, manipulationError);

        // Debug.Log("Time: " + taskTime.ToString("F1") + "\nPrecision: " + manipulationError.magnitude.ToString("F1"));
        Destroy(objectT);
        Destroy(targetT);
        StartCoroutine(Countdown(3f));
    }

    IEnumerator Countdown(float t)
    {
        taskTime = 0f;
        taskStartPanel.SetActive(true);
        isCountdown = true;
        completeCount += 1;

        if (completeCount > 4)
        {
            taskStartPanel.SetActive(false);
            scoreText.text = "Done Part" + part.ToString();
            part += 1;
            completeCount = 0;
        }
        else
        {
            yield return new WaitForSeconds(t);
            isCountdown = false;
            startPanelText.text = "start";
        }
        isCountdown = false;
        yield return 0;
    }
}
