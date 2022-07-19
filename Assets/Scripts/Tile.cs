using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public static Tile Instance;
    //TILE
    [SerializeField] private MeshRenderer _tileMeshRenderer;
    [Space]
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color[] _setupColors;
    [Space]    
    public TextMeshPro _hitText;
    public DeckSphere SphereDeckShip;

    public int ColorIndex = 0;
    private float _time =0;
    private float _timeDelay = 10f;

    public MeshRenderer MeshRenderer => _tileMeshRenderer;
    public Color BaseColor => _baseColor;
    public Color[] SetColorIndex => _setupColors;


    private void Awake() => Instance = this;
    private void Start()
    {
        if (Tile.Instance.GetComponentInParent<GenerateTileMap>() != null)
        {
            _hitText = GetComponentInChildren<TextMeshPro>();
            SphereDeckShip = GetComponentInChildren<DeckSphere>();
            _hitText.gameObject.SetActive(false);            
            SphereDeckShip.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void Update()
    {
        HitTextHide();
    }

    //Показ всплываюшего текстка при нажатие на тайл
    private void HitTextHide()
    {
        _time += Time.deltaTime;
        if (_time >= _timeDelay)
        {
            if (_hitText != null) _hitText.gameObject.SetActive(false);
            _time = 0;
        }
    }
}

