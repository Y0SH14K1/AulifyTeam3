using UnityEngine;

public class UIController : MonoBehaviour
{
    public void OnButtonClick()
    {
        Debug.Log("Boton presionado");
    }

    public void OnSliderValueChanged(float value)
    {
        Debug.Log("Valor actual: " + value);
    }
}
