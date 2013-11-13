using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Kosudoku.Models
{
    internal class Matrix
    {
        private int _dimension;

        public IList<Number> Numbers { get; private set; }

        public Matrix(int dimension)
        {
            _dimension = dimension;
            InitializeNumbers();
        }

        private void InitializeNumbers()
        {
            var numbers = new List<Number>();
            for (var i = 0; i < _dimension*_dimension*_dimension*_dimension; i++)
            {
                numbers.Add(new Number(i));
            }
            Numbers = numbers;
        }

        public Number ItemAt(int row, int column)
        {
            try
            {
                return Numbers[column + row*_dimension*_dimension];
            }
            catch
            {
                return null;
            }
        }

        internal void Reset()
        {
            foreach (var number in Numbers)
            {
                number.Value = 0;
            }
        }

        internal void SetComplexity(Complexity complexity)
        {
            var numbers = (int)(_dimension * _dimension * _dimension * _dimension * (double)complexity / 100);
            var random = new Random();

            while (InvisibleNumbersCount()<numbers)
            {
                var numberIndex = random.Next(0, _dimension * _dimension * _dimension * _dimension - 1);
                Numbers[numberIndex].IsVisible = false;
                Numbers[numberIndex].IsMutable = true;
            }
        }

        internal void Save()
        {
            var xElement = new XElement("Root", new XAttribute("Dimension", _dimension));
            xElement.Add(from number in Numbers
                         select new XElement("Number",
                             number.Value,
                             new XAttribute("IsMutable", number.IsMutable),
                             new XAttribute("UserNumber", number.UserNumber),
                             new XAttribute("IsVisible", number.IsVisible)));
            var xDocument = new XDocument(xElement);
            xDocument.Save("data.xml");
        }

        internal bool Load()
        {
            if (File.Exists("data.xml"))
            {
                var xDocument = XDocument.Load("data.xml");
                var xElement = xDocument.Element("Root");
                if (xElement != null)
                {
                    _dimension = Convert.ToInt32(xElement.Attribute("Dimension").Value);
                    InitializeNumbers();

                    var elements = xElement.Elements().ToList();

                    for (var i = 0; i < _dimension*_dimension*_dimension*_dimension; i++)
                    {
                        var value = Convert.ToInt32(elements[i].Value);
                        var isMutable = Convert.ToBoolean(elements[i].Attribute("IsMutable").Value);
                        var userNumber = Convert.ToInt32(elements[i].Attribute("UserNumber").Value);
                        var isVisible = Convert.ToBoolean(elements[i].Attribute("IsVisible").Value);
                        Numbers[i].Value = value;
                        Numbers[i].IsMutable = isMutable;
                        Numbers[i].UserNumber = userNumber;
                        Numbers[i].IsVisible = isVisible;
                    }
                }
                return true;
            }
            return false;
        }

        private int InvisibleNumbersCount()
        {
            return Numbers.Count(n => !n.IsVisible);
        }

    }
}
