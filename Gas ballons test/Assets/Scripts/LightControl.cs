using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LightControl : MonoBehaviour
{
    public int Status;
    private Light[] Lights;
    private GameObject[] Bulbs;

    private void Status0()
    {
        foreach (Light light in Lights)
        {
            light.intensity = 0;
        }
        foreach (GameObject light in Bulbs)
        {
            light.SetActive(false);
        }
    }

    private void Status14(int a, float b)
    {
        foreach (Light light in Lights)
        {
            int r = Random.Range(0, a);
            if (r == 0)
            {
                if (light.intensity == 0)
                {
                    light.intensity = b;
                }
                else
                {
                    light.intensity = 0;
                }
            }
        }
        foreach (GameObject light in Bulbs)
        {
            int r = Random.Range(0, a);
            if (r == 0)
            {
                if (light.activeSelf)
                {
                    light.SetActive(false);
                }
                else
                {
                    light.SetActive(true);
                }
            }
        }
    }

    private void Status1()
    {
        Status14(2, 0.3f);
    }

    private void Status2()
    {
        Status14(12, 0.6f);
    }

    private void Status3()
    {
        Status14(20, 0.8f);
    }

    private void Status4()
    {
        foreach (Light light in Lights)
        {
            light.intensity = 0.8f;
        }
    }

    private void Status5()
    {
        return;
    }

    private float timer;

    private void Start()
    {
        Lights = FindObjectsOfType<Light>();
        Bulbs = GameObject.FindGameObjectsWithTag("LampOn");

        timer = Random.Range(0.1f, 0.5f);
        Debug.Log(timer);
        if (Status == 0)
        {
            Status0();
        }
        else if (Status == 5)
        {
            Status5();
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            switch (Status)
            {
                case 1:
                    Status1();
                    break;
                case 2:
                    Status2();
                    break;
                case 3:
                    Status3();
                    break;
                case 4:
                    Status4();
                    break;
            }
            timer = Random.Range(0.1f, 0.5f);
        }
    }
}
