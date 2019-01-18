using CocosSharp;
using SpellDefense.Common.Entities;
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

        bool simReady = false;
        bool startGame = false;
        Team redTeam;
        Team blueTeam;
        UIcontainer battlefield;
        UIcontainer cardHUD;
        List<CCDrawNode> targetLines;
        GameState gameState;
        Stopwatch messageWatch;
        int messageTimeWait = 500;

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
            gameState = GameState.Paused;
            this.gameView = gameView;
            this.InitLayers();
            messageWatch = new Stopwatch();
            InitClient();
            Schedule(Activity);
        }

        private void InitGame()
        {
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
            GodClass.InitLibrary();

            GamesState = GameState.Playing;
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

        //Avoiding using any floats with lockstep multiplayer
        //Therefore float frameTimeInSeconds is being ignored
        private void Activity(float frameTimeInSeconds)
        {
            try
            {
                if (GodClass.online)
                {
                    startGame = client.ReceiveMessage();
                    if(startGame)
                    {
                        InitGame();
                        SendActions();
                        messageWatch.Start();
                    }
                    if (client.incomingActionQueue.Count >= 2)
                    {
                        simReady = true;
                    }
                    else if(messageWatch.ElapsedMilliseconds >= messageTimeWait)
                    {
                        SendActions(true);
                        messageWatch.Restart();
                    }
                }
                if (simReady)
                {
                    PlayActions();
                    SimulateGame();
                    SendActions();
                    messageWatch.Restart();
                    simReady = false;
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private void SimulateGame()
        {
            //Constant frame time of 30FPS
            float frameTimeInSeconds = 1.0f / 30.0f;

            redTeam.Cleanup();
            blueTeam.Cleanup();

            redTeam.MovePhase(frameTimeInSeconds);
            blueTeam.MovePhase(frameTimeInSeconds);

            redTeam.AttackPhase(frameTimeInSeconds, blueTeam.GetCombatants(), blueTeam.GetBase());
            blueTeam.AttackPhase(frameTimeInSeconds, redTeam.GetCombatants(), redTeam.GetBase());

            redTeam.SpawnPhase(frameTimeInSeconds);
            blueTeam.SpawnPhase(frameTimeInSeconds);
        }

        //Any actions received from the server are played 
        private void PlayActions()
        {
            int actionCount = client.incomingActionQueue.Count;
            if (actionCount > 1)
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