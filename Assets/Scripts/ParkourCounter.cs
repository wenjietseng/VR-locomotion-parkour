using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParkourCounter : MonoBehaviour
{
    public LocomotionTechnique locomotionTech;
    public bool isStageChange;
    // banners
    public GameObject startBanner;
    public GameObject firstBanner;
    public GameObject secondBanner;
    public GameObject finalBanner;
    // coins
    public GameObject firstCoins;
    public GameObject secondCoins;
    public GameObject finalCoins;
    // Object Interaction Task
    public GameObject objIX1;
    public GameObject objIX2;
    public GameObject objIX3;
    // respawn points
    public Transform start2FirstRespawn;
    public Transform first2SecondRespawn;
    public Transform second2FinalRespawn;
    public Vector3 currentRespawnPos;

    public float timeCounter;
    private float part1Time;
    private float part2Time;
    private float part3Time;
    public int coinCount;
    
    private int part1Count; // 17
    private int part2Count; // 33
    private int part3Count; // 24
    public bool parkourStart;

    public TMP_Text timeText;
    public TMP_Text coinText;
    public TMP_Text recordText;
    public GameObject timeTextGO;
    public GameObject coinTextGO;
    public GameObject recordTextGO;
    public GameObject endTextGO;
    public AudioSource backgroundMusic;
    public AudioSource endSoundEffect;
    public SelectionTaskMeasure selectionTaskMeasure;

    void Start()
    {
        coinCount = 0;
        timeCounter = 0.0f;
        firstBanner.SetActive(false);
        secondBanner.SetActive(false);
        finalBanner.SetActive(false);
        firstCoins.SetActive(false);
        secondCoins.SetActive(false);
        finalCoins.SetActive(false);
        objIX2.SetActive(false);
        objIX3.SetActive(false);
        objIX1.SetActive(false);
        parkourStart = false;
        endTextGO.SetActive(false);
        this.selectionTaskMeasure = this.GetComponent<SelectionTaskMeasure>();
    }

    void Update()
    {
        if (isStageChange)
        {
            isStageChange = false;
            if (locomotionTech.stage == startBanner.name)
            {
                parkourStart = true;
                startBanner.SetActive(false);
                firstBanner.SetActive(true);
                firstCoins.SetActive(true);
                objIX1.SetActive(true);
                this.GetComponent<SelectionTaskMeasure>().taskUI.transform.position = objIX1.transform.position;
                currentRespawnPos = start2FirstRespawn.position;
            }
            else if (locomotionTech.stage == firstBanner.name)
            {
                firstBanner.SetActive(false);
                firstCoins.SetActive(false);
                objIX1.SetActive(false);
                secondBanner.SetActive(true);
                secondCoins.SetActive(true);
                objIX2.SetActive(true);
                this.GetComponent<SelectionTaskMeasure>().taskUI.transform.position = objIX2.transform.position;
                part1Time = timeCounter;
                part1Count = coinCount;
                currentRespawnPos = first2SecondRespawn.position;
                UpdateRecordText(1, part1Time, part1Count, 16);
            }
            else if (locomotionTech.stage == secondBanner.name)
            {
                secondBanner.SetActive(false);
                secondCoins.SetActive(false);
                objIX2.SetActive(false);
                finalBanner.SetActive(true);
                finalCoins.SetActive(true);
                objIX3.SetActive(true);
                this.GetComponent<SelectionTaskMeasure>().taskUI.transform.position = objIX3.transform.position;
                part2Time = timeCounter - part1Time;
                part2Count = coinCount - part1Count;
                currentRespawnPos = second2FinalRespawn.position;
                UpdateRecordText(2, part2Time, part2Count, 30);
            }
            else if (locomotionTech.stage == finalBanner.name)
            {
                parkourStart = false;
                finalCoins.SetActive(false);
                objIX3.SetActive(false);
                part3Time = timeCounter - (part1Time + part2Time);
                part3Count = coinCount - (part1Count + part2Count);
                UpdateRecordText(3, part3Time, part3Count, 23);
                timeTextGO.SetActive(false);
                coinTextGO.SetActive(false);
                recordTextGO.SetActive(false);
                endTextGO.SetActive(true);
                endTextGO.GetComponent<TMP_Text>().text = "Parkour Finished!\n" + recordText.text +
                    "\ntotal: " + timeCounter.ToString("F1") + ", " + coinCount.ToString() + "/69";
                Debug.Log(endTextGO.GetComponent<TMP_Text>().text);
                endSoundEffect.Play();
            }
        }

        if (parkourStart)
        {
            timeCounter += Time.deltaTime;
            timeText.text = "time: " + timeCounter.ToString("F1");
            coinText.text = "coins: " + coinCount.ToString();
        }       
    }

    void UpdateRecordText(int part, float time, int coinsCount, int coinsInPart)
    {
        string newRecords = "loco" + part.ToString() + ": " + time.ToString("F1") + ", " + coinsCount + "/" + coinsInPart + "\n" +
                            "obj"  + part.ToString() + ": " + (selectionTaskMeasure.partSumTime/5f).ToString("F1") + "," + (selectionTaskMeasure.partSumErr/5).ToString("F2");
        recordText.text = recordText.text + "\n" + newRecords;
    }
}
