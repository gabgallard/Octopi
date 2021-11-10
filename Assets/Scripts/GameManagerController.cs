using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityCore.Audio;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] int maxNumPeople = 100;
    [SerializeField] int maxNumCars = 40;
    [SerializeField] int maxNumHelicopters = 5;
    [SerializeField] int maxNumTanks = 10;
    [SerializeField] int maxNumSoldiers = 20;

    [SerializeField] PlayableDirector endScene;

    int actualNumPeople;
    int actualNumCars;
    int actualNumHelicopters;
    int actualNumTanks;
    int actualNumSoldiers;

    bool armyActive = false;
    bool endGame = false;
    bool soundArmyPlaying = false;

    public bool cityActive = false;

    public static GameManagerController Instance;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
            DebugKeys();
        #endif
    }

    void DebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale += 0.5f;
            Debug.Log($"GameManager.timeScale: {Time.timeScale}");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            float timeScale = Time.timeScale;
            timeScale -= 0.5f;
            if(timeScale <= 0)
                timeScale = 0.5f;

            Time.timeScale = timeScale;

            Debug.Log($"GameManager.timeScale: {Time.timeScale}");
        }

        if(Input.GetKeyDown(KeyCode.Z))
            ActivateArmy();

        if(Input.GetKeyDown(KeyCode.E))
            PlayEndScene();
    }

    public void ActivateArmy()
    {
        if(!endGame && !armyActive)
        {
            Debug.Log("ActiveArmy()");
            armyActive = true;
            Invoke("SoundArmyPlay", 20f);
        }
    }

    public void IncreasePeople()
    {
        actualNumPeople ++;
    }

    public void DecreasePeople()
    {
        actualNumPeople --;
    }

    public bool CanMorePeople()
    {
        return maxNumPeople > actualNumPeople;
    }


    public void IncreaseCars()
    {
        actualNumCars ++;
    }

    public void DecreaseCars()
    {
        actualNumCars --;
    }

    public bool CanMoreCars()
    {
        return maxNumCars > actualNumCars;
    }


    public void IncreaseHelicopters()
    {
        actualNumHelicopters ++;
    }

    public void DecreaseHelicopters()
    {
        actualNumHelicopters --;
    }

    public bool CanMoreHelicopters()
    {
        return maxNumHelicopters > actualNumHelicopters && armyActive;
    }


    public void IncreaseTanks()
    {
        actualNumTanks ++;
    }

    public void DecreaseTanks()
    {
        actualNumTanks --;
    }

    public bool CanMoreTanks()
    {
        return maxNumTanks > actualNumTanks && armyActive;
    }


    public void IncreaseSoldiers()
    {
        actualNumSoldiers ++;
    }

    public void DecreaseSoldiers()
    {
        actualNumSoldiers --;
    }

    public bool CanMoreSoldiers()
    {
        return maxNumSoldiers > actualNumSoldiers && armyActive;
    }

    public bool CanShoot()
    {
        return !endGame;
    }

    public bool EndGame()
    {
        return endGame;
    }

    public void PlayEndScene()
    {
        Debug.Log("PlayEndScen()");

        StopCity();
        AudioController.instance.StopAudio(UnityCore.Audio.AudioType.MUS_militaryMarch, true);
        //AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.MUS_win, false);

        if(!endGame)
            endScene.Play();

        endGame = true;
    }

    public void StartCity()
    {
        cityActive = true;
        PlayCityBackground();
    }

    public void StopCity()
    {
        cityActive = false;
        StopCityBackground();
    }

    void PlayCityBackground()
    {
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.BCKGR_city, true);
    }

    public void StopCityBackground()
    {
        AudioController.instance.StopAudio(UnityCore.Audio.AudioType.BCKGR_city, true);
    }

    public void SoundArmyPlay()
    {
        if(!soundArmyPlaying)
        {
            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.MUS_militaryMarch, false);
            soundArmyPlaying = true;
        }
    }
}
