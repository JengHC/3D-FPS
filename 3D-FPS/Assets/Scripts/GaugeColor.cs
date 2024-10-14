using UnityEngine;
using UnityEngine.UI;

public class GaugeColor : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Image image = GetComponent<Image>();

        // fillamount값에 따라서 색이 바뀌게 됨
        image.color = Color.HSVToRGB(image.fillAmount/3 ,1.0f, 1.0f);
    }
}
