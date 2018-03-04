using UnityEngine;
using UniRx;
using DG.Tweening;

namespace RedChild
{
    public class EnemyAnim : BaseEnemyComponent
    {
        private Animator m_animator;
        int scream;
        int basicAttack;
        int clawAttack;
        int flameAttack;
        int defend;
        int getHit;
        int sleep;
        int walk;
        int run;
        int takeOff;
        int flyFlameAttack;
        int flyForward;
        int flyGlide;
        int land;
        int die;
        int idle02;

        protected override void OnStart()
        {
            m_animator = GetComponent<Animator>();
            scream = Animator.StringToHash("Scream");
            basicAttack = Animator.StringToHash("Basic Attack");
            clawAttack = Animator.StringToHash("Claw Attack");
            flameAttack = Animator.StringToHash("Flame Attack");
            defend = Animator.StringToHash("Defend");
            getHit = Animator.StringToHash("Get Hit");
            sleep = Animator.StringToHash("Sleep");
            walk = Animator.StringToHash("Walk");
            run = Animator.StringToHash("Run");
            takeOff = Animator.StringToHash("Take Off");
            flyFlameAttack = Animator.StringToHash("Fly Flame Attack");
            flyForward = Animator.StringToHash("Fly Forward");
            flyGlide = Animator.StringToHash("Fly Glide");
            land = Animator.StringToHash("Land");
            die = Animator.StringToHash("Die");
            idle02 = Animator.StringToHash("Idle02");

            m_core.m_isStart.Where(_ => _).Subscribe(_ => TakeOff()).AddTo(gameObject);

            m_core.m_damge.Where(_ => _ > 0f).Subscribe(_ =>
            {
                GetHit();
                DOVirtual.DelayedCall(.2f, () => TakeOff());
            }).AddTo(gameObject);

            m_core.m_attack.Where(_ => _ > 0f).Subscribe(_ => { FlyFlameAttack(); });

            m_core.m_PlayerCore.m_isJudge.Where(_ => _ == 3).Subscribe(_ => 
            {
                Die();
                DOVirtual.DelayedCall(1f, () => m_core.m_PlayerCore.m_isJudge.Value = 4);
                DOVirtual.DelayedCall(1.1f, () => Destroy(this.gameObject));
            }).AddTo(gameObject);
        }

        public void Scream()
        {
            m_animator.SetTrigger(scream);
        }

        public void BasicAttack()
        {
            m_animator.SetTrigger(basicAttack);
        }

        public void ClawAttack()
        {
            m_animator.SetTrigger(clawAttack);
        }

        public void FlameAttack()
        {
            m_animator.SetTrigger(flameAttack);
        }

        public void Defend()
        {
            m_animator.SetTrigger(defend);
        }

        public void GetHit()
        {
            m_animator.SetTrigger(getHit);
        }

        public void Sleep()
        {
            m_animator.SetTrigger(sleep);
        }

        public void Walk()
        {
            m_animator.SetTrigger(walk);
        }

        public void Run()
        {
            m_animator.SetTrigger(run);
        }

        public void TakeOff()
        {
            m_animator.SetTrigger(takeOff);
        }

        public void FlyFlameAttack()
        {
            m_animator.SetTrigger(flyFlameAttack);
        }

        public void FlyForward()
        {
            m_animator.SetTrigger(flyForward);
        }

        public void FlyGlide()
        {
            m_animator.SetTrigger(flyGlide);
        }

        public void Land()
        {
            m_animator.SetTrigger(land);
        }

        public void Die()
        {
            m_animator.SetTrigger(die);
        }

        public void Idle02()
        {
            m_animator.SetTrigger(idle02);
        }
    }
}
