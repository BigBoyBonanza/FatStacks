using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadPlayer : MonoBehaviour
{
    SaveSystem saveSystem;
    // Start is called before the first frame update
    void Start()
    {
        saveSystem = GameObject.FindGameObjectWithTag("SaveSystem")?.GetComponent<SaveSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Restarting scene
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject);
            if (saveSystem != null)
            {
                //saveSystem.LoadLatest();
                
            }
            else
            {
                Debug.Log("SaveSystem was not found in this scene");
            }
            
            
        }
    }
}
