using CocosSharp;
using SpellDefense.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Scenes
{
    public class TitleScene : CCScene
    {
        CCLayer layer;
        List<Button> buttons;
        public TitleScene(CCGameView gameView) : base(gameView)
        {
            try
            {
                layer = new CCLayer();
                this.AddLayer(layer);

                CreateButtons();

                CreateTouchListener();
            }
            catch(Exception ex)
            {
                string message = ex.Message;
            }
        }

        private void CreateButtons()
        {
            buttons = new List<Button>();

            Button localGame = new Button(CCColor4B.White, CCColor4B.Blue, 200,100,3,"Local Game", CCColor3B.Black,"Arial", 32);
            localGame.Position = new CCPoint(layer.ContentSize.Width / 2.0f, layer.ContentSize.Height / 2.0f);
            layer.AddChild(localGame);

            Button onlineGame = new Button(CCColor4B.White, CCColor4B.Blue, 200, 100, 3, "Online Game", CCColor3B.Black, "Arial", 32);
            onlineGame.Position = new CCPoint(localGame.PositionX, localGame.PositionY + 200);
            layer.AddChild(onlineGame);

            buttons.Add(localGame);
            buttons.Add(onlineGame);
        }

        private void CreateTouchListener()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = HandleTouchesBegan;
            layer.AddEventListener(touchListener);
        }

        private void HandleTouchesBegan(List<CCTouch> arg1, CCEvent arg2)
        {
            foreach (Button b in buttons)
            {
                if (b.GetBoundingBox().ContainsPoint(arg1[0].Location))
                {
                    switch(b.ButtonText)
                    {
                        case "Local Game":
                            GodClass.online = false;
                            break;
                        case "Online Game":
                            GodClass.online = true;
                            break;
                    }
                    var newScene = new GameScene(GameController.GameView);
                    GameController.GoToScene(newScene);
                }
            }
        }
    }
}
