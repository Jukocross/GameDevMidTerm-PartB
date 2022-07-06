using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;

    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;
    //Variables to set the fireRates
    private float defaultFireRate = 1.0f;
    private bool m_recharging;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update()
    {
        m_AimSlider.value = m_MinLaunchForce;

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            //Change the input into variables instead of an static value
            Fire(m_CurrentLaunchForce, defaultFireRate);
        }
        //Doesnt initiate the charging of the shooting while recharging
        else if (Input.GetButtonDown(m_FireButton) && !m_recharging)
        {
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            //Change the input into variables instead of an static value
            Fire(m_CurrentLaunchForce, defaultFireRate);
        }
    }


    public void Fire(float launchForce, float fireRate)
    {
        //Return if recharging
        if (m_recharging) return;
        m_recharging = true;
        //Initiate the interval waiting time as recharging
        StartCoroutine(recharging(fireRate));

        m_Fired = true;

        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    //Provide interval waiting time as the recharging time
    IEnumerator recharging(float fireRate)
    {
        yield return new WaitForSeconds(fireRate);
        m_recharging = false;
    }
    //Public set function for the fireRate
    public void setFireRate(float firerate)
    {   
        defaultFireRate = firerate;
    }
    
}