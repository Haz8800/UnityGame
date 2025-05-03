using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Points")]
    public int points = 0;
    public int pointsForUltimate = 100;

    [Header("UI")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI ultimateReadyText;

    void Start()
    {
        UpdatePointsDisplay();
        CheckUltimateStatus();
    }

    public void AddPoints(int amount)
    {
        if (points >= pointsForUltimate)
            return;

        points += amount;

        if (points > pointsForUltimate)
            points = pointsForUltimate;

        UpdatePointsDisplay();
        CheckUltimateStatus();
    }

    public void ResetPoints()
    {
        points = 0;
        UpdatePointsDisplay();
        CheckUltimateStatus();
    }

    void UpdatePointsDisplay()
    {
        GameObject pointsObject = GameObject.Find("Canvas/HUD/ScreenSpace/MiddleLeft/HUD_Ultimate/Label_Ultimate_Current");
        if (pointsObject != null)
        {
            pointsText = pointsObject.GetComponent<TextMeshProUGUI>();
            pointsText.text = points + "%";
        }
    }

    void CheckUltimateStatus()
    {
        if (ultimateReadyText != null)
            ultimateReadyText.gameObject.SetActive(points >= pointsForUltimate);
    }

    public bool CanUseUltimate()
    {
        return points >= pointsForUltimate;
    }
}