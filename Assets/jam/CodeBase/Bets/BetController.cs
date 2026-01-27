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
        public Action StartFtueAwait;
        public Action StopFtueAwait;

        public float CurrentBet => AliveBet + DieBet;
        public float AliveBet;
        public float DieBet;
        public float AliveBetCoefficient;
        public float DieBetCoefficient;
        public float MyBetAlive;
        public float MyBetDie;
        public float MyBet => MyBetAlive + MyBetDie;
        private const int BetTime = 30;

        private float _smoothAlive;
        private float _smoothDie;
        private const float CoeffSmooth = 1f;
        private BetSaveModel _saveModel;
        
        public BetController()
        {
            _saveModel = G.Saves.Get<BetSaveModel>();
            
            var runSave = G.Saves.Get<RunSaveModel>().Data;
            if (runSave.IsStarted)
            {
                Debug.LogError("INIT COEFS");
                AliveBet = _saveModel.Data.AliveBet;
                DieBet = _saveModel.Data.DieBet;
                MyBetAlive = _saveModel.Data.MyBetLive;
                MyBetDie = _saveModel.Data.MyBetDie;
                UpdateCoefficients();
            }

        }

        public void BetToDie(float bet)
        {
            if (bet <= 0) return;

            DieBet += bet;
            MyBetDie += bet;
            
            UpdateCoefficients();
        }

        public void BetToAlive(float bet)
        {
            if (bet <= 0) return;

            AliveBet += bet;
            MyBetAlive += bet;
            
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
                while (time < 10 && !G.Saves.Get<FTUESaveModel>().Data.ShowedBetFTUE)
                {
                    StartFtueAwait?.Invoke();
                    await UniTask.WaitForSeconds(1);
                }
                StopFtueAwait?.Invoke();
                
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

            _saveModel.Data.AliveBet = AliveBet;
            _saveModel.Data.DieBet = DieBet; 
            _saveModel.Data.MyBetLive = MyBetAlive; 
            _saveModel.Data.MyBetDie = MyBetDie; 
            _saveModel.ForceSave();
            
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

            AliveBetCoefficient = (_smoothDie + 1) * 2.5f;
            DieBetCoefficient = (_smoothAlive + 1) * 2.5f;
            Debug.Log(
                $"[Coeffs]  AliveBet: {AliveBet}, DieBet: {DieBet}, AliveBetCoefficient: {AliveBetCoefficient}, DieBetCoefficient: {DieBetCoefficient}");
            OnChangeAliveCoefficient?.Invoke(AliveBetCoefficient);
            OnChangeDieCoefficient?.Invoke(DieBetCoefficient);
        }
    }
}