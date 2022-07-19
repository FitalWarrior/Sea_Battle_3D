using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MessageShow : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;

    public Sprite[] Sprites => _sprites;
    [Space]
    [SerializeField] private TextMeshPro[] _hpText;
    [SerializeField] private TMP_Text _gameStatus;
    [SerializeField] private Image _gameStatusPanel;
    [SerializeField] private BoxCollider _hitBlockCollider;

    // сообщать кол-во палуб
    public void LifeMessage()
    {
        _hpText[0].text="Палуб - "+_hpText[0].GetComponentInParent<GenerateTileMap>().CheckLifeShips().ToString();
        if (_hpText[1].GetComponentInParent<GenerateTileMap>().enabled == true)
        {
            _hpText[1].text = "Палуб - " + _hpText[1].GetComponentInParent<GenerateTileMap>().CheckLifeShips().ToString();
        }
    }

    //Показ панели выигрыш/проигрыш
    public void GameStatusOver()
    {
        if (_hpText[1].GetComponentInParent<GenerateTileMap>().CheckLifeShips() <= 0)
        {
            _gameStatusPanel.gameObject.SetActive(true);
            _gameStatus.text = "ТЫ ПОБЕДИЛ";
            _gameStatus.color = Tile.Instance.SetColorIndex[1];
            _hitBlockCollider.enabled = true;
        }

        if (_hpText[0].GetComponentInParent<GenerateTileMap>().CheckLifeShips() <= 0)
        {
            _gameStatusPanel.gameObject.SetActive(true);
            _gameStatus.text = "ТЫ ПРОИГРАЛ";
            _gameStatus.color = Tile.Instance.SetColorIndex[2];
            _hitBlockCollider.enabled = true;
        }
    }

    //перезгрузить сцену
    public void ReloadScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }

}
