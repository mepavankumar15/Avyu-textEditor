namespace MyTextEditor;

public partial class Form1 : Form
{
    public Form1()
    {
        this.KeyPreview = true;

        InitializeComponent();

        // create new textbox
        var editor = new TextBox();

        // creating multiline 
        editor.Multiline = true;
        // add scrollbars

        editor.ScrollBars = ScrollBars.Both;

        // Accept Tab key inside the editor
        editor.AcceptsTab =  true;
        // Make it fill the entire window
        editor.Dock = DockStyle.Fill;

        // setting font for editing
        editor.Font = new Font("Consolas", 12);
        // adding text box to windows

        this.Controls.Add(editor);

        // create a menu bar
        var menu = new MenuStrip();

        // FILE MENU

        // Create "File" menu
        var fileMenu = new ToolStripMenuItem("File");


        // create item inside File menu
        var OpenItem = new ToolStripMenuItem("Open");
        var saveItem = new ToolStripMenuItem("Save");
        var saveAsItem = new ToolStripMenuItem("Save As");
        var exitItem = new ToolStripMenuItem("Exit");

        // Add items to File
        fileMenu.DropDownItems.Add(OpenItem);
        fileMenu.DropDownItems.Add(saveItem);
        fileMenu.DropDownItems.Add(saveAsItem);
        fileMenu.DropDownItems.Add(new ToolStripMenuItem());
        fileMenu.DropDownItems.Add(exitItem);

        //Add "File" to to top menu bar
        menu.Items.Add(fileMenu);

        // EDIT MENU 
        var editMenu = new ToolStripMenuItem("Edit");

        //Create items inszide Edit Menu
        var undoItem = new ToolStripMenuItem("Undo");
        var cutItem = new ToolStripMenuItem("Cut");
        var copyItem = new ToolStripMenuItem("Copy");
        var pasteItem = new ToolStripMenuItem("Paste");
        var selectAllItem = new ToolStripMenuItem("Select All");

        editMenu.DropDownItems.Add(undoItem);
        editMenu.DropDownItems.Add(new ToolStripMenuItem());
        editMenu.DropDownItems.Add(cutItem);
        editMenu.DropDownItems.Add(copyItem);
        editMenu.DropDownItems.Add(pasteItem);
        editMenu.DropDownItems.Add(new ToolStripMenuItem());
        editMenu.DropDownItems.Add(selectAllItem);

        menu.Items.Add(editMenu);


        // add the MenuStrip to the window
        this.Controls.Add(menu);

        menu.Dock = DockStyle.Top;

        // file ops
        OpenItem.Click += (s,e) => OpenFile(editor);
        saveItem.Click += (s,e) => SaveFile(editor);
        saveAsItem.Click += (s,e) => SaveFileAs(editor);
        exitItem.Click += (s,e) => this.Close();

        // edit ops
        undoItem.Click += (s,e) => editor.Undo();
        cutItem.Click += (s,e) => editor.Cut();
        copyItem.Click += (s,e) => editor.Copy();
        pasteItem.Click += (s,e) => editor.Paste();
        selectAllItem.Click += (s,e) => editor.SelectAll();


        // the shortcut keys implementation
        this.KeyDown += (s,e) => HandleShortcuts(e,editor);


    }

    private string currentFilePath = "";
    // nrml Open
    private void OpenFile(TextBox editor)
    {
        using (var dialog = new OpenFileDialog())
        {
            dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog()== DialogResult.OK)
            {
                currentFilePath = dialog.FileName;
                editor.Text = File.ReadAllText(currentFilePath);
            }
        }
    }
    // Save
    private void SaveFile(TextBox editor)
    {
        if(string.IsNullOrEmpty(currentFilePath))
        {
            SaveFileAs(editor);
            return;
        }

        File.WriteAllText(currentFilePath, editor.Text);
    }
    // SaveAs
    private void SaveFileAs(TextBox editor)
    {
        using(var dialog = new SaveFileDialog())
        {
            dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = dialog.FileName;
                File.WriteAllText(currentFilePath,editor.Text);
            }
        }
    }
        
        // short cuts to apply
    private void HandleShortcuts(KeyEventArgs e, TextBox editor){

        // ctrl + s
        if (e.Control && !e.Shift && e.KeyCode == Keys.S)
        {
            SaveFile(editor);
            e.SuppressKeyPress = true;
        }

        // ctrl + shift +s
        if( e.Control && e.Shift && e.KeyCode == Keys.S)
        {
            SaveFileAs(editor);
            e.SuppressKeyPress = true;
        }

        // ctrl + O
        if(e.Control && e.KeyCode == Keys.O)
        {
            OpenFile(editor);
            e.SuppressKeyPress = true;
        }

        // ctrl + W
        if(e.Control && e.KeyCode == Keys.W)
        {
            this.Close();
            e.SuppressKeyPress = true;
        }
        // undo
        if(e.Control && e.KeyCode == Keys.Z)
        {
            editor.Undo();
            e.Handled = true;
            return;
        }

        // Select ALL
        if(e.Control && e.KeyCode == Keys.A)
        {
            editor.SelectAll();
            e.Handled = true;
            return;
        }
    }

}
