using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] Eagle eagle;
    [SerializeField] Duck duck;
    [SerializeField] float initialTimer = 10;
    [SerializeField] UnityEvent OnDeploy;
    float timer;

    void Start()
    {
        timer = initialTimer;
        eagle.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timer <= 0 && eagle.gameObject.activeInHierarchy == false)
        {
            OnDeploy.Invoke();
            duck.SetMoveable(false);
            eagle.gameObject.SetActive(true);
            eagle.transform.position = duck.transform.position + new Vector3(0, 0, 13);
            
        }

        timer -= Time.deltaTime;
    }

    public void ResetTimer()
    {
        timer = initialTimer;
    }
}
