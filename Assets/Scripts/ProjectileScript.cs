using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Destroy(Instantiate(prefab, transform.position,Quaternion.identity),1f);
        Destroy(this.gameObject);
    }
}
