using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStart : MonoBehaviour
{
    [SerializeField] private KartControler _kartControler;
    public int countdown;
    public bool canRace = false, startBoost = false; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while(countdown != 0)
        {
            yield return new WaitForSeconds(1);
            countdown--;
        }

        _kartControler.Boost();
        canRace = true;
    }
}
