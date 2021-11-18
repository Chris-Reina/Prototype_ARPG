using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayersUtility
{
    //Simple Layer INDEX
    public const int PlayerMaskIndex = 8;
    public const int NodeMaskIndex = 9;
    public const int WallMaskIndex = 10;
    public const int InteractableMaskIndex = 12;
    public const int WorldItemMaskIndex = 13;
    public const int EntityMaskIndex = 29;
    public const int TraversableMaskIndex = 31;
    
    //Simple Layer Masks
    public static readonly LayerMask PlayerMask = 1 << PlayerMaskIndex;
    public static readonly LayerMask NodeMask = 1 << NodeMaskIndex;
    public static readonly LayerMask WallMask = 1 << WallMaskIndex;
    public static readonly LayerMask InteractableMask = 1 << InteractableMaskIndex;
    public static readonly LayerMask WorldItemMask = 1 << WorldItemMaskIndex;
    public static readonly LayerMask EntityMask = 1 << EntityMaskIndex;
    public static readonly LayerMask TraversableMask = 1 << TraversableMaskIndex;
    public static readonly LayerMask AllMask = ~0;

    //Compound Layer Masks
    public static readonly LayerMask CursorSelectorMask = TraversableMask | EntityMask | InteractableMask | WorldItemMask;
    public static readonly LayerMask NodeNeighbourCheck = WallMask | NodeMask;
    public static readonly LayerMask PlayerDetectionCheck = WallMask | PlayerMask;
   
    
    
    
    
}