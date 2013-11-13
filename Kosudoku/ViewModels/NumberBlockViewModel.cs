using System;
using System.Collections.Generic;
using System.Windows.Input;
using Kosudoku.Models;

namespace Kosudoku.ViewModels
{
    internal class NumberBlockViewModel
    {
        private readonly Action<object> _numberEditClick;
        private ICommand _numberClickCommand;
        private ICommand _numberRemoveCommand;

        public IList<IList<Number>> Numbers { get; private set; }

        public ICommand NumberClickCommand
        {
            get
            {
                return _numberClickCommand ?? (_numberClickCommand = new Command(p => _numberEditClick(p)));
            }
        }
        public ICommand NumberRemoveCommand
        {
            get
            {
                return _numberRemoveCommand ?? (_numberRemoveCommand = new Command(p =>
                {
                    var number = (Number) p;
                    number.UserNumber = 0;
                }));
            }
        }

        public NumberBlockViewModel(IList<Number> numbers, Action<object> numberEditClick)
        {
            _numberEditClick = numberEditClick;
            Numbers = new List<IList<Number>>();

            var dimension=(int)Math.Sqrt(numbers.Count);
            for (var i = 0; i < dimension; i++)
            {
                var row = new List<Number>();
                for (var j = 0; j < dimension; j++)
                {
                    row.Add(numbers[dimension * i + j]);
                }
                Numbers.Add(row);
            }
        }
    }
}
