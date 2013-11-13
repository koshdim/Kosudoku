using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Kosudoku.Models
{
    internal class Number : INotifyPropertyChanged
    {
        private int _value;
        private int _userNumber;

        public int? VisibleNumber { get; private set; }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                IsVisible = true;
            }
        }

        public bool IsVisible 
        {
            get { return VisibleNumber != null; }
            set
            {
                if (value)
                {
                    VisibleNumber = Value;
                }
                else
                {
                    VisibleNumber = null;
                }
            }
        }

        public bool IsMutable { get; set; }

        public int UserNumber
        {
            get { return _userNumber; }
            set
            {
                _userNumber = value;
                OnPropertyChanged(() => VisibleNumberString);
            }
        }

        public bool IsCorrect 
        {
            get { return !IsMutable || UserNumber == Value; }
        }

        public Number(int value)
        {
            Value = value;
        }

        public string VisibleNumberString
        {
            get
            {
                if (VisibleNumber != null)
                    return VisibleNumber.ToString();

                return UserNumber == 0 ? string.Empty : UserNumber.ToString(CultureInfo.InvariantCulture);
            }
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
