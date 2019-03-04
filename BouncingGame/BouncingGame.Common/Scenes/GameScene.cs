using CocosSharp;
using SpellDefense.Common.Entities;
using SpellDefense.Common.Entities.Cards;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static SpellDefense.Common.GodClass;
using static SpellDefense.Common.Networking.Messaging;

namespace SpellDefense.Common.Scenes
{
    public class GameScene : CCScene
    {
#region Variables
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

        private Client client;
        private CCGameView gameView;
        TurnManager turnManager;
        private List<MsgStruct> queuedCards;

        bool simReady = false;
        bool startGame = false;
        Team redTeam;
        Team blueTeam;
        UIcontainer battlefield;
        UIcontainer cardHUD;
        CardHistory cardHistory;
        List<CCDrawNode> targetLines;
        GameState gameState;

        public GameState GamesState
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
#endregion
        public GameScene(CCGameView gameView) : base(gameView)
        {
            try
            {
                gameState = GameState.Paused;
                this.gameView = gameView;
                this.InitLayers();
                InitClient();
                if (!GodClass.online)
                    startGame = true;
                Schedule(Activity);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        private void InitGame()
        {
            this.cardHUD = new CardHUD(0, 0, (int)GodClass.CardHUDdimensions.GetHeight(),
                                 (int)GodClass.CardHUDdimensions.GetWidth(),
                                 this.gameplayLayer);
            this.cardHistory = new CardHistory((int)(GodClass.BattlefieldDimensions.GetWidth()*0.4)
                                    , (int)(BattlefieldDimensions.GetHeight() * 0.95)
                                    , (int)(GodClass.BattlefieldDimensions.GetHeight())
                                    , (int)GodClass.BattlefieldDimensions.GetWidth()
                                    , this.gameplayLayer);
            this.battlefield = new UIcontainer(0,
                                            (int)GodClass.CardHUDdimensions.GetHeight(),
                                            (int)GodClass.BattlefieldDimensions.GetHeight(),
                                            (int)GodClass.BattlefieldDimensions.GetWidth(),
                                            this.gameplayLayer);
            GodClass.battlefield = battlefield;
            GodClass.cardHUD = this.cardHUD;
            GodClass.cardHistory = this.cardHistory;

            this.InitTeams();

                gameplayLayer.AddChild(battlefield);
                gameplayLayer.AddChild(cardHUD);
                gameplayLayer.AddChild(cardHistory);
                targetLines = new List<CCDrawNode>();

            GodClass.gameplayLayer = gameplayLayer;
            GodClass.InitLibrary();

            GamesState = GameState.Playing;

            turnManager = new TurnManager();
            turnManager.OnTurnTimeReached += redTeam.HandleTurnTimeReached;
            turnManager.OnTurnTimeReached += blueTeam.HandleTurnTimeReached;
            ShowTurnTimer();
        }

        private void InitClient()
        {
            client = new Client();
            GodClass.clientRef = client;
            if(GodClass.online)
            {
                client.StartClient();
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

        //Avoiding using any floats with lockstep multiplayer
        //Therefore float frameTimeInSeconds is being ignored
        private void Activity(float frameTimeInSeconds)
        {
            try
            {
                if (GodClass.online) {
                    OnlineGameLoop();
                }
                else {
                    OfflineGameLoop();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        private void OnlineGameLoop()
        {
            startGame = client.ReceiveMessage();
            if (startGame)
            {
                InitGame();
                SendActions();
            }
            if (client.incomingActionQueue.Count >= 2) //Hard coded to 2 player, fix?
            {
                simReady = true;
            }
            if (simReady)
            {
                PlayActions();
                SimulateGame();
                SendActions();
                simReady = false;
            }
        }

        private void OfflineGameLoop()
        {            
            if (startGame) {
                startGame = false;
                InitGame();
            }
            PlayActions();
            SimulateGame();
        }

        private void SimulateGame()
        {
            //Constant frame time of 30FPS
            float frameTimeInSeconds = 1.0f / 30.0f;

            turnManager.UpdateTurnCountDownLabel();

            redTeam.Cleanup();
            blueTeam.Cleanup();

            redTeam.MovePhase(frameTimeInSeconds);
            blueTeam.MovePhase(frameTimeInSeconds);

            redTeam.AttackPhase(frameTimeInSeconds, blueTeam.GetCombatants(), blueTeam.GetBase());
            blueTeam.AttackPhase(frameTimeInSeconds, redTeam.GetCombatants(), redTeam.GetBase());

            if (redTeam.GetBase().GetCurrentHealth() <= 0 ||
                blueTeam.GetBase().GetCurrentHealth() <= 0)
            {
                GameOver();
            }

            turnManager.Activity(frameTimeInSeconds);

        }

        private void GameOver()
        {
            string winningTeam = redTeam.GetBase().GetCurrentHealth() <= 0 ? winningTeam = "Blue" : winningTeam = "Red";
            this.ShowEndScreen(winningTeam);
            client.Disconnect();
            //client.SendMessage(new MsgStruct { type = MsgType.GameOver, Message = "" });            
        }

        //Any actions received from the server are played 
        private void PlayActions()
        {
            int actionCount = client.incomingActionQueue.Count;
            if (actionCount > 0)
            {
                for (int i = 0; i < actionCount; i++)
                {
                    ExecuteAction(client.incomingActionQueue.Dequeue());
                }
            }
        }
        
        //Send any queued actions from player to the server
        private void SendActions(bool sendPrev=false)
        {
            if (sendPrev)
            {
                client.SendPrevActionToServer();
            }
            else
            {
                client.SendActionToServer();
            }
        }

        private void ExecuteAction(MsgStruct msg)
        {
            int teamColor;
            //Parse actions
            switch(msg.type)
            {
                case MsgType.PlayCard:
                    Debug.WriteLine("Play Card: " + msg.Message);
                    string[] args = msg.Message.Split(';');                    
                    if (args.Length > 1)
                    {
                        string cardName = args[0];
                        int.TryParse(args[1], out teamColor);
                        GodClass.PlayCard(cardName, new int[] { teamColor });
                    }
                    break;
                default:
                    break;
            }
        }

        private void Destroy(Combatant combatant, List<Combatant> list)
        {
            combatant.RemoveFromParent();
            list.Remove(combatant);
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
            var newScene = new TitleScene(GameController.GameView);
            GameController.GoToScene(newScene);
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

        private void ShowTurnTimer()
        {
            var labelA = turnManager.GetTurnCountDownLabel();
            labelA.PositionX = gameplayLayer.ContentSize.Width * 0.5f;
            labelA.PositionY = gameplayLayer.ContentSize.Height * 0.725f;
            labelA.Color = CCColor3B.White;
            hudLayer.AddChild(labelA);
        }
    }
}