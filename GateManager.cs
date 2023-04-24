using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public static GameManager instance;
    public void GateCloser()
    {
        Gate[] Gates = GetComponentsInChildren<Gate>();
        foreach (var gate in Gates)
        {
            gate.isGateActive = false;
            gate.transform.position = new Vector3(0,0,-450);
            gate.gameObject.SetActive(false);
        }
    }

}
