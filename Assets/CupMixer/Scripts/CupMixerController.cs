using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CupMixer
{
    public class CupMixerController : MonoBehaviour
    {
        private int count;
        [SerializeField] private Text counterText;
        [SerializeField] private GameObject[] cups;
        [SerializeField] private int movementTime;
        public Image die;
        public float MixerUp;
        private bool finishingRevealing;

        [SerializeField] private int countMixes = 3;
        // [SerializeField] private Sprite standing;
        // [SerializeField] private Sprite laying;

        void Start()
        {
            count = 0;
            counterText.text += count;
            die.enabled = false;
            StartCoroutine(CupMixer());
        }

        public IEnumerator CupMixer(float wfs = 0.3f)
        {
            Vector3[] ts = new Vector3[cups.Length];
            for (int i = 0; i < cups.Length; i++)
            {
                ts[i] = cups[i].transform.position.Copy();
            }

            foreach (GameObject c in cups)
            {
                StartCoroutine(RevealCup(c));
            }

            yield return new WaitForSecondsRealtime(1);

            for (int i = 0; i < countMixes; i++)
            {
                Vector3[] randoms = ts.Randomize().Copy();
                foreach (var c in cups)
                {
                    StartCoroutine(c.TranslateOverTime(movementTime, Vector3.up * MixerUp));
                    Debug.Log(Vector3.up * MixerUp);
                    c.Button().interactable = false;
                }

                yield return new WaitForSecondsRealtime(wfs);
                for (int k = 0; k < ts.Length; k++)
                {
                    Vector3 movement = new Vector3(randoms[k].x - cups[k].transform.position.x, -MixerUp);
                    Debug.Log(movement);
                    StartCoroutine(cups[k].TranslateOverTime(movementTime, movement));
                    yield return new WaitForSecondsRealtime(0.5f);
                }

                yield return new WaitForSecondsRealtime(wfs);
            }

            foreach (var c in cups)
            {
                c.Button().interactable = true;
            }
        }

        public IEnumerator RevealCup(GameObject cup)
        {
            finishingRevealing = false;
            cup.SetActive(true);
            var t = cup.gameObject.transform.position;
            cup.gameObject.transform.position = t;
            die.enabled = true;
            yield return new WaitForSecondsRealtime(1);

            die.enabled = false;
            finishingRevealing = true;
        }

        public void UpdateCounter()
        {
            counterText.text = "Угадано " + count;
        }

        public void ClickedCup(GameObject g)
        {
            StartCoroutine(RevealCup(g));
            if (g.HasComponentInChildren<Image>()) // Если верная чашка.
            {
                count++;
                UpdateCounter();
            }
            else
            {
                count = 0;
                UpdateCounter();
            }
        }
    }
}