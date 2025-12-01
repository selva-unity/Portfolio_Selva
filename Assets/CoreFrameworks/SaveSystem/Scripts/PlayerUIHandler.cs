using TMPro;
using UnityEngine;


public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private PlayerHandler playerHandler;
    public TextMeshPro positionText;

    void OnEnable()
    {
        playerHandler.OnPlayerPositionChanged.AddListener(UpdatePositionUI);
    }
    void OnDisable()
    {
        playerHandler.OnPlayerPositionChanged.RemoveListener(UpdatePositionUI);
    }

    private void UpdatePositionUI(Vector3 newPosition)
    {
        positionText.text = $"Position: {newPosition.x:F2}, {newPosition.y:F2}, {newPosition.z:F2}";
    }
}
