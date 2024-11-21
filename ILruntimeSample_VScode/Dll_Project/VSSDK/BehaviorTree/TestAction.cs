using Kurisu.AkiBT;
using UnityEngine;
using UnityEngine.AI;

namespace BTAction
{
    public class TestAction : DllActionBase
    {
        public Animator animator;
        // 导航网格代理
        public NavMeshAgent navMeshAgent;
        // 导航系统计算的路径
        private NavMeshPath navMeshPath = null;
        private bool bFirst
        {
            get
            {
                return BaseAction.boolDatas[0].Value;
            }
            set
            {
                BaseAction.boolDatas[0].Value = value;
            }
        }

        public bool bOver
        {
            get
            {
                return BaseAction.boolDatas[1].Value;
            }
            set
            {
                BaseAction.boolDatas[1].Value = value;
            }
        }
        public Vector3 targetPos
        {
            get
            {
                return BaseAction.vector3Datas[0].Value;
            }
        }

        public float remainDistance
        {
            get
            {
                var RawDistance = Vector3.Distance(navMeshAgent.transform.position, targetPos);
                if (RawDistance <0.1f)
                    return navMeshAgent.remainingDistance;
                return RawDistance;
            }
        }
        public override void Abort()
        {

        }
        public override void Init()
        {
            if(BaseAction.objectDatas.Count > 0)
            {
                animator = (BaseAction.objectDatas[0].Value as GameObject).GetComponent<Animator>();
            }
            if (BaseAction.objectDatas.Count > 1)
            {
                navMeshAgent = (BaseAction.objectDatas[1].Value as GameObject).GetComponent<NavMeshAgent>();
            }
            //if (BaseAction.vector3Datas.Count > 0)
            //{
            //    targetPos = BaseAction.vector3Datas[0].Value;
            //}
        }
        public override void Awake()
        {

        }

        public override bool CanUpdate()
        {
            return true;
        }

        public override void Start()
        {
            navMeshPath = new NavMeshPath();
        }

        public override Status OnUpdate()
        {
            Debug.Log("test action " + this.GetHashCode());
            Debug.Log("test action bOver -> " + bOver + " " +this.GetHashCode());

            if (bOver)
            {
                return Status.Success;
            }

            if (bFirst)
            {
                // 移动角色
                Debug.Log("test action Start targetPos -> " + targetPos + " " + this.GetHashCode());
                navMeshAgent.SetDestination(targetPos);
                animator.SetFloat("speed", 0.5f);
                // 计算路径
                NavMesh.CalculatePath(navMeshAgent.transform.position, targetPos, NavMesh.AllAreas, navMeshPath);
                navMeshAgent.transform.LookAt(targetPos);
                bFirst = false;
            }

            if (remainDistance < 0.5f)
            {
                animator.SetFloat("speed", remainDistance);

                Debug.Log("test action Start remainingDistance -> " + remainDistance + " " + this.GetHashCode());
            }
            else
            {
                var speed = animator.GetFloat("speed");
                if(speed < 0.5f)
                {
                    animator.SetFloat("speed", 0.5f);
                }
                Debug.Log("test action Start remainingDistance -> " + remainDistance + " " + this.GetHashCode());
            }
            if (remainDistance <= 0)
            {
                bOver = true;
                Debug.Log("test action bOver2222 -> " + bOver + " " + this.GetHashCode());
                return Status.Success;
            }
            else
            {
                // 绘制路径
                for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
                {
                    Debug.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i + 1], Color.red);
                }
                return Status.Running;
            }

        }
    }
}
