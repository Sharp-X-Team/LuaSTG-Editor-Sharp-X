using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public enum MetaType { Proj, UserDefined
            , StageGroup, Boss, Bullet, ImageLoad, ImageGroupLoad
            , BGMLoad, BossBG, Laser, BentLaser, Object, Background, Enemy, Task
            , SELoad, AnimationLoad, ParticleLoad, TextureLoad, FXLoad, FontLoad, TTFLoad, Player, PlayerBullet, __max }

    //List version, used in non-immediate update cases
    [Serializable]
    public class MetaData : AbstractMetaData
    {
        [JsonIgnore]
        public IMetaInfoCollection StageGroupDefineData { get => aggregatableMetas[(int)MetaType.StageGroup]; }
        [JsonIgnore]
        public IMetaInfoCollection BossDefineData { get => aggregatableMetas[(int)MetaType.Boss]; }
        [JsonIgnore]
        public IMetaInfoCollection BulletDefineData { get => aggregatableMetas[(int)MetaType.Bullet]; }
        [JsonIgnore]
        public IMetaInfoCollection ImageLoadData { get => aggregatableMetas[(int)MetaType.ImageLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection ImageGroupLoadData { get => aggregatableMetas[(int)MetaType.ImageGroupLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection BGMLoadData { get => aggregatableMetas[(int)MetaType.BGMLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection BossBGDefineData { get => aggregatableMetas[(int)MetaType.BossBG]; }
        [JsonIgnore]
        public IMetaInfoCollection LaserDefineData { get => aggregatableMetas[(int)MetaType.Laser]; }
        [JsonIgnore]
        public IMetaInfoCollection BentLaserDefineData { get => aggregatableMetas[(int)MetaType.BentLaser]; }
        [JsonIgnore]
        public IMetaInfoCollection ObjectDefineData { get => aggregatableMetas[(int)MetaType.Object]; }
        [JsonIgnore]
        public IMetaInfoCollection BackgroundDefineData { get => aggregatableMetas[(int)MetaType.Background]; }
        [JsonIgnore]
        public IMetaInfoCollection EnemyDefineData { get => aggregatableMetas[(int)MetaType.Enemy]; }
        [JsonIgnore]
        public IMetaInfoCollection TaskEXDefineData { get => aggregatableMetas[(int)MetaType.Task]; }
        [JsonIgnore]
        public IMetaInfoCollection SEDefineData { get => aggregatableMetas[(int)MetaType.SELoad]; }
        [JsonIgnore]
        public IMetaInfoCollection AnimationLoadData { get => aggregatableMetas[(int)MetaType.AnimationLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection ParticleLoadData { get => aggregatableMetas[(int)MetaType.ParticleLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection TextureLoadData { get => aggregatableMetas[(int)MetaType.TextureLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection FXLoadData { get => aggregatableMetas[(int)MetaType.FXLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection FontLoadData { get => aggregatableMetas[(int)MetaType.FontLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection TTFLoadData { get => aggregatableMetas[(int)MetaType.TTFLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection PlayerDefineData { get => aggregatableMetas[(int)MetaType.Player]; }
        [JsonIgnore]
        public IMetaInfoCollection PlayerBulletDefineData { get => aggregatableMetas[(int)MetaType.PlayerBullet]; }

        public MetaData()
        {
            aggregatableMetas = new MetaInfoCollection[NodesConfig.MetaInfoCollectionTypeCount];
            for (int i = 0; i < aggregatableMetas.Length; i++)
            {
                aggregatableMetas[i] = new MetaInfoCollection();
            }
        }

        public MetaData(IMetaInfoCollection[] meta)
        {
            aggregatableMetas = meta;
        }
    }
}
