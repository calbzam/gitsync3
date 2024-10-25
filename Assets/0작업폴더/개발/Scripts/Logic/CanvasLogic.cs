using System;
using System.Collections;
using UnityEngine;

public class CanvasLogic : MonoBehaviour
{
    [SerializeField] private FadeInOut _respawnFadeInOut;
    public static FadeInOut RespawnFadeInOut;

    [SerializeField] private Letterboxes _letterboxes;
    public static Letterboxes Letterboxes;

    private void Awake()
    {
        RespawnFadeInOut = _respawnFadeInOut;
        Letterboxes = _letterboxes;
    }

    public static void EvalAndStartRespawnFade(int fromInstanceID)
    {
        if (RespawnFadeInOut.CurrentState == FadeInOut.FadingState.Ready)
            RespawnFadeInOut.StartFadeOutIn(fromInstanceID);
    }

    public static IEnumerator RespawnFadeInOut_AddReader(Action eventAddingMethod /* like () => (event) += (function) */)
    {
        yield return new WaitUntil(() => RespawnFadeInOut != null);
        eventAddingMethod();
    }
}
