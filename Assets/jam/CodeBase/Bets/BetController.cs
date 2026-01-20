using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Economy;
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

        private float _perSecondBet;


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

        private void MakeBaseBet()
        {
            var cfg = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();

            float baseBet = Random.Range(cfg.BaseBet.x, cfg.BaseBet.y);

            var dieBet = baseBet * Random.Range(cfg.DieBiddersProporionRange.x, cfg.DieBiddersProporionRange.y);
            var aliveBet = baseBet * Random.Range(cfg.AliveBiddersProporionRange.x, cfg.AliveBiddersProporionRange.y);

            Debug.LogError("Base AliveBet: " + aliveBet);
            Debug.LogError("Base DieBet: " + dieBet);

            BetToDie(dieBet);
            BetToAlive(aliveBet);
        }

        private float GetBetPerSecond()
        {
            var cfg = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();

            float baseMoney = G.Economy.CurrentMoney;
            float totalBetMultiplier =
                Random.Range(cfg.PercentsRangeFromPlayerMoney.x, cfg.PercentsRangeFromPlayerMoney.y);

            var totalAdditionalBet = baseMoney * totalBetMultiplier;
            _perSecondBet = totalAdditionalBet / BetTime;

            Debug.LogError($"[InitRound] perSecondBet: {_perSecondBet}");
            return _perSecondBet;
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
            MakeBaseBet();

            var cfg = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();

            float baseMoney = G.Economy.CurrentMoney;
            float totalBetMultiplier =
                Random.Range(cfg.PercentsRangeFromPlayerMoney.x, cfg.PercentsRangeFromPlayerMoney.y);

            float totalAdditionalBet = baseMoney * totalBetMultiplier;

            float[] tickBets = BuildTickBets(totalAdditionalBet);

            int tick = 0;
            int time = BetTime;

            while (time > 0)
            {
                MakeSecondBet(tickBets[tick]);

                await UniTask.WaitForSeconds(1);

                tick++;
                time--;
            }
            
            G.Menu.ViewService.HideView<BetPopup>();
        }

        private float[] BuildTickBets(float totalAdditionalBet)
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
                tickBets[i] = totalAdditionalBet * (weights[i] / totalWeight);
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

            float r = Random.value; // 0..1

            float aliveAdd = 0f;
            float dieAdd = 0f;

            if (r < 0.33f)
            {
                aliveAdd = bet;
                dieAdd = 0f;
            }
            else if (r < 0.66f)
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
            AliveBetCoefficient = AliveBet / CurrentBet;
            DieBetCoefficient = DieBet / CurrentBet;

            Debug.LogError(
                $"[Coeffs]  AliveBet: {AliveBet}, DieBet: {DieBet}, AliveBetCoefficient: {AliveBetCoefficient}, DieBetCoefficient: {DieBetCoefficient}");
            OnChangeAliveCoefficient?.Invoke(AliveBetCoefficient);
            OnChangeDieCoefficient?.Invoke(DieBetCoefficient);
        }
    }
}