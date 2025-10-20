using System;
using System.Collections;
using UnityEngine;
public class ArmorBlock : MonoBehaviour
{
    [SerializeField]
    private bool Demo;

    //ArmorLvel = Armor Level
    [SerializeField]
    private int ArmorLvl = 1;

    private int ArmorSet = 0;

    //MatArr = Material Array
    [SerializeField]
    private Material[] MatsArr;

    private Material mat;

    [SerializeField]
    private Animator anim;

    public static event Action blockDel;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        ArmorSet = ArmorLvl - 1;

        //Will set Armor Level as MatArr's Length in case unset
        if (ArmorLvl == 0)
        {
            if (MatsArr != null)
                ArmorLvl = MatsArr.Length;
            //If Armor Level is unset and no Materials in MatArr, Send Warning
            else
                Debug.LogWarning("No Material Set");
        }

        //Starting Material for the block is the highest in the heirarchy
        gameObject.GetComponent<MeshRenderer>().material = MatsArr[ArmorSet];
        mat = gameObject.GetComponent<MeshRenderer>().material;
    }

    //Custom Method used for calling from BallMove script
    // Now accepts the impact velocity so the block can pass it down to shatter logic
    public void ArmorDestroy(Vector3 impactVelocity)
    {
        if(anim != null)
        {
            //If the Armor Set is not the last one
            if (ArmorSet != 0)
                //Plays the Hit Animation
                anim.SetTrigger("hit");
        }

        //Adds a level to Armor Set
        ArmorSet--;
        Debug.Log("AD");
        //If Armor Set and Armor Level are the same
        if(ArmorSet == -1)
            //Calls DeleteBlock because the block would be destroyed
            DeleteBlock(impactVelocity);

        //Changes the Material to the next one in the set (only if valid index)
        if (ArmorSet >= 0 && MatsArr != null && ArmorSet < MatsArr.Length)
            gameObject.GetComponent<MeshRenderer>().material = MatsArr[ArmorSet];
    }

    private Coroutine ShatterCoroutine;

    //Custom Method used for Deleting Blocks
    // Now receives impact velocity and forwards it to the shatter coroutine
    public void DeleteBlock(Vector3 impactVelocity)
    {
        if (anim != null)
        {
            anim.SetTrigger("break");
        }

        //Reset Armor Set Variable
        ArmorSet = 0;

        //Calls the Block's Respawn script and lowers the Block Active Count
        BlockRespawn BR = gameObject.GetComponentInParent<BlockRespawn>();
        BR.BlockActive--;
        BR.CheckMusic();


        if (!Demo)
        {
            //Calls the Block's Scoring script and sets the score of the blocks
            Scoring SC = gameObject.GetComponentInParent<Scoring>();
            SC.BlockScore(mat);

            if (FindAnyObjectByType<PowerUpSpawn>() != null)
            {
                //Calls the PowerUpSpawn script to attempt to spawn a powerup
                PowerUpSpawn PUS = gameObject.GetComponentInParent<PowerUpSpawn>();
                PUS.SpawnPowerUp();
            }
        }

        blockDel();

        if(gameObject.GetComponent<ShardFling>() != null)
            ShatterCoroutine = StartCoroutine(ShatterTimer(impactVelocity));
        else
            gameObject.SetActive(false);

        /*if ( anim == null )
            //"Deletes" the block
            gameObject.SetActive(false);*/
    }

    private IEnumerator ShatterTimer(Vector3 impactVelocity)
    {
        ShardFling SF = gameObject.GetComponent<ShardFling>();
        SF.Shatter(impactVelocity);
        BoxCollider fuck = gameObject.GetComponent<BoxCollider>();
        fuck.enabled = false;
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }

}
