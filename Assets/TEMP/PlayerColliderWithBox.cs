using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderWithBox : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player") ;
    //    Debug.Log("Collided with Player");
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerStay");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Pressed E");
                Destroy(this.gameObject);
            }
        }
    }
}
