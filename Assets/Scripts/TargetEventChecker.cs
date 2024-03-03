using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEventChecker : MonoBehaviour
{
    public bool isDeath = false;

    public void setIsDeath(bool isOnIsland) { this.isDeath = isOnIsland; }

    public bool getIsDeath() { return this.isDeath; }


    private void OnTriggerEnter(Collider other)
    {
        //hello
        /*print("Enter");
        UnityEngine.Debug.Log("Enter");
        UnityEngine.Debug.Log("playertag: " + other.gameObject.tag);*/
        if (other.gameObject.tag == "Bullet")
        {
            setIsDeath(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //hello
        /*print("Stay");
        UnityEngine.Debug.Log("Stay");
        UnityEngine.Debug.Log("playertag: " + other.gameObject.tag);*/
        if (other.gameObject.tag == "Bullet")
        {
            setIsDeath(true);
        }
    }

}
