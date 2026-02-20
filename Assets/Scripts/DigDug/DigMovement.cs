using System.Collections;
using UnityEngine;

public class DigMovement : MonoBehaviour
{
    //Speed for the Paddle, 10 seems like good speed
    [SerializeField]
    private float speed = 15f;

    //pInput = Paddle/Player Input
    private PlayerMove pInput;
    [SerializeField]
    private float directionx, directiony, moveDirectionx, moveDirectiony;
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
                moveDirectionx = directionx * Time.deltaTime * speed;
                Normals = new Vector3(moveDirectionx, 0, 0);
            }

            else if (directiony != 0 && XFloat)
            {
                moveDirectiony = directiony * Time.deltaTime * speed;
                Normals = new Vector3(0, 0, moveDirectiony);
            }

            Normals = Normals.normalized / 5f;
            Debug.Log(Normals);
            transform.position += Normals;

            yield return new WaitForSeconds(0.15f);
        }
    }

    private bool IsWhole(float F)
    {
        Debug.Log(F + " - " + Mathf.Round(F));
        return Mathf.Approximately(F, Mathf.Round(F));
    }

    private void PosCor()
    {
        Vector3 POS = transform.position;
//        POS.x = 
    }
}
