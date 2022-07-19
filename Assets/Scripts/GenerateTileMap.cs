using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GenerateTileMap : MonoBehaviour
{
   [SerializeField] private Tile _tilePrefab;
   [SerializeField] private MessageShow _message;
   [Space]
   [SerializeField] private int _gridLenght = 10;   
   [SerializeField] private bool _enemyMode = false;
    

    public bool EnemyMode => _enemyMode;

    private Tile[,] _mapArea;

    private int[] ShipCount = { 0, 4, 3, 2, 1 };

    public readonly List<Ship> _shipsList = new List<Ship>();    
    //--------//    


    public struct SetCordinate 
    {
        public int X, Z;
    }

    public struct Ship 
    {
        public SetCordinate[] ShipCordinates;
    }

    private void Start()
    {        
        GenerateGridMap();        
        if (_enemyMode)
        {
            RandomSetShip();
            _message.LifeMessage();
        }
    
    }

    private void Update()
    {
        PlayerInput();

        /*Дебагинг проверки кол-во палуб*/
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(CheckLifeShips());
            
        }

        // Ходит враг бот
        TurnManager.Instance.EnemyTurn();        
    }

    // Чек ввода
    private void PlayerInput()
    {
        // для дебагинга
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // генерация сеточной карты в плоскостях X,Z
    private void GenerateGridMap() 
    {        

        Vector3 StartPos = transform.position;

        float tileX = StartPos.x + 1;
        float tileZ = StartPos.z - 1;


        tileX = StartPos.x + 1;
        tileZ = StartPos.z - 1;

        _mapArea = new Tile[_gridLenght, _gridLenght];

        for (int Z = 0; Z < _gridLenght; Z++)
        {
            for (int X = 0; X < _gridLenght; X++)
            {
                _mapArea[X, Z] = Instantiate(_tilePrefab,transform);                
                _mapArea[X, Z].transform.position = new Vector3(tileX,StartPos.y,tileZ);
                
                _mapArea[X, Z].GetComponent<PointClick>().PointX = X;
                _mapArea[X, Z].GetComponent<PointClick>().PointZ = Z;
                
                tileX++;
                
            }
            tileX = StartPos.x + 1;
            tileZ--;
        }
    }

    private bool CheckShipDistance(int X, int Z) 
    {
        if ((X>-1)&& (Z>-1) && (X<10)&&(Z<10))
        {
            int[] XPoint = new int[9],
                ZPoint = new int[9];
            //
            XPoint[0] = X + 1; ZPoint[0] = Z + 1;
            XPoint[1] = X; ZPoint[1] = Z + 1;
            XPoint[2] = X-1; ZPoint[2] = Z + 1;
            //
            XPoint[3] = X + 1; ZPoint[3] = Z;
            XPoint[4] = X; ZPoint[4] = Z;
            XPoint[5] = X-1; ZPoint[5] = Z;
            //
            XPoint[6] = X + 1; ZPoint[6] = Z-1;
            XPoint[7] = X; ZPoint[7] = Z - 1;
            XPoint[8] = X - 1; ZPoint[8] = Z - 1;
            //
            for (int i = 0; i < 9; i++)
            {
                if ((XPoint[i]>-1) && (ZPoint[i]>-1) && (XPoint[i]<10) && (ZPoint[i] <10))
                {
                    if (_mapArea[XPoint[i], ZPoint[i]].GetComponent<Tile>().MeshRenderer.material.color == Tile.Instance.SetColorIndex[1]) return false; 
                }
            }

            return true;
        }
        return false;
    }

    //вариация направление кораблей
    SetCordinate[] SetShipDirection(int ShipType, int XP, int ZP, int X, int Z) 
    {
        SetCordinate[] cordinates = new SetCordinate[ShipType];
        for (int P = 0; P < ShipType; P++)
        {
            if (CheckShipDistance(X, Z))
            {
                cordinates[P].X = X;
                cordinates[P].Z = Z;
            }
            else return null;

            X += XP;
            Z += ZP;
        }
        return cordinates;

    }

    //вариации кораблей
    SetCordinate[] SetShipVariant(int ShipType , int Direction,int X,int Z) 
    {
        SetCordinate[] cordinates = new SetCordinate[ShipType];

        if (CheckShipDistance(X,Z))
        {
            switch (Direction)
            {
                case 0:
                    cordinates = SetShipDirection(ShipType, 1, 0, X, Z);
                    if (cordinates == null) cordinates = SetShipDirection(ShipType, -1, 0, X, Z);
                    break;
                case 1:
                    cordinates = SetShipDirection(ShipType, 0, 1, X, Z);
                    if (cordinates == null) cordinates = SetShipDirection(ShipType, 0, -1, X, Z);
                    break;
            }
            return cordinates;
        }
        return null;
    }

    //установка самих палуб
    private bool CheckSetShip(int ShipType,int Direction,int X,int Z) 
    {
        SetCordinate[] cordinates = SetShipVariant(ShipType, Direction, X, Z);
        
        if (cordinates != null)
        {
            foreach (SetCordinate cordinate in cordinates)
            {                
                _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().MeshRenderer.material.color = Tile.Instance.SetColorIndex[1];
                _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().ColorIndex = 1;                
                _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().enabled = true;
                _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().material.color = Tile.Instance.SetColorIndex[1];

                if (_enemyMode == true && _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().ColorIndex == 1)
                {
                    _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().MeshRenderer.material.color = Tile.Instance.SetColorIndex[0];                    
                }
                else
                {
                    _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().MeshRenderer.material.color = Tile.Instance.SetColorIndex[1];                    
                    _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().enabled = true;
                    _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().material.color = Tile.Instance.SetColorIndex[1];
                }

            }

            Ship ShipTamplate;
            ShipTamplate.ShipCordinates = cordinates;
            _shipsList.Add(ShipTamplate);
            return true;
        }
        return false;
    }

    // вычисление кол-во палуб 
    private bool CalcCountShip() 
    {
        int Count = 0;

        foreach (int Ship in ShipCount) Count += Ship;
        if (Count != 0) return true;

        return false;
    }

    //проверка клика через bool shoot
    public bool Shoot(int X,int Z) 
    {
        int TileIndex = _mapArea[X, Z].GetComponent<Tile>().ColorIndex;
        bool Result = false;
        

        switch (TileIndex)
        {
            case 0:
                /*Промах*/
                _mapArea[X, Z].GetComponent<Tile>().ColorIndex = 2;
                _mapArea[X,Z].GetComponent<Tile>().MeshRenderer.material.color = Tile.Instance.SetColorIndex[2];                
                _mapArea[X, Z].GetComponent<Tile>()._hitText.gameObject.SetActive(true);
                _mapArea[X, Z].GetComponent<Tile>()._hitText.text = "Промах";
                _mapArea[X, Z].GetComponent<Tile>()._hitText.color = Tile.Instance.SetColorIndex[2];
                _mapArea[X, Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().enabled = false;
                _message.LifeMessage();
                _message.GameStatusOver();
                /*Промах*/
                Result = false;
                break;

            case 1:
                _mapArea[X, Z].GetComponent<Tile>().ColorIndex = 3;
                _mapArea[X, Z].GetComponent<Tile>().MeshRenderer.material.color = Tile.Instance.SetColorIndex[3];
                _message.LifeMessage();
                _message.GameStatusOver();
                Result = true;

                if (CheckShoot(X,Z))
                {   
                    _mapArea[X, Z].GetComponent<Tile>()._hitText.gameObject.SetActive(true);
                    _mapArea[X, Z].GetComponent<Tile>()._hitText.text = "Убит";
                    _mapArea[X, Z].GetComponent<Tile>()._hitText.color = Tile.Instance.SetColorIndex[1];                    
                    _mapArea[X, Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().enabled = true;
                    _mapArea[X, Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().material.color = Tile.Instance.SetColorIndex[0];
                    _message.LifeMessage();
                    _message.GameStatusOver();
                }
                else
                {                    
                    _mapArea[X, Z].GetComponent<Tile>()._hitText.gameObject.SetActive(true);
                    _mapArea[X, Z].GetComponent<Tile>()._hitText.text = "Попал";
                    _mapArea[X, Z].GetComponent<Tile>()._hitText.color = Tile.Instance.SetColorIndex[3];                    
                    _mapArea[X, Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().enabled = true;
                    _mapArea[X,Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().material.color = Tile.Instance.SetColorIndex[3];
                    _message.LifeMessage();
                    _message.GameStatusOver();
                }
                break;

        }

        return Result;        
    }

    //проверка можемли стрелять
    private bool CheckShoot(int X, int Z) 
    {
        bool Result = false;
        foreach (Ship Ships in _shipsList)
        {
            foreach (SetCordinate cordinate in Ships.ShipCordinates )
            {
                if (cordinate.X==X && cordinate.Z == Z)
                {
                    int CountKillShipDesk = 0;
                    foreach (SetCordinate killedShipCordinate in Ships.ShipCordinates)
                    {
                        int ShipDeskIndex = _mapArea[killedShipCordinate.X, killedShipCordinate.Z].GetComponent<Tile>().ColorIndex;
                        if (ShipDeskIndex == 3) CountKillShipDesk++;
                    }
                    if (CountKillShipDesk == Ships.ShipCordinates.Length) 
                        Result = true;
                    else 
                        Result = false;

                    return Result;
                }                
            }
        }
        return Result; 
    }

    public int GetDeckCordinate(int X,int Z) 
    {
        return _mapArea[X, Z].GetComponent<Tile>().ColorIndex;
    }

    //проверка кол-во палуб
    public int CheckLifeShips() 
    {
        int lifeShipCount = 0;

        foreach (Ship Ship in _shipsList)
        {
            foreach (SetCordinate cordinate in Ship.ShipCordinates)
            {
                int ShipDeskIndex = _mapArea[cordinate.X, cordinate.Z].GetComponent<Tile>().ColorIndex;
                if (ShipDeskIndex == 1) lifeShipCount++;
            }
        }

        return lifeShipCount;
    }

    //очистка арены
    public void CleareMap()     
    {
        ShipCount = new int[] { 0, 4, 3, 2, 1 };
        _shipsList.Clear();
        for (int Z = 0; Z < _gridLenght; Z++)
        {
            for (int X = 0; X < _gridLenght; X++)
            {                
                _mapArea[X,Z].GetComponent<Tile>().MeshRenderer.material.color = Tile.Instance.SetColorIndex[0];
                _mapArea[X, Z].GetComponent<Tile>().ColorIndex = 0;                              
                _mapArea[X, Z].GetComponent<Tile>().SphereDeckShip.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    // рандомноая установка палуб
    public void RandomSetShip()
    {
        CleareMap();        

        int ChooseShip = 4;
        int X, Z ,Direction;

        while (CalcCountShip())
        {
            X = Random.Range(0, 10);
            Z = Random.Range(0, 10);
            Direction = Random.Range(0, 2);

            if (CheckSetShip(ChooseShip, Direction, X, Z))             
            {
                ShipCount[ChooseShip]--;
                if (ShipCount[ChooseShip] == 0) 
                {
                    ChooseShip--;
                }
            }
        }
    }

    //событие клика исходя из хода 
    public void ClickEvent(int X, int Z)     
    {
        if (_mapArea[X, Z].GetComponent<Tile>().MeshRenderer.material.color == Tile.Instance.SetColorIndex[0] && _enemyMode==true)
        {
            TurnManager.Instance.PlayerClick(X, Z);
        }
        _message.LifeMessage();

    }

    
         
}
