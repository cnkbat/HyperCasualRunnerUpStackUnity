using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hat : MonoBehaviour
{
    Player player;

    BoxCollider BoxCollider;
    
    Rigidbody Rigidbody;
    FinishingManager FinishingManager;
    public bool isHatCollectible = true;

    public float throwspeed;

    [SerializeField] private float followSpeed;


    public Coroutine followCoroutine;
    private void Start() 
    {
        BoxCollider = GetComponent<BoxCollider>();
        Rigidbody = GetComponent<Rigidbody>();
        player = FindObjectOfType<Player>();
        FinishingManager = FindObjectOfType<FinishingManager>();
    }

    void Update() 
    {
        if(FinishingManager.isFinishLinePassed)
        {
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    public int GetIndexOfHatInCollectedHatList() 
    {
        player = FindObjectOfType<Player>();
        return player.collectedHatList.IndexOf(gameObject); 
    }
    public void ThrowHat()    
    {
        isHatCollectible = true;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Rigidbody.velocity = new Vector3(0, 0, throwspeed);
        Rigidbody.isKinematic = false;
    }

    public void FreezeHat()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ 
        | RigidbodyConstraints.FreezeRotation 
        | RigidbodyConstraints.FreezePositionX;
    }

    public void UpdateCubePosition(Transform followedHat, bool isFollowStart = false)
    {
        followCoroutine =  StartCoroutine(StartFollowingToLastHatPosition(followedHat, isFollowStart));
    }

    IEnumerator StartFollowingToLastHatPosition(Transform followedCube, bool isFollowStart = false)
    {
        while (isFollowStart && !FinishingManager.isFinishLinePassed )
        {
            yield return new WaitForEndOfFrame();
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, followedCube.position.x, followSpeed * Time.deltaTime),
                transform.position.y,
                Mathf.Lerp(transform.position.z, followedCube.position.z, followSpeed * Time.deltaTime));
        }
    }

    public void StopFollowing()
    {
        if(followCoroutine != null)
        {
            StopCoroutine(followCoroutine);
        }
    }
}
