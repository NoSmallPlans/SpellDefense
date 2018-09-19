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

        Team redTeam;
        Team blueTeam;
        UIcontainer battlefield;
        UIcontainer cardHUD;

        private bool hasGameEnded;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            this.gameView = gameView;
            this.InitLayers();
            this.cardHUD = new CardHUD(0, 0, (int)GameCoefficients.CardHUDdimensions.GetHeight(), 
                                             (int)GameCoefficients.CardHUDdimensions.GetWidth(), 
                                             this.gameplayLayer);
            this.battlefield = new UIcontainer(0, 
                                            (int)GameCoefficients.CardHUDdimensions.GetHeight(), 
                                            (int)GameCoefficients.BattlefieldDimensions.GetHeight(), 
                                            (int)GameCoefficients.BattlefieldDimensions.GetWidth(),
                                            this.gameplayLayer);
            this.CreateText();
            this.CreateCombatantSpawner();
            this.redTeam = new Team(Team.ColorChoice.RED);
            this.blueTeam = new Team(Team.ColorChoice.BLUE);
            this.CreateTeamBases();

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
            this.battlefield.AddChild(combatantSpawner);
            combatantSpawner.CreateSpawnPts();
        }

        private void CreateText()
        {
            System.Diagnostics.Debug.WriteLine("Battle Screen");
            CCLabel label = new CCLabel("Battle Screen", "Arial", 30, CCLabelFormat.SystemFont);
            label.PositionX = GameCoefficients.BattlefieldDimensions.GetWidth() / 2.0f;
            label.PositionY = GameCoefficients.BattlefieldDimensions.GetHeight() / 2.0f;
            label.Color = CCColor3B.White;

            hudLayer.AddChild(label);
        }

        private void CreateTeamBases()
        {
            gameplayLayer.AddChild(redTeam.makeBase());
            gameplayLayer.AddChild(blueTeam.makeBase());
        }

        private void Activity(float frameTimeInSeconds)
        {

            if (hasGameEnded == false)
            {

                redTeam.Cleanup();
                blueTeam.Cleanup();

                redTeam.MovePhase(frameTimeInSeconds);
                blueTeam.MovePhase(frameTimeInSeconds);

                redTeam.AttackPhase(frameTimeInSeconds, blueTeam.GetCombatants(), blueTeam.GetBase());
                blueTeam.AttackPhase(frameTimeInSeconds, redTeam.GetCombatants(), redTeam.GetBase());

                combatantSpawner.Activity(frameTimeInSeconds);

                //DebugActivity();

                //CheckCollisions();
            }
        }



        private void Destroy(Combatant combatant, List<Combatant> list)
        {
            combatant.RemoveFromParent();
            list.Remove(combatant);
        }

        private void HandleCombatantSpawned(Combatant combatant)
        {
            if (combatant.teamColor == Team.ColorChoice.RED)
                redTeam.AddCombatant(combatant);
            else
                blueTeam.AddCombatant(combatant);
            gameplayLayer.AddChild(combatant);
        }
    }
}