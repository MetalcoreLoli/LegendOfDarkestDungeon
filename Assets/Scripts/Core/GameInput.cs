using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class GameInput
    {
        private static Dictionary<string, KeyCode> keyValuePairs = new Dictionary<string, KeyCode>()
        { 
            ["Slot1"]                   = KeyCode.Alpha1,
            ["Slot2"]                   = KeyCode.Alpha2,
            ["Slot3"]                   = KeyCode.Alpha3,
            ["Slot4"]                   = KeyCode.Alpha4,
            ["Slot5"]                   = KeyCode.Alpha5,
            ["Use"]                     = KeyCode.LeftControl,
            ["Inventory"]               = KeyCode.I,
            ["InventoryMoveUp"]         = KeyCode.J,
            ["InventoryMoveDown"]       = KeyCode.K,
            ["InventoryMoveLeft"]       = KeyCode.H,
            ["InventoryMoveRight"]      = KeyCode.L,
            ["RemoveFromSlot"]          = KeyCode.X
        };

        public static void SetKey(string name, KeyCode code)
        {
            if (keyValuePairs.ContainsKey(name))
                keyValuePairs[name] = code;
            else
                throw new Exception("Something wrong");
        }

        public static bool GetKeyDown(string name) => Input.GetKeyDown(keyValuePairs[name]);
    }
}
