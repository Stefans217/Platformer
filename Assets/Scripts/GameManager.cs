using TMPro;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject coinCollection;


    private GameObject coinList;
    private CoinCollector coinCollector;

    private void Awake()
    {
        coinCollector = FindFirstObjectByType<CoinCollector>();
        if (coinCollector != null)
        {
            coinCollector.OnCoinCollected.AddListener(IncrementScore);
        }
    }

    private void IncrementScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }

}
