using SQLiteTutorials2.Models;
using SQLiteTutorials2.Services;

namespace SQLiteTutorials2
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing() // Override the OnAppearing method
        {
            base.OnAppearing(); // Call the base method
            await LoadTasks(); // Load tasks when the page appears
        }
        private async Task LoadTasks() // Load tasks from the database
        {
            TasksList.ItemsSource = await App.Database.GetTasksAsync(); // Load tasks from the database and set as the item source for the ListView
        }
        private async void OnAddTaskClicked(object sender, EventArgs e) // Add task button event handler
        {
            if (!string.IsNullOrWhiteSpace(TaskEntry.Text)) // Ensure the entry is not empty
            {
                await App.Database.SaveTaskAsync(new Models.TaskItem { Title = TaskEntry.Text }); // Save new task to the database
                TaskEntry.Text = string.Empty; // Clear the entry field
                await LoadTasks(); // Refresh the task list
            }
        }
        private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e) // Checkbox change event handler
        {
            if (sender is CheckBox checkbox && checkbox.BindingContext is TaskItem task) // Get the task associated with the checkbox
            {
                task.IsCompleted = e.Value; // Update the task's completion status
                await App.Database.SaveTaskAsync(task); // Save the updated task to the database
            }
        }
        private async void OnDeleteTaskClicked(object sender, EventArgs e) // Delete task event handler
        {
            var button = sender as Button; // Get the button that was clicked
            var taskToDelete = button?.CommandParameter as TaskItem; // Get the task associated with the button

            if (taskToDelete != null) // If a task is found
            {
                bool confirm = await DisplayAlert("Delete Task", $"Delete '{taskToDelete.Title}'?", "Yes", "No"); // Confirm deletion
                if (confirm) // If user confirms
                {
                    await App.Database.DeleteTaskAsync(taskToDelete); // Delete the task from the database
                    await LoadTasks(); // Refresh the task list
                }
            }
        }


    }
}
