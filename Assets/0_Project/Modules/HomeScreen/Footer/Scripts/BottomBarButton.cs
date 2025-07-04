using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

    public class BottomBarButton : MonoBehaviour
    {
        private static readonly int LockedAnimatorProperty = Animator.StringToHash("Locked");
        private static readonly int SelectedAnimatorProperty = Animator.StringToHash("Selected");

        [Header("Components")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Button _button;
        [SerializeField] private bool _lockOnAwake;

        [Header("Events")]
        public UnityEvent<BottomBarButton> OnButtonClickedEvent;

        //Internal
        private bool _selected;
        private bool _locked;

        void Awake()
        {
            SetLock(_lockOnAwake);
        }

        void Start()
        {
            _button.onClick.AddListener(() =>
            {
                OnButtonClickedEvent?.Invoke(this);
            });
        }

        public void SetLock(bool locked)
        {
            _locked = locked;
            _button.interactable = _locked == false;
            _animator.SetBool(LockedAnimatorProperty, _locked);
        }

        public void SetSelect(bool selected)
        {
            _selected = selected;
            _animator.SetBool(SelectedAnimatorProperty, _selected);
        }
    }

