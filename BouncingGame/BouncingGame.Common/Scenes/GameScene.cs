﻿using CocosSharp;
using SpellDefense.Common.Entities;
using System;
using System.Collections.Generic;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Scenes
{
    public class GameScene : CCScene
    {
        public enum GameState
        {
            Playing,
            Paused,
            Over
        }

        CCLayer backgroundLayer;
        CCLayer gameplayLayer;
        CCLayer foregroundLayer;
        CCLayer hudLayer;
        CCLayer restartLayer;

        private CCGameView gameView;

        Team redTeam;
        Team blueTeam;
        UIcontainer battlefield;
        UIcontainer cardHUD;
        List<CCDrawNode> targetLines;

        public GameState gameState
        {
            get
            {
                return gameState;
            }
            set
            {
                gameState = value;
                switch(value)
                {
                    case GameState.Playing:
                        break;
                    case GameState.Paused:
                        break;
                    case GameState.Over:                        
                        break;
                }
            }
        }

        public GameScene(CCGameView gameView) : base(gameView)
        {
            this.gameView = gameView;
            this.InitLayers();
            this.cardHUD = new CardHUD(0, 0, (int)GodClass.CardHUDdimensions.GetHeight(), 
                                             (int)GodClass.CardHUDdimensions.GetWidth(), 
                                             this.gameplayLayer);
            this.battlefield = new UIcontainer(0, 
                                            (int)GodClass.CardHUDdimensions.GetHeight(), 
                                            (int)GodClass.BattlefieldDimensions.GetHeight(), 
                                            (int)GodClass.BattlefieldDimensions.GetWidth(),
                                            this.gameplayLayer);
            GodClass.battlefield = battlefield;
            GodClass.cardHUD = this.cardHUD;
            this.InitTeams();

            gameplayLayer.AddChild(battlefield);
            gameplayLayer.AddChild(cardHUD);
            targetLines = new List<CCDrawNode>();

            GodClass.gameplayLayer = gameplayLayer;
            Schedule(Activity);
            gameState = GameState.Playing;
        }


        private void InitTeams()
        {
            this.redTeam = new Team(TeamColor.RED);
            this.blueTeam = new Team(TeamColor.BLUE);
            gameplayLayer.AddChild(redTeam.makeBase());
            gameplayLayer.AddChild(blueTeam.makeBase());
            redTeam.SetEnemyBase(blueTeam.GetBase());
            blueTeam.SetEnemyBase(redTeam.GetBase());
            redTeam.CreateCombatantSpawner();
            blueTeam.CreateCombatantSpawner();
            redTeam.GameOver += this.GameOver;
            blueTeam.GameOver += this.GameOver;
            GodClass.red = redTeam;
            GodClass.blue = blueTeam;
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

        private void Activity(float frameTimeInSeconds)
        {

            if (gameState == GameState.Playing)
            {

                redTeam.Cleanup();
                blueTeam.Cleanup();

                redTeam.MovePhase(frameTimeInSeconds);
                blueTeam.MovePhase(frameTimeInSeconds);

                redTeam.AttackPhase(frameTimeInSeconds, blueTeam.GetCombatants(), blueTeam.GetBase());
                blueTeam.AttackPhase(frameTimeInSeconds, redTeam.GetCombatants(), redTeam.GetBase());

                redTeam.SpawnPhase(frameTimeInSeconds);
                blueTeam.SpawnPhase(frameTimeInSeconds);
            }
        }



        private void Destroy(Combatant combatant, List<Combatant> list)
        {
            combatant.RemoveFromParent();
            list.Remove(combatant);
        }

        private void HandleCardDrawn(Card card)
        {
            cardHUD.AddChild(card);
        }

        private void GameOver(TeamColor team)
        {
            gameState = GameState.Over;
            string label;
            if(team == TeamColor.BLUE)
            {
                label = "Blue Team Wins!";
            }
            else
            {
                label = "Red Team Wins!";
            }
        }
    }
}