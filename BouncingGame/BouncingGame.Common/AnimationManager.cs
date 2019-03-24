using CocosSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public class AnimationManager : CCNode
    {
        public CCSprite sprite;
        CCSpriteSheet spriteSheet;
        Dictionary<string, CCAction> anims;
        public CCSize spriteSize = CCSize.Zero;

        public AnimationManager(string spriteName)
        {
            sprite = new CCSprite(spriteName);
            anims = new Dictionary<string, CCAction>();
            this.AddChild(sprite);
        }

        public AnimationManager(string spriteName, JObject json)
        {
            sprite = new CCSprite();
            anims = new Dictionary<string, CCAction>();
            spriteSheet = new CCSpriteSheet(spriteName + ".plist", spriteName + ".png");
            JArray animations = (JArray)json["animations"];

            foreach (JObject animation in animations)
            {
                string name = (string)animation["name"];
                float delay = (float)animation["delay"];
                bool repeat = (bool)animation["repeat"];
                AddAnimation(name, delay, repeat);
            }
            this.AddChild(sprite);
        }

        public void AddAnimation(string name, float delay, bool repeat)
        {
            CCAction newAction;
            var animFrames = spriteSheet.Frames.FindAll(item => item.TextureFilename.ToLower().Contains(name));
            if(spriteSize == CCSize.Zero)
                spriteSize = animFrames[0].ContentSize;
            if (repeat)
            {
                newAction = new CCRepeatForever(new CCAnimate(new CCAnimation(animFrames, delay)));
            }
            else
            {
                newAction = new CCRepeat(new CCAnimate(new CCAnimation(animFrames, delay)), 1);
            }
            sprite.AddAction(newAction);
            anims.Add(name, newAction);
        }

        public void Play(string name)
        {
            if (anims.ContainsKey(name))
                sprite.RunAction(anims[name]);
        }
        
        /* Play a list of animations */
        public void PlayAnims(string[] animList)
        {
            CCFiniteTimeAction[] actions = new CCFiniteTimeAction[animList.Length];
            for (int i = 0; i < animList.Length; i++)
            { 
                if (anims.ContainsKey(animList[i]))
                    actions[i] = (CCFiniteTimeAction)anims[animList[i]];
                else
                    return;
            }
            sprite.RunActions(actions);
        }

        public void StopActions()
        {
            sprite.StopAllActions();
        }
        
    }
}
