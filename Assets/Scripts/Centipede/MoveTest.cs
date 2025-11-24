using UnityEngine;

public class MoveTest : MonoBehaviour
{
    //Speed for the Paddle, 10 seems like good speed
    [SerializeField]
    private float speed = 15f;

    //pInput = Paddle/Player Input
    private PlayerMove pInput;
    [SerializeField]
    private float directionx, directiony ,moveDirectionx, moveDirectiony;

    [SerializeField]
    private GameObject projectilePrefab;

    private float cooldown = 0.2f;

    private void OnEnable()
    {
        pInput = new PlayerMove();
        //Enable the Movement Script
        pInput.Enable();
    }

    private void OnDisable()
    {
        pInput.Disable();
    }

    private void Update()
    {
        //Reads Player's Input
        directionx = pInput.Movement.FullMove.ReadValue<Vector2>().x;
        directiony = pInput.Movement.FullMove.ReadValue<Vector2>().y;
        //Normalize and Multiply the Direction of the Player
        moveDirectionx = directionx * Time.deltaTime * speed;
        moveDirectiony = directiony * Time.deltaTime * speed;
        //Moves the Player
        transform.position += new Vector3(moveDirectionx, 0, moveDirectiony);

        if(cooldown > 0)
            cooldown -= Time.deltaTime;

        if (pInput.Movement.Play.IsPressed() && cooldown <= 0)
        {
            cooldown = 0.2f;
            Instantiate(projectilePrefab, (transform.position + new Vector3(0, 0, 1f)), Quaternion.identity);
        }
    }
    }
