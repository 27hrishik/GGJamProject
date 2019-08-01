using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int requiredKey;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&other.gameObject.GetComponent<InputController>().ifAccessKeyObtained>=requiredKey)
        {
            GetComponent<Animator>().SetBool("open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Animator>().SetBool("open", false);
        }
    }
}
