using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace RedChild
{
    public class PlayerRc : BasePlayerComponent
    {
        [SerializeField] private Transform m_parent;
        [SerializeField] private float m_recastTime = 3f;
        [SerializeField] private Transform m_bow;

        [SerializeField] private TorusRotate m_torusRotate;

        private readonly string m_path = "Rc/";
        public struct RcData
        {
            public string m_name;
            public Vector3 m_vec;

            public RcData(string name, Vector3 vec)
            {
                m_name = name;
                m_vec = vec;
            }
        }
        private RcData[] m_rcData = {
            new RcData("Sphere_1", new Vector3(0f, .3f, 1f)),
            new RcData("Sphere_2", new Vector3(0.7f, .3f, 0.7f)),
            new RcData("Sphere_3", new Vector3(1f, .3f, 0f)),
            new RcData("Sphere_4", new Vector3(0.7f, .3f, -0.7f)),
            new RcData("Sphere_5", new Vector3(0f, .3f, -1f)),
            new RcData("Sphere_6", new Vector3(-0.7f, .3f, -0.7f)),
            new RcData("Sphere_7", new Vector3(-1f, .3f, 0f)),
            new RcData("Sphere_8", new Vector3(-0.7f, .3f, 0.7f)),
        };

        readonly private Vector3 BASE_SCALE = new Vector3(.5f, .5f, .5f);

        protected override void OnStart()
        {
            m_core.m_isTorus.Subscribe(_ => { TorusSetup(_); }).AddTo(this.gameObject);

            m_core.RecastRcData.
                Where(_ => _ != RcList.none).
                Subscribe(_ => {
                StartCoroutine(RespawnRc(_));
            });

            m_core.m_isJudge.Where(_ => _ == 1).Subscribe(_ => StartCoroutine(JudgeArrowActibate()));
        }

        private void TorusSetup(bool isTrue = true)
        {
            m_parent.gameObject.SetActive(isTrue);
        }

        IEnumerator RespawnRc(RcList rctype)
        {
            yield return new WaitForSeconds(m_recastTime);

            var idx = (int)rctype;
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(m_path + m_rcData[idx].m_name), m_parent);
            obj.transform.localScale = Vector3.zero;
            obj.transform.localPosition = m_rcData[idx].m_vec;
            obj.transform.DOScale(BASE_SCALE, .5f);

        }

        private IEnumerator JudgeArrowActibate()
        {
            m_torusRotate.StopRotate();

            m_parent.DOLocalRotate(new Vector3(0f, -90f, 90f), 3f).SetEase(Ease.OutCubic);
            yield return m_parent.DOMove(m_bow.transform.position + (Vector3.forward * .5f), 3f).SetEase(Ease.OutCubic).WaitForCompletion();

            m_parent.DOScale(Vector3.one * 0.1f, 2.5f).SetEase(Ease.InCirc);
            m_parent.DOLocalRotate(new Vector3(1080f, 0f, 0f), 2.4f).SetRelative().SetEase(Ease.InCubic);
            yield return new WaitForSeconds(1.5f);
            m_core.m_isJudge.Value = 2;
            yield return new WaitForSeconds(1f);
            m_parent.DOMove(m_bow.transform.position + new Vector3(0f, 100f, 0f), 3f);

            DOVirtual.DelayedCall(20f, () => m_parent.gameObject.SetActive(false));
        }
    }
}
