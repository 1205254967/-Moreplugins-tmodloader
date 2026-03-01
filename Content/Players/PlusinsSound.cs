using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Moreplugins.Content.Players;
public class PluginsPlayer : ModPlayer{

    public bool SoundAcc = false;
    public bool SoundAccOld = false;

    public static SoundStyle DefaultSound = new SoundStyle("Moreplugins/Assets/Sounds/Accessories/bobobo"); // 自定义饰品音效（使用ogg格式）

    public override void ResetEffects(){
        SoundAcc = false;
    }

    public override void PostUpdateMiscEffects(){
        if(SoundAcc != SoundAccOld){
            Bobobo();
        }
        SoundAccOld = SoundAcc;
    }
    private void Bobobo()
    {
        SoundEngine.PlaySound(DefaultSound);
    }
}
