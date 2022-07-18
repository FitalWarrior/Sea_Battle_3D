using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GenerateTileMap _playerMap, _enemyMap;

    public static TurnManager Instance;
    public bool WhoTurn => whoTurn;
    private bool whoTurn = true;

    private void Awake()
    {
        Instance = this;
    }
    // Ход Бота
    public void EnemyTurn()
    {
        if (whoTurn == false)
        {
            int RandomX = Random.Range(0, 10);
            int RandomZ = Random.Range(0, 10);

            whoTurn = !_playerMap.Shoot(RandomX, RandomZ); 
        }

    }

    // Клик игрока
    public void PlayerClick(int X, int Z)
    {
        if (whoTurn == true)
        {
            if (_enemyMap.EnemyMode == true) { whoTurn = _enemyMap.Shoot(X, Z); }
        }

        Debug.Log("X=" + X + "_" + "Z=" + Z);
    }
}
