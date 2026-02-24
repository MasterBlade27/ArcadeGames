using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class DigMovement : MonoBehaviour
{
    //Speed for the Paddle, 10 seems like good speed
    [SerializeField]
    private float speed = 15f;

    //pInput = Paddle/Player Input
    private PlayerMove pInput;
    [SerializeField]
    private Vector3 POS;
    [SerializeField]
    private float directionx, directiony;
    [SerializeField]
    private bool XFloat, YFloat;

    [SerializeField]
    private AudioController AC;

    private void OnEnable()
    {
        pInput = new PlayerMove();
        pInput.Enable();

        StartCoroutine(Movement());
    }

    private void OnDisable()
    {
        pInput.Disable();
    }

    private void Update()
    {
        XFloat = IsWhole(transform.position.x);
        YFloat = IsWhole(transform.position.z);

//        if (!XFloat || !YFloat)
  //          PosCor();
    }

    private IEnumerator Movement()
    {
        while (true)
        {
            Vector3 Normals = Vector3.zero;

            //Reads Player's Input
            directionx = pInput.Movement.FullMove.ReadValue<Vector2>().x;
            directiony = pInput.Movement.FullMove.ReadValue<Vector2>().y;

            //Normalize and Multiply the Direction of the Player
            if (directionx != 0 && YFloat)
            {
                Debug.Log("X MOVE");
                if (directionx > 0)
                    MOVERIGHT();
                else if (directionx < 0)
                    MOVELEFT();
            }

            else if (directiony != 0 && XFloat)
            {
                Debug.Log("Y MOVE");
                if (directiony > 0)
                    MOVEUP();

                else if (directiony < 0)
                    MOVEDOWN();
            }

            else if (directionx != 0 && !YFloat)
                ChangeAxX();

            else if (directiony != 0 && !XFloat)
                ChangeAxY();

            yield return new WaitForSeconds(0.15f);
        }
    }

    private void MOVERIGHT()
    {
        transform.position += Vector3.right * 2f;
    }

    private void MOVELEFT()
    {
        transform.position += Vector3.left * 2f;
    }

    private void MOVEUP()
    {
        transform.position += Vector3.forward * 2f;
    }

    private void MOVEDOWN()
    {
        transform.position += Vector3.back * 2f;
    }

    private bool IsWhole(float F)
    {
        return F % 10 == 0;
    }

    private void ChangeAxY()
    {
        POS = transform.position;

        float ModX = POS.x % 10;

        if (ModX > 5 || ModX < -5)
        {
            MOVERIGHT();
        }

        else if (ModX > -5 || ModX < 5)
        {
            MOVELEFT();
        }
    }

    private void ChangeAxX()
    {
        POS = transform.position;

        float ModY = POS.z % 10;

        if (ModY > 5 || ModY < -5)
        {
            MOVEUP();
        }

        else if (ModY > -5 || ModY < 5)
        {
            MOVEDOWN();
        }
    }
}
