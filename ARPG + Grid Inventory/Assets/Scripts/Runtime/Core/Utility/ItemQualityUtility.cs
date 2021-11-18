using System;
using UnityEngine;

public enum ItemQuality
{
    Trash,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class ItemQualityUtility : MonoBehaviour
{
    [SerializeField] private Color _trashColor = default;
    [SerializeField] private Color _commonColor = default;
    [SerializeField] private Color _uncommonColor = default;
    [SerializeField] private Color _rareColor = default;
    [SerializeField] private Color _epicColor = default;
    [SerializeField] private Color _legendaryColor = default;

    public static Color TrashColor { get; private set; }
    public static Color CommonColor { get; private set; }
    public static Color UncommonColor { get; private set; }
    public static Color RareColor { get; private set; }
    public static Color EpicColor { get; private set; }
    public static Color LegendaryColor { get; private set; }

    private void Awake()
    {
        TrashColor = _trashColor;
        CommonColor = _commonColor;
        UncommonColor = _uncommonColor;
        RareColor = _rareColor;
        EpicColor = _epicColor;
        LegendaryColor = _legendaryColor;
    }

    public static Color GetColor(ItemQuality quality)
    {
        switch (quality)
        {
            case ItemQuality.Trash:
                return TrashColor;
            case ItemQuality.Common:
                return CommonColor;
            case ItemQuality.Uncommon:
                return UncommonColor;
            case ItemQuality.Rare:
                return RareColor;
            case ItemQuality.Epic:
                return EpicColor;
            case ItemQuality.Legendary:
                return LegendaryColor;
            default:
                throw new ArgumentOutOfRangeException(nameof(quality), quality, null);
        }
    }
}