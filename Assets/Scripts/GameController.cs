using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // private
    private int _coin = 0;

    // set and get
    public int Coin { get => _coin; set => _coin = value; }
}
