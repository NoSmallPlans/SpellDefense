using CocosSharp;
using System;

namespace SpellDefense.Common.Scenes
{
    public class GameScene : CCScene
    {
        CCLayer backgroundLayer;
        CCLayer gameplayLayer;
        CCLayer foregroundLayer;
        CCLayer hudLayer;

        private CCGameView gameView;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            this.gameView = gameView;
            this.InitLayers();
            this.CreateText();
        }

        private void InitLayers()
        {
            this.backgroundLayer = new CCLayer();
            this.gameplayLayer = new CCLayer();
            this.foregroundLayer = new CCLayer();
            this.hudLayer = new CCLayer();
            this.AddLayer(this.hudLayer);
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
    }
}