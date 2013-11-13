using Kosudoku.Models;
using System.Collections.Generic;

namespace Kosudoku.Mocks
{
    internal class MockNumberBlockViewModel
    {
        private IList<IList<Number>> _numbers;
        public IList<IList<Number>> Numbers
        {
            get
            {
                if (_numbers == null)
                {
                    var row0 = new List<Number> 
                { 
                    new Number(1),
                    new Number(2),
                    new Number(3)
                };
                    var row1 = new List<Number> 
                { 
                    new Number(4),
                    new Number(5),
                    new Number(6)
                };
                    var row2 = new List<Number> 
                {
                    new Number(7),
                    new Number(8),
                    new Number(9)
                };
                    _numbers = new List<IList<Number>> { row0, row1, row2 };
                }
                return _numbers;
            }
        }
    }
}
