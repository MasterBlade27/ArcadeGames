using UnityEngine;

public class TestMod : MonoBehaviour
{
    public bool BOON;
    [Range(-10f, 10f), SerializeField]
    private float Slider;
    private float Qu = 0.2f;


    private void Update()
    {
        if (BOON)
        {
            BOON = false;
            Debug.Log(Slider + " % " + Qu + " = " + Slider % Qu);

            float Slider2 = Slider * 10;
            float Qu2 = Qu * 10;
            Debug.Log(Slider2 + " % " + Qu2 + " = " + Slider2 % Qu2);
        }
    }
}
