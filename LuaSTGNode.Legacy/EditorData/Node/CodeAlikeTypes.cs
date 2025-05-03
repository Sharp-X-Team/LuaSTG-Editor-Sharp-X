﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public class CodeAlikeTypes : ITypeEnumerable
    {
        private static readonly Type[] types =
            [ typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Object.BGOnFrame), typeof(Object.PlayerDefine), typeof(Object.PlayerInit), typeof(Object.BackgroundInit), typeof(Render.BGOnRender), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
            , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
            , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit), typeof(Data.Function), typeof(Task.TaskDefine)
            , typeof(Boss.BossInit), typeof(Render.OnRender), typeof(Boss.Dialog), typeof(Enemy.EnemyInit)
            , typeof(Object.ObjectInit)
            , typeof(Boss.BossSCBeforeStart), typeof(Boss.BossSCBeforeFinish), typeof(Boss.BossSCAfter)
            , typeof(Bullet.PlayerBulletInit), typeof(Bullet.PlayerBulletFrame), typeof(Bullet.PlayerBulletRender), typeof(Bullet.PlayerBulletColli), typeof(Bullet.PlayerBulletKill), typeof(Bullet.PlayerBulletDel)
            , typeof(Object.DefineItem) ];

        public IEnumerator<Type> GetEnumerator()
        {
            foreach(Type t in types)
            {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
