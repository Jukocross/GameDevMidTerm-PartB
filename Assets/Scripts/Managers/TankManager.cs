using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public int m_Wins;

    private TankMovement m_Movement;
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;
    private StateController m_StateController;

    public void SetupAI(List<Transform> wayPointList)
    {
        m_StateController = m_Instance.GetComponent<StateController>();
        m_StateController.SetupAI(true, wayPointList);

        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        m_ColoredPlayerText = $"<color=#{ColorUtility.ToHtmlStringRGB(m_PlayerColor)}>PLAYER {m_PlayerNumber}</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++) renderers[i].material.color = m_PlayerColor;
    }


    public void SetupPlayerTank()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_ColoredPlayerText = $"<color=#{ColorUtility.ToHtmlStringRGB(m_PlayerColor)}>PLAYER {m_PlayerNumber}</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++) renderers[i].material.color = m_PlayerColor;
    }

    public void DisableControl()
    {
        if (m_Movement != null) m_Movement.enabled = false;

        if (m_StateController != null) m_StateController.enabled = false;

        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }

    public void EnableControl()
    {
        if (m_Movement != null) m_Movement.enabled = true;

        if (m_StateController != null) m_StateController.enabled = true;

        m_Shooting.enabled = true;
        // Initiate balance on each start of the round to set FireRate
        balance();
        m_CanvasGameObject.SetActive(true);
    }

    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
    
    //Helper function to set the tank shooting fire rate
    private void balance()
    {
        switch (m_Wins)
        {
            case 1:
                m_Shooting.setFireRate(1.2f);
                break;
            case 2: m_Shooting.setFireRate(1.5f); break;
            case 3: m_Shooting.setFireRate(1.8f); break;
            case 4: m_Shooting.setFireRate(2f); break;
            case 5: m_Shooting.setFireRate(2.5f); break;
            default: m_Shooting.setFireRate(1.0f); break;
        }
    }   

}