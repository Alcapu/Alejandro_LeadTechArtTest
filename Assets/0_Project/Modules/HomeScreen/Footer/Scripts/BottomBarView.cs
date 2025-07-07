using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class BottomBarView : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private GameObject _selectionIndicator;

    [SerializeField] private BottomBarButton _defaultSelection;
    [SerializeField] private List<BottomBarButton> _bottomBarButtons;

    [Header("Transition Parameters")] [SerializeField]
    private float _transitionDuration = 0.25f;

    [SerializeField] private Ease _transitionEaseType = Ease.OutSine;

    //Internal
    private BottomBarButton _buttonSelected;
    private BottomBarButton _currentSelection;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_defaultSelection != null)
        {
            OnButtonClickedEvent(_defaultSelection);
        }
        else
        {
            _selectionIndicator.SetActive(false);
        }
    }

    private void OnEnable()
    {
        foreach (var btn in _bottomBarButtons)
        {
            btn.OnButtonClickedEvent.AddListener(OnButtonClickedEvent);
        }
    }

    private void OnDisable()
    {
        foreach (var btn in _bottomBarButtons)
        {
            btn.OnButtonClickedEvent.RemoveListener(OnButtonClickedEvent);
        }
    }

    private void OnButtonClickedEvent(BottomBarButton clickedButton)
    {
        if (!_bottomBarButtons.Contains(clickedButton)) return;

        var wasAlreadySelected = _buttonSelected == clickedButton;

        if (wasAlreadySelected)
        {
            DeselectAllButtons();
            return;
        }

        SelectButton(clickedButton);
    }

    private void DeselectAllButtons()
    {
        _buttonSelected = null;
        _currentSelection = null;

        foreach (var btn in _bottomBarButtons)
        {
            btn.SetSelect(false);
        }

        _selectionIndicator.SetActive(false);
    }

    private void SelectButton(BottomBarButton button)
    {
        _buttonSelected = button;
        _currentSelection = _buttonSelected;

        foreach (var btn in _bottomBarButtons)
        {
            btn.SetSelect(btn == _buttonSelected);
        }

        AnimateIndicatorToTarget(_currentSelection.transform);
    }

    private void AnimateIndicatorToTarget(Transform target)
    {
        var targetX = target.position.x;

        _selectionIndicator.SetActive(true);
        _selectionIndicator.transform.DOKill();
        _selectionIndicator.transform.DOMoveX(targetX, _transitionDuration)
            .SetEase(_transitionEaseType)
            .OnComplete(() =>
            {
                _selectionIndicator.transform.position = new Vector3(targetX,
                    _selectionIndicator.transform.position.y,
                    _selectionIndicator.transform.position.z);
            });
    }
}