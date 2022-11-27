using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;


    [SerializeField] private Material catchMaterial;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Material protectionMaterial;
    private Vector3 velocity;
    private Transform mainCamera;
    public bool isCatch;
    public bool isProtected;

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 rotF = mainCamera.forward.normalized;
        Vector3 rotR = mainCamera.right.normalized;
        rotF.y = 0f;
        rotR.y = 0f;

        transform.Translate((h * rotR.normalized + rotF.normalized * v).normalized * Time.deltaTime * speed *
                            (isCatch ? 1.3f : 1f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        var botComponent = collision.transform.GetComponent<Bot>();
        if (botComponent != null)
        {
            if (isCatch)
            {
                if (botComponent.isProtected)
                {
                    return;
                }

                isCatch = false;
                botComponent.isCatch = true;
                SetProtection();
                transform.GetComponent<MeshRenderer>().material = protectionMaterial;
                botComponent.GetComponent<MeshRenderer>().material = catchMaterial;
            }
        }
    }

    public void SetProtection()
    {
        isProtected = true;
        StartCoroutine("CloseProtection");
    }

    private IEnumerator CloseProtection()
    {
        yield return new WaitForSeconds(15f);
        isProtected = false;
        transform.GetComponent<MeshRenderer>().material = playerMaterial;
    }
}