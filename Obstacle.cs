using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Player player;

    MeshCollider MeshCollider;
    void Start() 
    {   
        MeshCollider = GetComponent<MeshCollider>();
        player = FindObjectOfType<Player>();
    }
    private void OnCollisionEnter(Collision other) 
    {
        other.transform.TryGetComponent(out Hat hat);

        if(other.gameObject.CompareTag("Hat") && !hat.isHatCollectible)
        {  
            // getting the index of hat that hit the obstacle and returning that index to the drop
            // in order to drop only hat that hit and above

            List<int> nums = new List<int> {120};
            int min = nums[0];
            nums.Add(120);
            nums.Add(hat.GetIndexOfHatInCollectedHatList());

            for(int i = 0; i < nums.Count; i ++)
            {
                if(nums[i] < min)
                {
                    min = nums[i];
                }
            }
            player.DropHat(min);
        }
    }
}
