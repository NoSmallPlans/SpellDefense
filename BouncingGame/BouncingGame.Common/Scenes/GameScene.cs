using CocosSharp;
using SpellDefense.Common.Entities;
using System;
using System.Collections.Generic;
using static SpellDefense.Common.GodClass;

namespace SpellDefense.Common.Scenes
{
    public class GameScene : CCScene
    {
        CCLayer backgroundLayer;
        CCLayer gameplayLayer;
        CCLayer foregroundLayer;
        CCLayer hudLayer;
        CCSprite battleBackground;
        private CCGameView gameView;

        Team redTeam;
        Team blueTeam;
        UIcontainer battlefield;
        UIcontainer cardHUD;
        List<CCDrawNode> targetLines;

        private bool hasGameEnded;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            try
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
                battleBackground = new CCSprite("Battleground4");
                battleBackground.AnchorPoint = new CCPoint(0, 0);
                battleBackground.Scale = 0.6666f;
                backgroundLayer.AddChild(battleBackground);
                gameplayLayer.AddChild(battlefield);
                gameplayLayer.AddChild(cardHUD);
                targetLines = new List<CCDrawNode>();

                GodClass.gameplayLayer = gameplayLayer;
                Schedule(Activity);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
            }
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
            try
            {
                if (hasGameEnded == false)
                {

                    redTeam.Cleanup();
                    blueTeam.Cleanup();

                    redTeam.MovePhase(frameTimeInSeconds);
                    blueTeam.MovePhase(frameTimeInSeconds);

                    redTeam.AttackPhase(frameTimeInSeconds, blueTeam.GetCombatants(), blueTeam.GetBase());
                    blueTeam.AttackPhase(frameTimeInSeconds, redTeam.GetCombatants(), redTeam.GetBase());

                    if(redTeam.GetBase().GetCurrentHealth() <= 0 || 
                       blueTeam.GetBase().GetCurrentHealth() <= 0)
                    {
                        string winningTeam = redTeam.GetBase().GetCurrentHealth() <= 0 ? winningTeam = "Blue" : winningTeam = "Red";
                        this.ShowEndScreen(winningTeam);
                        this.hasGameEnded = true;
                    }

                    redTeam.SpawnPhase(frameTimeInSeconds);
                    blueTeam.SpawnPhase(frameTimeInSeconds);
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
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

        private void ShowEndScreen(string teamName)
        {
            var labelA = new CCLabel("Game Over", "Arial", 30, CCLabelFormat.SystemFont);
            labelA.PositionX = gameplayLayer.ContentSize.Width / 2.0f;
            labelA.PositionY = gameplayLayer.ContentSize.Height / 2.0f;
            labelA.Color = CCColor3B.White;
            gameplayLayer.AddChild(labelA);

            string winnerString = teamName + " wins!";
            var labelB = new CCLabel(winnerString, "Arial", 30, CCLabelFormat.SystemFont);
            labelB.PositionX = gameplayLayer.ContentSize.Width / 2.0f;
            labelB.PositionY = gameplayLayer.ContentSize.Height / 2.25f;
            labelB.Color = CCColor3B.White;
            gameplayLayer.AddChild(labelB);

            CreateTouchListener();
        }

        private void StartOver()
        {
            CCGameView tempGameView = GameController.GameView;
            GameController.Initialize(tempGameView);
        }

        private void CreateTouchListener()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = HandleTouchesBegan;
            gameplayLayer.AddEventListener(touchListener);
        }

        private void HandleTouchesBegan(List<CCTouch> arg1, CCEvent arg2)
        {
            this.StartOver();
        }
    }
}