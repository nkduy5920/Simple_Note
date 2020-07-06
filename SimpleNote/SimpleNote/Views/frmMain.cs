using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleNote.Controllers;
using SimpleNote.Models;

namespace SimpleNote.Views
{
    public partial class frmMain : Form
    {
        private int ID;


        public static List<Note> lstNote;
        public static List<Note> lstTrash;

        public frmMain()
        {
            InitializeComponent();

            this.ID = -1;

            lstNote = new List<Note>();
            lstTrash = new List<Note>();

            this.richTextBoxDescription.ForeColor = Color.Black;
            this.richTextBoxDescription.Font = new Font(this.richTextBoxDescription.Font, FontStyle.Regular);

            foreach (Note note in NoteController.GetListNote())
            {
                Button btn = new Button();
                btn.Dock = DockStyle.Top;
                btn.Width = 340;
                btn.Height = 50;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Font = new Font("Open Sans", 12, FontStyle.Regular);
                btn.Padding = new Padding(10, 0, 0, 0);

                btn.Text = note.description;
                btn.Click += Btn_Click;

                CheckBox checkbox = new CheckBox();
                if (note.isPinned == true)
                    checkbox.Checked = true;
                checkbox.Text = "";
                checkbox.AutoSize = true;
                checkbox.Dock = DockStyle.Left;
                checkbox.CheckedChanged += Checkbox_CheckedChanged;

                btn.Controls.Add(checkbox);

                this.flpNote.Controls.Add(btn);

                if (checkbox.Checked)
                {
                    NoteController.MoveToFirst(note.ID);
                    this.flpNote.Controls.SetChildIndex(checkbox.Parent, 0);
                }

                this.ID++;
            }

            this.textBoxTags.Text = "Add a tag...";
            this.textBoxTags.ForeColor = Color.Gray;
            this.textBoxTags.TextAlign = HorizontalAlignment.Center;
            this.textBoxTags.GotFocus += TextBoxTags_GotFocus;
            this.textBoxTags.LostFocus += TextBoxTags_LostFocus;
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            var index = 0;
            foreach (Note note in NoteController.GetListNote())
                if (note.description == checkbox.Parent.Text)
                    index = note.ID;

            if (checkbox.Checked)
            {
                NoteController.MoveToFirst(index);
                this.flpNote.Controls.SetChildIndex(checkbox.Parent, 0);
            } else
            {
                NoteController.MoveToLast(index);
                this.flpNote.Controls.SetChildIndex(checkbox.Parent, NoteController.GetListNote().Count);
            }
        }

        private void TextBoxTags_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBoxTags.Text))
            {
                this.textBoxTags.Text = "Add a tag...";
                this.textBoxTags.ForeColor = Color.Gray;
            }
        }

        private void TextBoxTags_GotFocus(object sender, EventArgs e)
        {
            if (this.textBoxTags.Text == "Add a tag...")
            {
                this.textBoxTags.Text = "";
                this.textBoxTags.ForeColor = Color.Black;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.panelSlideMenu.Hide();

            this.richTextBoxDescription.BackColor = Color.White;
            formReload();
        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }



        private void pictureBoxSlideMenu_Click(object sender, EventArgs e)
        {
            if (this.panelSlideMenu.Visible)
            {
                this.panelSlideMenu.Hide();
            }
            else
            {
                this.panelSlideMenu.Show();
            }
        }

        private void pictureBoxToggleSidebar_Click(object sender, EventArgs e)
        {
            if (this.panelNoteReview.Visible)
            {
                this.panelNoteReview.Hide();
            }
            else
            {
                this.panelNoteReview.Show();
            }
        }

        private void formReload()
        {
            if (NoteController.GetListNote().Count > this.flpNote.Controls.Count)
                for (int i = this.flpNote.Controls.Count; i < NoteController.GetListNote().Count; i++)
                {
                    Button btn = new Button();
                    btn.Dock = DockStyle.Top;
                    btn.Width = 340;
                    btn.Height = 50;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                    btn.Font = new Font("Open Sans", 12, FontStyle.Regular);
                    btn.Padding = new Padding(10, 0, 0, 0);

                    btn.Text = NoteController.GetListNote()[i].description;
                    btn.Click += Btn_Click;

                    CheckBox checkbox = new CheckBox();
                    if (NoteController.GetListNote()[i].isPinned == true)
                        checkbox.Checked = true;
                    checkbox.Text = "";
                    checkbox.AutoSize = true;
                    checkbox.Dock = DockStyle.Left;
                    checkbox.CheckedChanged += Checkbox_CheckedChanged;

                    btn.Controls.Add(checkbox);

                    this.flpNote.Controls.Add(btn);

                    if (checkbox.Checked)
                    {
                        NoteController.MoveToFirst(NoteController.GetListNote()[i].ID);
                        this.flpNote.Controls.SetChildIndex(checkbox.Parent, 0);
                    }
                }
        }

        private void pictureBoxAllNote_Click(object sender, EventArgs e)
        {

            this.panelSlideMenu.Hide();
            formReload();
        }

        private void textBoxNoteSearch_Enter(object sender, EventArgs e)
        {

            if (this.textBoxNoteSearch.Text == " All Note")
            {
                this.textBoxNoteSearch.Text = "";
                this.textBoxNoteSearch.ForeColor = Color.Black;
            }

        }

        private void textBoxNoteSearch_Leave(object sender, EventArgs e)
        {

        }

        private void richTextBoxDescription_Enter(object sender, EventArgs e)
        {

        }

        private void richTextBoxDescription_Leave(object sender, EventArgs e)
        {

        }

        private void pictureBoxTrashNote_Click(object sender, EventArgs e)
        {
            frmTrash trash = new frmTrash();
            //this.trash.MdiParent = this;
            trash.Show();
        }

        private void pictureBoxNewNote_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < flpNote.Controls.Count; i++)
                if (flpNote.Controls[i].Text == "New Note")
                {
                    MessageBox.Show("Already has the new note that doesn't have a description, please enter the description for the note before add new note",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            Button btn = new Button();
            btn.Dock = DockStyle.Top;
            btn.Width = 330;
            btn.Height = 40;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Font = new Font("Open Sans", 12, FontStyle.Regular);
            btn.AutoEllipsis = true;
            btn.Padding = new Padding(10, 0, 0, 0);

            btn.Text = "New Note";

            this.richTextBoxDescription.Text = "";
            for (int i = 0; i < flpNote.Controls.Count; i++)
                flpNote.Controls[i].BackColor = Color.White;


            btn.BackColor = Color.LightGray;
            btn.Select();
            btn.Click += new EventHandler(this.Btn_Click);

            CheckBox checkbox = new CheckBox();
            checkbox.Text = "";
            checkbox.AutoSize = true;
            checkbox.Dock = DockStyle.Left;
            checkbox.CheckedChanged += Checkbox_CheckedChanged;

            btn.Controls.Add(checkbox);

            flpNote.Controls.Add(btn);

            this.richTextBoxDescription.Focus();
            this.richTextBoxDescription.ForeColor = Color.Black;
            this.richTextBoxDescription.Font = new Font(this.richTextBoxDescription.Font, FontStyle.Regular);
            this.ID++;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            for (int i = 0; i < this.flpNote.Controls.Count; i++)
                flpNote.Controls[i].BackColor = Color.White;

            for (int i = this.flpTags.Controls.Count - 1; i >= 0; i--)
            {
                Control c = this.flpTags.Controls[i];
                if (c.GetType() != typeof(TextBox))
                {
                    this.flpTags.Controls.RemoveAt(i);
                }
            }
                
            btn.BackColor = Color.LightGray;
            
            for (int i = 0; i < flpNote.Controls.Count; i++)
                if (flpNote.Controls[i].BackColor == Color.LightGray)
                {
                    this.richTextBoxDescription.Text = NoteController.GetNote(i).description;
                    Note note = NoteController.GetNote(i);
                    if (string.IsNullOrEmpty(note.tags))
                    {
                        note.tags = "";                     
                        break;
                    }

                    string[] tags = note.tags.Split(' ');

                    foreach (var tag in tags)
                    {
                        if (string.IsNullOrWhiteSpace(tag))
                            continue;
                        bool isContinue = false;
                        foreach (Control control in this.flpTags.Controls)
                            if (control.GetType() == typeof(RoundedButton) && control.Text == tag)
                                isContinue = true;

                        if (isContinue)
                        {
                            this.textBoxTags.Text = "";
                            continue;
                        }
                        RoundedButton rbtn = new RoundedButton();
                        rbtn.Width = 5;
                        rbtn.AutoSize = true;
                        rbtn.Text = tag;
                        rbtn.FlatStyle = FlatStyle.Flat;
                        rbtn.FlatAppearance.BorderSize = 0;
                        rbtn.TextAlign = ContentAlignment.MiddleCenter;
                        rbtn.Font = new Font("Open Sans", 9, FontStyle.Regular);
                        rbtn.Margin = new Padding(5, 5, 5, 5);
                        rbtn.Anchor = AnchorStyles.None;
                        rbtn.BackColor = Color.LightGray;

                        rbtn.Click += Btn_Click1;

                        int index = this.flpTags.Controls.IndexOf(this.textBoxTags) > 0 ? this.flpTags.Controls.IndexOf(this.textBoxTags) : 0;

                        this.flpTags.SuspendLayout();
                        this.flpTags.Controls.Add(rbtn);
                        this.flpTags.Controls.SetChildIndex(rbtn, index > 0 ? index : 0);
                        this.flpTags.ResumeLayout();
                    }
                    break;
                }
        }

        private void checkedListBoxNote_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBoxNote_Click(object sender, EventArgs e)
        {

        }

        private void richTextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            if (flpNote.Controls.Count <= 0)
                return;

            for (int i = 0; i < flpNote.Controls.Count; i++)
                if (flpNote.Controls[i].BackColor == Color.LightGray)
                {
                    if (i >= NoteController.GetListNote().Count)
                        break;

                    Note note = NoteController.GetNote(i);
                    if (string.IsNullOrEmpty(note.tags))
                    {
                        note.tags = "";
                        for (int j = this.flpTags.Controls.Count - 1; j >= 0; j--)
                        {
                            Control c = this.flpTags.Controls[j];
                            if (c.GetType() != typeof(TextBox))
                            {
                                this.flpTags.Controls.RemoveAt(j);
                            }
                        }

                        break;
                    }                   
                }

            // add note & save note by changing text (?)
            if (richTextBoxDescription.Text.Length > 0)
            {
                Note note = new Note();

                note.description = this.richTextBoxDescription.Text.Trim();
                

                for (int i = 0; i < flpNote.Controls.Count; i++)
                    if (flpNote.Controls[i].BackColor == Color.LightGray)
                    {
                        if (NoteController.GetNote(i) is null)
                        {
                            note.tags = "";
                            note.dateCreated = DateTime.Now;
                            break;
                        }
                        Note n = NoteController.GetNote(i);
                        note.dateCreated = n.dateCreated;
                        note.isPinned = n.isPinned;
                        if (string.IsNullOrEmpty(n.tags))
                            note.tags = "";
                        else note.tags = n.tags;
                        break;
                    }

                int index = 0;

                for (int i = 0; i < flpNote.Controls.Count; i++)
                    if (flpNote.Controls[i].BackColor == Color.LightGray)
                    {
                        index = i;
                        break;
                    }

                if (NoteController.GetNote(index) != null)
                {
                    note.ID = index;
                    NoteController.UpdateNote(note);
                }
                else
                {
                    note.ID = this.ID;
                    NoteController.AddNote(note);
                }

                flpNote.Controls[index].Text = note.description;
            }

        }

        private void pictureBoxTrash_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < flpNote.Controls.Count; i++)
                if (flpNote.Controls[i].BackColor == Color.LightGray)
                {
                    Trash trash = new Trash();
                    Note note = NoteController.GetNote(i);

                    trash.ID = TrashController.GetListTrash().Count;
                    trash.description = note.description;
                    trash.dateCreated = note.dateCreated;
                    trash.tags = note.tags;
                    trash.isPinned = note.isPinned;

                    TrashController.AddTrash(trash);
                    NoteController.RemoveNote(note);
                    NoteController.RefreshNote();

                    flpNote.Controls.Remove(flpNote.Controls[i]);
                    this.richTextBoxDescription.Text = "";
                    break;
                }
            for (int i = this.flpTags.Controls.Count - 1; i >= 0; i--)
            {
                Control c = this.flpTags.Controls[i];
                if (c.GetType() != typeof(TextBox))
                {
                    this.flpTags.Controls.RemoveAt(i);
                }
            }
        }

        private void frmMain_MdiChildActivate(object sender, EventArgs e)
        {
        }

        private void newNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < flpNote.Controls.Count; i++)
                if (flpNote.Controls[i].Text == "New Note")
                {
                    MessageBox.Show("Already has the new note that doesn't have a description, please enter the description for the note before add new note",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            Button btn = new Button();
            btn.Dock = DockStyle.Top;
            btn.Width = 330;
            btn.Height = 40;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Font = new Font("Open Sans", 12, FontStyle.Regular);
            btn.AutoEllipsis = true;
            btn.Padding = new Padding(10, 0, 0, 0);

            btn.Text = "New Note";

            this.richTextBoxDescription.Text = "";
            for (int i = 0; i < flpNote.Controls.Count; i++)
                flpNote.Controls[i].BackColor = Color.White;


            btn.BackColor = Color.LightGray;
            btn.Select();
            btn.Click += new EventHandler(this.Btn_Click);

            CheckBox checkbox = new CheckBox();
            checkbox.Text = "";
            checkbox.AutoSize = true;
            checkbox.Dock = DockStyle.Left;
            checkbox.CheckedChanged += Checkbox_CheckedChanged;

            btn.Controls.Add(checkbox);

            flpNote.Controls.Add(btn);

            this.richTextBoxDescription.Focus();
            this.richTextBoxDescription.ForeColor = Color.Black;
            this.richTextBoxDescription.Font = new Font(this.richTextBoxDescription.Font, FontStyle.Regular);
            this.ID++;
        }

        private void trashNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTrash trash = new frmTrash();
            trash.Show();
        }

        private void textBoxNoteSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = this.textBoxNoteSearch.Text.Split(' ');
            for (int i = 0; i < NoteController.GetListNote().Count; i++)
            {
                bool isVisible = true;
                foreach (var text in searchText)
                {
                    if (!text.Contains("tag:"))
                    {
                        if (!NoteController.GetListNote()[i].description.Contains(text))
                            isVisible = false;
                    }
                    else
                    {
                        var t = text.Remove(0, ("tag:").Length);

                        bool isContain = false;
                        var tags = NoteController.GetListNote()[i].tags.Split(' ');

                        foreach (var tag in tags)
                            if (tag == t)
                                isContain = true;

                        if (!isContain)
                            isVisible = false;

                        /*if (!NoteController.GetListNote()[i].tags.Contains(t))
                            isVisible = false;*/
                    }

                }

                if (!isVisible)
                    this.flpNote.Controls[i].Hide();
                else this.flpNote.Controls[i].Show();
            }
                
        }

        private void panelTags_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxTags_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                string[] tags = this.textBoxTags.Text.Split(' ');

                //Add tags to note

                for (int i = 0; i < flpNote.Controls.Count; i++)
                    if (flpNote.Controls[i].BackColor == Color.LightGray)
                    {
                        if (NoteController.GetNote(i) is null)
                        {
                            MessageBox.Show("The selected note doesn't have a description. Please enter a description before adding tags", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.textBoxTags.Text = "";
                            return;
                        }
                        Note note = NoteController.GetNote(i);
                        if (string.IsNullOrEmpty(note.tags))
                            note.tags = "";
                        foreach (var tag in tags)
                            if (!note.tags.Contains(tag))
                                note.tags += tag + " ";

                        NoteController.UpdateNote(note);
                    }

                foreach (var tag in tags)
                {
                    if (tag == "")
                        continue;
                    bool isContinue = false;
                    foreach (Control control in this.flpTags.Controls)
                        if (control.GetType() == typeof(RoundedButton) && control.Text == tag)
                            isContinue = true;

                    if (isContinue)
                    {
                        this.textBoxTags.Text = "";
                        continue;
                    }

                    //Add tags to view
                    RoundedButton btn = new RoundedButton();
                    btn.Width = 5;
                    btn.AutoSize = true;
                    btn.Text = tag;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.Font = new Font("Open Sans", 9, FontStyle.Regular);
                    btn.Margin = new Padding(5, 5, 5, 5);
                    btn.Anchor = AnchorStyles.None;
                    btn.BackColor = Color.LightGray;

                    btn.Click += Btn_Click1;

                    int index = this.flpTags.Controls.IndexOf(textBoxTags) > 0 ? this.flpTags.Controls.IndexOf(textBoxTags) : 0;

                    this.flpTags.SuspendLayout();
                    this.flpTags.Controls.Add(btn);
                    this.flpTags.Controls.SetChildIndex(btn, index > 0 ? index : 0);
                    this.flpTags.ResumeLayout();

                    this.textBoxTags.Text = "";
                    this.textBoxTags.Focus();
                }

                
            }
        }

        private void Btn_Click1(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            for (int i = 0; i < flpNote.Controls.Count; i++)
                if (flpNote.Controls[i].BackColor == Color.LightGray)
                {
                    Note note = NoteController.GetNote(i);
                    note.tags = note.tags.TrimStart();
                    var newTags = note.tags.Remove(note.tags.IndexOf(btn.Text.Trim()), btn.Text.Length);

                    btn.Parent.Controls.Remove(btn);
                    btn.Dispose();

                    note.tags = newTags;
                    NoteController.UpdateNote(note);
                }
        }

        private void textBoxTags_TextChanged(object sender, EventArgs e)
        {
            Size size = TextRenderer.MeasureText(textBoxTags.Text, textBoxTags.Font);
            textBoxTags.Width = size.Width;
            textBoxTags.Height = size.Height;
        }
    }
}

class RoundedButton : Button
{
    GraphicsPath GetRoundPath(RectangleF Rect, int radius)
    {
        float r2 = radius / 2f;
        GraphicsPath GraphPath = new GraphicsPath();
        GraphPath.AddArc(Rect.X, Rect.Y, radius, radius, 180, 90);
        GraphPath.AddLine(Rect.X + r2, Rect.Y, Rect.Width - r2, Rect.Y);
        GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y, radius, radius, 270, 90);
        GraphPath.AddLine(Rect.Width, Rect.Y + r2, Rect.Width, Rect.Height - r2);
        GraphPath.AddArc(Rect.X + Rect.Width - radius,
                         Rect.Y + Rect.Height - radius, radius, radius, 0, 90);
        GraphPath.AddLine(Rect.Width - r2, Rect.Height, Rect.X + r2, Rect.Height);
        GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - radius, radius, radius, 90, 90);
        GraphPath.AddLine(Rect.X, Rect.Height - r2, Rect.X, Rect.Y + r2);
        GraphPath.CloseFigure();
        return GraphPath;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        RectangleF Rect = new RectangleF(0, 0, this.Width, this.Height);
        using (GraphicsPath GraphPath = GetRoundPath(Rect, 25))
        {
            this.Region = new Region(GraphPath);         
        }
    }
}