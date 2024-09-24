using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Data;
using LuaSTGEditorSharp.EditorData.Node.Stage;
using LuaSTGEditorSharp.EditorData.Node.Task;
using LuaSTGEditorSharp.EditorData.Node.Enemy;
using LuaSTGEditorSharp.EditorData.Node.Boss;
using LuaSTGEditorSharp.EditorData.Node.Bullet;
using LuaSTGEditorSharp.EditorData.Node.Laser;
using LuaSTGEditorSharp.EditorData.Node.Object;
using LuaSTGEditorSharp.EditorData.Node.Graphics;
using LuaSTGEditorSharp.EditorData.Node.Audio;
using LuaSTGEditorSharp.EditorData.Node.Render;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using System.Windows;
using System.Windows.Resources;
using System.IO;
using MoonSharp.Interpreter;
using LuaSTGEditorSharp.CustomNodes;

namespace LuaSTGEditorSharp
{
    public class PluginToolbox : AbstractToolbox
    {
        public PluginToolbox(IMainWindow mw) : base(mw) { }
        
        public override void InitFunc()
        {
            ToolInfo["Advanced"].Add(new ToolboxItemData(true), null);
            ToolInfo["Advanced"].Add(new ToolboxItemData("taskrepeatadv", "/LuaSTGNode.Legacy;component/images/taskadvancedrepeat.png", "Task Advanced Repeat")
                , new AddNode(AddAdvancedRepeatWithWait));

            var data = new Dictionary<ToolboxItemData, AddNode>();
            #region data
            data.Add(new ToolboxItemData("localvar", "/LuaSTGNode.Legacy;component/images/variable.png", "Local Variable")
                , new AddNode(AddLocalVarNode));
            data.Add(new ToolboxItemData("assign", "/LuaSTGNode.Legacy;component/images/assignment.png", "Assignment")
                , new AddNode(AddAssignmentNode));
            data.Add(new ToolboxItemData("function", "/LuaSTGNode.Legacy;component/images/func.png", "Define Function")
                , new AddNode(AddFunctionNode));
            data.Add(new ToolboxItemData("callfunction", "/LuaSTGNode.Legacy;component/images/callfunc.png", "Call Function")
                , new AddNode(AddCallFunctionNode));
            data.Add(new ToolboxItemData(true), null);
            data.Add(new ToolboxItemData("recordpos", "/LuaSTGNode.Legacy;component/images/positionVar.png", "Record Position")
                ,  new AddNode(AddRecordPosNode));
            data.Add(new ToolboxItemData(true), null);
            data.Add(new ToolboxItemData("listcreate", "/LuaSTGNode.Legacy;component/images/listcreate.png", "Create a list")
                , new AddNode(AddListCreateNode));
            data.Add(new ToolboxItemData("listadd", "/LuaSTGNode.Legacy;component/images/listadd.png", "Insert to a list")
                , new AddNode(AddListAddNode));
            data.Add(new ToolboxItemData("listremove", "/LuaSTGNode.Legacy;component/images/listremove.png", "Remove item from a list")
                , new AddNode(AddListRemoveNode));
            #endregion
            ToolInfo.Add("Data", data);

            var stage = new Dictionary<ToolboxItemData, AddNode>();
            #region stage
            stage.Add(new ToolboxItemData("stagegroup", "/LuaSTGNode.Legacy;component/images/stagegroup.png", "Stage Group")
                , new AddNode(AddStageGroupNode));
            stage.Add(new ToolboxItemData("stage", "/LuaSTGNode.Legacy;component/images/stage.png", "Stage")
                , new AddNode(AddStageNode));
            stage.Add(new ToolboxItemData(true), null);
            stage.Add(new ToolboxItemData("stagegoto", "/LuaSTGNode.Legacy;component/images/stagegoto.png", "Go to Stage")
                , new AddNode(AddStageGoToNode));
            stage.Add(new ToolboxItemData("stagegroupfinish", "/LuaSTGNode.Legacy;component/images/stagefinishgroup.png", "Finish Stage Group")
                , new AddNode(AddStageGroupFinishNode));
            stage.Add(new ToolboxItemData("stagefinishreplay", "/LuaSTGNode.Legacy;component/images/stagefinishreplay.png", "Finish Stage Replay")
                , new AddNode(AddStageFinishReplayNode));
            stage.Add(new ToolboxItemData(true), null);
            stage.Add(new ToolboxItemData("setstagebg", "/LuaSTGNode.Legacy;component/images/bgstage.png", "Set Stage Background")
                , new AddNode(AddSetStageBGNode));
            stage.Add(new ToolboxItemData(true), null);
            stage.Add(new ToolboxItemData("shakescreen", "/LuaSTGNode.Legacy;component/images/shakescreen.png", "Shake Screen")
                , new AddNode(AddShakeScreenNode));
            stage.Add(new ToolboxItemData("maskfader", "/LuaSTGNode.Legacy;component/images/maskfader.png", "Mask Fader")
                , new AddNode(AddMaskFaderNode));
            stage.Add(new ToolboxItemData("hinter", "/LuaSTGNode.Legacy;component/images/hinter.png", "Hinter")
                , new AddNode(AddHinterNode));
            #endregion
            ToolInfo.Add("Stage", stage);

            var task = new Dictionary<ToolboxItemData, AddNode>();
            #region task
            task.Add(new ToolboxItemData("task", "/LuaSTGNode.Legacy;component/images/task.png", "Task")
                , new AddNode(AddTaskNode));
            task.Add(new ToolboxItemData("taskforobject", "/LuaSTGNode.Legacy;component/images/taskforobject.png", "Task For Another Object")
                , new AddNode(AddTaskForObjectNode));
            task.Add(new ToolboxItemData("tasker", "/LuaSTGNode.Legacy;component/images/tasker.png", "Tasker")
                , new AddNode(AddTaskerNode));
            task.Add(new ToolboxItemData("taskdefine", "/LuaSTGNode.Legacy;component/images/taskdefine.png", "Define Task")
                , new AddNode(AddTaskDefineNode));
            task.Add(new ToolboxItemData("taskcreate", "/LuaSTGNode.Legacy;component/images/taskattach.png", "Create Task")
                , new AddNode(AddTaskCreateNode));
            task.Add(new ToolboxItemData("taskfinish", "/LuaSTGNode.Legacy;component/images/taskreturn.png", "Finish Task")
                , new AddNode(AddTaskFinishNode));
            task.Add(new ToolboxItemData("taskclear", "/LuaSTGNode.Legacy;component/images/taskclear.png", "Clear Task")
                , new AddNode(AddTaskClearNode));
            task.Add(new ToolboxItemData(true), null);
            task.Add(new ToolboxItemData("wait", "/LuaSTGNode.Legacy;component/images/taskwait.png", "Wait")
                , new AddNode(AddTaskWaitNode));
            task.Add(new ToolboxItemData("taskrepeat", "/LuaSTGNode.Legacy;component/images/taskrepeat.png", "Task Repeat")
                , new AddNode(AddTaskRepeatNode));
            task.Add(new ToolboxItemData(true), null);
            task.Add(new ToolboxItemData("moveto", "/LuaSTGNode.Legacy;component/images/taskmoveto.png", "Move To")
                , new AddNode(AddTaskMoveToNode));
            task.Add(new ToolboxItemData("moveby", "/LuaSTGNode.Legacy;component/images/taskmovetoex.png", "Move By")
                , new AddNode(AddTaskMoveByNode));
            task.Add(new ToolboxItemData("movetocurve", "/LuaSTGNode.Legacy;component/images/taskBeziermoveto.png", "Move To (Curve)")
                , new AddNode(AddTaskMoveToCurveNode));
            task.Add(new ToolboxItemData("movebycurve", "/LuaSTGNode.Legacy;component/images/taskBeziermovetoex.png", "Move By (Curve)")
                , new AddNode(AddTaskMoveByCurveNode));
            task.Add(new ToolboxItemData(true), null);
            task.Add(new ToolboxItemData("smoothset", "/LuaSTGNode.Legacy;component/images/tasksetvalue.png", "Smooth set value to")
                , new AddNode(AddSmoothSetValueNode));
            task.Add(new ToolboxItemData(true), null);
            task.Add(new ToolboxItemData("setsignal", "/LuaSTGNode.Legacy;component/images/signal.png", "Set Signal")
                , new AddNode(AddSetSignal));
            task.Add(new ToolboxItemData("waitsignal", "/LuaSTGNode.Legacy;component/images/taskwaitfor.png", "Wait for Signal")
                , new AddNode(AddWaitSignal));
            #endregion
            ToolInfo.Add("Task", task);
            
            var enemy = new Dictionary<ToolboxItemData, AddNode>();
            #region enemy
            enemy.Add(new ToolboxItemData("defenemy", "/LuaSTGNode.Legacy;component/images/enemydefine.png", "Define Enemy")
                , new AddNode(AddEnemyDefineNode));
            enemy.Add(new ToolboxItemData("createenemy", "/LuaSTGNode.Legacy;component/images/enemycreate.png", "Create Enemy")
                , new AddNode(AddEnemyCreateNode));
            enemy.Add(new ToolboxItemData(true), null);
            enemy.Add(new ToolboxItemData("enemysimple", "/LuaSTGNode.Legacy;component/images/enemysimple.png", "Create Simple Enemy with Task")
                , new AddNode(AddCreateSimpleEnemyNode));
            enemy.Add(new ToolboxItemData(true), null);
            enemy.Add(new ToolboxItemData("enemycharge", "/LuaSTGNode.Legacy;component/images/pactrometer.png", "Enemy Charge")
                , new AddNode(AddEnemyChargeNode));
            enemy.Add(new ToolboxItemData("enemywander", "/LuaSTGNode.Legacy;component/images/taskbosswander.png", "Enemy Wander")
                , new AddNode(AddEnemyWanderNode));
            #endregion
            ToolInfo.Add("Enemy", enemy);

            var boss = new Dictionary<ToolboxItemData, AddNode>();
            #region boss
            boss.Add(new ToolboxItemData("defboss", "/LuaSTGNode.Legacy;component/images/bossdefine.png", "Define Boss")
                , new AddNode(AddDefineBossNode));
            boss.Add(new ToolboxItemData("bosssc", "/LuaSTGNode.Legacy;component/images/bossspellcard.png", "Define Spell Card")
                , new AddNode(AddBossSCNode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("createboss", "/LuaSTGNode.Legacy;component/images/bosscreate.png", "Create Boss")
                , new AddNode(AddCreateBossNode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("bosssetwisys", "/LuaSTGNode.Legacy;component/images/bosswalkimg.png", "Set Walk Image of an Object")
                , new AddNode(AddSetBossWISysNode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("bossmoveto", "/LuaSTGNode.Legacy;component/images/bossmoveto.png", "Boss Move To")
                , new AddNode(AddBossMoveToNode));
            boss.Add(new ToolboxItemData("dialog", "/LuaSTGNode.Legacy;component/images/dialog.png", "Dialog")
                , new AddNode(AddDialogNode));
            boss.Add(new ToolboxItemData("sentence", "/LuaSTGNode.Legacy;component/images/sentence.png", "Sentence")
                , new AddNode(AddSentenceNode));
            boss.Add(new ToolboxItemData("advancedsentence", "/LuaSTGNode.Legacy;component/images/advancedsentence.png", "Advanced Sentence")
                , new AddNode(AddAdvancedSentenceNode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("chargeball", "/LuaSTGNode.Legacy;component/images/chargeball.png", "Make chargeball")
                , new AddNode(AddChargeballNode));
            boss.Add(new ToolboxItemData("bosscast", "/LuaSTGNode.Legacy;component/images/bosscast.png", "Play cast animation")
                , new AddNode(AddBossCastNode));
            boss.Add(new ToolboxItemData("bossexplode", "/LuaSTGNode.Legacy;component/images/bossexplode.png", "Boss Explode")
                , new AddNode(AddBossExplodeNode));
            boss.Add(new ToolboxItemData("bossaura", "/LuaSTGNode.Legacy;component/images/bossshowaura.png", "Boss Aura")
                , new AddNode(AddBossAuraNode));
            boss.Add(new ToolboxItemData("bossui", "/LuaSTGNode.Legacy;component/images/bosssetui.png", "Set Boss UI")
                , new AddNode(AddBossUINode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("defbossbg", "/LuaSTGNode.Legacy;component/images/bgdefine.png", "Define Boss Background")
                , new AddNode(AddBossBGDefineNode));
            boss.Add(new ToolboxItemData("bossbglayer", "/LuaSTGNode.Legacy;component/images/bglayer.png", "Define Boss Background Layer")
                , new AddNode(AddBossBGLayerNode));
            #endregion
            ToolInfo.Add("Boss", boss);

            var bullet = new Dictionary<ToolboxItemData, AddNode>();
            #region bullet
            bullet.Add(new ToolboxItemData("defbullet", "/LuaSTGNode.Legacy;component/images/bulletdefine.png", "Define Bullet")
                , new AddNode(AddDefineBulletNode));
            bullet.Add(new ToolboxItemData("createbullet", "/LuaSTGNode.Legacy;component/images/bulletcreate.png", "Create Bullet")
                , new AddNode(AddCreateBulletNode));
            bullet.Add(new ToolboxItemData(true), null);
            bullet.Add(new ToolboxItemData("simplebullet", "/LuaSTGNode.Legacy;component/images/bulletcreatestraight.png", "Create Simple Bullet")
                , new AddNode(AddCreateSimpleBulletNode));
            bullet.Add(new ToolboxItemData("bulletgroup", "/LuaSTGNode.Legacy;component/images/bulletcreatestraightex.png", "Create Simple Bullet Group")
                , new AddNode(AddCreateBulletGroupNode));
            bullet.Add(new ToolboxItemData(true), null);
            bullet.Add(new ToolboxItemData("bulletstyle", "/LuaSTGNode.Legacy;component/images/bulletchangestyle.png", "Change Bullet Style")
                , new AddNode(AddBulletChangeStyleNode));
            bullet.Add(new ToolboxItemData(true), null);
            bullet.Add(new ToolboxItemData("bulletclear", "/LuaSTGNode.Legacy;component/images/bulletclear.png", "Clear Bullets")
                , new AddNode(AddBulletClearNode));
            bullet.Add(new ToolboxItemData("bulletclearrange", "/LuaSTGNode.Legacy;component/images/bulletcleanrange.png", "Clear Bullets in range")
                , new AddNode(AddBulletClearRangeNode));
            #endregion
            ToolInfo.Add("Bullet", bullet);

            var laser = new Dictionary<ToolboxItemData, AddNode>();
            #region laser
            laser.Add(new ToolboxItemData("deflaser", "/LuaSTGNode.Legacy;component/images/laserdefine.png", "Define Laser")
                , new AddNode(AddDefineLaserNode));
            laser.Add(new ToolboxItemData("createlaser", "/LuaSTGNode.Legacy;component/images/lasercreate.png", "Create Laser")
                , new AddNode(AddCreateLaserNode));
            laser.Add(new ToolboxItemData(true), null);
            laser.Add(new ToolboxItemData("defbentlaser", "/LuaSTGNode.Legacy;component/images/laserbentdefine.png", "Define Bent Laser")
                , new AddNode(AddDefineBentLaserNode));
            laser.Add(new ToolboxItemData("createbentlaser", "/LuaSTGNode.Legacy;component/images/laserbentcreate.png", "Create Bent Laser")
                , new AddNode(AddCreateBentLaserNode));
            laser.Add(new ToolboxItemData(true), null);
            laser.Add(new ToolboxItemData("laserturnhalfon", "/LuaSTGNode.Legacy;component/images/laserturnhalfon.png", "Turn Half On Laser")
                , new AddNode(AddLaserTurnHalfOnNode));
            laser.Add(new ToolboxItemData("laserturnon", "/LuaSTGNode.Legacy;component/images/laserturnon.png", "Turn On Laser")
                , new AddNode(AddLaserTurnOnNode));
            laser.Add(new ToolboxItemData("laserturnoff", "/LuaSTGNode.Legacy;component/images/laserturnoff.png", "Turn Off Laser")
                , new AddNode(AddLaserTurnOffNode));
            laser.Add(new ToolboxItemData(true), null);
            laser.Add(new ToolboxItemData("lasergrow", "/LuaSTGNode.Legacy;component/images/lasergrow.png", "Grow Laser")
                , new AddNode(AddLaserGrowNode));
            laser.Add(new ToolboxItemData("laserchangestyle", "/LuaSTGNode.Legacy;component/images/laserchangestyle.png", "Change Laser Style")
                , new AddNode(AddLaserChangeStyleNode));
            #endregion
            ToolInfo.Add("Laser", laser);

            var obj = new Dictionary<ToolboxItemData, AddNode>();
            #region object
            obj.Add(new ToolboxItemData("defobject", "/LuaSTGNode.Legacy;component/images/objectdefine.png", "Define Object")
                , new AddNode(AddDefineObjectNode));
            obj.Add(new ToolboxItemData("createobject", "/LuaSTGNode.Legacy;component/images/objectcreate.png", "Create Object")
                , new AddNode(AddCreateObjectNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("callbackfunc", "/LuaSTGNode.Legacy;component/images/callbackfunc.png", "Call Back Functions")
                , new AddNode(AddCallBackFuncNode));
            obj.Add(new ToolboxItemData("defaultaction", "/LuaSTGNode.Legacy;component/images/defaultaction.png", "Default Action")
                , new AddNode(AddDefaultActionNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("setv", "/LuaSTGNode.Legacy;component/images/setv.png", "Set Velocity")
                , new AddNode(AddSetVNode));
            obj.Add(new ToolboxItemData("seta", "/LuaSTGNode.Legacy;component/images/setaccel.png", "Set Acceleration")
                , new AddNode(AddSetANode));
            obj.Add(new ToolboxItemData("setg", "/LuaSTGNode.Legacy;component/images/setgravity.png", "Set Gravity")
                , new AddNode(AddSetGNode));
            obj.Add(new ToolboxItemData("setvlim", "/LuaSTGNode.Legacy;component/images/setfv.png", "Set Velocity Limit")
                , new AddNode(AddSetVLimitNode));
            obj.Add(new ToolboxItemData("delete", "/LuaSTGNode.Legacy;component/images/unitdel.png", "Delete Unit")
                , new AddNode(AddDelNode));
            obj.Add(new ToolboxItemData("kill", "/LuaSTGNode.Legacy;component/images/unitkill.png", "Kill Unit")
                , new AddNode(AddKillNode));
            obj.Add(new ToolboxItemData("preserve", "/LuaSTGNode.Legacy;component/images/unitpreserve.png", "Preserve Unit")
                , new AddNode(AddPreserveNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("setimage", "/LuaSTGNode.Legacy;component/images/objectsetimg.png", "Set Image")
                , new AddNode(AddSetObjectImageNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("setblend", "/LuaSTGNode.Legacy;component/images/setcolor.png", "Set Color and Blend Mode")
                , new AddNode(AddSetBlendNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("setbinding", "/LuaSTGNode.Legacy;component/images/connect.png", "Set Parent")
                , new AddNode(AddSetBindingNode));
            obj.Add(new ToolboxItemData("setrelativepos", "/LuaSTGNode.Legacy;component/images/setrelpos.png", "Set Relative Position")
                , new AddNode(AddSetRelativePositionNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("smear", "/LuaSTGNode.Legacy;component/images/smear.png", "Create Smear")
                , new AddNode(AddMakeSmearNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("defineitem", "/LuaSTGNode.Legacy;component/images/itemdefine.png", "Define Item")
                , new AddNode(AddDefineItemNode));
            obj.Add(new ToolboxItemData("createitem", "/LuaSTGNode.Legacy;component/images/createitem.png", "Create Item")
                , new AddNode(AddCreateItemNode));
            obj.Add(new ToolboxItemData("dropitem", "/LuaSTGNode.Legacy;component/images/dropitem.png", "Drop Item")
                , new AddNode(AddDropItemNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("groupforeach", "/LuaSTGNode.Legacy;component/images/unitforeach.png", "For Each Unit in Group")
                , new AddNode(AddGroupForEachNode));
            obj.Add(new ToolboxItemData("listforeach", "/LuaSTGNode.Legacy;component/images/listforeach.png", "For Each Unit in List")
                , new AddNode(AddListForEachNode));
            #endregion
            ToolInfo.Add("Object", obj);

            var ctrol = new Dictionary<ToolboxItemData, AddNode>();
            #region control
            ctrol.Add(new ToolboxItemData("properties", "/LuaSTGNode.Legacy;component/images/properties.png", "Set Unit Property")
                , new AddNode(AddPropertyNode));
            ctrol.Add(new ToolboxItemData(true), null);
            ctrol.Add(new ToolboxItemData("objectrotation", "/LuaSTGNode.Legacy;component/images/objectrotation.png", "Set Object Rotation")
                , new AddNode(AddSetRotationNode));
            ctrol.Add(new ToolboxItemData("objectautorotation", "/LuaSTGNode.Legacy;component/images/objectautorotation.png", "Set Object Autorotation")
                , new AddNode(AddSetAutoRotationNode));
            ctrol.Add(new ToolboxItemData("objectsize", "/LuaSTGNode.Legacy;component/images/objectsize.png", "Set Object Size")
                , new AddNode(AddSetSizeNode));
            ctrol.Add(new ToolboxItemData("objecthitbox", "/LuaSTGNode.Legacy;component/images/objecthitbox.png", "Set Hitbox Size")
                , new AddNode(AddSetHitboxNode));
            ctrol.Add(new ToolboxItemData("objectcollision", "/LuaSTGNode.Legacy;component/images/objectcollision.png", "Set Collision Detection")
                , new AddNode(AddSetCollisionNode));
            ctrol.Add(new ToolboxItemData("objectkillflag", "/LuaSTGNode.Legacy;component/images/objectkillflag.png", "Set Object Killflag")
                , new AddNode(AddSetKillflagNode));
            ctrol.Add(new ToolboxItemData(true), null);
            ctrol.Add(new ToolboxItemData("objectbound", "/LuaSTGNode.Legacy;component/images/objectbound.png", "Set Object Border Autodeletion")
                , new AddNode(AddSetBoundNode));
            ctrol.Add(new ToolboxItemData("objectshuttle", "/LuaSTGNode.Legacy;component/images/objectshuttle.png", "Set Object Border Shuttle")
                , new AddNode(AddSetShuttleNode));
            ctrol.Add(new ToolboxItemData("objectrebounce", "/LuaSTGNode.Legacy;component/images/objectrebounce.png", "Set Object Border Rebounce")
                , new AddNode(AddSetRebounceNode));
            ctrol.Add(new ToolboxItemData(true), null);
            ctrol.Add(new ToolboxItemData("assignpos", "/LuaSTGNode.Legacy;component/images/positionassignment.png", "Position Assignment")
                , new AddNode(AddPositionAssignmentNode));
            ctrol.Add(new ToolboxItemData("objectxyvel", "/LuaSTGNode.Legacy;component/images/objectxyvel.png", "Set Object X/Y Velocity")
                , new AddNode(AddSetXYVelNode));
            ctrol.Add(new ToolboxItemData("objectavel", "/LuaSTGNode.Legacy;component/images/objectavel.png", "Set Object X/Y Acceleration")
                , new AddNode(AddSetAVelNode));
            ctrol.Add(new ToolboxItemData("objectomiga", "/LuaSTGNode.Legacy;component/images/objectomiga.png", "Set Object Omiga")
                , new AddNode(AddSetOmigaNode));
            ctrol.Add(new ToolboxItemData(true), null);
            ctrol.Add(new ToolboxItemData("objectgroup", "/LuaSTGNode.Legacy;component/images/objectgroup.png", "Set Object Group")
                , new AddNode(AddSetGroupNode));
            ctrol.Add(new ToolboxItemData("objectlayer", "/LuaSTGNode.Legacy;component/images/objectlayer.png", "Set Object Layer")
                , new AddNode(AddSetLayerNode));
            ctrol.Add(new ToolboxItemData("objectvisibility", "/LuaSTGNode.Legacy;component/images/objectvisibility.png", "Set Object Visibility")
                , new AddNode(AddSetVisibilityNode));
            #endregion
            ToolInfo.Add("Control", ctrol);
            
            var graphics = new Dictionary<ToolboxItemData, AddNode>();
            #region graphics
            graphics.Add(new ToolboxItemData("loadimage", "/LuaSTGNode.Legacy;component/images/loadimage.png", "Load Image")
                , new AddNode(AddLoadImageNode));
            graphics.Add(new ToolboxItemData("loadimagegroup", "/LuaSTGNode.Legacy;component/images/loadimagegroup.png", "Load Image Group")
                , new AddNode(AddLoadImageGroupNode));
            graphics.Add(new ToolboxItemData("loadparticle", "/LuaSTGNode.Legacy;component/images/loadparticle.png", "Load Particle")
                , new AddNode(AddLoadParticleNode));
            graphics.Add(new ToolboxItemData("loadani", "/LuaSTGNode.Legacy;component/images/loadani.png", "Load Animation")
                , new AddNode(AddLoadAnimationNode));
            graphics.Add(new ToolboxItemData("loadfx", "/LuaSTGNode.Legacy;component/images/loadFX.png", "Load FX")
                , new AddNode(AddLoadFXNode));
            graphics.Add(new ToolboxItemData("loadtexture", "/LuaSTGNode.Legacy;component/images/loadtexture.png", "Load Texture")
                , new AddNode(AddLoadTextureNode));
            graphics.Add(new ToolboxItemData(true), null);
            graphics.Add(new ToolboxItemData("loadfont", "/LuaSTGNode.Legacy;component/images/loadfont.png", "Load Font")
                , new AddNode(AddLoadFontNode));
            graphics.Add(new ToolboxItemData("loadfontimage", "/LuaSTGNode.Legacy;component/images/loadfontimage.png", "Load Font Image")
                , new AddNode(AddLoadFontImageNode));
            graphics.Add(new ToolboxItemData("loadttf", "/LuaSTGNode.Legacy;component/images/loadttf.png", "Load TTF")
                , new AddNode(AddLoadTTFNode));
            graphics.Add(new ToolboxItemData(true), null);
            graphics.Add(new ToolboxItemData("setimagecenter", "/LuaSTGNode.Legacy;component/images/setimagecenter.png", "Set Image Center")
                , new AddNode(AddSetImageCenterNode));
            graphics.Add(new ToolboxItemData("setanimationcenter", "/LuaSTGNode.Legacy;component/images/setanimationcenter.png", "Set Animation Center")
                , new AddNode(AddSetAnimationCenterNode));
            graphics.Add(new ToolboxItemData(true), null);
            graphics.Add(new ToolboxItemData("setfontstate", "/LuaSTGNode.Legacy;component/images/setfontstate.png", "Set Font State")
                , new AddNode(AddSetFontStateNode));
            graphics.Add(new ToolboxItemData("setimagestate", "/LuaSTGNode.Legacy;component/images/setimagestate.png", "Set Image State")
                , new AddNode(AddSetImageStateNode));
            graphics.Add(new ToolboxItemData("setanimationstate", "/LuaSTGNode.Legacy;component/images/setanimationstate.png", "Set Animation State")
                , new AddNode(AddSetAnimationStateNode));
            #endregion
            ToolInfo.Add("Graphics", graphics);

            var audio = new Dictionary<ToolboxItemData, AddNode>();
            #region audio
            audio.Add(new ToolboxItemData("loadse", "/LuaSTGNode.Legacy;component/images/loadsound.png", "Load Sound Effect")
                , new AddNode(AddLoadSENode));
            audio.Add(new ToolboxItemData("playse", "/LuaSTGNode.Legacy;component/images/playsound.png", "Play Sound Effect")
                , new AddNode(AddPlaySENode));
            audio.Add(new ToolboxItemData(true), null);
            audio.Add(new ToolboxItemData("loadbgm", "/LuaSTGNode.Legacy;component/images/loadbgm.png", "Load Background Music")
                , new AddNode(AddLoadBGMNode));
            audio.Add(new ToolboxItemData("playbgm", "/LuaSTGNode.Legacy;component/images/playbgm.png", "Play Background Music")
                , new AddNode(AddPlayBGMNode));
            audio.Add(new ToolboxItemData("pausebgm", "/LuaSTGNode.Legacy;component/images/pausebgm.png", "Pause Background Music")
                , new AddNode(AddPauseBGMNode));
            audio.Add(new ToolboxItemData("resumebgm", "/LuaSTGNode.Legacy;component/images/resumebgm.png", "Resume Background Music")
                , new AddNode(AddResumeBGMNode));
            audio.Add(new ToolboxItemData("stopbgm", "/LuaSTGNode.Legacy;component/images/stopbgm.png", "Stop Background Music")
                , new AddNode(AddStopBGMNode));
            audio.Add(new ToolboxItemData("setbgmvolume", "/LuaSTGNode.Legacy;component/images/bgmvolume.png", "Set BGM Volume")
                , new AddNode(AddSetBGMVolumeNode));
            audio.Add(new ToolboxItemData("fadeoutbgm", "/LuaSTGNode.Legacy;component/images/fadeoutbgm.png", "Fade-out Music")
                , new AddNode(AddFadeOutBGM));
            audio.Add(new ToolboxItemData("setpace", "/LuaSTGNode.Legacy;component/images/setpace.png", "Set Music Pace")
                , new AddNode(AddSetPaceNode));
            #endregion
            ToolInfo.Add("Audio", audio);

            //var inherit = new Dictionary<ToolboxItemData, AddNode>();
            //ToolInfo.Add("Inheritance", inherit);

            var render = new Dictionary<ToolboxItemData, AddNode>();
            #region render
            render.Add(new ToolboxItemData("onrender", "/LuaSTGNode.Legacy;component/images/onrender.png", "On Render")
                , new AddNode(AddOnRenderNode));
            render.Add(new ToolboxItemData("setviewmode", "/LuaSTGNode.Legacy;component/images/setviewmode.png", "Set View Mode")
                , new AddNode(AddSetViewModeNode));
            render.Add(new ToolboxItemData(true), null);
            render.Add(new ToolboxItemData("rendernode", "/LuaSTGNode.Legacy;component/images/rendernode.png", "Render Image")
                , new AddNode(AddRenderNodeNode));
            render.Add(new ToolboxItemData("renderrect", "/LuaSTGNode.Legacy;component/images/renderrect.png", "Render Rect Image")
                , new AddNode(AddRenderRectNode));
            render.Add(new ToolboxItemData("r4v", "/LuaSTGNode.Legacy;component/images/render4v.png", "Render4V")
                , new AddNode(AddR4VNode));
            render.Add(new ToolboxItemData(true), null);
            render.Add(new ToolboxItemData("creatertar", "/LuaSTGNode.Legacy;component/images/CreateRenderTarget.png", "Create Render Target")
                , new AddNode(AddCreateRenderTargetNode));
            render.Add(new ToolboxItemData("rtarop", "/LuaSTGNode.Legacy;component/images/RenderTarget.png", "Push/Pop Render Target")
                , new AddNode(AddRenderTargetNode));
            //render.Add(new ToolboxItemData("cap", "/LuaSTGNode.Legacy;component/images/PostEffectCapture.png", "Begin Texture Capturing")
            //    , new AddNode(AddPostEffectCaptureNode));
            render.Add(new ToolboxItemData("posteff", "/LuaSTGNode.Legacy;component/images/PostEffect.png", "Post Effect")
                , new AddNode(AddPostEffectNode));
            render.Add(new ToolboxItemData(true), null);
            render.Add(new ToolboxItemData("rendertext", "/LuaSTGNode.Legacy;component/images/rendertext.png", "Render Text")
                , new AddNode(AddRenderTextNode));
            render.Add(new ToolboxItemData("renderttf", "/LuaSTGNode.Legacy;component/images/renderttf.png", "Render TTF")
                , new AddNode(AddRenderTTFNode));
            render.Add(new ToolboxItemData(true), null);
            render.Add(new ToolboxItemData("particlefire", "/LuaSTGNode.Legacy;component/images/particlefire.png", "Fire Particles")
                , new AddNode(AddParticleFire));
            render.Add(new ToolboxItemData("particlestop", "/LuaSTGNode.Legacy;component/images/particlestop.png", "Stop Particles")
                , new AddNode(AddParticleStop));
            #endregion
            ToolInfo.Add("Render", render);

            var background = new Dictionary<ToolboxItemData, AddNode>();
            #region background
            background.Add(new ToolboxItemData("set3d", "/LuaSTGNode.Legacy;component/images/set3d.png", "Set 3D Viewpoint")
                , new AddNode(AddSet3DNode));
            background.Add(new ToolboxItemData("camerasetter", "/LuaSTGNode.Legacy;component/images/camerasetter.png", "Create Camera Setter")
                , new AddNode(AddCameraSetterNode));
            background.Add(new ToolboxItemData(true), null);
            background.Add(new ToolboxItemData("bgstagecreate", "/LuaSTGNode.Legacy;component/images/bgstagecreate.png", "Define Background")
                , new AddNode(AddDefineBackgroundNode));
            background.Add(new ToolboxItemData("bgonframe", "/LuaSTGNode.Legacy;component/images/callbackfunc.png", "BG On Frame")
                , new AddNode(AddBGOnFrameNode));
            background.Add(new ToolboxItemData("bgonrender", "/LuaSTGNode.Legacy;component/images/bgonrender.png", "BG On Render")
                , new AddNode(AddBGOnRenderNode));
            background.Add(new ToolboxItemData(true), null);
            background.Add(new ToolboxItemData("renderclear", "/LuaSTGNode.Legacy;component/images/renderclear.png", "Render Clear")
                , new AddNode(AddRenderClearNode));
            background.Add(new ToolboxItemData("render4v3d", "/LuaSTGNode.Legacy;component/images/render4v3d.png", "Render4V 3D")
                , new AddNode(AddRender4V3DNode));
            background.Add(new ToolboxItemData("bgstagewarp", "/LuaSTGNode.Legacy;component/images/bgstagewarp.png", "Background Warp Effect Capture/Apply")
                , new AddNode(AddBGWarpNode));
            #endregion
            ToolInfo.Add("Background", background);

            var player = new Dictionary<ToolboxItemData, AddNode>();
            #region player
            player.Add(new ToolboxItemData("playerdefine", "/LuaSTGNode.Legacy;component/images/playerdefine.png", "Define Player")
                , new AddNode(AddPlayerDefineNode));
            player.Add(new ToolboxItemData("playerwalkimg", "/LuaSTGNode.Legacy;component/images/playerwalkimg.png", "Set Player Walk Image")
                , new AddNode(AddPlayerWalkImageNode));
            player.Add(new ToolboxItemData("playerclassrender", "/LuaSTGNode.Legacy;component/images/playerclassrender.png", "Set Player Class Render")
                , new AddNode(AddPlayerClassRenderNode));
            player.Add(new ToolboxItemData("playerclassframe", "/LuaSTGNode.Legacy;component/images/playerclassframe.png", "Set Player Class Frame")
                , new AddNode(AddPlayerClassFrameNode));
            player.Add(new ToolboxItemData(true), null);
            player.Add(new ToolboxItemData("playeroptionlist", "/LuaSTGNode.Legacy;component/images/playeroptionlist.png", "Create Player Option List"),
                new AddNode(AddPlayerOptionListNode));
            player.Add(new ToolboxItemData("playeroptionrender", "/LuaSTGNode.Legacy;component/images/playeroptionrender.png", "Render Player Options"),
                new AddNode(AddPlayerOptionRenderNode));
            player.Add(new ToolboxItemData("playeroptionforeach", "/LuaSTGNode.Legacy;component/images/playeroptionforeach.png", "For Each Player Option"),
                new AddNode(AddPlayerOptionForEachNode));
            player.Add(new ToolboxItemData(true), null);
            player.Add(new ToolboxItemData("playerspeed", "/LuaSTGNode.Legacy;component/images/playerspeed.png", "Set Player Speed")
                , new AddNode(AddPlayerSpeedNode));
            player.Add(new ToolboxItemData("playerprotect", "/LuaSTGNode.Legacy;component/images/playerprotect.png", "Set Player Protect")
                , new AddNode(AddPlayerProtectNode));
            player.Add(new ToolboxItemData("playerspellmask", "/LuaSTGNode.Legacy;component/images/playerspellmask.png", "Create Player Spell Mask")
                , new AddNode(AddPlayerSpellMaskNode));
            player.Add(new ToolboxItemData("playertarget", "/LuaSTGNode.Legacy;component/images/playertarget.png", "Find Player Target")
                , new AddNode(AddPlayerFindTargetNode));
            player.Add(new ToolboxItemData(true), null);
            player.Add(new ToolboxItemData("playersimplebullet", "/LuaSTGNode.Legacy;component/images/playersimplebullet.png", "Create Simple Player Bullet")
                , new AddNode(AddPlayerSimpleBulletNode));
            player.Add(new ToolboxItemData("playerdefinebullet", "/LuaSTGNode.Legacy;component/images/playerdefinebullet.png", "Define Player Bullet")
                , new AddNode(AddPlayerDefineBulletNode));
            player.Add(new ToolboxItemData("playerbulletcreate", "/LuaSTGNode.Legacy;component/images/playerbulletcreate.png", "Create Player Bullet")
                , new AddNode(AddCreatePlayerBulletNode));
            player.Add(new ToolboxItemData(true), null);
            player.Add(new ToolboxItemData("playernextshoot", "/LuaSTGNode.Legacy;component/images/playernextshoot.png", "Set Player Shooting Delay")
                , new AddNode(AddPlayerNextShootNode));
            player.Add(new ToolboxItemData("playernextspell", "/LuaSTGNode.Legacy;component/images/playernextspell.png", "Set Player Spell Delay")
                , new AddNode(AddPlayerNextSpellNode));
            player.Add(new ToolboxItemData("playernextsp", "/LuaSTGNode.Legacy;component/images/playernextsp.png", "Set Player Special Delay")
                , new AddNode(AddPlayerNextSpecialNode));
            #endregion 
            ToolInfo.Add("Player", player);

            var gdata = new Dictionary<ToolboxItemData, AddNode>();
            #region gamedata
            gdata.Add(new ToolboxItemData("gdatalife", "/LuaSTGNode.Legacy;component/images/gdatalife.png", "Set Player Lives")
                , new AddNode(AddSetGameLifeNode));
            gdata.Add(new ToolboxItemData("gdatalifepiece", "/LuaSTGNode.Legacy;component/images/gdatalifepiece.png", "Set Player Life Pieces")
                , new AddNode(AddSetGameLifePieceNode));
            gdata.Add(new ToolboxItemData("gdatabomb", "/LuaSTGNode.Legacy;component/images/gdatabomb.png", "Set Player Bombs")
                , new AddNode(AddSetGameBombNode));
            gdata.Add(new ToolboxItemData("gdatabombpiece", "/LuaSTGNode.Legacy;component/images/gdatabombpiece.png", "Set Player Bomb Pieces")
                , new AddNode(AddSetGameBombPieceNode));
            gdata.Add(new ToolboxItemData(true), null);
            gdata.Add(new ToolboxItemData("gdatapower", "/LuaSTGNode.Legacy;component/images/gdatapower.png", "Set Player Power")
                , new AddNode(AddSetGamePowerNode));
            gdata.Add(new ToolboxItemData("gdatapoint", "/LuaSTGNode.Legacy;component/images/gdatapoint.png", "Set Player Pointrate")
                , new AddNode(AddSetGamePointNode));
            gdata.Add(new ToolboxItemData("gdatafaith", "/LuaSTGNode.Legacy;component/images/gdatafaith.png", "Set Player Faith")
                , new AddNode(AddSetGameFaithNode));
            gdata.Add(new ToolboxItemData(true), null);
            gdata.Add(new ToolboxItemData("gdatagraze", "/LuaSTGNode.Legacy;component/images/gdatagraze.png", "Set Player Graze")
                , new AddNode(AddSetGameGrazeNode));
            gdata.Add(new ToolboxItemData("gdatascore", "/LuaSTGNode.Legacy;component/images/gdatascore.png", "Set Game Score")
                , new AddNode(AddSetGameScoreNode));
            gdata.Add(new ToolboxItemData(true), null);
            gdata.Add(new ToolboxItemData("gdatakillplayer", "/LuaSTGNode.Legacy;component/images/gdatakillplayer.png", "Kill Player")
                , new AddNode(AddSetGameKillPlayerNode));
            gdata.Add(new ToolboxItemData(true), null);
            gdata.Add(new ToolboxItemData("setfps", "/LuaSTGNode.Legacy;component/images/setfps.png", "Set FPS")
                , new AddNode(AddSetFPS));
            gdata.Add(new ToolboxItemData(true), null);
            gdata.Add(new ToolboxItemData("snapshot", "/LuaSTGNode.Legacy;component/images/snapshot.png", "Snapshot Screen")
                , new AddNode(AddSnapshot));
            gdata.Add(new ToolboxItemData(true), null);
            gdata.Add(new ToolboxItemData("raiseerror", "/LuaSTGNode.Legacy;component/images/raiseerror.png", "Raise Error")
                , new AddNode(AddRaiseError));
            gdata.Add(new ToolboxItemData("raisewarning", "/LuaSTGNode.Legacy;component/images/raisewarning.png", "Raise Warning")
                , new AddNode(AddRaiseWarning));
            gdata.Add(new ToolboxItemData("setsplash", "/LuaSTGNode.Legacy;component/images/setsplash.png", "Show Mouse")
                , new AddNode(AddSetSplash));
            #endregion
            ToolInfo.Add("Game Data", gdata);

            var cnodes = new Dictionary<ToolboxItemData, AddCustomNode>();
            #region cnodes
            // Don't do anything if the Init.lua file doesn't exist in the CustomNodes folder.
            if (File.Exists(@"CustomNodes/Init.lua"))
            {
                CustomNodeLoader customNodeLoader = new CustomNodeLoader(this);
                customNodeLoader.LoadCustomNodes(ref cnodes);

                ToolInfoCustom.Add("Custom Nodes", cnodes);
            }
            #endregion
        }

        #region data
        private void AddLocalVarNode()
        {
            parent.Insert(new LocalVar(parent.ActivatedWorkSpaceData));
        }

        private void AddAssignmentNode()
        {
            parent.Insert(new Assignment(parent.ActivatedWorkSpaceData));
        }
        private void AddFunctionNode()
        {
            parent.Insert(new Function(parent.ActivatedWorkSpaceData));
        }

        private void AddCallFunctionNode()
        {
            parent.Insert(new CallFunction(parent.ActivatedWorkSpaceData));
        }

        private void AddRecordPosNode()
        {
            parent.Insert(new RecordPos(parent.ActivatedWorkSpaceData));
        }

        private void AddPositionAssignmentNode()
        {
            parent.Insert(new PositionAssignment(parent.ActivatedWorkSpaceData));
        }

        private void AddListCreateNode()
        {
            parent.Insert(new ListCreate(parent.ActivatedWorkSpaceData));
        }

        private void AddListAddNode()
        {
            parent.Insert(new ListAdd(parent.ActivatedWorkSpaceData));
        }

        private void AddListRemoveNode()
        {
            parent.Insert(new ListRemove(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region stage
        private void AddStageGroupNode()
        {
            TreeNode newStG = new StageGroup(parent.ActivatedWorkSpaceData);
                TreeNode newSt = new Stage(parent.ActivatedWorkSpaceData);
                    TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
                        TreeNode newFolder = new Folder(parent.ActivatedWorkSpaceData, "Initialize");
                            newFolder.AddChild(new StageBG(parent.ActivatedWorkSpaceData));
                        newTask.AddChild(newFolder);
                        newTask.AddChild(new TaskWait(parent.ActivatedWorkSpaceData, "240"));
                    newSt.AddChild(newTask);
                newStG.AddChild(newSt);
            parent.Insert(newStG);
        }

        private void AddStageNode()
        {
            TreeNode newSt = new Stage(parent.ActivatedWorkSpaceData);
                TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
                    TreeNode newFolder = new Folder(parent.ActivatedWorkSpaceData, "Initialize");
                        newFolder.AddChild(new StageBG(parent.ActivatedWorkSpaceData));
                    newTask.AddChild(newFolder);
                    newTask.AddChild(new TaskWait(parent.ActivatedWorkSpaceData, "240"));
                newSt.AddChild(newTask);
            parent.Insert(newSt);
        }

        private void AddSetStageBGNode()
        {
            TreeNode newStBG = new StageBG(parent.ActivatedWorkSpaceData);
            parent.Insert(newStBG);
        }

        private void AddShakeScreenNode()
        {
            parent.Insert(new ShakeScreen(parent.ActivatedWorkSpaceData));
        }

        private void AddMaskFaderNode()
        {
            parent.Insert(new MaskFader(parent.ActivatedWorkSpaceData));
        }

        private void AddHinterNode()
        {
            parent.Insert(new Hinter(parent.ActivatedWorkSpaceData));
        }

        private void AddStageFinishReplayNode()
        {
            parent.Insert(new StageFinishReplay(parent.ActivatedWorkSpaceData));
        }

        private void AddStageGoToNode()
        {
            parent.Insert(new StageGoto(parent.ActivatedWorkSpaceData));
        }

        private void AddStageGroupFinishNode()
        {
            parent.Insert(new StageGroupFinish(parent.ActivatedWorkSpaceData));
        }

        #endregion
        #region task
        private void AddTaskNode()
        {
            parent.Insert(new TaskNode(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskForObjectNode()
        {
            parent.Insert(new TaskForObject(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskerNode()
        {
            parent.Insert(new Tasker(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskDefineNode()
        {
            parent.Insert(new TaskDefine(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskCreateNode()
        {
            parent.Insert(new TaskCreate(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskFinishNode()
        {
            parent.Insert(new TaskFinish(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskClearNode()
        {
            parent.Insert(new TaskClear(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskWaitNode()
        {
            parent.Insert(new TaskWait(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskRepeatNode()
        {
            TreeNode repeat = new Repeat(parent.ActivatedWorkSpaceData, "_infinite");
            repeat.AddChild(new TaskWait(parent.ActivatedWorkSpaceData));
            parent.Insert(repeat);
        }

        private void AddTaskMoveToNode()
        {
            parent.Insert(new TaskMoveTo(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskMoveByNode()
        {
            parent.Insert(new TaskMoveBy(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskMoveToCurveNode()
        {
            parent.Insert(new TaskMoveToCurve(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskMoveByCurveNode()
        {
            parent.Insert(new TaskMoveByCurve(parent.ActivatedWorkSpaceData));
        }

        private void AddSmoothSetValueNode()
        {
            parent.Insert(new SmoothSetValueTo(parent.ActivatedWorkSpaceData));
        }

        private void AddSetSignal()
        {
            parent.Insert(new SetSignal(parent.ActivatedWorkSpaceData), true, ["LuaSTG ExPlus", "LuaSTG Evo", "LuaSTG Sub"]);
        }
        private void AddWaitSignal()
        {
            parent.Insert(new WaitForSignal(parent.ActivatedWorkSpaceData), true, ["LuaSTG ExPlus", "LuaSTG Evo", "LuaSTG Sub"]);
        }

        private void AddSetFPS()
        {
            parent.Insert(new SetFPS(parent.ActivatedWorkSpaceData));
        }

        private void AddSnapshot()
        {
            parent.Insert(new Snapshot(parent.ActivatedWorkSpaceData));
        }

        private void AddRaiseError()
        {
            parent.Insert(new RaiseError(parent.ActivatedWorkSpaceData));
        }

        private void AddRaiseWarning()
        {
            parent.Insert(new RaiseWarning(parent.ActivatedWorkSpaceData));
        }
        #endregion

        private void AddAdvancedRepeatWithWait()
        {
            TreeNode repeat = new EditorData.Node.Advanced.AdvancedRepeat.AdvancedRepeat(parent.ActivatedWorkSpaceData, "_infinite");
            repeat.AddChild(new EditorData.Node.Advanced.AdvancedRepeat.VariableCollection(parent.ActivatedWorkSpaceData));
            repeat.AddChild(new TaskWait(parent.ActivatedWorkSpaceData));
            parent.Insert(repeat);
        }

        #region enemy
        private void AddEnemyDefineNode()
        {
            TreeNode newDef = new EnemyDefine(parent.ActivatedWorkSpaceData);
            newDef.AddChild(new EnemyInit(parent.ActivatedWorkSpaceData));
            parent.Insert(newDef);
        }

        private void AddEnemyCreateNode()
        {
            parent.Insert(new CreateEnemy(parent.ActivatedWorkSpaceData));
        }

        private void AddCreateSimpleEnemyNode()
        {
            parent.Insert(new CreateSimpleEnemy(parent.ActivatedWorkSpaceData));
        }

        private void AddEnemyChargeNode()
        {
            parent.Insert(new EnemyCharge(parent.ActivatedWorkSpaceData));
        }

        private void AddEnemyWanderNode()
        {
            parent.Insert(new EnemyWander(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region boss
        private void AddDefineBossNode()
        {
            TreeNode newDef = new BossDefine(parent.ActivatedWorkSpaceData);
            TreeNode init = new BossInit(parent.ActivatedWorkSpaceData);
            TreeNode newSC = new BossSpellCard(parent.ActivatedWorkSpaceData);
            TreeNode newSCBeforeStart = new BossSCBeforeStart(parent.ActivatedWorkSpaceData);
            TreeNode newSCStart = new BossSCStart(parent.ActivatedWorkSpaceData);
            TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
            //TreeNode newSCBeforeFinish = new BossSCBeforeFinish(parent.ActivatedWorkSpaceData);
            //TreeNode newSCAfter = new BossSCAfter(parent.ActivatedWorkSpaceData);
            newSCStart.AddChild(newTask);
            newTask.AddChild(new TaskMoveTo(parent.ActivatedWorkSpaceData, "0,120", "60", "MOVE_NORMAL"));
            newSC.AddChild(newSCBeforeStart);
            newSC.AddChild(newSCStart);
            //newSC.AddChild(newSCBeforeFinish);
            newSC.AddChild(new BossSCFinish(parent.ActivatedWorkSpaceData));
            //newSC.AddChild(newSCAfter);
            newDef.AddChild(init);
            newDef.AddChild(newSC);
            parent.Insert(newDef);
        }

        private void AddBossSCNode()
        {
            TreeNode newSC = new BossSpellCard(parent.ActivatedWorkSpaceData);
            TreeNode newSCBeforeStart = new BossSCBeforeStart(parent.ActivatedWorkSpaceData);
            TreeNode newSCStart = new BossSCStart(parent.ActivatedWorkSpaceData);
            TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
            //TreeNode newSCBeforeFinish = new BossSCBeforeFinish(parent.ActivatedWorkSpaceData);
            //TreeNode newSCAfter = new BossSCAfter(parent.ActivatedWorkSpaceData);
            newSCStart.AddChild(newTask);
            newTask.AddChild(new TaskMoveTo(parent.ActivatedWorkSpaceData, "0,120", "60", "MOVE_NORMAL"));
            newSC.AddChild(newSCBeforeStart);
            newSC.AddChild(newSCStart);
            //newSC.AddChild(newSCBeforeFinish);
            newSC.AddChild(new BossSCFinish(parent.ActivatedWorkSpaceData));
            //newSC.AddChild(newSCAfter);
            parent.Insert(newSC);
        }

        private void AddCreateBossNode()
        {
            parent.Insert(new CreateBoss(parent.ActivatedWorkSpaceData));
        }

        private void AddBossMoveToNode()
        {
            parent.Insert(new BossMoveTo(parent.ActivatedWorkSpaceData));
        }

        private void AddDialogNode()
        {
            TreeNode dialog = new Dialog(parent.ActivatedWorkSpaceData);
            dialog.AddChild(new TaskNode(parent.ActivatedWorkSpaceData));
            parent.Insert(dialog);
        }

        private void AddSentenceNode()
        {
            parent.Insert(new Sentence(parent.ActivatedWorkSpaceData));
        }

        private void AddAdvancedSentenceNode()
        {
            parent.Insert(new AdvancedSentence(parent.ActivatedWorkSpaceData));
        }

        private void AddSetBossWISysNode()
        {
            parent.Insert(new SetBossWalkImageSystem(parent.ActivatedWorkSpaceData));
        }

        private void AddBossCastNode()
        {
            parent.Insert(new BossCast(parent.ActivatedWorkSpaceData));
        }

        private void AddChargeballNode()
        {
            parent.Insert(new Chargeball(parent.ActivatedWorkSpaceData), true, ["LuaSTG ExPlus", "LuaSTG Evo", "LuaSTG Sub"]);
        }

        private void AddBossExplodeNode()
        {
            parent.Insert(new BossExplode(parent.ActivatedWorkSpaceData));
        }

        private void AddBossAuraNode()
        {
            parent.Insert(new BossAura(parent.ActivatedWorkSpaceData));
        }

        private void AddBossUINode()
        {
            parent.Insert(new BossUI(parent.ActivatedWorkSpaceData));
        }

        private void AddBossBGDefineNode()
        {
            parent.Insert(new BossBGDefine(parent.ActivatedWorkSpaceData));
        }

        private void AddBossBGLayerNode()
        {
            TreeNode newDef = new BossBGLayer(parent.ActivatedWorkSpaceData);
            newDef.AddChild(new BossBGLayerInit(parent.ActivatedWorkSpaceData));
            newDef.AddChild(new BossBGLayerFrame(parent.ActivatedWorkSpaceData));
            newDef.AddChild(new BossBGLayerRender(parent.ActivatedWorkSpaceData));
            parent.Insert(newDef);
        }
        #endregion
        #region bullet
        private void AddDefineBulletNode()
        {
            TreeNode newDef = new BulletDefine(parent.ActivatedWorkSpaceData);
            newDef.AddChild(new BulletInit(parent.ActivatedWorkSpaceData));
            parent.Insert(newDef);
        }

        private void AddCreateBulletNode()
        {
            parent.Insert(new CreateBullet(parent.ActivatedWorkSpaceData));
        }

        private void AddCreateSimpleBulletNode()
        {
            parent.Insert(new CreateSimpleBullet(parent.ActivatedWorkSpaceData));
        }

        private void AddCreateBulletGroupNode()
        {
            parent.Insert(new CreateBulletGroup(parent.ActivatedWorkSpaceData));
        }

        private void AddBulletChangeStyleNode()
        {
            parent.Insert(new BulletChangeStyle(parent.ActivatedWorkSpaceData));
        }

        private void AddBulletClearNode()
        {
            parent.Insert(new BulletClear(parent.ActivatedWorkSpaceData));
        }

        private void AddBulletClearRangeNode()
        {
            parent.Insert(new BulletClearRange(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region laser
        private void AddDefineLaserNode()
        {
            TreeNode newDef = new LaserDefine(parent.ActivatedWorkSpaceData);
            TreeNode newInit = new LaserInit(parent.ActivatedWorkSpaceData);
            TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
            TreeNode newTurnOn = new LaserTurnOn(parent.ActivatedWorkSpaceData);
            newTask.AddChild(newTurnOn);
            newInit.AddChild(newTask);
            newDef.AddChild(newInit);
            parent.Insert(newDef);
        }

        private void AddCreateLaserNode()
        {
            parent.Insert(new CreateLaser(parent.ActivatedWorkSpaceData));
        }

        private void AddDefineBentLaserNode()
        {
            TreeNode newDef = new BentLaserDefine(parent.ActivatedWorkSpaceData);
            newDef.AddChild(new BentLaserInit(parent.ActivatedWorkSpaceData));
            parent.Insert(newDef);
        }

        private void AddCreateBentLaserNode()
        {
            parent.Insert(new CreateBentLaser(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserTurnHalfOnNode()
        {
            parent.Insert(new LaserTurnHalfOn(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserTurnOnNode()
        {
            parent.Insert(new LaserTurnOn(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserTurnOffNode()
        {
            parent.Insert(new LaserTurnOff(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserGrowNode()
        {
            parent.Insert(new LaserGrow(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserChangeStyleNode()
        {
            parent.Insert(new LaserChangeStyle(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region object
        private void AddDefineObjectNode()
        {
            TreeNode objdef = new ObjectDefine(parent.ActivatedWorkSpaceData);
            objdef.AddChild(new ObjectInit(parent.ActivatedWorkSpaceData));
            parent.Insert(objdef);
        }

        private void AddCreateObjectNode()
        {
            parent.Insert(new CreateObject(parent.ActivatedWorkSpaceData));
        }

        private void AddCallBackFuncNode()
        {
            TreeNode newCBF = new CallBackFunc(parent.ActivatedWorkSpaceData);
            newCBF.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData));
            parent.Insert(newCBF);
        }

        private void AddDefaultActionNode()
        {
            parent.Insert(new DefaultAction(parent.ActivatedWorkSpaceData));
        }

        private void AddSetVNode()
        {
            parent.Insert(new SetVelocity(parent.ActivatedWorkSpaceData));
        }

        private void AddSetANode()
        {
            parent.Insert(new SetAccel(parent.ActivatedWorkSpaceData));
        }

        private void AddSetGNode()
        {
            parent.Insert(new SetGravity(parent.ActivatedWorkSpaceData));
        }

        private void AddSetVLimitNode()
        {
            parent.Insert(new SetVelocityLimit(parent.ActivatedWorkSpaceData));
        }

        private void AddDelNode()
        {
            parent.Insert(new Del(parent.ActivatedWorkSpaceData));
        }

        private void AddKillNode()
        {
            parent.Insert(new Kill(parent.ActivatedWorkSpaceData));
        }

        private void AddPreserveNode()
        {
            parent.Insert(new UnitPreserve(parent.ActivatedWorkSpaceData));
        }

        private void AddSetObjectImageNode()
        {
            parent.Insert(new SetObjectImage(parent.ActivatedWorkSpaceData));
        }

        private void AddSetBlendNode()
        {
            parent.Insert(new SetBlend(parent.ActivatedWorkSpaceData));
        }

        private void AddSetBindingNode()
        {
            parent.Insert(new SetBinding(parent.ActivatedWorkSpaceData));
        }

        private void AddSetRelativePositionNode()
        {
            parent.Insert(new SetRelativePosition(parent.ActivatedWorkSpaceData));
        }

        private void AddDefineItemNode()
        {
            TreeNode itemdef = new DefineItem(parent.ActivatedWorkSpaceData);
            itemdef.AddChild(new ItemInit(parent.ActivatedWorkSpaceData));
            
            itemdef.AddChild(new ItemOnFrame(parent.ActivatedWorkSpaceData));
            itemdef.AddChild(new ItemOnColli(parent.ActivatedWorkSpaceData));
            var itemcolli = itemdef.Children.Last();

            itemdef.AddChild(new ItemOnRender(parent.ActivatedWorkSpaceData));
            var itemrender = itemdef.Children.Last();

            itemrender.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData));

            itemcolli.AddChild(new Comment(parent.ActivatedWorkSpaceData, "Code here.", "false"));
            itemcolli.AddChild(new Kill(parent.ActivatedWorkSpaceData, "self", "false"));

            parent.Insert(itemcolli);
            parent.Insert(itemrender);
            parent.Insert(itemdef);
        }

        private void AddCreateItemNode()
        {
            parent.Insert(new CreateItem(parent.ActivatedWorkSpaceData));
        }

        private void AddDropItemNode()
        {
            parent.Insert(new DropItem(parent.ActivatedWorkSpaceData));
        }

        private void AddMakeSmearNode()
        {
            parent.Insert(new MakeSmear(parent.ActivatedWorkSpaceData));
        }

        private void AddGroupForEachNode()
        {
            parent.Insert(new GroupForEach(parent.ActivatedWorkSpaceData));
        }

        private void AddListForEachNode()
        {
            parent.Insert(new UnitForEach(parent.ActivatedWorkSpaceData));
        }
        #endregion

        #region control
        private void AddPropertyNode()
        {
            parent.Insert(new Property(parent.ActivatedWorkSpaceData));
        }

        private void AddSetRotationNode()
        {
            parent.Insert(new SetRotation(parent.ActivatedWorkSpaceData));
        }

        private void AddSetAutoRotationNode()
        {
            parent.Insert(new SetAutoRotation(parent.ActivatedWorkSpaceData));
        }

        private void AddSetSizeNode()
        {
            parent.Insert(new SetSize(parent.ActivatedWorkSpaceData));
        }

        private void AddSetHitboxNode()
        {
            parent.Insert(new SetHitbox(parent.ActivatedWorkSpaceData));
        }

        private void AddSetCollisionNode()
        {
            parent.Insert(new SetCollision(parent.ActivatedWorkSpaceData));
        }

        private void AddSetKillflagNode()
        {
            parent.Insert(new SetKillflag(parent.ActivatedWorkSpaceData));
        }

        private void AddSetBoundNode()
        {
            parent.Insert(new SetBound(parent.ActivatedWorkSpaceData));
        }

        private void AddSetShuttleNode()
        {
            parent.Insert(new SetShuttle(parent.ActivatedWorkSpaceData));
        }

        private void AddSetRebounceNode()
        {
            parent.Insert(new SetRebounce(parent.ActivatedWorkSpaceData));
        }

        private void AddSetXYVelNode()
        {
            parent.Insert(new SetXYVel(parent.ActivatedWorkSpaceData));
        }

        private void AddSetAVelNode()
        {
            parent.Insert(new SetAVel(parent.ActivatedWorkSpaceData));
        }

        private void AddSetOmigaNode()
        {
            parent.Insert(new SetOmiga(parent.ActivatedWorkSpaceData));
        }

        private void AddSetGroupNode()
        {
            parent.Insert(new SetGroup(parent.ActivatedWorkSpaceData));
        }

        private void AddSetLayerNode()
        {
            parent.Insert(new SetLayer(parent.ActivatedWorkSpaceData));
        }

        private void AddSetVisibilityNode()
        {
            parent.Insert(new SetVisibility(parent.ActivatedWorkSpaceData));
        }
        #endregion

        #region graphics
        private void AddLoadImageNode()
        {
            parent.Insert(new LoadImage(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadImageGroupNode()
        {
            parent.Insert(new LoadImageGroup(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadParticleNode()
        {
            parent.Insert(new LoadParticle(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadAnimationNode()
        {
            parent.Insert(new LoadAnimation(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadFXNode()
        {
            parent.Insert(new LoadFX(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadTextureNode()
        {
            parent.Insert(new LoadTexture(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadFontNode()
        {
            parent.Insert(new LoadFont(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadFontImageNode()
        {
            parent.Insert(new LoadFontImage(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadTTFNode()
        {
            parent.Insert(new LoadTTF(parent.ActivatedWorkSpaceData));
        }

        private void AddSetFontStateNode()
        {
            parent.Insert(new SetFontState(parent.ActivatedWorkSpaceData));
        }
        private void AddSetImageStateNode()
        {
            parent.Insert(new SetImageState(parent.ActivatedWorkSpaceData));
        }
        private void AddSetAnimationStateNode()
        {
            parent.Insert(new SetAnimationState(parent.ActivatedWorkSpaceData));
        }
        private void AddSetImageCenterNode()
        {
            parent.Insert(new SetImageCenter(parent.ActivatedWorkSpaceData));
        }
        private void AddSetAnimationCenterNode()
        {
            parent.Insert(new SetAnimationCenter(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region audio
        private void AddLoadSENode()
        {
            parent.Insert(new LoadSE(parent.ActivatedWorkSpaceData));
        }

        private void AddPlaySENode()
        {
            parent.Insert(new PlaySE(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadBGMNode()
        {
            parent.Insert(new LoadBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayBGMNode()
        {
            parent.Insert(new PlayBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddSetBGMVolumeNode()
        {
            parent.Insert(new SetBGMVolume(parent.ActivatedWorkSpaceData));
        }

        private void AddPauseBGMNode()
        {
            parent.Insert(new PauseBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddResumeBGMNode()
        {
            parent.Insert(new ResumeBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddStopBGMNode()
        {
            parent.Insert(new StopBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddSetPaceNode()
        {
            parent.Insert(new SetPace(parent.ActivatedWorkSpaceData), true, ["LuaSTG ExPlus", "LuaSTG Sub"]);
        }

        private void AddFadeOutBGM()
        {
            parent.Insert(new FadeOutBGM(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region render
        private void AddOnRenderNode()
        {
            var o = new OnRender(parent.ActivatedWorkSpaceData);
            o.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData));
            parent.Insert(o);
        }

        private void AddSetViewModeNode()
        {
            parent.Insert(new SetViewMode(parent.ActivatedWorkSpaceData));
        }

        private void AddRenderNodeNode()
        {
            parent.Insert(new RenderNode(parent.ActivatedWorkSpaceData));
        }

        private void AddRenderRectNode()
        {
            parent.Insert(new RenderRect(parent.ActivatedWorkSpaceData));
        }

        private void AddR4VNode()
        {
            parent.Insert(new Render4V(parent.ActivatedWorkSpaceData));
        }

        private void AddCreateRenderTargetNode()
        {
            parent.Insert(new CreateRenderTarget(parent.ActivatedWorkSpaceData));
        }

        private void AddRenderTargetNode()
        {
            parent.Insert(new RenderTarget(parent.ActivatedWorkSpaceData));
        }

        private void AddPostEffectCaptureNode()
        {
            parent.Insert(new PostEffectCapture(parent.ActivatedWorkSpaceData));
        }

        private void AddPostEffectNode()
        {
            parent.Insert(new PostEffect(parent.ActivatedWorkSpaceData));
        }

        private void AddRenderTextNode()
        {
            parent.Insert(new RenderText(parent.ActivatedWorkSpaceData));
        }

        private void AddRenderTTFNode()
        {
            parent.Insert(new RenderTTF(parent.ActivatedWorkSpaceData));
        }

        private void AddParticleFire()
        {
            parent.Insert(new ParticleFire(parent.ActivatedWorkSpaceData));
        }

        private void AddParticleStop()
        {
            parent.Insert(new ParticleStop(parent.ActivatedWorkSpaceData));
        }
        #endregion

        #region background
        private void AddSet3DNode()
        {
            parent.Insert(new Set3D(parent.ActivatedWorkSpaceData));
        }

        private void AddCameraSetterNode()
        {
            parent.Insert(new CameraSetter(parent.ActivatedWorkSpaceData));
        }

        private void AddDefineBackgroundNode()
        {
            TreeNode bgdef = new BackgroundDefine(parent.ActivatedWorkSpaceData);
            bgdef.AddChild(new BackgroundInit(parent.ActivatedWorkSpaceData));
            var bginit = bgdef.Children.Last();
            bgdef.AddChild(new BGOnFrame(parent.ActivatedWorkSpaceData));
            bgdef.AddChild(new BGOnRender(parent.ActivatedWorkSpaceData));
            var bgrend = bgdef.Children.Last();

            bginit.AddChild(new Set3D(parent.ActivatedWorkSpaceData, "\"eye\"", "0,0,0"));
            bginit.AddChild(new Set3D(parent.ActivatedWorkSpaceData, "\"at\"", "0,0,0"));
            bginit.AddChild(new Set3D(parent.ActivatedWorkSpaceData, "\"up\"", "0,0,0"));
            bginit.AddChild(new Set3D(parent.ActivatedWorkSpaceData, "\"z\"", "0,0"));
            bginit.AddChild(new Set3D(parent.ActivatedWorkSpaceData, "\"fovy\"", "0"));
            bginit.AddChild(new Set3D(parent.ActivatedWorkSpaceData, "\"fog\"", "0,0,Color(255, 255, 255, 255)"));

            bgrend.AddChild(new SetViewMode(parent.ActivatedWorkSpaceData, "3d"));
            bgrend.AddChild(new BGWarp(parent.ActivatedWorkSpaceData, "Capture"));
            bgrend.AddChild(new RenderClear(parent.ActivatedWorkSpaceData));
            bgrend.AddChild(new Comment(parent.ActivatedWorkSpaceData, "Start code here", "false"));
            bgrend.AddChild(new BGWarp(parent.ActivatedWorkSpaceData, "Apply"));
            bgrend.AddChild(new SetViewMode(parent.ActivatedWorkSpaceData, "world"));

            parent.Insert(bginit);
            parent.Insert(bgrend);
            parent.Insert(bgdef);
        }

        private void AddBGOnFrameNode()
        {
            var o = new BGOnFrame(parent.ActivatedWorkSpaceData);
            parent.Insert(o);
        }

        private void AddBGOnRenderNode()
        {
            var o = new BGOnRender(parent.ActivatedWorkSpaceData);
            o.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData));
            parent.Insert(o);
        }

        private void AddRenderClearNode()
        {
            parent.Insert(new RenderClear(parent.ActivatedWorkSpaceData));
        }

        private void AddBGWarpNode()
        {
            parent.Insert(new BGWarp(parent.ActivatedWorkSpaceData));
        }

        private void AddRender4V3DNode()
        {
            parent.Insert(new Render4V3D(parent.ActivatedWorkSpaceData));
        }
        #endregion

        #region player
        private void AddPlayerDefineNode()
        {
            var pl = new PlayerDefine(parent.ActivatedWorkSpaceData);
            pl.AddChild(new PlayerInit(parent.ActivatedWorkSpaceData));
            var pl2 = pl.Children.Last();
            pl2.AddChild(new Comment(parent.ActivatedWorkSpaceData, "Add Option List Here", "false"));
            pl2.AddChild(new SetPlayerWalkImageSystem(parent.ActivatedWorkSpaceData));
            pl2.AddChild(new PlayerSpeed(parent.ActivatedWorkSpaceData));
            pl2.AddChild(new PlayerProtect(parent.ActivatedWorkSpaceData));
            parent.Insert(pl2);
            pl.AddChild(new PlayerFrame(parent.ActivatedWorkSpaceData));
            var plF = pl.Children.Last();
            plF.AddChild(new PlayerClassFrame(parent.ActivatedWorkSpaceData));
            parent.Insert(plF);
            pl.AddChild(new PlayerRender(parent.ActivatedWorkSpaceData));
            var plR = pl.Children.Last();
            plR.AddChild(new PlayerClassRender(parent.ActivatedWorkSpaceData));
            plR.AddChild(new PlayerOptionRender(parent.ActivatedWorkSpaceData));
            parent.Insert(plR);
            pl.AddChild(new PlayerShoot(parent.ActivatedWorkSpaceData));
            var pl3 = pl.Children.Last();
            pl3.AddChild(new PlayerNextShoot(parent.ActivatedWorkSpaceData));
            pl3.AddChild(new PlaySE(parent.ActivatedWorkSpaceData, "\"plst00\"", "0.3", "self.x/1024", "false"));
            parent.Insert(pl3);
            pl.AddChild(new PlayerSpell(parent.ActivatedWorkSpaceData));
            var pl4 = pl.Children.Last();
            pl4.AddChild(new PlayerNextSpell(parent.ActivatedWorkSpaceData));
            pl4.AddChild(new PlaySE(parent.ActivatedWorkSpaceData, "\"nep00\"", "0.8", "self.x/1024", "false"));
            pl4.AddChild(new PlayerSpellMask(parent.ActivatedWorkSpaceData));
            parent.Insert(pl4);
            pl.AddChild(new PlayerSpecial(parent.ActivatedWorkSpaceData));
            var pl5 = pl.Children.Last();
            pl5.AddChild(new PlayerNextSP(parent.ActivatedWorkSpaceData));
            pl5.AddChild(new PlaySE(parent.ActivatedWorkSpaceData, "\"slash\"", "0.8", "self.x/1024", "false"));
            parent.Insert(pl5);
            parent.Insert(pl);
        }
        private void AddPlayerOptionListNode()
        {
            var listOpt = new PlayerOptionList(parent.ActivatedWorkSpaceData);
            for (int i = 1; i <= 4; i++)
            {
                var powerOpt = new PlayerOptionPower(parent.ActivatedWorkSpaceData);
                for (int k = 0; k < i; k++)
                {
                    powerOpt.AddChild(new PlayerOptionPosition(parent.ActivatedWorkSpaceData));
                }
                listOpt.AddChild(powerOpt);
            }
            parent.Insert(listOpt);
        }
        private void AddPlayerOptionRenderNode()
        {
            parent.Insert(new PlayerOptionRender(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerOptionForEachNode()
        {
            parent.Insert(new PlayerOptionForEach(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerSpeedNode()
        {
            parent.Insert(new PlayerSpeed(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerWalkImageNode()
        {
            parent.Insert(new SetPlayerWalkImageSystem(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerProtectNode()
        {
            parent.Insert(new PlayerProtect(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerNextShootNode()
        {
            parent.Insert(new PlayerNextShoot(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerNextSpellNode()
        {
            parent.Insert(new PlayerNextSpell(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerNextSpecialNode()
        {
            parent.Insert(new PlayerNextSP(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerClassRenderNode()
        {
            parent.Insert(new PlayerClassRender(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerClassFrameNode()
        {
            parent.Insert(new PlayerClassFrame(parent.ActivatedWorkSpaceData));
        }
        private void AddPlayerSpellMaskNode()
        {
            parent.Insert(new PlayerSpellMask(parent.ActivatedWorkSpaceData));
        }
        private void AddPlayerFindTargetNode()
        {
            parent.Insert(new PlayerFindTarget(parent.ActivatedWorkSpaceData));
        }
        private void AddPlayerSimpleBulletNode()
        {
            parent.Insert(new PlayerSimpleBullet(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayerDefineBulletNode()
        {
            var pl = new PlayerBulletDefine(parent.ActivatedWorkSpaceData);
            pl.AddChild(new PlayerBulletInit(parent.ActivatedWorkSpaceData));
            pl.AddChild(new PlayerBulletFrame(parent.ActivatedWorkSpaceData));
            var pl2 = pl.Children.Last();
            pl2.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData, "frame"));
            pl.AddChild(new PlayerBulletRender(parent.ActivatedWorkSpaceData));
            var pl3 = pl.Children.Last();
            pl3.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData, "render"));
            pl.AddChild(new PlayerBulletColli(parent.ActivatedWorkSpaceData));
            var pl4 = pl.Children.Last();
            pl4.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData, "colli"));
            pl.AddChild(new PlayerBulletKill(parent.ActivatedWorkSpaceData));
            var pl5 = pl.Children.Last();
            //pl5.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData, "kill"));
            //pl5.AddChild(new Del(parent.ActivatedWorkSpaceData, "self", "false"));
            pl.AddChild(new PlayerBulletDel(parent.ActivatedWorkSpaceData));
            var pl6 = pl.Children.Last();
            pl6.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData, "del"));
            parent.Insert(pl);
        }

        private void AddCreatePlayerBulletNode()
        {
            parent.Insert(new CreatePlayerBullet(parent.ActivatedWorkSpaceData));
        }
        #endregion

        #region game data
        private void AddSetGameLifeNode()
        {
            parent.Insert(new SetGameLife(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGameBombNode()
        {
            parent.Insert(new SetGameBomb(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGameLifePieceNode()
        {
            parent.Insert(new SetGameLifePiece(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGameBombPieceNode()
        {
            parent.Insert(new SetGameBombPiece(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGamePointNode()
        {
            parent.Insert(new SetGamePoint(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGamePowerNode()
        {
            parent.Insert(new SetGamePower(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGameFaithNode()
        {
            parent.Insert(new SetGameFaith(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGameGrazeNode()
        {
            parent.Insert(new SetGameGraze(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGameScoreNode()
        {
            parent.Insert(new SetGameScore(parent.ActivatedWorkSpaceData));
        }
        private void AddSetGameKillPlayerNode()
        {
            parent.Insert(new SetGameKillPlayer(parent.ActivatedWorkSpaceData));
        }
        private void AddSetSplash()
        {
            parent.Insert(new SetSplash(parent.ActivatedWorkSpaceData));
        }
        #endregion
    }
}
