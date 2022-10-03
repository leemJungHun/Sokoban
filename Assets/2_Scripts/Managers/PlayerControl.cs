using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] RectTransform[] _wall;
    [SerializeField] RectTransform[] _push;
    [SerializeField] RectTransform[] _target;
    RectTransform _playerRT;
    VirtualStick _vStick;
    float _moveTime = 0.2f;
    float _checkTime = 0f;
    float _baseMove = 100f;
    float _mapSize = 1000f;
    bool _isClear = false;
    void Awake()
    {
        _playerRT = GetComponent<RectTransform>();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            GameObject go = GameObject.FindGameObjectWithTag("VStickObj");
            _vStick = go.GetComponent<VirtualStick>();
        }
    }

    
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            return;
        }
        _checkTime += Time.deltaTime;
        if (_checkTime >= _moveTime)
        {
            if(_vStick._roundVector.x!=0 && _vStick._roundVector.y != 0 || _isClear)
            {
                return;
            }
            float _move;
            switch (_vStick._horizVal)
            {
                case 1:
                    _move = _playerRT.anchoredPosition.x + _baseMove;
                    if (_move < _mapSize)
                    {
                        if (!WallCheck(_move, _playerRT.anchoredPosition.y) && !PushCheck(_move, _playerRT.anchoredPosition.y, _baseMove, 0))
                        {
                            _playerRT.anchoredPosition = new Vector2(_move, _playerRT.anchoredPosition.y);
                        }
                    }
                    break;
                case -1:
                    _move = _playerRT.anchoredPosition.x - _baseMove;
                    if (_move >= 0)
                    {
                        if (!WallCheck(_move, _playerRT.anchoredPosition.y) && !PushCheck(_move, _playerRT.anchoredPosition.y, -_baseMove, 0))
                        {
                            _playerRT.anchoredPosition = new Vector2(_move, _playerRT.anchoredPosition.y);
                        }
                    }
                    break; 
            }
            switch (_vStick._vetizVal)
            {
                case 1:
                    _move = _playerRT.anchoredPosition.y + _baseMove;
                    if (_move < _mapSize)
                    {
                        if (!WallCheck(_playerRT.anchoredPosition.x, _move) && !PushCheck(_playerRT.anchoredPosition.x, _move, 0, _baseMove))
                        {
                            _playerRT.anchoredPosition = new Vector2(_playerRT.anchoredPosition.x, _move);
                        }
                    }
                    break;
                case -1:
                    _move = _playerRT.anchoredPosition.y - _baseMove;
                    if (_move >= 0)
                    {
                        if (!WallCheck(_playerRT.anchoredPosition.x, _move)&& !PushCheck(_playerRT.anchoredPosition.x, _move, 0, -_baseMove))
                        {
                            _playerRT.anchoredPosition = new Vector2(_playerRT.anchoredPosition.x, _move);
                        }
                    }
                    break;
            }
            _checkTime = 0;
        }
    }

    void SuccesCheck()
    {
        for (int i = 0; i<_target.Length;i++)
        {
            _isClear = true;
            if (_target[i].anchoredPosition.x != _push[i].anchoredPosition.x || _target[i].anchoredPosition.y != _push[i].anchoredPosition.y)
            {
                _isClear = false;
                break;
            }
        }
        if (_isClear)
        {
            GameManager._Instance.GameClear();
        }
    }
    bool WallCheck(float x, float y)
    {
        for(int i = 0; i < _wall.Length; i++)
        {
            if (x == _wall[i].anchoredPosition.x && y == _wall[i].anchoredPosition.y)
            {
                return true;
            }
        }
        return false;
    }
    bool PushCheck(float x, float y, float moveX, float moveY)
    {
        for (int i = 0; i < _push.Length; i++)
        {
            if (x == _push[i].anchoredPosition.x && y == _push[i].anchoredPosition.y)
            {
                Vector2 _pushM = new Vector2(_push[i].anchoredPosition.x + moveX, _push[i].anchoredPosition.y + moveY);
                //_push[i].anchoredPosition = _pushM;
                if (_pushM.x>=0&&_pushM.x<_mapSize&& _pushM.y >= 0 && _pushM.y < _mapSize)
                {
                    if (!WallCheck(_pushM.x, _pushM.y))// 미는 블럭 다음에 벽이 있으면 못움직이도록
                    {
                        for (int n = 0; n < _push.Length; n++)// 미는 불럭 다음에 또 미는 블럭이 있으면 못움직이도록
                        {
                            if (_pushM.x == _push[n].anchoredPosition.x && _pushM.y == _push[n].anchoredPosition.y)
                            {
                                return true;
                            }
                        }
                        _push[i].anchoredPosition = _pushM;
                        SuccesCheck();
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }

                break;
            }
        }
        return false;
    }
}
