using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject camera;
    public GameObject healthUI;
    public Slider healthSlider;
    public GameObject stunUI;
    public Slider stunSlider;
    public GameObject triangleIcon;


    private void Awake()
    {
        healthUI.SetActive(true);
        stunUI.SetActive(true);
        triangleIcon.SetActive(false);

        healthSlider = healthUI.GetComponent<Slider>();
        stunSlider = stunUI.GetComponent<Slider>();
    }

    void Update()
    {
        transform.LookAt(camera.transform);
        this.transform.Rotate(0, 180, 0);
    }

    void BecomeStunned()
    {
        healthUI.SetActive(false);
        stunUI.SetActive(false);
        triangleIcon.SetActive(true);
    }

    void SetHealthAndStun(int[] vitals)
    {
        healthSlider.value = vitals[0];
        stunSlider.value = vitals[1];
    }

    void Initialise(int[] maximums)
    {
        healthSlider.maxValue = maximums[0];
        stunSlider.maxValue = maximums[1];

        healthSlider.value = maximums[0];
        stunSlider.value = 0;
    }
}
