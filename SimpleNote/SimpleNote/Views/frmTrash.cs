using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleNote.Controllers;
using SimpleNote.Models;

namespace SimpleNote.Views
{
    public partial class frmTrash : Form
    {
        public static List<Note> lstTrash = new List<Note>();

        public frmTrash()
        {
            InitializeComponent();
            
        }

        private void textBoxTrashNoteSearch_Enter(object sender, EventArgs e)
        {
            if (this.textBoxTrashNoteSearch.Text == "Trash")
            {
                this.textBoxTrashNoteSearch.Text = "";
                this.textBoxTrashNoteSearch.ForeColor = Color.Black;
            }
        }

        private void textBoxTrashNoteSearch_Leave(object sender, EventArgs e)
        {
            if (this.textBoxTrashNoteSearch.Text == "")
            {
                this.textBoxTrashNoteSearch.ForeColor = Color.LightGray;
                this.textBoxTrashNoteSearch.Text = "Trash";
            }
        }

        private void frmTrash_Load(object sender, EventArgs e)
        {
            if (this.richTextBoxTrashDescription.ReadOnly == true)
            {
                this.richTextBoxTrashDescription.BackColor = Color.White;
            }
            
            foreach (Trash trash in TrashController.GetListTrash())
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

                btn.Text = trash.description;
                btn.Click += Btn_Click;

                CheckBox checkbox = new CheckBox();
                if (trash.isPinned == true)
                    checkbox.Checked = true;
                checkbox.Text = "";
                checkbox.AutoSize = true;
                checkbox.Dock = DockStyle.Left;
                checkbox.Enabled = false;

                btn.Controls.Add(checkbox);

                this.flpTrash.Controls.Add(btn);

                if (checkbox.Checked)
                {
                    TrashController.MoveToFirst(trash.ID);
                    this.flpTrash.Controls.SetChildIndex(checkbox.Parent, 0);
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            for (int i = 0; i < flpTrash.Controls.Count; i++)
                flpTrash.Controls[i].BackColor = Color.White;

            btn.BackColor = Color.LightGray;
            this.richTextBoxTrashDescription.Text = btn.Text;
        }

        private void btnDeleteTrash_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.flpTrash.Controls.Count; i++)
                if (this.flpTrash.Controls[i].BackColor == Color.LightGray)
                {
                    TrashController.RefreshTrash();
                    Trash trash = TrashController.GetTrash(i);                
                    TrashController.DeleteForever(trash);
                    flpTrash.Controls.Remove(flpTrash.Controls[i]);
                }
            this.richTextBoxTrashDescription.Text = "";
        }

        private void checkedListBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnRestoreTrash_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < flpTrash.Controls.Count; i++)
                if (flpTrash.Controls[i].BackColor == Color.LightGray)
                {
                    TrashController.RefreshTrash();
                    Trash trash = TrashController.GetTrash(i);
                    Note note = new Note();

                    note.ID = NoteController.GetListNote().Count;
                    note.description = trash.description;
                    note.dateCreated = trash.dateCreated;
                    note.tags = trash.tags;
                    note.isPinned = trash.isPinned;

                    TrashController.DeleteForever(trash);
                    TrashController.RefreshTrash();
                    NoteController.AddNote(note);
                    

                    flpTrash.Controls.Remove(flpTrash.Controls[i]);
                    this.richTextBoxTrashDescription.Text = "";
                }
        }

        private void textBoxTrashNoteSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = this.textBoxTrashNoteSearch.Text.Split(' ');
            for (int i = 0; i < TrashController.GetListTrash().Count; i++)
            {
                bool isVisible = true;
                foreach (var text in searchText)
                {
                    if (!text.Contains("tag:"))
                    {
                        if (!TrashController.GetListTrash()[i].description.Contains(text))
                            isVisible = false;
                    }
                    else
                    {
                        var t = text.Remove(0, ("tag:").Length);

                        bool isContain = false;
                        var tags = TrashController.GetListTrash()[i].tags.Split(' ');

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
                    this.flpTrash.Controls[i].Hide();
                else this.flpTrash.Controls[i].Show();
            }
        }
    }
}
