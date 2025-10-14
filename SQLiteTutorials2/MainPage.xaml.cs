using SQLiteTutorials2.Models;

namespace SQLiteTutorials2
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTasks();
        }
        private async Task LoadTasks()
        {
            TasksList.ItemsSource = await App.Database.GetTasksAsync();
        }
        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
            {
                await App.Database.SaveTaskAsync(new Models.TaskItem { Title = TaskEntry.Text });
                TaskEntry.Text = string.Empty;
                await LoadTasks();
            }
        }
        private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.BindingContext is TaskItem task)
            {
                task.IsCompleted = e.Value;
                await App.Database.SaveTaskAsync(task);
            }
        }
        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var taskToDelete = button?.CommandParameter as TaskItem;

            if (taskToDelete != null)
            {
                bool confirm = await DisplayAlert("Delete Task", $"Delete '{taskToDelete.Title}'?", "Yes", "No");
                if (confirm)
                {
                    await App.Database.DeleteTaskAsync(taskToDelete);
                    await LoadTasks();
                }
            }
        }


    }
}
