using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] public Transform image;
    [SerializeField] float Rocketspeed;

    public Vector3 targetPosition;

    private void Awake()
    {
        explosion.Stop();
    }

    private void Update()
    {
        if ((this.transform.position - targetPosition).magnitude> 0.02)
        {transform.position = Vector3.Lerp(transform.position, targetPosition, Rocketspeed);
        }else{
            explosion.Play();
            StartCoroutine(Deactivate());
            image.gameObject.SetActive(false);
        }
    }

    IEnumerator Deactivate()
    {

        yield return new WaitForSeconds(0.5f);
        explosion.Stop();
        this.gameObject.SetActive(false);
        
    }
}
