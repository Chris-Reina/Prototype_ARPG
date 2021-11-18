using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySpriteManager : MonoBehaviour
{
    public InventoryData inventoryData;
    public InventoryActionData inventoryActionData;
    public InventoryGrid grid;

    public Image prefab;

    private Vector2 screenDimensions;
    private List<Image> currentSprites = new List<Image>();

    private void Awake() //WIP
    {
        var temp = FindObjectOfType<Canvas>().transform as RectTransform;

        screenDimensions = temp.sizeDelta;
    }

    private void LateUpdate()
    {
        for (int i = currentSprites.Count-1; i >= 0; i--)
        {
            var temp = currentSprites[i];
            currentSprites.RemoveAt(i);
            Destroy(temp.gameObject);
        }

        foreach (var item in inventoryData.inventoryItems)
        {
            var image = Instantiate(prefab, transform, true);
            currentSprites.Add(image);

            var anchor = GetSpriteAnchor(item);

            image.rectTransform.anchorMax = anchor.Max;
            image.rectTransform.anchorMin = anchor.Min;
            //image.rectTransform.sizeDelta = new Vector2(anchor.XSize, anchor.YSize);

            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);
            image.rectTransform.anchoredPosition = new Vector2(0,0);
            image.rectTransform.sizeDelta = new Vector2(0,0);

            image.sprite = item.item.sprite;
        }
    }

    private Anchor GetSpriteAnchor(InventoryItem item)
    {
        var topRight = GetTopRightPosition(item.positions);
        var botLeft = GetBottomLeftPosition(item.positions);
        
        var temp = grid.gridSlots[topRight.x, topRight.y];
        var topRightT = temp.transform as RectTransform;
        var topRightCornerArray =  new Vector3[4];
        topRightT.GetWorldCorners(topRightCornerArray);
        
        var tempTwo = grid.gridSlots[botLeft.x, botLeft.y];
        var botLeftT = tempTwo.transform as RectTransform;
        var botLeftCornerArray =  new Vector3[4];
        botLeftT.GetWorldCorners(botLeftCornerArray);
        

        var topRightPos = topRightCornerArray.OrderByDescending(x => x.x).ThenByDescending(x => x.y).FirstOrDefault();
        var botLeftPos = botLeftCornerArray.OrderBy(x => x.x).ThenBy(x => x.y).FirstOrDefault();

        return new Anchor(new Vector2(topRightPos.x, topRightPos.y), new Vector2(botLeftPos.x, botLeftPos.y),
            screenDimensions);
    }
    private Vector2Int GetTopRightPosition(List<Vector2Int> list)
    {
        if (list.Count == 0) return default;

        var bestMatch = list
            .OrderByDescending(n => n.x)
            .ThenByDescending(n => n.y)
            .FirstOrDefault();
        
        return bestMatch;
    }
    private Vector2Int GetBottomLeftPosition(List<Vector2Int> list)
    {
        if (list.Count == 0) return default;

        var bestMatch = list
            .OrderBy(n => n.x)
            .ThenBy(n => n.y)
            .FirstOrDefault();

        return bestMatch;
    }
}

public class Anchor
{
    private Vector2 _max;
    private Vector2 _min;
    private float xSize;
    private float ySize;

    public Vector2 Max => _max;
    public Vector2 Min => _min;
    public float XSize => xSize;
    public float YSize => ySize;

    /// <summary>
    /// Creates Anchor values given a screen size and the min and max corners of the anchored area.
    /// </summary>
    /// <param name="upperRightCorner">Upper Right Corner</param>
    /// <param name="bottomLeftCorner">Bottom Left Corner</param>
    public Anchor(Vector2 upperRightCorner, Vector2 bottomLeftCorner, Vector2 screenSize)
    {
        xSize = (upperRightCorner.x - bottomLeftCorner.x) / screenSize.x;
        ySize = (upperRightCorner.y - bottomLeftCorner.y) / screenSize.y;
        
        _max = new Vector2(upperRightCorner.x / screenSize.x, upperRightCorner.y / screenSize.y);
        _min = new Vector2(bottomLeftCorner.x / screenSize.x, bottomLeftCorner.y / screenSize.y);
    }
}
