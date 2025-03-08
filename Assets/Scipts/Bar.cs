using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] Slider slider;
    
    public void UpdateBar(float max, float cur)
    {
        slider.value = cur/max;
    }
}
