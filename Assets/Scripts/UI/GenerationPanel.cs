using UnityEngine;
using UnityEngine.UI;

public class GenerationPanel : MonoBehaviour
{
    public Text generationNumberText;

    public void SetGenerationNumber(int gen)
    {
        this.generationNumberText.text = gen.ToString();
    }
}
