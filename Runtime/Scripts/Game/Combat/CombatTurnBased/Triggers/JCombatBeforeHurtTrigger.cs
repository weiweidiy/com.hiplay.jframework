using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 受伤之前触发
    /// </summary>
    public class JCombatBeforeHurtTrigger : JCombatTriggerBase
    {
        List<IJCombatCasterTargetableUnit> targets;
        public JCombatBeforeHurtTrigger(float[] args, IJCombatTargetsFinder finder) : base(args, finder)
        {
        }

        protected override int GetValidArgsCount()
        {
            return 0; // 不需要参数
        }

        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);

            if(finder == null)
            {
                throw new System.Exception("JCombatBeforeHurtTrigger requires a finder to be set.");
            }

            var executeArgs = finder.GetTargetsData();
            targets = executeArgs.TargetUnits;

            foreach (var target in targets)
            {
                target.onBeforeHurt += OnBeforeHurt;
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (targets == null)
                return;
            foreach (var targetable in targets)
            {
                if (targetable != null)
                {
                    targetable.onBeforeHurt -= OnBeforeHurt;
                }
            }
            targets.Clear();
        }

        private void OnBeforeHurt(IJCombatTargetable targetable, IJCombatDamageData data)
        {
            //executeArgs.Clear();
            executeArgs.DamageData = data;
            executeArgs.TargetUnits = new List<IJCombatCasterTargetableUnit> { targetable as IJCombatCasterTargetableUnit };
            TriggerOn(executeArgs);
        }




    }


}
