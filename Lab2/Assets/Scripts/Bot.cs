using System.Collections;
using System.Linq;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public Transform[] others;

    public bool isCatch;

    private State currentState;

    [SerializeField] private Material catchMaterial;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private Material protectionMaterial;

    private State[] states = new State[3];
    private Vector3 movementVector;
    public bool isProtected;

    // Start is called before the first frame update
    void Start()
    {
        states[0] = new TargetState();
        states[1] = new CatchState();
        states[2] = new WanderState();
        StartCoroutine("ChangeState");
    }

    private IEnumerator ChangeState()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);


            Transform catcher = null;
            if (isCatch)
            {
                currentState = states[1];
                transform.GetComponent<MeshRenderer>().material = catchMaterial;
            }
            else
            {
                catcher = others.FirstOrDefault(
                    t => t.GetComponent<Bot>() != null && t.GetComponent<Bot>().isCatch ||
                         t.GetComponent<Player>() != null && t.GetComponent<Player>().isCatch);

                if (Vector3.Distance(catcher.position, transform.position) >= 7f)
                {
                    currentState = states[2];
                }
                else
                {
                    currentState = states[0];
                }
            }

            movementVector = currentState.CalculateMovementVector(others, isCatch ? transform : catcher, transform);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var botComponent = other.transform.GetComponent<Bot>();
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
        else
        {
            var playerComp = other.transform.GetComponent<Player>();
            if (playerComp != null)
            {
                if (isCatch)
                {
                    if (playerComp.isProtected)
                    {
                        return;
                    }

                    isCatch = false;
                    playerComp.isCatch = true;
                    SetProtection();
                    transform.GetComponent<MeshRenderer>().material = protectionMaterial;
                    playerComp.GetComponent<MeshRenderer>().material = catchMaterial;
                }
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
        transform.GetComponent<MeshRenderer>().material = targetMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movementVector * Time.deltaTime * (isCatch ? 1.3f : 1f));
    }
}