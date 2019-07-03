﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using StardewModdingAPI;
using StardewModdingAPI.Events;

using StardewValley;
using StardewValley.Characters;

namespace AdoptSkin.Framework
{

    class Stray
    {
        /// <summary>RNG for selecting randomized aspects</summary>
        private readonly Random Randomizer = new Random();

        /// <summary>The GameLocation for Marnie's house</summary>
        internal static readonly GameLocation Marnies = Game1.getLocationFromName("AnimalShop");
        /// <summary>Warp location for a potential pet at the beginning of the day</summary>
        internal static Vector2 CreationLocation = new Vector2(16, 16);

        /// <summary>The identifying number for a potential pet</summary>
        internal static readonly int StrayID = 8000;

        internal Pet PetInstance;
        internal string PetType;
        internal int SkinID;


        /// <summary>Creates a new Stray</summary>
        internal Stray()
        {
            // Create Stray traits
            PetType = ModEntry.PetAssets.Keys.ToList()[Randomizer.Next(0, ModEntry.PetAssets.Count)];
            SkinID = Randomizer.Next(1, ModEntry.PetAssets[PetType].Count + 1);

            // Create Pet instance
            PetInstance = (Pet)Activator.CreateInstance(ModEntry.PetTypeMap[PetType], (int)CreationLocation.X, (int)CreationLocation.Y);
            PetInstance.Sprite = new AnimatedSprite(ModEntry.GetSkinFromID(PetType, SkinID).AssetKey, 28, 32, 32);

            // Temporary Stray traits
            PetInstance.Manners = StrayID;
            PetInstance.Name = "Stray";
            PetInstance.displayName = "Stray";
            PetInstance.farmerPassesThrough = true;

            // Put that thing where it belongs
            Game1.warpCharacter(PetInstance, Marnies, CreationLocation);

            if (ModEntry.Config.NotifyStraySpawn)
            {
                string message = $"A stray pet is available to adopt at Marnie's!";
                ModEntry.SMonitor.Log(message, LogLevel.Debug);
                Game1.chatBox.addInfoMessage(message);
            }
        }


        internal static bool IsStray(Pet pet)
        {
            if (pet.Manners == StrayID)
                return true;
            return false;
        }


        /// <summary>Remove this Stray's Pet instance from its map</summary>
        internal void RemoveFromWorld()
        {
            Game1.removeThisCharacterFromAllLocations(this.PetInstance);
        }
    }
}
