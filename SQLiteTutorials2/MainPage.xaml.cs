using SQLiteTutorials2.Models;
using SQLiteTutorials2.Services;

namespace SQLiteTutorials2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Set defaults
            StatusPicker.SelectedIndex = 0;  // All
            PriorityPicker.SelectedIndex = 0; // All
            DuePicker.SelectedIndex = 0; // Any
            SortPicker.SelectedIndex = 0; // Created
            DescCheck.IsChecked = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTasks();
        }

        private TaskQuery BuildQueryFromUI()
        {
            var q = new TaskQuery
            {
                Search = string.IsNullOrWhiteSpace(SearchBox.Text) ? null : SearchBox.Text.Trim(),
                Desc = DescCheck.IsChecked
            };

            // Status
            q.Status = StatusPicker.SelectedIndex switch
            {
                1 => StatusFilter.Active,
                2 => StatusFilter.Completed,
                _ => StatusFilter.All
            };

            // Priority
            q.Priority = PriorityPicker.SelectedIndex switch
            {
                1 => PriorityLevel.Low,
                2 => PriorityLevel.Medium,
                3 => PriorityLevel.High,
                _ => null
            };

            // Due
            q.Due = DuePicker.SelectedIndex switch
            {
                1 => DueFilter.Overdue,
                2 => DueFilter.Today,
                3 => DueFilter.ThisWeek,
                4 => DueFilter.NoDueDate,
                _ => DueFilter.Any
            };

            // Sort
            q.SortBy = SortPicker.SelectedIndex switch
            {
                1 => SortField.DueDate,
                2 => SortField.Priority,
                3 => SortField.Title,
                _ => SortField.CreatedAt
            };

            // Tags (OR across tokens)
            if (!string.IsNullOrWhiteSpace(TagsFilterEntry.Text))
            {
                q.Tags = TagsFilterEntry.Text
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .ToList();
            }

            return q;
        }

        private async Task LoadTasks()
        {
            var query = BuildQueryFromUI();
            TasksList.ItemsSource = await App.Database.GetTasksAsync(query);
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
            {
                var task = new TaskItem
                {
                    Title = TaskEntry.Text.Trim(),
                    // Priority defaults to Medium via model initializer
                    // CreatedAt defaults in model
                    Tags = string.IsNullOrWhiteSpace(TagEntry.Text) ? null : TagEntry.Text.Trim()
                };

                await App.Database.SaveTaskAsync(task);

                TaskEntry.Text = string.Empty;
                TagEntry.Text = string.Empty;

                await LoadTasks();
            }
        }

        private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.BindingContext is TaskItem task)
            {
                task.IsCompleted = e.Value;
                await App.Database.SaveTaskAsync(task);
                // Keep list order consistent after status change
                await LoadTasks();
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

        private async void OnFilterChanged(object sender, EventArgs e)
        {
            await LoadTasks();
        }
    }
}
