using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectWnd : MonoBehaviour
{
    static MapSelectWnd _unique;
    [SerializeField] GameObject[] _mapPrefabs;
    GameObject _preMap;
    int _mapNum = 0;

    public int GetMapNum
    {
        get { return _mapNum; }
    }
    public static MapSelectWnd _Instance
    {
        get { return _unique; }
    }
    private void Awake()
    {
        _unique = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _preMap = Instantiate(_mapPrefabs[_mapNum],transform.GetChild(0).transform);
        _preMap.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapSelect(int mapNum)
    {
        Destroy(_preMap);
        _preMap = Instantiate(_mapPrefabs[mapNum], transform.GetChild(0).transform);
        _preMap.transform.position = new Vector3(0, 0, 0);
        _mapNum = mapNum;
    }

    public void GamePlayBtn()
    {
        SceneManager.LoadScene("GameScene");
    }
}
