using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]

public class EquipableItem : MonoBehaviour
{
    [Header("Conditions")]
    [SerializeField] private Animator animator;
    [SerializeField] private bool isHittingAxe = false;
   


    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
   private void Update()


    {
        HitTreeAxe();


    }

    private void HitTreeAxe()
    {

        //left click
        if (Input.GetMouseButtonDown(0) &&
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false &&
            SelectionManager.Instance.handIsVisible == false &&
            !isHittingAxe

            )

        {
            isHittingAxe = true;
            animator.SetTrigger("hit");
        }
    }



    public void GetHitOnceAxe()
    {

        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.axeHitTreeSound);// tree gets hit here
            selectedTree.GetComponent<ChoppableTree>().GetHitTree();

        }

    }




    public void PlayToolSwingSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }
    public void ResetHitAxe()
    {
        isHittingAxe = false;
    }
}
