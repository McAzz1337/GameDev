using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{

    [SerializeField] private float duration = 1.0f;
    void Start()
    {

        StartCoroutine("destruct");
    }

    void Update()
    {

    }

    private IEnumerator destruct()
    {

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }



    public void setDuration(float duration)
    {

        this.duration = duration;
    }
}
