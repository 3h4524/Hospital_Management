using System.Windows;
using System.Windows.Controls;
using Model;
using Service;

namespace View
{
    public partial class AssignRoleView : UserControl
    {
        private readonly UserRoleService _userRoleService;

        public AssignRoleView(UserRoleService userRoleService)
        {
            InitializeComponent();
            _userRoleService = userRoleService;
            UsersDataGrid.ItemsSource = _userRoleService.GetAllUsers();
        }

        private async void AssignRoleButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as SystemUser;
            if (selectedUser == null)
            {
                MessageTextBlock.Text = "Vui lòng chọn người dùng.";
                return;
            }
            var row = (DataGridRow)UsersDataGrid.ItemContainerGenerator.ContainerFromItem(selectedUser);
            if (row == null)
            {
                MessageTextBlock.Text = "Không thể xác định dòng dữ liệu.";
                return;
            }
            var comboBox = FindVisualChild<ComboBox>(row, "RoleComboBox");
            if (comboBox == null || comboBox.SelectedItem == null)
            {
                MessageTextBlock.Text = "Vui lòng chọn vai trò.";
                return;
            }
            var selectedRole = comboBox.SelectedItem as string;
            bool result = await _userRoleService.AssignRoleToUser(selectedUser.UserId, selectedRole);
            if (result)
            {
                MessageTextBlock.Text = $"Đã gán vai trò '{selectedRole}' cho người dùng ID {selectedUser.UserId}.";
                UsersDataGrid.ItemsSource = null;
                UsersDataGrid.ItemsSource = _userRoleService.GetAllUsers();
            }
            else
            {
                MessageTextBlock.Text = "Gán vai trò thất bại. Vui lòng thử lại.";
                comboBox.SelectedItem = selectedUser.Role;
            }
        }

        private T FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T childType && (child as FrameworkElement)?.Name == name)
                    return childType;
                var result = FindVisualChild<T>(child, name);
                if (result != null) return result;
            }
            return null;
        }
    }
}