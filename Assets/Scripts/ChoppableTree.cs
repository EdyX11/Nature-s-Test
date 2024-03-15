using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;


    public Animator animator;

    public float caloriesSpentCuttingTrees = 20;


   private void Start()
    {

        treeHealth = treeMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerInRange = false;
        }


    }
    public void GetHitTree()
    {
        animator.SetTrigger("shake");


        treeHealth -= 1;
        PlayerState.Instance.currentCalories -= caloriesSpentCuttingTrees;

        if (treeHealth <= 0)
        {
            CutTree();


        }

        StartCoroutine(hit());

    }
    public IEnumerator hit()
    {
        yield return new WaitForSeconds(0.6f);
        
    }

    void CutTree()
    {
        Vector3 treePosition = transform.position;// save postion to spawn the logs where the tree was cut 
        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),
            new Vector3(treePosition.x, treePosition.y+1, treePosition.z), Quaternion.Euler(0, 0, 0));





    }
    private void Update()
    {
        // Check if GlobalState.Instance is not null before accessing its members
        if (GlobalState.Instance != null)
        {
            if (canBeChopped)
            {
                GlobalState.Instance.resourceHealth = treeHealth;
                GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
            }
        }
        else
        {
            Debug.LogWarning("GlobalState.Instance is null in ChoppableTree.Update");
        }
    }


}
