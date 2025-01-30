using TMPro;
using UnityEngine;

public class LeaderboardRowHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _positionText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void SetData(int pPosition, string pName, int pScore, bool pIsYourScore)
    {
        _positionText.text = string.Concat(pPosition, "#");
        _nameText.text = pName;
        _scoreText.text = pScore.ToString();

        if (pIsYourScore)
            TintTexts(Color.red);
        else
            TintTexts(Color.black);
    }

    private void TintTexts(Color pColor)
    {
        _positionText.color = pColor;
        _nameText.color = pColor;
        _scoreText.color = pColor;
    }
}
