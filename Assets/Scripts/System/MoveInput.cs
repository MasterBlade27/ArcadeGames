using UnityEngine;

public class MoveInput : MonoBehaviour
{
    public bool BO, CT;

    [SerializeField]
    private float speed = 15f; //Speed for the Player

    private PlayerMove pInput; //pInput = Player Input
    private Vector3 direction, moveDirection;
    public float clamping;

    public bool GameOver;

    private void OnEnable()
    {
        pInput = new PlayerMove();
        //Enable the Movement Script
        pInput.Enable();

        if (BO)
            clamping = 7.5f;
    }

    private void OnDisable()
    {
        pInput.Disable();
    }

    private void Update()
    {
        if (!GameOver)
        {
            if (FindAnyObjectByType<MouseMobile>() != null)
            {
                if (direction == Vector3.zero)
                    FindAnyObjectByType<MouseMobile>().enabled = true;
                else
                    FindAnyObjectByType<MouseMobile>().enabled = false;
            }
        }

        direction = pInput.Movement.Paddle.ReadValue<Vector2>(); //Reads Player's Input
        moveDirection = direction * Time.deltaTime * speed; //Normalize and Multiply the Direction of the Paddle

        if (BO)
        {
            transform.position += new Vector3(moveDirection.x, 0, 0); //Moves the Paddle
            transform.position = new Vector3( //Clamp the Paddle's Position
                Mathf.Clamp(transform.position.x, -clamping, clamping),
                transform.position.y,
                transform.position.z
                );
        }
    }
}
