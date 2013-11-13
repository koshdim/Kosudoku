using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Kosudoku.Models;

namespace Kosudoku.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private const int DefaultDimension = 3;
        private ICommand _startNewGameCommand;
        private bool _isNumberSelectionControlVisible;

        private Number _numberInEdit;

        public Matrix Matrix { get; private set; }

        public IList<List<NumberBlockViewModel>> NumberBlocks { get; private set; }

        public NumberBlockViewModel SelectionNumbers { get; private set; }

        public int Dimension 
        { 
            get { return DefaultDimension; } 
            set { throw new NotImplementedException(); }
        }

        public bool AreAllNumbersCorrect
        {
            get
            {
                return Matrix.Numbers.All(n => n.IsCorrect);
            }
        }

        public bool IsNumberSelectionControlVisible
        {
            get { return _isNumberSelectionControlVisible; }
            set
            {
                _isNumberSelectionControlVisible = value;
                OnPropertyChanged(() => IsNumberSelectionControlVisible);
            }
        }

        public ICommand StartNewGameCommand
        {
            get
            {
                return _startNewGameCommand ?? (_startNewGameCommand = new Command(p =>
                {
                    Matrix = GameGenerator.Generate(Dimension);
                    Matrix.SetComplexity((Complexity)p);
                    UpdateNumberBlocks();
                    OnPropertyChanged(() => NumberBlocks);
                }));
            }
        }

        public MainWindowViewModel()
        {
            Matrix=new Matrix(0);
            try
            {
                if (!Matrix.Load())
                    StartNewGameCommand.Execute(Complexity.Hardcore);
                else
                {
                    UpdateNumberBlocks();
                    OnPropertyChanged(() => NumberBlocks);
                }
            }
            catch (Exception)
            {
                StartNewGameCommand.Execute(Complexity.Hardcore);
            }
        }

        internal void Save()
        {
            Matrix.Save();
        }

        private void UpdateNumberBlocks()
        {
            NumberBlocks = new List<List<NumberBlockViewModel>>();

            for (var i = 0; i < Dimension; i++)
            {
                var numberBlockRow = new List<NumberBlockViewModel>();
                NumberBlocks.Add(numberBlockRow);
                for (var j = 0; j < Dimension; j++)
                {
                    var blockNumbers = new List<Number>();

                    var topLeftCell = j*Dimension + i*Dimension*Dimension*Dimension;
                    for (var ci = 0; ci < Dimension; ci++)
                    {
                        for (var cj = 0; cj < Dimension; cj++)
                        {
                            var numberIndex = topLeftCell + cj + ci*Dimension*Dimension;
                            var number = Matrix.Numbers[numberIndex];
                            blockNumbers.Add(number);
                        }
                    }
                    numberBlockRow.Add(new NumberBlockViewModel(blockNumbers, p =>
                    {
                        _numberInEdit = (Number)p;
                        if(_numberInEdit.IsMutable)
                            IsNumberSelectionControlVisible = !IsNumberSelectionControlVisible;
                    }));
                }
            }


            IList<Number> seletionNumbers = new List<Number>();
            for (var i = 0; i < Dimension*Dimension; i++)
            {
                seletionNumbers.Add(new Number(i + 1) {IsVisible = true, IsMutable = true});
            }

            SelectionNumbers = new NumberBlockViewModel(seletionNumbers, p =>
            {
                var selectedNumber = (Number)p;
                _numberInEdit.UserNumber = selectedNumber.Value;
                if (AreAllNumbersCorrect)
                {
                    if (CultureInfo.CurrentCulture.Name.ToLower().Contains("ru") || CultureInfo.CurrentCulture.Name.ToLower().Contains("ua"))
                        MessageBox.Show("Ви виграли!");
                    else
                        MessageBox.Show("You won!");
                }
                else
                {
                    if(Matrix.Numbers.All(n=>!n.IsMutable || n.UserNumber>0))
                        if (CultureInfo.CurrentCulture.Name.ToLower().Contains("ru") || CultureInfo.CurrentCulture.Name.ToLower().Contains("ua"))
                            MessageBox.Show("Не всі числа введені вірно");
                        else
                            MessageBox.Show("Some numbers are not correct");
                }
                IsNumberSelectionControlVisible = !IsNumberSelectionControlVisible;
            });
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanged(Expression<Func<object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
                memberExpression = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            else
                memberExpression = lambda.Body as MemberExpression;

            if (memberExpression == null)
                throw new Exception("Please provide a lambda expression like '() => PropertyName'");

            if (memberExpression.Member is PropertyInfo)
                OnPropertyChanged(memberExpression.Member.Name);
        }
        #endregion

    }
}
