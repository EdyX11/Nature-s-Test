using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]

public class EquipableItem : MonoBehaviour
{

    private Animator animator;
    private bool isHitting = false; 


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()


    {

        //left click
        if (Input.GetMouseButtonDown(0) &&
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false &&
            SelectionManager.Instance.handIsVisible == false &&
            !isHitting

            )

        {
            isHitting = true;
            animator.SetTrigger("hit");
        }
    }
       public  void GetHitOnce()
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
    public void ResetHit()
    {
        isHitting = false;
    }

}
