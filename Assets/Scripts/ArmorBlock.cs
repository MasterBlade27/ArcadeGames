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
    public void ArmorDestroy()
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

        //If Armor Set and Armor Level are the same
        if(ArmorSet == -1)
            //Calls DeleteBlock because the block would be destroyed
            DeleteBlock();

        //Changes the Material to the next one in the set
        gameObject.GetComponent<MeshRenderer>().material = MatsArr[ArmorSet];
    }

    //Custom Method used for Deleting Blocks
    private void DeleteBlock()
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

        if( anim == null )
            //"Deletes" the block
            gameObject.SetActive(false);
    }

    private void AnimDelete()
    {
        gameObject.SetActive(false);
    }

}
