using TMPro;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private InputManager inputManager;

    
    private GameObject pinObjects;



    
    

    private void IncrementScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }

}
