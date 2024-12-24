using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryImage : MonoBehaviour
{
    [SerializeField] private CategoryImageHolder[] _imageHolders;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    public void SetImage(PlantCategory category)
    {
        foreach (var image in _imageHolders)
        {
            if (image.Category != category) continue;
            _image.sprite = image.Sprite;

            if (_text != null)
                _text.text = image.Name;

            return;
        }
    }
}

[Serializable]
public class CategoryImageHolder
{
    public PlantCategory Category;
    public Sprite Sprite;
    public string Name;
}