using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ZoneTrigger : MonoBehaviour
{

    [SerializeField] string tagFilter;
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;


    void OnTriggerEnter(Collider other)
    {

        if (!String.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
            return;
        onTriggerEnter.Invoke();
    }

    void OnTriggerExit(Collider other)
    {

        if (!String.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
            return;
        onTriggerExit.Invoke();
    }
}
