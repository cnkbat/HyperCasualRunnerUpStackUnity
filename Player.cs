using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Component
    CapsuleCollider capsuleCollider;
    Rigidbody rBody;

    FinishingManager FinishingManager;
    [HideInInspector] public Animator animator;

    [Header("Movement")]
    [SerializeField] float positiveLimitValue;
    [SerializeField] float negativeLimitValue;
    [SerializeField] float forwardMoveSpeed;
    [SerializeField] float horizontalMsMultipiler;
    float horizontalPos;

    [Header("Stack")]
    [SerializeField] float hatSize;
    [SerializeField] float hatSizeZAxis;
    [SerializeField] Transform stackTransform;

    [SerializeField] Transform prevHat;

    [SerializeField] Transform backupPrevHat;
    public List<GameObject> collectedHatList = new List<GameObject>();

   // Vector3 nextPos; old
    private Vector3 firstHatPos;
    private Vector3 nextHatPos;
    
    public int followingHatCounter;
    void Start() 
    {
        rBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        FinishingManager = FindObjectOfType<FinishingManager>();
    }

    void Update() 
    {
        
        if(!GameManager.instance.gameHasEnded && !FinishingManager.isFinishLinePassed)
        {
            MoveCharacter();
            rBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if(FinishingManager.isFinishLinePassed)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
            FinishingManager.PlayerFollowObject.transform.position, 
            forwardMoveSpeed * Time.deltaTime);
        } 
    }

    // moves character constantly forwards and limitedly horizontal
    void MoveCharacter()
    {
        float halfScreen = Screen.width /2;

        float finalXPos = (Input.mousePosition.x - halfScreen) / halfScreen;

        horizontalPos = Mathf.Clamp(finalXPos * horizontalMsMultipiler, negativeLimitValue, positiveLimitValue);
        
        float verticalPos = transform.position.z + forwardMoveSpeed * Time.deltaTime;

        transform.position = new Vector3(horizontalPos, transform.position.y, verticalPos);
       
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Hat") && other.TryGetComponent(out Hat hat))
        { 
            if(hat.isHatCollectible)
            {
                PickUpHat(other.gameObject,true);
                hat.isHatCollectible = false;
            }
        }
    }

    public void  PickUpHat(GameObject hatToPickUp, bool downOrUp = true)
    {
        hatToPickUp.TryGetComponent(out Hat hat);
        hatToPickUp.TryGetComponent(out Rigidbody rbody); 
        if(hat)
        {
            collectedHatList.Add(hatToPickUp);

            GameManager.instance.UpdateHatCounter();
            
            hat.FreezeHat();
            rBody.velocity = new Vector3 (0,0,0);
            rbody.isKinematic = true;
            if (collectedHatList.Count==1)
            {
                firstHatPos = stackTransform.transform.position;
                nextHatPos = new Vector3(hatToPickUp.transform.position.x, firstHatPos.y, hatToPickUp.transform.position.z);
                hatToPickUp.gameObject.transform.position = nextHatPos;
                nextHatPos = new Vector3(hatToPickUp.transform.position.x, stackTransform.transform.position.y + hatSize, hatToPickUp.transform.position.z);
                hatToPickUp.gameObject.GetComponent<Hat>().UpdateCubePosition(transform, true);
            }
            else if (collectedHatList.Count > 1)
            {
                UpdateNextHatPos();
                hatToPickUp.gameObject.transform.position = nextHatPos;
                nextHatPos = new Vector3(hatToPickUp.transform.position.x, hatToPickUp.gameObject.transform.position.y + hatSize, hatToPickUp.transform.position.z);
                hatToPickUp.gameObject.GetComponent<Hat>().UpdateCubePosition(collectedHatList[collectedHatList.Count - 2].transform, true);
                followingHatCounter++;
            }

            //OLD
           /* hatToPickUp.transform.parent = stackTransform.transform;

            Vector3 nextPos = prevHat.localPosition;

            nextPos.y += downOrUp ? hatSize : -hatSize;

            nextPos.z += hatSizeZAxis;

            hatToPickUp.transform.localPosition = nextPos;
            hatToPickUp.transform.position = Vector3.Lerp(hatToPickUp.transform.localPosition, nextPos , 1f * Time.deltaTime); 
            
            prevHat = hatToPickUp.transform;

            hatToPickUp.gameObject.GetComponent<Hat>().UpdateCubePosition(collectedHatList[collectedHatList.Count-1].transform, true);
            */
        }
    }
    //updates prevhat in order to prevent any bugs related to removing from list.
    public void UpdateNextHatPos()
    {
        if(collectedHatList.Count > 1)
        {
            nextHatPos = new Vector3(collectedHatList[collectedHatList.Count - 2].transform.position.x ,
            collectedHatList[collectedHatList.Count - 2].transform.position.y + hatSize ,  collectedHatList[collectedHatList.Count - 2].transform.position.y);  
        }
        else if(collectedHatList.Count == 1)
        {
            nextHatPos = stackTransform.transform.position;
        }
    }

    /// drops the hats and revomes from the collecthatslist.
    public void DropHat(int indexOfHittedHat)
    {   
        if(collectedHatList.Count > 0)
        {  
            // dropping only the hats that hitted and above it
            for (int i = collectedHatList.Count -1 ; i > indexOfHittedHat; i--)
            { 
                if(collectedHatList[i])
                {
                    collectedHatList[i].TryGetComponent(out Hat Hat);
                    Hat.isHatCollectible = true;
                    collectedHatList[i].transform.parent = null;
                    collectedHatList.RemoveAt(i);
                    Hat.StopFollowing();
                    followingHatCounter--;
                    Hat.ThrowHat();
                    UpdateNextHatPos();
                    GameManager.instance.UpdateHatCounter();  
                }
            }  
        }
    } 
}
