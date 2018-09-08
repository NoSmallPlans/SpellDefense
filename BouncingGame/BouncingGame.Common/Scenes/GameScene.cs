using CocosSharp;
using SpellDefense.Common.Entities;
using System;
using System.Collections.Generic;

namespace SpellDefense.Common.Scenes
{
    public class GameScene : CCScene
    {
        CCLayer backgroundLayer;
        CCLayer gameplayLayer;
        CCLayer foregroundLayer;
        CCLayer hudLayer;

        private CCGameView gameView;
        private CombatantSpawner combatantSpawner;

        List<Combatant> redTeam;
        List<Combatant> blueTeam;

        private bool hasGameEnded;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            this.gameView = gameView;
            this.InitLayers();
            this.CreateText();
            this.CreateCombatantSpawner();
            this.redTeam = new List<Combatant>();
            this.blueTeam = new List<Combatant>();

        Schedule(Activity);
        }

        private void InitLayers()
        {
            this.backgroundLayer = new CCLayer();
            this.gameplayLayer = new CCLayer();
            this.foregroundLayer = new CCLayer();
            this.hudLayer = new CCLayer();

            this.AddLayer(this.backgroundLayer);
            this.AddLayer(this.gameplayLayer);
            this.AddLayer(this.foregroundLayer);
            this.AddLayer(this.hudLayer);
        }

        private void CreateCombatantSpawner()
        {
            combatantSpawner = new CombatantSpawner();
            combatantSpawner.CombatantSpawned += HandleCombatantSpawned;
            combatantSpawner.Layer = gameplayLayer;
            combatantSpawner.CreateSpawnPts();
        }

        private void CreateText()
        {
            System.Diagnostics.Debug.WriteLine("Battle Screen");
            CCLabel label = new CCLabel("Battle Screen", "Arial", 30, CCLabelFormat.SystemFont);
            label.PositionX = hudLayer.ContentSize.Width / 2.0f;
            label.PositionY = hudLayer.ContentSize.Height / 2.0f;
            label.Color = CCColor3B.White;

            hudLayer.AddChild(label);
        }

        private void Activity(float frameTimeInSeconds)
        {
            if (hasGameEnded == false)
            {
                //paddle.Activity(frameTimeInSeconds);

                foreach (var combatant in redTeam)
                {
                   combatant.Activity(frameTimeInSeconds);
                }
                foreach (var combatant in blueTeam)
                {
                    combatant.Activity(frameTimeInSeconds);
                }

                combatantSpawner.Activity(frameTimeInSeconds);

                //DebugActivity();

                //PerformCollision();
            }
        }

        private void HandleCombatantSpawned(Combatant combatant)
        {
            if (combatant.teamColor == GameCoefficients.TeamColor.RED)
                redTeam.Add(combatant);
            else
                blueTeam.Add(combatant);
            gameplayLayer.AddChild(combatant);
        }
    }
}