using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Moreplugins.Content.Players
{
    public partial class PluginPlayer : ModPlayer
    {
        // 插拔音效用
        public bool soundAcc = false;
        public bool soundAccOld = false;
        public static SoundStyle DefaultSound = new SoundStyle("Moreplugins/Assets/Sounds/Accessories/bobobo");
        // 世纪小花客串用
        public bool budEquipped;
        private int timer;
        // 雷管插件用
        public bool detonatorPluginsEquipped;
        // 迪斯科插件用
        public bool discoEquipped;
        private int crystalIndex = -1;
        // 黄昏插件用
        public bool duskEquipped;
        // 附魔插件用
        public bool enchantedacc;
        // 幽魂插件用
        public bool ghostEquipped;
        // 石巨人之拳插件用
        public bool giantfistEquipped;
        private int punchTimer;
        // 村正插件用
        public bool kaishakuninEquipped; // 饰品是否装备
        public int boostTimer; // 伤害提升计时器
        public bool damageBoostActive; // 伤害提升是否激活
                                       // 熔岩之心插件用
        public bool lavaSeedEquipped;
        // 血腥插件用
        public bool massacreEquipped; // 饰品是否装备
        private Dictionary<int, int> bleedingNPCs; // 存储流血的NPC和计时器
                                                   // 肉球插件用
        public bool meatballEquipped;
        private int thornTimer;
        // 永夜插件用
        public bool nightEquipped; // 饰品是否装备
        private int attackTimer;
        // 幻龙插件用
        public bool nothosaurEquipped; // 饰品是否装备
        private int bubbleTimer;
        // 核弹插件用
        public bool nuclearWarheadEquipped; // 饰品是否装备
        private int nukeTimer;
        // 户外生存装置插件用
        public bool kitEquipped; // 饰品是否装备
        private int laserTimer;
        private int eyeTimer;
        private int spazmaminiProjectileId = -1; // 存储Spazmamini的 projectile ID
        private int retaniminiProjectileId = -1; // 存储Retanimini的 projectile ID
                                                 // 纯净插件用
        public bool pureEquipped; // 饰品是否装备
                                  // 萨满插件用
        public bool shamanEquipped;
        private int samanAttackTimer;
        // 腐化插件用
        public bool shadowyeggplantEquipped;
        // 荆棘插件用
        public bool thornEquipped;
        // 泰拉之心插件用
        public bool terraHeartEquipped; // 饰品是否装备
        public int terraHeartAttackTimer;
        public bool terraHeartAamageBoostActive;
        private bool isBoostedHit; // 标记是否是提升后的伤害
                                   // 团结插件用
        public bool unityEquipped;
        private int[] starCellProjectileIds = new int[3] { -1, -1, -1 };
        // 黄蜂插件用
        public bool vibrissaEquipped; // 饰品是否装备
        private int beeTimer;
        private int hornetProjectileId1 = -1; // 存储第一个黄蜂的projectile ID
        private int hornetProjectileId2 = -1; // 存储第二个黄蜂的projectile ID
        // 姜饼人插件用
        public bool gingerbreadmanPluginsEquipped;
        public bool hasUsedEffect = false;
        public int dieTimer;
        // 木塞插件用
        public bool woodPluginsEquipped;
        public int woodPluginsTime = 0;
        // 神圣插件用
        public bool holyPluginsEquipped;

        public override void ResetEffects()
        {
            soundAcc = false;
            budEquipped = false;
            detonatorPluginsEquipped = false;
            discoEquipped = false;
            enchantedacc = false;
            ghostEquipped = false;
            giantfistEquipped = false;
            kaishakuninEquipped = false;
            lavaSeedEquipped = false;
            massacreEquipped = false;
            meatballEquipped = false;
            nightEquipped = false;
            nothosaurEquipped = false;
            nuclearWarheadEquipped = false;
            kitEquipped = false;
            pureEquipped = false;
            shamanEquipped = false;
            shadowyeggplantEquipped = false;
            thornEquipped = false;
            unityEquipped = false;
            terraHeartEquipped = false;
            vibrissaEquipped = false;
            gingerbreadmanPluginsEquipped = false;
            woodPluginsEquipped = false;
            holyPluginsEquipped = false;
        }
    }
}
