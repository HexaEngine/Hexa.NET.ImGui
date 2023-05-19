namespace HexaEngine.Core.Collections
{
    using System.ComponentModel;

    [Serializable]
    public class ObservableKeyValuePair<TKey, TValue> : INotifyPropertyChanged
    {
        #region properties

#pragma warning disable CS8618 // Non-nullable field 'key' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        private TKey key;
#pragma warning restore CS8618 // Non-nullable field 'key' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'value' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        private TValue value;
#pragma warning restore CS8618 // Non-nullable field 'value' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.

        public TKey Key
        {
            get { return key; }
            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        public TValue Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion properties

        #region INotifyPropertyChanged Members

        [field: NonSerialized]
#pragma warning disable CS8612 // Nullability of reference types in type of 'event PropertyChangedEventHandler ObservableKeyValuePair<TKey, TValue>.PropertyChanged' doesn't match implicitly implemented member 'event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged'.
#pragma warning disable CS8618 // Non-nullable event 'PropertyChanged' must contain a non-null value when exiting constructor. Consider declaring the event as nullable.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8618 // Non-nullable event 'PropertyChanged' must contain a non-null value when exiting constructor. Consider declaring the event as nullable.
#pragma warning restore CS8612 // Nullability of reference types in type of 'event PropertyChangedEventHandler ObservableKeyValuePair<TKey, TValue>.PropertyChanged' doesn't match implicitly implemented member 'event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged'.

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}