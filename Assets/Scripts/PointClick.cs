using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointClick : MonoBehaviour
{
    [SerializeField] private GenerateTileMap _tileMapParrent = null;
    [SerializeField] private MessageShow _message;

    public GameObject TileMapParrent => _tileMapParrent.gameObject;

    private GenerateTileMap tileMap;
    public int PointX {get;set;}
    public int PointZ { get; set; }

    private void Awake()
    {
        _tileMapParrent = FindObjectOfType<GenerateTileMap>();
        _message = FindObjectOfType<MessageShow>();
    }

    //клик мышкой по арене
    private void OnMouseDown()
    {
        if (_tileMapParrent != null)
        {            
           _tileMapParrent.ClickEvent(PointX,PointZ);             
            _message.GameStatusOver();           

        }
    }
}
