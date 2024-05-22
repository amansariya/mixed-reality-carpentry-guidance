using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

public class SceneCycler : MonoBehaviour
{
    private static SceneCycler instance;
    private int currentSceneIndex = 0;
    private List<String> scenes = new List<string>{ "VisualLowInfo", "VisualMedInfo", "VisualHighInfo", "Voice" };

    void Awake()
    {
        // Ensure only one instance of SceneCycler exists
        string sceneName = SceneManager.GetActiveScene().name;
        currentSceneIndex = scenes.IndexOf(sceneName);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.N))
        //{
        //    GoToNextScene();
        //}
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    GoToPreviousScene();
        //}

        if (OVRInput.GetDown(OVRInput.Button.One)) // A button
        {
            GoToNextScene();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two)) // B button
        {
            GoToPreviousScene();
        }
    }

    void GoToNextScene()
    {
        if (currentSceneIndex < scenes.Count - 1)
        {
            currentSceneIndex++;
            SceneManager.LoadScene(scenes[currentSceneIndex]);
        }
    }

    void GoToPreviousScene()
    {
        if (currentSceneIndex > 0)
        {
            currentSceneIndex--;
            SceneManager.LoadScene(scenes[currentSceneIndex]);
        }
    }
}
