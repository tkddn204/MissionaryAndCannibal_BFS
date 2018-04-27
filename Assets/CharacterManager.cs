using System.Collections;
using System.Collections.Generic;
using State;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public Transform missionary;
    public Transform cannibal;
    public Transform boat;

    public Text missionaryText;
    public Text cannibalText;
    public string missionaryTextStringValue;
    public string cannibalTextStringValue;

    public List<Transform> leftMissionaryObjectList = new List<Transform>();
    public List<Transform> leftCannibalObjectList = new List<Transform>();
    public List<Transform> boatPeopleObjectList = new List<Transform>();
    public List<Transform> rightMissionaryObjectList = new List<Transform>();
    public List<Transform> rightCannibalObjectList = new List<Transform>();
    
    public List<Transform> spawnPoints = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        spawnPoints = new List<Transform>(
            GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>());
        setLeftMissionaryNumber(missionaryText.text);
        setLeftCannibalNumber(cannibalText.text);
    }

    void Update()
    {
        if (missionaryText.text != missionaryTextStringValue)
        {
            setLeftMissionaryNumber(missionaryText.text);
        }

        if (cannibalText.text != cannibalTextStringValue)
        {
            setLeftCannibalNumber(cannibalText.text);
        }
    }

    private void changeLeftMissionary(int missionaryNumber)
    {
        changeCurrentState(missionaryNumber, 1,
            leftMissionaryObjectList, missionary);
    }

    private void changeLeftCannibal(int cannibalNumber)
    {
        changeCurrentState(cannibalNumber, 2,
            leftCannibalObjectList, cannibal);
    }

    private void changeBoatPeople(State.Boat boatState, int boatMissionaryNumber, int boatCannibalNumber)
    {
        int peopleOnBoatSpawnPoint;
        int boatSpawnPoint;
        if (boatState == Boat.LEFT_R || boatState == Boat.FROM_LEFT_TO_RIGHT_R)
        {
            peopleOnBoatSpawnPoint = 3;
            boatSpawnPoint = 7;
        }
        else
        {
            peopleOnBoatSpawnPoint = 4;
            boatSpawnPoint = 8;
        }

        changeBoat(boatState, boatSpawnPoint);
        
        // 없애고 다시만듬ㅎ
        for (var i = 0; i < boatPeopleObjectList.Count; i++)
        {
            Destroy(boatPeopleObjectList[i].gameObject);
        }
        boatPeopleObjectList.Clear();
        
        changeCurrentState(boatMissionaryNumber,
            peopleOnBoatSpawnPoint, boatPeopleObjectList, missionary);

        changeCurrentState(boatMissionaryNumber + boatCannibalNumber,
            peopleOnBoatSpawnPoint, boatPeopleObjectList, cannibal);
        
        // 이동
        float increaseX = 0f;
        if (boatState == Boat.FROM_LEFT_TO_RIGHT_R)
        {
            foreach (var boatObject in boatPeopleObjectList)
            {
                Vector3 position = spawnPoints[4].position;
                position.x += increaseX;
                boatObject.position = position;
                increaseX += 1.0f;
            }
            // 위치 변경
            boat.position = spawnPoints[boatSpawnPoint+1].position;
        }
        else if (boatState == Boat.FROM_RIGHT_TO_LEFT_L)
        {
            foreach (var boatObject in boatPeopleObjectList)
            {
                Vector3 position = spawnPoints[3].position;
                position.x += increaseX;
                boatObject.position = position;
                increaseX += 1.0f;
            }
            // 위치 변경
            boat.position = spawnPoints[boatSpawnPoint-1].position;
        }
    }

    private void changeBoat(State.Boat boatState, int boatSpawnPoint)
    {
        var speed = GameObject.Find("/Managers/ProceedManager")
            .GetComponent<ProceedManager>().speedSlider.value;
        
        // 방향 변경
        if (boatState == Boat.LEFT_R || boatState == Boat.FROM_LEFT_TO_RIGHT_R)
        {
            boat.localScale = new Vector3(2, 2, 1);
        }
        else
        {
            boat.localScale = new Vector3(-2, 2, 1);
        }
    }

    private void changeRightMissionary(int missionaryNumber)
    {
        changeCurrentState(missionaryNumber, 5,
            rightMissionaryObjectList, missionary);
    }

    private void changeRightCannibal(int cannibalNumber)
    {
        changeCurrentState(cannibalNumber, 6,
            rightCannibalObjectList, cannibal);
    }

    private void changeCurrentState(
        int changeToNumber, int spawnPointIndex,
        List<Transform> changeTransformList, Transform transform)
    {
        if (changeToNumber < 0)
        {
            return;
        }

        Vector3 pos = spawnPoints[spawnPointIndex].position;
        var changeNumber = changeToNumber - changeTransformList.Count;
        if (changeNumber > 0)
        {
            for (var i = 0; i < changeNumber; i++)
            {
                if (changeTransformList.Count > 0)
                {
                    pos.x = changeTransformList[changeTransformList.Count - 1].position.x + 1.0f;
                    changeTransformList.Add(Instantiate(transform, pos, Quaternion.identity) as Transform);
                }
                else
                {
                    changeTransformList.Add(Instantiate(transform, pos, Quaternion.identity) as Transform);
                }
            }
        }
        else
        {
            for (var i = 0; i < -changeNumber; i++)
            {
                Destroy(changeTransformList[changeTransformList.Count - 1].gameObject);
                changeTransformList.RemoveAt(changeTransformList.Count - 1);
            }
        }
    }

    public void renderGameState(GameState gameState)
    {
        changeLeftMissionary(gameState.LeftMissionaryNumber);
        changeLeftCannibal(gameState.LeftCannibalNumber);
        changeBoatPeople(gameState.BoatState, gameState.BoatMissionaryNumber, gameState.BoatCannibalNumber);
        changeRightMissionary(gameState.RightMissionaryNumber);
        changeRightCannibal(gameState.RightCannibalNumber);
        
        switch (gameState.PeopleState)
        {
            case State.People.LEFT_RIDDEN_BOAT:
                break;
            case State.People.RIGHT_RIDDEN_BOAT:
                break;
            case State.People.MOVING_FROM_LEFT_TO_RIGHT:
                break;
            case State.People.MOVING_FROM_RIGHT_TO_LEFT:
                break;
            case State.People.READY:
            default:
                break;
        }
    }

    public void setLeftMissionaryNumber(string missionaryNumberString)
    {
        missionaryTextStringValue = missionaryNumberString;
        changeLeftMissionary(int.Parse(missionaryNumberString));
    }

    public void setLeftCannibalNumber(string cannibalNumberString)
    {
        cannibalTextStringValue = cannibalNumberString;
        changeLeftCannibal(int.Parse(cannibalNumberString));
    }
}