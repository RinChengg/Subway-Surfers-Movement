using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CoinCollection : MonoBehaviour
{
    private GameController _gameManager;
    private TextMeshProUGUI _coinText;

    public UnityEvent OnCollected;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameController>();
        _coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterMovement player))
        {
            // play SFX or VFX
            OnCollected.Invoke();

            // increase the number
            _gameManager.Coin++;

            // update the UI text
            _coinText.text = _gameManager.Coin.ToString();

            // destroyit
            Destroy(gameObject);
        }
    }
}
