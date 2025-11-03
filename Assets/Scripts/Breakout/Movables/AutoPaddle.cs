using UnityEngine;

public class AutoPaddle : MonoBehaviour
{
    private Transform Ball;

    void Start()
    {
        Ball = FindAnyObjectByType<BallMove>().gameObject.transform;
    }

    void Update()
    {
        Vector3 newpos = transform.position;
        
        if (Ball.position.x - newpos.x > 0.75)
            newpos.x = Ball.position.x - 0.75f;

        else if (Ball.position.x - newpos.x < -0.75)
            newpos.x = Ball.position.x + 0.75f;
        
        transform.position = new Vector3(Mathf.Clamp(newpos.x, -7.5f, 7.5f), newpos.y, newpos.z); ;
    }
}
