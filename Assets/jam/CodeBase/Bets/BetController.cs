using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Economy;
using jam.CodeBase.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jam.CodeBase.Bets
{
    public class BetController
    {
        public Action<float> OnChangeDieCoefficient;
        public Action<float> OnChangeAliveCoefficient;

        public float CurrentBet => AliveBet + DieBet;
        public float AliveBet;
        public float DieBet;
        public float AliveBetCoefficient;
        public float DieBetCoefficient;

        private Vector2 BetBaseDieCoefficientRange;
        private Vector2 BetBasAliveCoefficientRange;

        private const int BetTime = 30;

        private float _smoothAlive;
        private float _smoothDie;
        private const float CoeffSmooth = 1f;

        public BetController()
        {
            var baseEconomyTag = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();
            BetBaseDieCoefficientRange = baseEconomyTag.BetBaseDieCoefficientRange;
            BetBasAliveCoefficientRange = baseEconomyTag.BetBaseAliveCoefficientRange;

            AliveBet = 0;
            DieBet = 0;

            AliveBetCoefficient = BetBasAliveCoefficientRange.x;
            DieBetCoefficient = BetBaseDieCoefficientRange.x;
        }

        public void BetToDie(float bet)
        {
            if (bet <= 0) return;

            DieBet += bet;
            UpdateCoefficients();
        }

        public void BetToAlive(float bet)
        {
            if (bet <= 0) return;

            AliveBet += bet;
            UpdateCoefficients();
        }

        public async UniTask BetProcess()
        {
            G.Menu.ViewService.ShowView<BetPopup>();

            var cfg = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();

            float baseBet = Random.Range(cfg.BaseBet.x, cfg.BaseBet.y);
            float baseMoney = G.Economy.CurrentMoney;
            float totalBetMultiplier =
                Random.Range(cfg.PercentsRangeFromPlayerMoney.x, cfg.PercentsRangeFromPlayerMoney.y);

            float totalAdditionalBet = baseMoney * totalBetMultiplier;
            float totalPool = totalAdditionalBet + baseBet;

            float[] tickBets = BuildTickBets(totalPool);

            int tick = 0;
            int time = BetTime;

            while (time > 0)
            {
                MakeSecondBet(tickBets[tick]);

                await UniTask.WaitForSeconds(1);

                tick++;
                time--;
            }

            await UniTaskHelper.SmartWaitSeconds(5);

            G.Menu.ViewService.HideView<BetPopup>();
        }

        private float[] BuildTickBets(float totalPool)
        {
            float[] weights = new float[BetTime];
            float totalWeight = 0f;

            for (int i = 0; i < BetTime; i++)
            {
                float w = GetTimeWeight(i);
                weights[i] = w;
                totalWeight += w;
            }

            float[] tickBets = new float[BetTime];
            for (int i = 0; i < BetTime; i++)
            {
                tickBets[i] = totalPool * (weights[i] / totalWeight);
            }

            return tickBets;
        }

        private float GetTimeWeight(int t)
        {
            float x = (float)t / BetTime;
            float peak = 0.5f;
            float d = x - peak;
            return 1f - 4f * d * d;
        }

        private void MakeSecondBet(float bet)
        {
            if (bet <= 0f)
                return;

            var cfg = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();

            float r = Random.value;

            float aliveAdd = 0f;
            float dieAdd = 0f;

            if (r < 0.25f)
            {
                aliveAdd = bet;
                dieAdd = 0f;
            }
            else if (r < 0.5f)
            {
                aliveAdd = 0f;
                dieAdd = bet;
            }
            else
            {
                float aliveShare = Random.Range(cfg.AliveBiddersProporionRange.x, cfg.AliveBiddersProporionRange.y);
                float dieShare = Random.Range(cfg.DieBiddersProporionRange.x, cfg.DieBiddersProporionRange.y);

                float totalShare = aliveShare + dieShare;
                if (totalShare <= 0f)
                {
                    aliveAdd = bet * 0.5f;
                    dieAdd = bet * 0.5f;
                }
                else
                {
                    aliveAdd = bet * (aliveShare / totalShare);
                    dieAdd = bet * (dieShare / totalShare);
                }
            }

            AliveBet += aliveAdd;
            DieBet += dieAdd;

            UpdateCoefficients();
        }

        private void UpdateCoefficients()
        {
            if (CurrentBet <= 0f)
                return;

            float rawAlive = AliveBet / CurrentBet;
            float rawDie = DieBet / CurrentBet;

            if (_smoothAlive <= 0f && _smoothDie <= 0f)
            {
                _smoothAlive = rawAlive;
                _smoothDie = rawDie;
            }
            else
            {
                _smoothAlive = Mathf.Lerp(_smoothAlive, rawAlive, CoeffSmooth);
                _smoothDie = Mathf.Lerp(_smoothDie, rawDie, CoeffSmooth);
            }

            AliveBetCoefficient = _smoothDie + 1;
            DieBetCoefficient = _smoothAlive + 1;

            Debug.LogError(
                $"[Coeffs]  AliveBet: {AliveBet}, DieBet: {DieBet}, AliveBetCoefficient: {AliveBetCoefficient}, DieBetCoefficient: {DieBetCoefficient}");
            OnChangeAliveCoefficient?.Invoke(AliveBetCoefficient);
            OnChangeDieCoefficient?.Invoke(DieBetCoefficient);
        }
    }
}