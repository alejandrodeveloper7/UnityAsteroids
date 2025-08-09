using TMPro;
using UnityEngine;
using static ToolsACG.ApiCaller.DreamloLeaderboardApiCaller.DreamloLeaderboardApiCaller;

public class LeaderboardRowHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _positionText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void SetData(int pPosition, LeaderboardEntry pData, Color pPlayerColor)
    {
        _positionText.text = string.Concat(pPosition, "#");
        _nameText.text = pData.Name;
        _scoreText.text = pData.Score.ToString();

        bool isYourScore = (pData.Name == PersistentDataManager.UserName);

        if (isYourScore)
            TintTexts(pPlayerColor);
        else
            TintTexts(Color.black);
    }

    public void Restart() 
    {
        _positionText.text = "-";
        _nameText.text = "---------";
        _scoreText.text = "----";
        TintTexts(Color.black);
    }

    private void TintTexts(Color pColor)
    {
        _positionText.color = pColor;
        _nameText.color = pColor;
        _scoreText.color = pColor;
    }
}
