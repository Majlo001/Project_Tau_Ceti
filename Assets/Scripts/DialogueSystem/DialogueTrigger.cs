using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    private float m_interactionTimer = 0f;
    private bool m_hasInteracted = false; 

    public Transform Player;
    public GameObject CanvasObject;
    public Slider InteractionSlider;
    public TextAsset InkJSON;

    public float InteractionDistance = 3f;
    public float InteractionDuration = 3f;
    public float DecreaseSpeed = 1f;


    void Start()
    {
        CanvasObject.SetActive(false);
        InteractionSlider.value = 0f;
    }

    void Update()
    {
        if (m_hasInteracted)
        {
            if (CanvasObject.activeSelf) {
                HideTooltip();
            }
            Destroy(this);
            return;
        }
        
        if (DialogueManager.GetInstance().IsDialoguePlaying) {
            if (CanvasObject.activeSelf) {
                HideTooltip();
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer <= InteractionDistance)
        {
            // Debug.Log("In interaction range!");

            if (Input.GetKey(KeyCode.I))
            {
                m_interactionTimer += Time.deltaTime;
                InteractionSlider.value = m_interactionTimer / InteractionDuration;

                if (m_interactionTimer >= InteractionDuration)
                {
                    m_hasInteracted = true;
                    HideTooltip();
                    DialogueManager.GetInstance().EnterDialogueMode(InkJSON);
                }
            }
            else
            {
                if (m_interactionTimer > 0f)
                {
                    m_interactionTimer -= Time.deltaTime * DecreaseSpeed;
                    InteractionSlider.value = m_interactionTimer / InteractionDuration;
                }
            }

            ShowTooltip();
        }
        else
        {
            HideTooltip();
        }
    }

    void ShowTooltip() {
        CanvasObject.SetActive(true);
    }

    void HideTooltip() {
        CanvasObject.SetActive(false);
        ResetInteractionTimer();
    }

    void ResetInteractionTimer() {
        m_interactionTimer = 0f;
        InteractionSlider.value = 0f;
    }

    // void Collect() {
    //     Debug.Log("Item collected!");
    //     canvasObject.SetActive(false);
    //     //TODO: Collect

    //     Destroy(gameObject);
    // }
}
