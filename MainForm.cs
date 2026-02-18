using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Laboratory1
{
    public partial class Compiler : Form
    {
        string? filename;
        OpenFileDialog openDialog = new OpenFileDialog();
        SaveFileDialog saveDialog = new SaveFileDialog();
        private Stack<string> redoStack = new Stack<string>();
        private Stack<string> undoStack = new Stack<string>();
        bool isProgrammaticChange = false;

        public Compiler()
        {
            InitializeComponent();
            openDialog.Filter = "Text files(*.txt)|*.txt|CSV files(*.csv)|*.csv|Word files(*.doc;*.docx)|*.doc;*.docx|All files(*.*)|*.*";
            saveDialog.Filter = "Text files(*.txt)|*.txt|CSV files(*.csv)|*.csv|Word files(*.doc;*.docx)|*.doc;*.docx|All files(*.*)|*.*";


        }

        private void aboutProgrammButton_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm("aboutProgramm");
            helpForm.ShowDialog();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            saveDialog.Title = "Создание файла";

            if (saveDialog.ShowDialog() == DialogResult.Cancel)
                return;

            filename = saveDialog.FileName;

            try
            {
                // Создаем пустой файл
                File.Create(filename).Close();
                fileInformationTextBox.Text = "";

                if (File.Exists(filename))
                    MessageBox.Show($"Файл создан: {Path.GetFileName(filename)}");
                else
                    MessageBox.Show("Произошла ошибка при создании");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании файла: {ex.Message}");
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

            File.WriteAllText(filename, fileInformationTextBox.Text);
            if (File.Exists(filename)) MessageBox.Show("Изменения успешно сохранены");
            else MessageBox.Show("Произошла ошибка при сохранении");
        }

        private void fileOpenButton_Click(object sender, EventArgs e)
        {
            if (filename != null && File.Exists(filename))
            {
                if (File.ReadAllText(filename) != fileInformationTextBox.Text)
                {
                    DialogResult result = MessageBox.Show("Последние изменения не были сохранены. Сохранить изменения?",
                        "Открыть файл", MessageBoxButtons.YesNoCancel);

                    if (result == DialogResult.Yes)
                    {
                        
                        File.WriteAllText(filename, fileInformationTextBox.Text);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                       
                        return;
                    }
                }
            }

            if (openDialog.ShowDialog() == DialogResult.Cancel)
                return;

            filename = openDialog.FileName;
            string fileText = File.ReadAllText(filename);

          
            redoStack.Clear();
            undoStack.Clear();


            undoStack.Push(fileText);

           
            isProgrammaticChange = true;
            fileInformationTextBox.Text = fileText;
            isProgrammaticChange = false;
        }
        

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            saveDialog.Title = "Сохранить как";
            if (saveDialog.ShowDialog() == DialogResult.Cancel)
                return;
            filename = saveDialog.FileName;
            File.WriteAllText(filename, fileInformationTextBox.Text);
            if (File.Exists(filename)) MessageBox.Show("Файл успешно сохранен");
            else MessageBox.Show("Произошла ошибка при сохранении");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            if (File.ReadAllText(filename) != fileInformationTextBox.Text)
            {
                DialogResult result = MessageBox.Show("Последние изменения не были сохранены. Вы уверены что хотите выйти?",
                    "Выход из приложения", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) this.Close();
            }
            else this.Close();
        }

        private void cutButton_Click(object sender, EventArgs e)
        {
            string selectedText = fileInformationTextBox.SelectedText;
            string text = fileInformationTextBox.Text;
            if (!string.IsNullOrEmpty(text))
                Clipboard.SetText(text);
            else Clipboard.SetText(selectedText);
            if (fileInformationTextBox.SelectionLength > 0)
            {
                fileInformationTextBox.SelectedText = "";
            }
            else
            {
                fileInformationTextBox.Text = "";
            }

        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            string selectedText = fileInformationTextBox.SelectedText;
            string text = fileInformationTextBox.Text;

            if (!string.IsNullOrEmpty(selectedText))
                Clipboard.SetText(text);
            else Clipboard.SetText(selectedText);

        }

        private void pastetButton_Click(object sender, EventArgs e)
        {
            fileInformationTextBox.SelectedText = Clipboard.GetText();

        }

        private void selectAllButton_Click(object sender, EventArgs e)
        {
            fileInformationTextBox.SelectAll();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            string text = fileInformationTextBox.Text;
            fileInformationTextBox.Text = text.Remove(
            fileInformationTextBox.SelectionStart,
            fileInformationTextBox.SelectionLength);
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 1) 
            {
                isProgrammaticChange = true;
                redoStack.Push(fileInformationTextBox.Text);
                undoStack.Pop();
                fileInformationTextBox.Text = undoStack.Peek();

                isProgrammaticChange = false;
            }
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                isProgrammaticChange = true;

                string text = redoStack.Pop();
                undoStack.Push(text);
                fileInformationTextBox.Text = text;

                isProgrammaticChange = false;
            }
        }

        private void fileInformationTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!isProgrammaticChange)
            {

                undoStack.Push(fileInformationTextBox.Text);

                redoStack.Clear();
            }


        }

      
    }
}

