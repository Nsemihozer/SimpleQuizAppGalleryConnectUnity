using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void ActivatePanel(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    
    public void DeActivatePanel(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SetCategory(string category)
    {
        PlayerPrefs.SetString("category",category);
    }
    
}
