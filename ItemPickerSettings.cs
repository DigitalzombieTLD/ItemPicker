using System.IO;
using System.Reflection;
using UnityEngine;
using ModSettings;

namespace ItemPicker
{
    internal class ItemPickerSettingsMain : JsonModSettings
    {    
        [Section("Keybind")]

		[Name("Pick up")]
		[Description("Pick up items in radius around you")]
		public KeyCode buttonPickSticks = KeyCode.LeftAlt;

		[Section("General")]

		[Name("What to pick")]
		[Description("Choose between stick, stones or sticks & stones")]
		[Choice("Sticks", "Stones", "Sticks and stones")]
		public int pickupChoice = 2;

		[Name("Pickup radius")]
		[Description("Choose the radius (sphere) around the player which gets searched for objects")]
		[Slider(0f, 25f)]
		public float pickupRadius = 2.0f;

		[Name("Pickup calorie cost")]
		[Description("Calorie penalty for picking up a single stick/stone (multiplied if more than one items gets picked up)")]
		[Slider(0, 25)]
		public int calorieCost = 5;

		[Name("Enable custom item list")]
		[Description("Picks up any (valid) item listed in the StickPickCustomList.txt")]
		public bool pickUpAdditionalItems = false;

		[Section("Other")]

		[Name("Skip inspection on pick up")]
		[Description("Skips inspection on (manual) pick up and puts item directly into your inventory (sticks/stones)")]
		public bool skipInspect = false;

		[Name("Skip every item inspection")]
		[Description("Skips inspection on (manual) pick up for all \"GEAR_\" items (Warning: untested!)")]
		public bool skipInspectAll = false;

		protected override void OnConfirm()
        {
            base.OnConfirm();
			ItemPickerMain.loadCustomItemList();
		}
    }

    internal static class Settings
    {
        public static ItemPickerSettingsMain options;

        public static void OnLoad()
        {
            options = new ItemPickerSettingsMain();
            options.AddToModSettings("ItemPicker");
            ItemPickerMain.loadCustomItemList();
		}
    }
}
