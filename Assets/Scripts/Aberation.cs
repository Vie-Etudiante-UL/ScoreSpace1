using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class Aberation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private readonly UnityEvent quandAberationEstGrande = new UnityEvent();
    private readonly UnityEvent quandAberationEstMort = new UnityEvent();


    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnityEvent[] LancerAnim()
    {
        gameObject.SetActive(true);
        
        quandAberationEstGrande.RemoveAllListeners();
        quandAberationEstMort.RemoveAllListeners();
        
        animator.Play("CroissanceChaire");
        return new[] {quandAberationEstGrande, quandAberationEstMort};
    }

    private void AberationEstGrande()
    {
        quandAberationEstGrande.Invoke();
        print("Aberation est grande");
    }

    private void AberationMeurt()
    {
        quandAberationEstMort.Invoke();
        gameObject.SetActive(false);
    }
}
