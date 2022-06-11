using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Canvas startMenuCanvas;
    [SerializeField] private Canvas endMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(hasStarted)
        // {

        // }
    }

    public void StartGame()
    {
        startMenuCanvas.enabled = false;
        Debug.Log("AAAAAAAAA");
        // ChooseMusic();
        // StartMusic();
        // StartTimer();
    }

    private void EndGame()
    {

    }

    private void ResetVariables()
    {

    }

    public void PlayAgain()
    {

    }
}
