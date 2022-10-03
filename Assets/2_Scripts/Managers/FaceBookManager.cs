using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//  페이스북 로그인의 순서 : 초기화 -> 로그인

//  초기화를 만들기 전까지의 과정

//  개발자가입 했음.
//  대시보드에 앱생성
//  프로젝트 만들면서 SDK, JDK, NDK를 설치
//  preferences External Tools 연결
//  페이스북 SDK 설치
//  유니티 페이스북에디터에 앱네임 앱아이디 설정
//  오픈 SSL 설치
//  설치후 오픈 SSL 설치 환경변수.. Window검색 -> 고급 시스템 설정 보기 -> 환경변수 -> 시스템 변수(JAVA_HOME,OPENSSL_CONF)없으면 새로 만들기
//                                                                                  사용자 변수(path편집 -> openSSL, jdk였나 추가)
//  cmd창 열어서 Gen key    (C:/ user / admin / .android)가 있으면 그냥 쓰면 됌
//  디버그 keystore 만들고
//  페이스북 개발자
//  받아온 해시키 저장
//  클라이언트 토큰 생성완료~!
// 유니티 토큰에 지정

public class FaceBookManager : MonoBehaviour
{
    [SerializeField] GameObject _loginUi;
    [SerializeField] GameObject _selectWnd;
    [SerializeField] GameObject _loginBtn;
    [SerializeField] RawImage _pic;
    [SerializeField] Text _nameTxt;
    [SerializeField] Text _idTxt;
    static FaceBookManager _unique;
    string _name;
    string _id;
    Texture _picTexture;



    public string GetId
    {
        get { return _id; }
    }

    public string GetName
    {
        get { return _name; }
    }

    public Texture GetTexture
    {
        get { return _picTexture; }
    }

    public static FaceBookManager _Instance
    {
        get { return _unique; }
    }


    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            InitFacebook();
        }
        
        _unique = this;
    }

    private void Start()
    {
        if (FB.IsLoggedIn)
        {
            DealWithFBMenus(FB.IsLoggedIn);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        SpaceBarClick();
        ClickButton();
#elif UNITY_STANDALONE_WIN
        SpaceBarClick();
        ClickButton();
#elif UNITY_ANDROID
        SpaceBarClick();
        //TouchButton();
#endif
    }

    #region [초기화]
    //  페이스북 초기화.
    public void InitFacebook()
    {
        //  초기화가 안됐다면
        if (!FB.IsInitialized)
        {
            Debug.Log("페이스북 SDK 초기화 시작");
            FB.Init(InitCallBack, OnHideUnity);
        }
        else
        {
            Debug.Log("페이스북 SDK 초기화 완료");
            FB.ActivateApp();
        }
    }

    //  초기화 명령을 실행한 뒤, 호출되는 함수
    void InitCallBack()
    {
        if (FB.IsInitialized)
        {
            Debug.Log("페이스북 SDK 초기화 완료");
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("페이스북 SDK 초기화 실패");
        }
    }

    //  초기화 명령 실행도중 호출되는 함수
    //  페이스북 로그인중 유니티를 잠시 멈추게 하는 기능을 구현.
    void OnHideUnity(bool isGameShow)
    {
        if (isGameShow)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }
#endregion [초기화]
#region [로그인]
    void LogInFaceBook()
    {
        if (!FB.IsLoggedIn)
        {
            List<string> parms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(parms, AuthCallBack);
        }
    }

    void AuthCallBack(ILoginResult result)
    {
        if (result.Error != null)
            Debug.LogFormat("Auth Error : {0}", result.Error);
        else
        {
            //  메뉴...
            DealWithFBMenus(FB.IsLoggedIn);
        }
    }

    void DealWithFBMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            _loginBtn.SetActive(false);
            _loginUi.SetActive(true);
            FB.API("/me?fields=first_name,last_name", HttpMethod.GET, SettingUserName);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, SettingprofilePic);

        }
    }

    void SettingUserName(IResult ret)
    {
        if (ret.Error == null)
        {
            _name = ret.ResultDictionary["first_name"] as string;
            _name += " "+ ret.ResultDictionary["last_name"];
            _id = ret.ResultDictionary["id"] as string;
            _nameTxt.text = _name;
            _idTxt.text = _id;
        }
        else
            Debug.LogFormat("UserName Error : {0}", ret.Error);

    }
    void SettingprofilePic(IGraphResult ret)
    {
        if(ret.Error == null)
        {
            _picTexture = ret.Texture;
            _pic.texture = _picTexture;
        }
    }
#endregion [로그인]

    public void LogoutFaceBook()
    {
        if (FB.IsLoggedIn)
        {
            FB.LogOut();
        }
    }

    public void LoginBtn()
    {
        if (!FB.IsLoggedIn)
        {
            LogInFaceBook();
        }
        else
        {
            DealWithFBMenus(FB.IsLoggedIn);
        }
    }

    void SpaceBarClick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_loginBtn.activeSelf)
            {
                LoginBtn();
            }
            else
            {
                SelectWnd();
            }
        }
    }

    void TouchButton()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Camera.main.transform.forward);
            if (hit.collider != null)
            {
                GameObject touchedObject = hit.transform.gameObject;
                if (touchedObject.name == "LoginButton")
                {
                    if (t.phase == TouchPhase.Ended)
                    {
                        LoginBtn();
                    }
                }else if (touchedObject.name == "LoginButton")
                {
                    if (t.phase == TouchPhase.Ended)
                    {
                        SelectWnd();
                    }
                }
            }
        }
    }

    void ClickButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Camera.main.transform.forward);
            Debug.Log(hit + "");
            if (hit.collider != null)
            {
                GameObject touchedObject = hit.transform.gameObject;
                Debug.Log(touchedObject.name);
                if (touchedObject.name == "LoginButton")
                {
                    if(Input.GetMouseButtonUp(0))
                    {
                        LoginBtn();
                    }
                }
                else if (touchedObject.name == "LoginButton")
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        SelectWnd();
                    }
                }
            }
        }
    }

    public void SelectWnd()
    {
        _selectWnd.SetActive(true);
    }
}