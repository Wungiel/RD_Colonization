﻿using System;
using Microsoft.Xna.Framework;
using RD_Colonization.Code.Commands;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;

namespace RD_Colonization.Code.Entities
{
    public class Unit
    {
        public int playerId = -1;
        public UnitData type;
        public Tile currentTile;
        public int remainingEnergy;
        public int health;
        public ICommand currentCommand = null;

        public Unit()
        {
            
        }

        public Unit(UnitData type, Tile position, int playerId)
        {
            this.type = type;
            this.currentTile = position;
            this.playerId = playerId;
            this.remainingEnergy = type.speed;
            this.health = type.maxHealth;
            TurnManager.Instance.turnEvent += RegenerateEnergy;
        }

        public Rectangle GetPosition()
        {
            return currentTile.CreateRectangle();
        }

        public void RegenerateEnergy()
        {
            remainingEnergy = type.speed; ;
        }

        public void removeCommand()
        {
            currentCommand = null;
        }
    }
}