using UnityEngine;

public class MoveInput : MonoBehaviour
{
    //Speed for the Paddle, 10 seems like good speed
    [SerializeField]
    private float speed = 10f;

    private Vector3 position;
    //pInput = Paddle/Player Input
    private PaddleMove pInput;
    [SerializeField]
    private Vector3 direction, moveDirection;

    private void Start()
    {
        position = transform.position;
        pInput = new PaddleMove();
        //Enable the Movement Script
        pInput.Enable();
    }

    private void Update()
    {
        //Reads Player's Input
        direction = new Vector3(pInput.Movement.Move.ReadValue<Vector2>().x,
                                0,
                                0);

        //Normalize and Multiply the Direction of the Paddle
        moveDirection = direction * Time.deltaTime * speed;
        //Moves the Paddle
        transform.position += moveDirection;
    }
}
