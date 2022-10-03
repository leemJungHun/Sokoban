using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] Image _stickBG;
    [SerializeField] Image _stick;
    Vector3 _inputVector;
    public float _horizVal
    {
        get { return _inputVector.normalized.x > 0.7f? 1 : _inputVector.normalized.x < -0.7f ? -1 : 0; }
    }

    public float _vetizVal
    {
        get { return _inputVector.normalized.y > 0.7f ? 1 : _inputVector.normalized.y < -0.7f ? -1 : 0; }
    }
    public Vector3 _roundVector
    {
        get { return new Vector2(_horizVal, _vetizVal); }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_stickBG.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / _stickBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / _stickBG.rectTransform.sizeDelta.y);

            _inputVector = new Vector3(pos.x, pos.y, 0);
            _inputVector = (_inputVector.magnitude > 1) ? _inputVector.normalized : _inputVector;

            _stick.rectTransform.anchoredPosition = new Vector3(_inputVector.x * (_stickBG.rectTransform.sizeDelta.x / 3), _inputVector.y * (_stickBG.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector3.zero;
        _stick.rectTransform.anchoredPosition = Vector3.zero;
    }
  

    void Awake()
    {
    }

}
