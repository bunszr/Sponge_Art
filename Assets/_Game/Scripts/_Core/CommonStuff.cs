using UnityEngine;
using System.Collections;

public class CommonStuff : MonoBehaviour
{
    // If CommonStuff Deactivated, Coroutine does not run. Solution "GameManager.Ins.StartCoroutine"

    public void Active(float delay)
    {
        Debug.Log(gameObject, gameObject);
        GameManager.Ins.StartCoroutine(ActivationIE(delay, true));
    }

    public void Deactive(float delay)
    {
        GameManager.Ins.StartCoroutine(ActivationIE(delay, false));
    }

    IEnumerator ActivationIE(float delay, bool active)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(active);
    }
}

