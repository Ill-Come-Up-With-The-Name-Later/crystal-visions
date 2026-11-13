using System.Collections;
using UnityEngine;

public class PeriodicAction : MonoBehaviour
{
    [Header("Action timer")]
    [SerializeField]
    private float _interval;

    protected virtual void Start()
    {
        StartCoroutine(ActionCoroutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator ActionCoroutine()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(_interval);
            PerformAction();
        }

        yield return null;
    }

    protected virtual void PerformAction() {}
}
