using System.ComponentModel;

namespace ToDoApp.Models
{
    public class TaskItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        public int Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }

        private string _title = string.Empty;
        public string Title { get => _title; set { _title = value; OnPropertyChanged(nameof(Title)); } }

        private string _time = string.Empty;
        public string Time { get => _time; set { _time = value; OnPropertyChanged(nameof(Time)); } }

        private string _image = string.Empty;
        public string Image { get => _image; set { _image = value; OnPropertyChanged(nameof(Image)); } }

        private string _description = string.Empty;
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }

        private string _details = string.Empty;
        public string Details { get => _details; set { _details = value; OnPropertyChanged(nameof(Details)); } }

        private string _status = string.Empty;
        public string Status { get => _status; set { _status = value; OnPropertyChanged(nameof(Status)); } }

        private int _userId;
        public int UserId { get => _userId; set { _userId = value; OnPropertyChanged(nameof(UserId)); } }

        private string _statusColor = string.Empty;
        public string StatusColor { get => _statusColor; set { _statusColor = value; OnPropertyChanged(nameof(StatusColor)); } }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}