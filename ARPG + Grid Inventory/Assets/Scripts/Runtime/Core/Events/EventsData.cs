using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoaT
{
    public static class EventsData
    {
        /// <summary>
        /// Parameters: Type
        /// </summary>
        public const string OnCharacterStateChange = "OnCharacterStateChange";

        /// <summary>
        /// Parameters: Int(Ability Index)
        /// </summary>
        public const string OnAbilityUsed = "OnAbilityUsed";

        /// <summary>
        ///  Parameters: Vector3(Location), IAttackable(target), float(Damage), bool(isCrit)
        /// </summary>
        public const string OnEntityDamageTaken = "OnEntityDamageTaken";

        /// <summary>
        /// Parameters: ITargetable
        /// </summary>
        public const string OnSlotPointerEnter = "OnSlotPointerEnter";

        /// <summary>
        /// Parameters: UIInventorySlots
        /// </summary>
        public const string OnSlotPointerExit = "OnSlotPointerExit";

        /// <summary>
        /// Parameters: List(UIInventorySlots)
        /// </summary>
        public const string OnInventoryDraw = "OnInventoryDraw";

        /// <summary>
        /// Parameters: null
        /// </summary>
        public const string OnInventoryActionUpdate = "OnInventoryDraw";

        public const string OnInteractionWithUI = "OnInteractionWithUI";



        public const string OnWorldClick = "OnWorldClick";


        /// <summary>
        /// Parameters: Vector3(Location), Item(Spawnned Item)
        /// </summary>
        public const string OnWorldLootSpawn = "OnWorldLootSpawn";


        /// <summary>
        /// Parameters: Item(Picked Item), Action (Dispose)
        /// </summary>
        public const string OnItemPickUp = "OnItemPickUp";


        public const string OnEntityKilled = "OnEntityKilled";
    }
}
