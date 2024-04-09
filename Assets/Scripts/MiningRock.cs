using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(BoxCollider))]
public class MiningRock : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeMined;

    public float rockMaxHealth;
    public float rockHealth;


   // public Animator animator;

    public float caloriesSpentMiningRocks = 40;


    private void Start()
    {

        rockHealth = rockMaxHealth;
       // animator = transform.parent.transform.parent.GetComponent<Animator>();

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
       // animator.SetTrigger("shake");


        rockHealth -= 1;
        PlayerState.Instance.currentCalories -= caloriesSpentMiningRocks;

        if (rockHealth <= 0)
        {
            MineROCK();


        }

        StartCoroutine(hit());

    }
    public IEnumerator hit()
    {
        yield return new WaitForSeconds(0.6f);

    }

    void MineROCK()
    {
        Vector3 rockPosition = transform.position;// save postion to spawn the logs where the tree was cut 
        Destroy(transform.parent.transform.parent.gameObject);
        canBeMined = false;
       // SelectionManager.Instance.selectedTree = null;
       // SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenRock = Instantiate(Resources.Load<GameObject>("Ore"),
            new Vector3(rockPosition.x, rockPosition.y + 1, rockPosition.z), Quaternion.Euler(0, 0, 0));





    }
    private void Update()
    {
        // Check if GlobalState.Instance is not null before accessing its members
        if (GlobalState.Instance != null)
        {
            if (canBeMined)
            {
                GlobalState.Instance.resourceHealth = rockHealth;
                GlobalState.Instance.resourceMaxHealth = rockMaxHealth;
            }
        }
        else
        {
            Debug.LogWarning("GlobalState.Instance is null in ChoppableTree.Update");
        }
    }


}

