using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CategoryHolder : MonoBehaviour
{
    [SerializeField] private PlantCategory _category;

    private Button _button;

    public event Action<CategoryHolder> ButtonClicked;

    public PlantCategory Category => _category;

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

    private void OnButtonClicked() => ButtonClicked?.Invoke(this);
}
