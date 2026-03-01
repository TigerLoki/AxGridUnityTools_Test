using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;
using UnityEngine.UI;

namespace TASK3.Scripts
{
    public class SlotReelView : MonoBehaviourExtBind
    {
        [SerializeField] private int reelIndex;
        [SerializeField] private RectTransform top;
        [SerializeField] private RectTransform middle;
        [SerializeField] private RectTransform bottom;
        
        public int ReelIndex => reelIndex;

        [Header("Symbol Sprites")]
        [SerializeField] private Sprite[] symbols;
        
        [Header("Debug")]
        [SerializeField] private bool logResult = true;
        
        private float MaxSpeed => Settings.Model.GetFloat("MaxSpeed");
        private float AccelerationTime => Settings.Model.GetFloat("AccelerationTime");
        private float DecelerationTime => Settings.Model.GetFloat("DecelerationTime");
        private float BounceTime => Settings.Model.GetFloat("BounceTime");

        private float itemHeight;
        private float currentSpeed;

        private bool spinning;
        private bool forwardSpin;
        private bool stopping;
        private bool allowShift;

        private Image topImg;
        private Image midImg;
        private Image botImg;

        private int topId;
        private int midId;
        private int botId;

        private int targetId;
        private bool forceTargetNext;

        private float stopShiftAccumulator;

        [OnStart]
        private void Init()
        {
            topImg = top.GetComponent<Image>();
            midImg = middle.GetComponent<Image>();
            botImg = bottom.GetComponent<Image>();
            
            itemHeight = middle.rect.height;

            SetY(top, itemHeight);
            SetY(middle, 0f);
            SetY(bottom, -itemHeight);

            topId = GetRandomId();
            midId = GetRandomId();
            botId = GetRandomId();
            
            Settings.Model.Set($"SymbolsCount{reelIndex}", symbols.Length);

            ApplySymbol(topImg, topId);
            ApplySymbol(midImg, midId);
            ApplySymbol(botImg, botId);
        }

        [OnUpdate]
        private void Tick()
        {
            if (!spinning || !forwardSpin) return;

            float delta = currentSpeed * Time.deltaTime;

            Move(top, delta);
            Move(middle, delta);
            Move(bottom, delta);

            if (!allowShift) return;

            while (GetY(bottom) <= -itemHeight)
                ShiftOnce();
        }

        [Bind("StartReel")]
        private void StartReel(int index)
        {
            if (index != reelIndex) return;

            spinning = true;
            stopping = false;

            forwardSpin = false;
            allowShift = false;

            targetId = Settings.Model.GetInt($"Win{reelIndex}");

            if (logResult)
                Debug.Log($"[Reel {reelIndex}] Predetermined result = {targetId}");

            float backKick = itemHeight * 0.1f;
            float prev = 0f;

            Path = new CPath();

            Path
                .EasingQuadEaseOut(
                    0.12f,
                    0f,
                    backKick,
                    v =>
                    {
                        float d = v - prev;
                        prev = v;
                        OffsetAll(d);
                    })
                .Action(() =>
                {
                    forwardSpin = true;
                    allowShift = true;
                })
                .EasingQuadEaseIn(
                    AccelerationTime,
                    0f,
                    MaxSpeed,
                    v => currentSpeed = v
                );
        }

        [Bind("StopReel")]
        private void StopReel(int index)
        {
            if (index != reelIndex || stopping) return;

            stopping = true;
            spinning = false;

            forwardSpin = true;
            allowShift = true;

            float mainPart = Mathf.Max(DecelerationTime * 0.5f, DecelerationTime - BounceTime);

            float remainder = Mathf.Repeat(GetY(middle), itemHeight);
            float distance = remainder + 3f * itemHeight;

            float prevDistance = 0f;
            bool targetArmed = false;

            stopShiftAccumulator = 0f;

            float overshoot1 = itemHeight * 0.08f;
            float overshoot2 = itemHeight * 0.04f;
            float overshoot3 = itemHeight * 0.02f;

            float localPrev = 0f;

            Path = new CPath();

            Path
                .EasingQuadEaseOut(
                    mainPart,
                    0f,
                    distance,
                    v =>
                    {
                        float delta = v - prevDistance;
                        prevDistance = v;

                        Move(top, delta);
                        Move(middle, delta);
                        Move(bottom, delta);

                        stopShiftAccumulator += delta;

                        while (stopShiftAccumulator >= itemHeight)
                        {
                            stopShiftAccumulator -= itemHeight;

                            if (!targetArmed && prevDistance >= distance - 2f * itemHeight)
                            {
                                forceTargetNext = true;
                                targetArmed = true;
                            }

                            ShiftOnce();
                        }
                    })
                .Action(() =>
                {
                    allowShift = false;
                    forwardSpin = false;
                    currentSpeed = 0f;

                    SetY(top, itemHeight);
                    SetY(middle, 0f);
                    SetY(bottom, -itemHeight);
                })
                .EasingQuadEaseOut(
                    BounceTime * 0.4f,
                    0f,
                    -overshoot1,
                    v =>
                    {
                        float d = v - localPrev;
                        localPrev = v;
                        OffsetAll(d);
                    })
                .EasingQuadEaseIn(
                    BounceTime * 0.3f,
                    -overshoot1,
                    overshoot2,
                    v =>
                    {
                        float d = v - localPrev;
                        localPrev = v;
                        OffsetAll(d);
                    })
                .EasingQuadEaseOut(
                    BounceTime * 0.2f,
                    overshoot2,
                    -overshoot3,
                    v =>
                    {
                        float d = v - localPrev;
                        localPrev = v;
                        OffsetAll(d);
                    })
                .EasingQuadEaseIn(
                    BounceTime * 0.1f,
                    -overshoot3,
                    0f,
                    v =>
                    {
                        float d = v - localPrev;
                        localPrev = v;
                        OffsetAll(d);
                    })
                .Action(() =>
                {
                    stopping = false;

                    if (logResult)
                        Debug.Log($"[Reel {reelIndex}] STOPPED: top={topId}, mid={midId}, bot={botId}");
                    
                    Settings.Invoke("ReelStopped");
                });
        }

        private void ShiftOnce()
        {
            RectTransform oldBottom = bottom;
            Image oldBottomImg = botImg;

            bottom = middle;
            botImg = midImg;
            botId = midId;

            middle = top;
            midImg = topImg;
            midId = topId;

            top = oldBottom;
            topImg = oldBottomImg;

            SetY(top, GetY(middle) + itemHeight);

            int newId = forceTargetNext ? targetId : GetRandomId();
            forceTargetNext = false;

            topId = newId;
            ApplySymbol(topImg, topId);
        }

        private int GetRandomId()
        {
            if (symbols == null || symbols.Length == 0) return 1;
            return Random.Range(1, symbols.Length + 1);
        }

        private void ApplySymbol(Image img, int id)
        {
            int index = Mathf.Clamp(id - 1, 0, symbols.Length - 1);
            img.sprite = symbols[index];
            img.gameObject.name = $"Symbol{id}";
        }

        private void OffsetAll(float delta)
        {
            top.anchoredPosition += Vector2.up * delta;
            middle.anchoredPosition += Vector2.up * delta;
            bottom.anchoredPosition += Vector2.up * delta;
        }
        
        private static void Move(RectTransform rt, float delta)
        {
            rt.anchoredPosition -= Vector2.up * delta;
        }

        private static float GetY(RectTransform rt)
        {
            return rt.anchoredPosition.y;
        }

        private static void SetY(RectTransform rt, float y)
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, y);
        }
    }
}