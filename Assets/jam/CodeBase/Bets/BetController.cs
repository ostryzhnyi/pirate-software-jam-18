using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Economy;
using UnityEngine;

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
        
        private float CoefficientScale;

        private const int BetTime = 30;

        private float _roundTotalMaxBet;
        private float _perSecondBet;
        private float _remainingRoundBet;

        public BetController()
        {
            var baseEconomyTag = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();
            BetBaseDieCoefficientRange = baseEconomyTag.BetBaseDieCoefficientRange;
            BetBasAliveCoefficientRange = baseEconomyTag.BetBaseAliveCoefficientRange;
            CoefficientScale = baseEconomyTag.CoefficientScale;
            
            AliveBet = 0;
            DieBet = 0;

            AliveBetCoefficient = BetBasAliveCoefficientRange.x;
            DieBetCoefficient = BetBaseDieCoefficientRange.x;

            InitRoundTotalMaxBet();
            AddBaseBet();
            UpdateCoefficients();
        }

        private void InitRoundTotalMaxBet()
        {
            var cfg = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();

            float baseBetMax = cfg.BaseBet.y;
            float baseMoney  = cfg.BaseMoney;
            float percentMax = cfg.PercentsRangeFromPlayerMoney.y;

            _roundTotalMaxBet = baseBetMax + baseMoney * percentMax;
            _perSecondBet = _roundTotalMaxBet / BetTime;
            _remainingRoundBet = _roundTotalMaxBet;

            Debug.LogError($"[InitRound] roundTotalMaxBet: {_roundTotalMaxBet}, perSecondBet: {_perSecondBet}");
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
            var time = BetTime;

            while (time > 0)
            {
                await UniTask.WaitForSeconds(1);
                AddSecondBets();
                time -= 1;
            }
        }

        private void AddBaseBet()
        {
            var baseBetConfig = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();
            float baseBet = UnityEngine.Random.Range(baseBetConfig.BaseBet.x, baseBetConfig.BaseBet.y);

            float percent = UnityEngine.Random.Range(
                baseBetConfig.PercentsRangeFromPlayerMoney.x,
                baseBetConfig.PercentsRangeFromPlayerMoney.y
            );

            float finalBet = baseBet + G.Economy.CurrentMoney * percent;

            float aliveShare = baseBetConfig.BiddersProporion.y;
            float dieShare   = baseBetConfig.BiddersProporion.x;

            float addAlive = finalBet * aliveShare;
            float addDie   = finalBet * dieShare;

            AliveBet += addAlive;
            DieBet   += addDie;

            Debug.LogError($"[BaseBet] baseBet: {baseBet}, percent: {percent}, finalBet: {finalBet}, AliveBet: {AliveBet}, DieBet: {DieBet}");
        }

        private void AddSecondBets()
        {
            if (_remainingRoundBet <= 0)
                return;

            var cfg = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();

            float secondPool = Mathf.Min(_perSecondBet, _remainingRoundBet);
            _remainingRoundBet -= secondPool;

            int biddersCount = UnityEngine.Random.Range(
                (int)cfg.AmountBetsRange.x,
                (int)cfg.AmountBetsRange.y + 1
            );

            if (biddersCount <= 0)
                return;

            float perBidder = secondPool / biddersCount;

            float aliveShare = cfg.BiddersProporion.y;
            float dieShare   = cfg.BiddersProporion.x;

            for (int i = 0; i < biddersCount; i++)
            {
                float aliveAdd = perBidder * aliveShare;
                float dieAdd   = perBidder * dieShare;

                AliveBet += aliveAdd;
                DieBet   += dieAdd;
            }

            Debug.LogError($"[Second] secondPool: {secondPool}, remaining: {_remainingRoundBet}, bidders: {biddersCount}, perBidder: {perBidder}, AliveBet: {AliveBet}, DieBet: {DieBet}");
            UpdateCoefficients();
        }

        private void UpdateCoefficients()
        {
            float totalAlive = AliveBet;
            float totalDie   = DieBet;
            float totalBets  = totalAlive + totalDie;

            float aliveShare = totalBets > 0 ? totalAlive / totalBets : 0.5f;
            float dieShare   = totalBets > 0 ? totalDie   / totalBets : 0.5f;

            float alivePressure = Mathf.Clamp01(aliveShare * CoefficientScale);
            float diePressure   = Mathf.Clamp01(dieShare   * CoefficientScale);

            float aliveK = Mathf.Lerp(
                BetBasAliveCoefficientRange.y,
                BetBasAliveCoefficientRange.x,
                alivePressure
            );

            float dieK = Mathf.Lerp(
                BetBaseDieCoefficientRange.y,
                BetBaseDieCoefficientRange.x,
                diePressure
            );

            if (totalBets <= 0)
            {
                aliveK = BetBasAliveCoefficientRange.x;
                dieK   = BetBaseDieCoefficientRange.x;
            }

            if (Mathf.Abs(AliveBetCoefficient - aliveK) > 0.0001f ||
                Mathf.Abs(DieBetCoefficient   - dieK)   > 0.0001f)
            {
                AliveBetCoefficient = aliveK;
                DieBetCoefficient   = dieK;

                Debug.LogError($"[Coeffs] AliveShare: {aliveShare}, DieShare: {dieShare}, AliveK: {aliveK}, DieK: {dieK}, AliveBet: {AliveBet}, DieBet: {DieBet}");
                OnChangeAliveCoefficient?.Invoke(AliveBetCoefficient);
                OnChangeDieCoefficient?.Invoke(DieBetCoefficient);
            }
        }
    }
}
