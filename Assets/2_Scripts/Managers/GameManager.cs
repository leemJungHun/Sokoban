using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager _unique;
    [SerializeField] RawImage _pic;
    [SerializeField] Text _nameTxt;
    [SerializeField] Text _clearTxt;
    [SerializeField] GameObject[] _mapPrefabs;
    [SerializeField] Transform _parent;
    GameObject _map;
    float _playTime = 0;

    public static GameManager _Instance
    {
        get { return _unique; }
    }

    private void Awake()
    {
        _unique = this;
    }

    void Update()
    {
        _playTime += Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        _nameTxt.text = FaceBookManager._Instance.GetName;
        _pic.texture = FaceBookManager._Instance.GetTexture;
        _map = Instantiate(_mapPrefabs[MapSelectWnd._Instance.GetMapNum], _parent);
    }

    public void RestartGame()
    {
        Destroy(_map);
        _map = Instantiate(_mapPrefabs[MapSelectWnd._Instance.GetMapNum], _parent);
        _clearTxt.gameObject.SetActive(false);
    }

    public void GameClear()
    {
        _clearTxt.gameObject.SetActive(true);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
