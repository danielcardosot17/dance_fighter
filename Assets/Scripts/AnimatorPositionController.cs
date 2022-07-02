using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPositionController : MonoBehaviour
{
    private GameEventSO animationFinishedEvent;
    public GameEventSO AnimationFinishedEvent { get => animationFinishedEvent; set => animationFinishedEvent = value; }

    public void RaiseAnimationFinishedEvent()
    {
        AnimationFinishedEvent.Raise();
    }
}
