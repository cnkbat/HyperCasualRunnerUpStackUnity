using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public void ClickedPlay()
    {
        SceneManager.LoadScene(1);
    }
    
}
