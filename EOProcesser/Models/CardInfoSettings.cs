using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOProcesser.Models
{
    public static class CardInfoSettings
    {
        public static List<string>? GetValueListForKey(string key)
        {
            if (!key.StartsWith('"'))
            {
                key = '"' + key;
            }
            if (!key.StartsWith('"'))
            {
                key += '"';
            }
            return key switch
            {
                @"""種類""" => ["効果モン", "通常モン", "儀式", "融合", "シンクロ",
                    "エクシーズ", "リンク", "超次元", "通常魔法", "装備魔法",
                    "速攻魔法", "永続魔法", "フィールド", "罠全般", "通常罠", "永続罠", "トークン"],
                @"""レベル""" => ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
                @"""属性""" => ["闇属性", "光属性", "地属性", "水属性", "炎属性", "風属性", "神属性"],
                @"""種族""" => ["魔法使い族", "ドラゴン族", "アンデット族", "戦士族", "機械族", "獣族",
                    "鳥獣族", "鱗獣族", "悪魔族", "天使族", "昆虫族", "元素霊族", "植物族",
                    "サイキック族", "サイバース族", "海生族", "幻神獣族"],
                @"""攻撃力""" or @"""守備力""" => [.. Enumerable.Range(1, 30)
                        .Select(i => (i * 100).ToString())],
                @"""性別""" => ["無性", "牝性", "雄性", "ふたなり", "性別在", "雌以外"],
                @"""召喚不可""" or @"""生産不可""" => ["1", "0"],
                _ => null,
            };
        }
        public static List<string> GetKeyList()
        {
            return ["種類", "レベル", "属性", "種族", "攻撃力", "守備力", 
                "性別", "召喚不可", "生産不可"];
        }
    }
}
