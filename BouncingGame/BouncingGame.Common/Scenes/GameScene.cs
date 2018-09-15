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
        Base redBase;
        Base blueBase;
        UIcontainer battlefield;
        UIcontainer cardHUD;

        private bool hasGameEnded;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            this.gameView = gameView;
            this.InitLayers();
            this.cardHUD = new CardHUD(0, 0, (int)GameCoefficients.CardHUDdimensions.GetHeight(), 
                (int)GameCoefficients.CardHUDdimensions.GetWidth(), this.gameplayLayer);
            this.battlefield = new UIcontainer(0, (int)GameCoefficients.CardHUDdimensions.GetHeight(), 
                (int)GameCoefficients.BattlefieldDimensions.GetHeight(), (int)GameCoefficients.BattlefieldDimensions.GetWidth(),
                this.gameplayLayer);
            this.CreateText();
            this.CreateCombatantSpawner();
            this.redTeam = new List<Combatant>();
            this.blueTeam = new List<Combatant>();
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
            redBase = new Base(GameCoefficients.TeamColor.RED, gameplayLayer);
            blueBase = new Base(GameCoefficients.TeamColor.BLUE, gameplayLayer);

            gameplayLayer.AddChild(redBase);
            gameplayLayer.AddChild(blueBase);
        }

        private void Activity(float frameTimeInSeconds)
        {
            if (hasGameEnded == false)
            {
                for(int i = redTeam.Count-1; i >= 0; i--)
                {
                    if (redTeam[i].state == Combatant.State.dead)
                    {
                        Destroy(redTeam[i], redTeam);
                    }
                    redTeam[i].Activity(frameTimeInSeconds);
                }
                for (int i = blueTeam.Count-1; i >= 0; i--)
                {
                    if (blueTeam[i].state == Combatant.State.dead)
                    {
                        Destroy(blueTeam[i], blueTeam);
                    }
                    blueTeam[i].Activity(frameTimeInSeconds);
                }

                combatantSpawner.Activity(frameTimeInSeconds);

                //DebugActivity();

                CheckCollisions();
            }
        }

        private bool CombatantCollided(Combatant one, Combatant two)
        {
            CCRect rect1 = new CCRect(one.PositionX, one.PositionY, one.collisionWidth, one.collisionHeight);
            CCRect rect2 = new CCRect(two.PositionX, two.PositionY, two.collisionWidth, two.collisionHeight);

            return rect1.IntersectsRect(rect2);
        }

        private void CheckCollisions()
        {
            foreach(Combatant cbRed in redTeam)
            {
                foreach(Combatant cbBlue in blueTeam)
                {
                    if(CombatantCollided(cbRed,cbBlue))
                    {
                        cbRed.Collided(cbBlue);
                        cbBlue.Collided(cbRed);
                    }
                }
            }
        }

        private void Destroy(Combatant combatant, List<Combatant> list)
        {
            combatant.RemoveFromParent();
            list.Remove(combatant);
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