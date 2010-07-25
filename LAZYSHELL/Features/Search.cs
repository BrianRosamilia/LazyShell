﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LAZYSHELL
{
    public partial class Search : Form
    {
        private delegate void Function();
        private Delegate function;
        private ToolStripTextBox searchField;
        private ToolStripButton searchButton;
        private System.Windows.Forms.ToolStripComboBox searchIndexName;
        private ToolStripNumericUpDown searchIndexNum;
        private IList names; public IList Names { get { return names; } set { names = value; } }
        private bool searchFieldEnter = false;
        private bool initialized = false;
        /// <summary>
        /// Loads a search form containing the results of a search query.
        /// </summary>
        /// <param name="searchIndexNum">The search index control to update when a search result is selected.</param>
        /// <param name="searchField">The search field control containing the search query text.</param>
        /// <param name="searchButton">The search button control that invokes the search.</param>
        /// <param name="names">The data list to search for a specified query in.</param>
        public Search(ToolStripNumericUpDown searchIndexNum, ToolStripTextBox searchField, ToolStripButton searchButton, IList names)
        {
            InitializeComponent();
            this.listBox.Enabled = true;
            this.listBox.Show();
            this.names = names;
            this.searchIndexNum = searchIndexNum;
            this.searchField = searchField;
            this.searchButton = searchButton;
            InitializeProperties();
            this.function = new Function(LoadSearch);
            this.function.DynamicInvoke();
        }
        /// <summary>
        /// Loads a search form containing the results of a search query.
        /// </summary>
        /// <param name="searchIndexName">The search index control to update when a search result is selected.</param>
        /// <param name="searchField">The search field control containing the search query text.</param>
        /// <param name="searchButton">The search button control that invokes the search.</param>
        /// <param name="names">The data list to search for a specified query in.</param>
        public Search(System.Windows.Forms.ToolStripComboBox searchIndexName, ToolStripTextBox searchField, ToolStripButton searchButton, IList names)
        {
            InitializeComponent();
            this.listBox.Enabled = true;
            this.listBox.Show();
            this.names = names;
            this.searchIndexName = searchIndexName;
            this.searchField = searchField;
            this.searchButton = searchButton;
            InitializeProperties();
            this.function = new Function(LoadSearch);
            this.function.DynamicInvoke();
        }
        /// <summary>
        /// Loads a search form containing the results of a search query.
        /// </summary>
        /// <param name="searchIndexNum">The search index control to update when a search result is selected.</param>
        /// <param name="searchField">The search field control containing the search query text.</param>
        /// <param name="searchButton">The search button control that invokes the search.</param>
        /// <param name="function">The function to execute when a search is invoked.</param>
        /// <param name="type">The type of control that contains the search results.
        /// Options include: treeView, richTextBox</param>
        public Search(ToolStripNumericUpDown searchIndexNum, ToolStripTextBox searchField, ToolStripButton searchButton, Delegate function, string type)
        {
            InitializeComponent();
            this.searchIndexNum = searchIndexNum;
            this.searchField = searchField;
            this.searchButton = searchButton;
            InitializeProperties();
            this.function = function;
            if (type == "treeView")
            {
                this.treeView.Enabled = true;
                this.treeView.Show();
                this.function.DynamicInvoke(treeView);
            }
            else if (type == "richTextBox")
            {
                this.richTextBox.Enabled = true;
                this.richTextBox.Show();
                this.function.DynamicInvoke(richTextBox);
            }
        }
        private void InitializeProperties()
        {
            this.searchField.ForeColor = SystemColors.ControlDark;
            this.searchField.Text = "Search...";
            this.searchField.KeyDown += new KeyEventHandler(searchField_KeyDown);
            this.searchField.KeyUp += new KeyEventHandler(searchField_KeyUp);
            this.searchField.MouseDown += new MouseEventHandler(searchField_MouseDown);
            this.searchField.Leave += new EventHandler(searchField_Leave);
            this.searchButton.CheckOnClick = true;
            this.searchButton.Click += new EventHandler(searchButton_Click);
        }
        public void LoadSearch()
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            if (searchField.Text == "")
            {
                listBox.EndUpdate();
                this.Height = 32;
                return;
            }
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i].ToString().IndexOf(searchField.Text, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchItem searchItem = new SearchItem(i, (string)names[i]);
                    listBox.Items.Add(searchItem);
                }
            }
            this.Height = Math.Min(
                listBox.Items.Count * listBox.ItemHeight + 32,
                Screen.PrimaryScreen.WorkingArea.Height - this.Top - 16);
            listBox.EndUpdate();
        }
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem == null) return;
            int index = ((SearchItem)listBox.SelectedItem).Index;
            if (searchIndexNum != null && index < searchIndexNum.Maximum)
                searchIndexNum.Value = index;
            else if (searchIndexName != null && index < searchIndexName.Items.Count)
                searchIndexName.SelectedIndex = index;
        }
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (searchIndexNum != null)
                searchIndexNum.Value = (int)treeView.SelectedNode.Tag;
            else if (searchIndexName != null)
                searchIndexName.SelectedIndex = (int)treeView.SelectedNode.Tag;
        }
        private void searchField_MouseDown(object sender, MouseEventArgs e)
        {
            if (!searchFieldEnter)
            {
                searchField.Text = "";
                searchField.ForeColor = SystemColors.ControlText;
            }
            searchFieldEnter = true;
        }
        private void searchField_KeyDown(object sender, KeyEventArgs e)
        {
            if (!searchFieldEnter)
                searchField.ForeColor = SystemColors.ControlText;
            searchFieldEnter = true;
            if (e.KeyData == Keys.Enter)
            {
                searchButton.Checked = true;
                searchButton_Click(null, null);
                if (richTextBox.Enabled)
                    function.DynamicInvoke(richTextBox);
            }
        }
        private void searchField_KeyUp(object sender, KeyEventArgs e)
        {
            if (listBox.Enabled)
                this.function.DynamicInvoke();
            if (treeView.Enabled)
                this.function.DynamicInvoke(treeView);
            searchField.Focus();
        }
        private void searchField_Leave(object sender, EventArgs e)
        {
            if (searchField.Text == "")
            {
                searchField.Text = "Search...";
                searchField.ForeColor = SystemColors.ControlDark;
                searchFieldEnter = false;
            }
        }
        private void searchButton_Click(object sender, EventArgs e)
        {
            searchField.Visible = searchButton.Checked;
            this.Visible = searchButton.Checked;
            if (this.Visible && !initialized)
            {
                this.Location = searchField.Control.PointToScreen(new Point(0, searchField.Height));
                initialized = true;
            }
        }
        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.searchButton.Checked = false;
                this.Hide();
            }
        }
        private class SearchItem
        {
            public int Index;
            public string Text;
            public SearchItem(int index, string text)
            {
                this.Index = index;
                this.Text = text;
            }
            public override string ToString()
            {
                return this.Text;
            }
        }
    }
}
