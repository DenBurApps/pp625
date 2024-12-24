using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CareButton : MonoBehaviour
{
    [SerializeField] private CareType _type;
    
    private Button _button;

    public event Action<CareButton> ButtonClicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    public CareType Type => _type;
    public Button Button => _button;

    private void OnButtonClicked() => ButtonClicked?.Invoke(this);
}
