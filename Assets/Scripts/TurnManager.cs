using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GenerateTileMap _playerMap, _enemyMap;        

    public static TurnManager Instance;
    public bool WhoTurn => whoTurn;
    private bool whoTurn = true;
    private int shootDelay;

    private void Awake()
    {
        Instance = this;
    }

    /*Проверка нахождение палуб */
    GenerateTileMap.SetCordinate GetCordinate()
    {
        GenerateTileMap.SetCordinate cordinate;
        cordinate.X = -1;
        cordinate.Z = -1;

        foreach (GenerateTileMap.Ship ship in _playerMap.GetComponent<GenerateTileMap>()._shipsList)
        {
            foreach (GenerateTileMap.SetCordinate pointDeck in ship.ShipCordinates)
            {
                int DeckCordinate = _playerMap.GetComponent<GenerateTileMap>().GetDeckCordinate(pointDeck.X, pointDeck.Z);
                if (DeckCordinate==1)
                {
                    return pointDeck;
                }
            }
        }

        return cordinate;
    }

    // Ход Бота
    public void EnemyTurn()
    {
        if (whoTurn == false)
        {
            int RandomX = Random.Range(0, 10);
            int RandomZ = Random.Range(0, 10);

            int PlayerDeckCount = _enemyMap.GetComponent<GenerateTileMap>().CheckLifeShips();
            if (PlayerDeckCount < Random.Range(4,10))
            {
                if (shootDelay == 0)
                {
                    GenerateTileMap.SetCordinate cordinate = GetCordinate();

                    if (cordinate.X >= 0 && cordinate.Z >= 0)
                    {
                        RandomX = cordinate.X;
                        RandomZ = cordinate.Z;
                    }
                    shootDelay++;
                }
                else 
                {
                    shootDelay = 0;
                }
            }

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
