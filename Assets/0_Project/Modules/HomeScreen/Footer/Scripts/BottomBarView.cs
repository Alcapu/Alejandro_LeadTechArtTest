using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


    public class BottomBarView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject _selectionIndicator;
        [SerializeField] private BottomBarButton _defaultSelection;
        [SerializeField] private List<BottomBarButton> _bottomBarButtons;

        //Internal
        private BottomBarButton _buttonSelected;
        private GameObject _currentSlot;

        void Start()
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

        void OnEnable()
        {
            foreach (var btn in _bottomBarButtons)
            {
                btn.OnButtonClickedEvent.AddListener(OnButtonClickedEvent);
            }
        }

        void OnDisable()
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
            _currentSlot = null;

            foreach (var btn in _bottomBarButtons)
            {
                btn.SetSelect(false);
            }

            _selectionIndicator.SetActive(false);
        }
        
        private void SelectButton(BottomBarButton button)
        {
            _buttonSelected = button;

            foreach (var btn in _bottomBarButtons)
            {
                btn.SetSelect(btn == _buttonSelected);
            }

            AnimateIndicatorToSelected();
        }


        private void AnimateIndicatorToSelected()
        {
            if (_buttonSelected == null) return;

            if (_currentSlot == _buttonSelected.gameObject) return;

            _currentSlot = _buttonSelected.gameObject;

            _selectionIndicator.SetActive(true);
            _selectionIndicator.transform.DOKill();
            _selectionIndicator.transform.DOMoveX(_currentSlot.transform.position.x, .25f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                _selectionIndicator.transform.position = new Vector3(_currentSlot.transform.position.x,
                                                            _selectionIndicator.transform.position.y,
                                                            _selectionIndicator.transform.position.z);
            });
        }
    }
