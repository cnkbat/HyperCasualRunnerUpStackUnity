using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Gate : MonoBehaviour
{
    //Variables
    int randNumForSubtract;

    int randNumForSum;    
    Player player;

    GateManager GateManager;

    [Header("Operations")]
    [SerializeField] List<int> numbersForSubtract;
    [SerializeField ] List <int> numberstoAddon; 

    [SerializeField] bool addition, subtraction;

    public bool isGateActive;

    [SerializeField] Material greenMaterial, redMaterial;


    [Header ("Get Colors")]
    [SerializeField] TMP_Text gateText;
    [SerializeField] Color[] Colors;
    [SerializeField] GameObject Screen;

    void Start()
    {
        // calculating a random number to process
        int rand = Random.Range(0, numbersForSubtract.Count);
        randNumForSubtract = numbersForSubtract[rand];
        randNumForSum = numberstoAddon[rand];
        ChooseOperation();
        player = FindObjectOfType<Player>();
        GateManager = transform.parent.GetComponent<GateManager>();
        isGateActive = true;
    }

    // choosing witch option the process
    private void ChooseOperation()
    {
        int operationSelector = Random.Range(1, 3);

        //addition
        if (operationSelector == 1)
        {
            addition = true;
            
            gateText.text = " + " + randNumForSum;

            UpdateTheColorOfGate(greenMaterial);
            Screen.GetComponent<SpriteRenderer>().color = Colors[0];
        }
        //subtraction
        else if (operationSelector == 2)
        {
            subtraction = true;

            gateText.text = " - " + randNumForSubtract;

            UpdateTheColorOfGate(redMaterial);
            Screen.GetComponent<SpriteRenderer>().color = Colors[1];
        }
    }

    private void UpdateTheColorOfGate(Material material)
    {
        for (int i = 0; i < transform.childCount -2 ; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material = material;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        //according to the type of door is doing the relevant operation
        if(other.CompareTag("Player") && isGateActive)
        {        
            GateManager.GateCloser();
            transform.position = new Vector3(-100,-40,0);
            if(addition)
            {
                GameManager.instance.AddHats(randNumForSum);
            } 

            if(subtraction) 
            {    
                if(player.collectedHatList.Count > 0)
                {
                    if(randNumForSubtract > player.collectedHatList.Count)
                    {
                        randNumForSubtract = player.collectedHatList.Count;
                    }
                    GameManager.instance.SubtractHats(randNumForSubtract);
                }
            }
        }
    }
}
