using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MudrakPatel_Bonus_DisplayAuthorsTable
{
    public partial class DisplayAuthorsTable : Form
    {
        public DisplayAuthorsTable()
        {
            InitializeComponent();
        }
        private MudrakPatel_Practice.BooksEntities dbcontext = new MudrakPatel_Practice.BooksEntities();

        private void DisplayAuthorsTable_Load(object sender, EventArgs e)
        {
            dbcontext.Authors
                .OrderBy(author => author.LastName)
                .ThenBy(author => author.FirstName)
                .Load();

            authorBindingSource.DataSource = dbcontext.Authors.Local;
        }

        private void authorBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            authorBindingSource.EndEdit();

            try
            {
                dbcontext.SaveChanges();
            }
            catch (DbEntityValidationException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedAuthor = from author in dbcontext.Authors
                                     where author.LastName.Equals(searchTextBox.Text)
                                     select author;

                var details = "";
                foreach (var author in selectedAuthor)
                {
                    details = details + "<<" + author.FirstName + ">>"
                                      + "<<" + author.LastName + ">>"
                                      + "<<" + author.AuthorID + ">>"
                                      + Environment.NewLine;
                }
                MessageBox.Show(details, "--Searched Authors--");
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message, "Exception occured!");
            }
            catch (Exception exception)
            {
                MessageBox.Show("The author you are looking for might not exist in the database." 
                                + Environment.NewLine + "OR" + "Check that you entered the correct lastname." 
                                + Environment.NewLine + exception.Message, "Exception or input error!");
            }
        }
    }
}
