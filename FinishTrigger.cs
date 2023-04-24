using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{

    [SerializeField] Material platformMaterial;
    FinishingManager finishingManager;

    void Start() 
    {
        finishingManager = transform.parent.GetComponent<FinishingManager>();
    }
    
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Hat") && !other.GetComponent<Hat>().isHatCollectible)
        {   
            GameManager.instance.UpdateTotalScore();
            GetComponent<MeshRenderer>().material = platformMaterial;
        }
    }
}
