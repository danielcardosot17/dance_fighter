using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private float pollingTime = 1f;

    private float time = 0.0f;
    private int frameCount = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if(time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            displayText.text = frameRate.ToString("00") + " FPS";

            time = 0;
            frameCount = 0;
        }
    }
}
