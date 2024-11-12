using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;

public class NPCTutorialTalk : MonoBehaviour
{
    FlowerSystem flowerSys;
    private int progress = 0;
    private bool isTalking = false;

    public GameObject PressEHint;
    public float showDistance = 1f;
    public GameObject Player;
    public GameObject NPC;
    public PlayerController playerController;

    public AudioSource audioSource;

    void Start()
    {
        isTalking = false;
        PressEHint.SetActive(false);

        flowerSys = FlowerManager.Instance.CreateFlowerSystem("FlowerSample", true);

        // Register commands and effects if needed
        flowerSys.RegisterCommand("UsageCase", CustomizedFunction);
        flowerSys.RegisterEffect("customizedRotation", EffectCustomizedRotation);

        // Setup UI Layer
        flowerSys.SetupUIStage();
    }

    void Update()
    {
        if (Vector3.Distance(Player.transform.position, NPC.transform.position) <= showDistance)
        {
            PressEHint.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && !isTalking)
            {
                isTalking = true;
                audioSource.Play(0);
                StopMove();
                StartDialogue();
            }
        }
        else
        {
            PressEHint.SetActive(false);
        }

        // Continue dialogue if not at the end of the game
        if (isTalking && flowerSys.isCompleted)
        {
            switch (progress)
            {
                case 0:
                    flowerSys.ReadTextFromResource("start");
                    break;
                default:
                    isTalking = false; // Mark the end of the dialogue
                    ResumeMove(); // Resume movement after the last dialogue
                    flowerSys.RemoveDialog();
                    break;  
            }
            progress++;
        }

        // Handle input for dialogue progression
        if (isTalking && Input.GetKeyDown(KeyCode.E))
        {
            audioSource.Play(0);
            flowerSys.Next(); // Continue dialogue
        }
    }

    private void StartDialogue()
    {
        flowerSys.SetupDialog();
        progress = 0; // Reset progress to start
    }

    public void StopMove()
    {
        playerController.PlayerCanMove = false;
    }

    public void ResumeMove()
    {
        playerController.PlayerCanMove = true;
    }

    private void CustomizedFunction(List<string> _params)
    {
        // Implement any custom logic you want
        Debug.Log($"Custom function executed with params: {string.Join(", ", _params)}");
    }

    private void EffectCustomizedRotation(string key, List<string> _params)
    {
        // Here, you could implement your rotation logic, similar to the UsageCase example
        Debug.Log($"Customized rotation effect called for: {key}");
    }
}
