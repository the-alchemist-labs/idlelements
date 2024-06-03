using UnityEngine;
using TMPro;

public class ChangeTokenLabel : MonoBehaviour
{
    public TMP_Text tokensText;

    public void UpdateTokenLabel(int tokensGained)
    {
        tokensText.text = $"X{tokensGained}";
    }
}
