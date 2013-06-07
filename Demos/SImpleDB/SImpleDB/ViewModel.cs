using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SImpleDB.Annotations;
using SQLite;

namespace SImpleDB
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            AddThingCommand = new AddThingCommand(this);
            InitializeDB();
            LoadData();
        }

        private void InitializeDB()
        {
            var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "data.db3");
            using (var db = new SQLiteConnection(path))
            {
                db.CreateTable<Thing>();
                db.Commit();
                db.Dispose();
                db.Close();
            }
        }

        private void LoadData()
        {
            var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "data.db3");
            using (var db = new SQLiteConnection(path))
            {
                Things = new ObservableCollection<Thing>(db.Table<Thing>().ToList());
                db.Dispose();
                db.Close();
            }           
        }

        public ObservableCollection<Thing> Things { get; set; }

        public AddThingCommand AddThingCommand { get; set; }

        public void AddThing(string thing)
        {
            Things.Add(new Thing{Name=thing});

            var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "data.db3");
            using (var db = new SQLiteConnection(path))
            {
                db.Insert(new Thing {Name = thing});
                db.Commit();
                db.Dispose();
                db.Close();
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyOfPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AddThingCommand : ICommand
    {
        private readonly ViewModel _vm;

        public AddThingCommand(ViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event System.EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _vm.AddThing(parameter as string);
        }
    }
}