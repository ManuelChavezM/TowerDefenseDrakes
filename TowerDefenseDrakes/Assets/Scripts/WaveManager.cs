using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WaveManager : MonoBehaviour
{

    public List<WaveObject> waves = new List<WaveObject>();
    public bool isWaitingForNextWave;
    public bool waveFinish;
    public int CurrentWave;
    public Transform initPosition;

    public TextMeshProUGUI counterText;
    public GameObject buttonNextWave;


    private void Start()
    {
        StartCoroutine(ProcesWave());
    }
    private void Update()
    {
        CheckCounterAndShowButton();
        checkCountForNextWave();
    }

    public void ChangeWave()
    {
        if (waveFinish)
            return;
        CurrentWave++;
        StartCoroutine(ProcesWave());
    }

    public void checkCountForNextWave()
    {
        if(isWaitingForNextWave && !waveFinish)
        {
            waves[CurrentWave].counterToNextWave -= 1 * Time.deltaTime;
            counterText.text = waves[CurrentWave].counterToNextWave.ToString("00"); 
            if(waves[CurrentWave].counterToNextWave <= 0 )
            {
                ChangeWave();
                Debug.Log("set next wave");
            }
        }

    }
    private IEnumerator ProcesWave()
    {
        if (waveFinish)
            yield break;
        isWaitingForNextWave = false;
        waves[CurrentWave].counterToNextWave = waves[CurrentWave].timerForNextWave;
        for(int i = 0; i<waves[CurrentWave].enemys.Count; i++)
        {
            var enemyInsta = Instantiate(waves[CurrentWave].enemys[i], initPosition.position, initPosition.rotation);
            yield return new WaitForSeconds(waves[CurrentWave].timerPerCreation);
        }
        isWaitingForNextWave = true;
        if (CurrentWave >= waves.Count - 1)
        {
            Debug.Log("nivel terminado");
            waveFinish = true;
        }
    }

    public void CheckCounterAndShowButton()
    {
        if (!waveFinish)
        {
            buttonNextWave.SetActive(isWaitingForNextWave);
            counterText.gameObject.SetActive(isWaitingForNextWave);
        }
    }




}


[System.Serializable]
public class WaveObject
{
    public float timerPerCreation = 1;
    public float timerForNextWave = 10;
    [HideInInspector] public float counterToNextWave = 0;
    public List<EnemyController> enemys = new List<EnemyController>();
}

