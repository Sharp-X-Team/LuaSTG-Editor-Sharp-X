using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public class BulletAlikeTypes : ITypeEnumerable
    {
        private static readonly Type[] types =
            { typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Bullet.PlayerBulletInit), typeof(Data.Function), typeof(Task.TaskDefine)
         ,   typeof(Bullet.PlayerBulletFrame), typeof(Bullet.PlayerBulletColli), typeof(Bullet.PlayerBulletColli), typeof(Bullet.PlayerBulletDel), typeof(Bullet.PlayerBulletKill)};

        public IEnumerator<Type> GetEnumerator()
        {
            foreach (Type t in types)
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
