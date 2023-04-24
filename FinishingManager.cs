using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FinishingManager : MonoBehaviour
{
    Player player;

    [Header("Finishing Variables")]
    [SerializeField] List<GameObject> finishingHatLocations;
    [SerializeField] int moveSpeed;
    [SerializeField] float delayTime;
    public GameObject PlayerFollowObject;

    int hatPerYardCounter;
    public bool isFinishLinePassed;

    void Start() 
    {
        player = FindObjectOfType<Player>();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            isFinishLinePassed = true;
            if(!player)
            {
                return;
            }
            /*for(int i = 0; i < player.collectedHatList.Count; i++)
            {   
                player.collectedHatList[i].transform.parent = null;
            } */
            GameManager.instance.gameHud.gameObject.SetActive(false);
            GameManager.instance.endHud.gameObject.SetActive(true);    
        }
        if(other.CompareTag("Hat"))
        {
            other.TryGetComponent(out Hat hat);
            {
                if(hat && hat.isHatCollectible == true)
                {
                    other.transform.position = new Vector3(-100,-40,0);
                    other.gameObject.SetActive(false);
                }
            }
        }
    }
    void Update() 
    {
        if(player.collectedHatList.Count > 0 && isFinishLinePassed)
        {
            MoveHatstoFinishLine();
        }     
    }

    void MoveHatstoFinishLine()
    {   
        for(int i = 0; i < player.collectedHatList.Count; i++)
        {   
            player.collectedHatList[player.collectedHatList.Count - 1 -i].transform.position = Vector3.MoveTowards(
            player.collectedHatList[player.collectedHatList.Count - 1 -i].transform.position,
            finishingHatLocations[i].transform.position,
            moveSpeed * Time.deltaTime);
            PlayerFollowObject.transform.position = new Vector3(PlayerFollowObject.transform.position.x , 
            PlayerFollowObject.transform.position.y, finishingHatLocations[i].transform.position.z);
        }
        
    }

}
