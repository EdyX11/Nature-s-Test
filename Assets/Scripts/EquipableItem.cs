using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]

public class EquipableItem : MonoBehaviour
{

    public Animator animator;



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
            SelectionManager.Instance.handIsVisible == false
            )

        {
            StartCoroutine(SwingSoundDelay());
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

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);

    }
    
}
