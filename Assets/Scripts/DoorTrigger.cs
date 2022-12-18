using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]private Animator animator;
    // Start is called before the first frame update
    [SerializeField] private UnityEvent onDoorOpen;

    private bool isClosed = true;
    void Start()
    {
        if(animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider thisCollider = GetComponent<Collider>();
        thisCollider.enabled = false;
        animator.SetTrigger("Open");
        if(onDoorOpen != null)
        {
            onDoorOpen.Invoke();
        }
        isClosed = false;
    }

    public void ToggleDoor()
    {
        if (isClosed)
        {
            animator.SetTrigger("Open");
            if (onDoorOpen != null)
            {
                onDoorOpen.Invoke();
            }
            isClosed = false;
        }
        else
        {
            animator.SetTrigger("Close");
            isClosed = true;
        }
    }
}
