using System;
using System.Collections.Generic;
using Sources.Architecture;
using Sources.Architecture.Interfaces;
using Sources.Presenters;

namespace Sources.GameLoop.Services
{
    public class BuyService: IBuyService
    {
        private readonly IEnumerable<GeneratorPresenter> _generatorPresenters;
        private BuyAmount _current;

        public BuyService(IEnumerable<GeneratorPresenter> generatorPresenters)
        {
            _generatorPresenters = generatorPresenters;
        }

        public void Enable()
        {
            ChangeAmount(BuyAmount.One);
        }

        public void GoToNextAmount()
        {
            if (_current == BuyAmount.Max)
            {
                ChangeAmount(BuyAmount.One);
            }
            else
            {
                var value = (int)_current + 1;
                ChangeAmount((BuyAmount)value);
            }
        }

        public void ChangeAmount(BuyAmount amount)
        {
            _current = amount;
            var value = GetAmountByType(_current);
            foreach (var generatorPresenter in _generatorPresenters)
            {
                generatorPresenter.ChangeLevelsAmount(value);
            }
        }

        public string GetCurrentAmount()
        {
            return _current.ToString();
        }

        private int GetAmountByType(BuyAmount amount) =>
            amount switch
            {
                BuyAmount.One => 1,
                BuyAmount.Ten => 10,
                BuyAmount.Hundred => 100,
                BuyAmount.Max => -1,
                _ => throw new ArgumentOutOfRangeException(nameof(amount), amount, null)
            };
    }
}