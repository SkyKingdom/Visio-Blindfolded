using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacleManager : MonoBehaviour
{
    public GameObject[] ObstaclesInsideSchoolBuilding;
    public GameObject[] ObstaclesInsideSchoolGrounds;
    public GameObject[] ObstaclesOutsideSchoolGrounds;

    void Start()
    {
        RandomObstacleGeneration();
    }

    public void RandomObstacleGeneration()
    {
        int randomNumber;

        randomNumber = Random.Range(0, ObstaclesInsideSchoolBuilding.Length);
        ObstaclesInsideSchoolBuilding[randomNumber].SetActive(true);

        randomNumber = Random.Range(0, ObstaclesInsideSchoolGrounds.Length);
        ObstaclesInsideSchoolGrounds[randomNumber].SetActive(true);

        randomNumber = Random.Range(0, ObstaclesOutsideSchoolGrounds.Length);
        ObstaclesOutsideSchoolGrounds[randomNumber].SetActive(true);
    }
}
