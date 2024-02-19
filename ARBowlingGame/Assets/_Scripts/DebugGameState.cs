using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugGameState : MonoBehaviour
{

    public TextMeshProUGUI debugText;
    public GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = gameState.CurrentGameState.ToString();
    }
}
