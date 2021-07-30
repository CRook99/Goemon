using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FinisherScript : MonoBehaviour
{
    [Header("References")]
    public GameObject enemy;
    public Animator animator;
    public CameraFollow cf;

    [Header("Camera")]
    public Transform camPos;
    public Transform executePos;
    public Transform splashPos;

    [Space]
    public Image triangleIcon;
    private int prepare = Animator.StringToHash("Execute_Prepare");
    private int slash = Animator.StringToHash("Execute_Slash");
    //private int sheath = Animator.StringToHash("Execute_Sheath");

    [SerializeField] bool prepared = false;

    private void Update()
    {
        if (Input.GetButtonDown("Heavy Attack") && prepared)
        {
            StartCoroutine(Slash());
        }
    }

    IEnumerator PrepareFinisher()
    {
        animator.Play(prepare);
        CameraTransition(executePos);
        triangleIcon.enabled = false;
        yield return new WaitForSeconds(2.1f);
        triangleIcon.enabled = true;
        prepared = true;
    }

    IEnumerator Slash()
    {
        cf.SendMessage("ToggleLookAt");
        animator.Play(slash);
        //enemy.GetComponent<BoxCollider>().enabled = false;

        Vector3 origin = transform.position;
        Vector3 between = (enemy.transform.position - origin);
        Vector3 destination = between * 5f;
        float t = 0;
        while (t < 0.4f)
        {
            transform.position = Vector3.Lerp(origin, destination, t);
            t += Time.deltaTime;
        }

        yield return new WaitForSeconds(1f);

        Sheath();
    }

    void Sheath()
    {
        cf.SendMessage("ToggleLookAt");
        //animator.Play(sheath);
        CameraTransition(splashPos);
    }

    void CameraTransition(Transform target)
    {
        Vector3 origin = camPos.position;
        Vector3 destination = target.position;

        float t = 0f;
        while (t < 2f)
        {
            Vector3 smoothedPosition = Vector3.Lerp(camPos.transform.position, destination, 1f * Time.deltaTime);
            camPos.transform.position = smoothedPosition;
            t += Time.deltaTime;
        }

    }
}
