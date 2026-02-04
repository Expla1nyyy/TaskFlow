namespace TaskFlow
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label labelView;
        private System.Windows.Forms.Button buttonListView;
        private System.Windows.Forms.Button buttonCardsView;
        private System.Windows.Forms.GroupBox groupBoxDayAfter;
        private System.Windows.Forms.Label labelDayAfterCount;
        private System.Windows.Forms.Label labelDayAfter;
        private System.Windows.Forms.GroupBox groupBoxTomorrow;
        private System.Windows.Forms.Label labelTomorrowCount;
        private System.Windows.Forms.Label labelTomorrow;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemAccount;
        private System.Windows.Forms.ToolStripMenuItem menuItemTheme;
        private System.Windows.Forms.ToolStripMenuItem menuItemLightTheme;
        private System.Windows.Forms.ToolStripMenuItem menuItemDarkTheme;
        private System.Windows.Forms.ToolStripMenuItem menuItemSettings;
        private System.Windows.Forms.Button buttonAddTask;
        private System.Windows.Forms.ListBox listBoxActiveTasks;
        private System.Windows.Forms.GroupBox groupBoxCompleted;
        private System.Windows.Forms.ListBox listBoxCompletedTasks;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonComplete;
        private System.Windows.Forms.Panel panelTaskCards;
        private System.Windows.Forms.Label labelTodayDate;
        private System.Windows.Forms.Panel panelTaskDetails;
        private System.Windows.Forms.Label labelTaskTitle;
        private System.Windows.Forms.Label labelTaskDescription;
        private System.Windows.Forms.Label labelDueTime;
        private System.Windows.Forms.Label labelCreatedTime;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Button buttonImportant;
        private System.Windows.Forms.Label labelDate19;
        private System.Windows.Forms.Label labelDate20;
        private System.Windows.Forms.Label labelDate21;
        private System.Windows.Forms.Label labelDate22;
        private System.Windows.Forms.Label labelDate23;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Panel panelNoTaskSelected;
        private System.Windows.Forms.Label labelNoTaskSelected;
        private System.Windows.Forms.Label labelDateFull;
        private System.Windows.Forms.Label labelTimeFull;
        private System.Windows.Forms.Button buttonEditTask;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelLeft = new System.Windows.Forms.Panel();
            this.groupBoxDayAfter = new System.Windows.Forms.GroupBox();
            this.labelDayAfterCount = new System.Windows.Forms.Label();
            this.labelDayAfter = new System.Windows.Forms.Label();
            this.groupBoxTomorrow = new System.Windows.Forms.GroupBox();
            this.labelTomorrowCount = new System.Windows.Forms.Label();
            this.labelTomorrow = new System.Windows.Forms.Label();
            this.buttonCardsView = new System.Windows.Forms.Button();
            this.buttonListView = new System.Windows.Forms.Button();
            this.labelView = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuItemAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLightTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDarkTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMain = new System.Windows.Forms.Panel();
            this.buttonEditTask = new System.Windows.Forms.Button();
            this.buttonComplete = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAddTask = new System.Windows.Forms.Button();
            this.panelTaskCards = new System.Windows.Forms.Panel();
            this.labelTodayDate = new System.Windows.Forms.Label();
            this.groupBoxCompleted = new System.Windows.Forms.GroupBox();
            this.listBoxCompletedTasks = new System.Windows.Forms.ListBox();
            this.listBoxActiveTasks = new System.Windows.Forms.ListBox();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelNoTaskSelected = new System.Windows.Forms.Panel();
            this.labelNoTaskSelected = new System.Windows.Forms.Label();
            this.panelTaskDetails = new System.Windows.Forms.Panel();
            this.labelTimeFull = new System.Windows.Forms.Label();
            this.labelDateFull = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelCreatedTime = new System.Windows.Forms.Label();
            this.labelDueTime = new System.Windows.Forms.Label();
            this.labelTaskDescription = new System.Windows.Forms.Label();
            this.labelTaskTitle = new System.Windows.Forms.Label();
            this.buttonImportant = new System.Windows.Forms.Button();
            this.labelDate23 = new System.Windows.Forms.Label();
            this.labelDate22 = new System.Windows.Forms.Label();
            this.labelDate21 = new System.Windows.Forms.Label();
            this.labelDate20 = new System.Windows.Forms.Label();
            this.labelDate19 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelLeft.SuspendLayout();
            this.groupBoxDayAfter.SuspendLayout();
            this.groupBoxTomorrow.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.groupBoxCompleted.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelNoTaskSelected.SuspendLayout();
            this.panelTaskDetails.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLeft.Controls.Add(this.groupBoxDayAfter);
            this.panelLeft.Controls.Add(this.groupBoxTomorrow);
            this.panelLeft.Controls.Add(this.buttonCardsView);
            this.panelLeft.Controls.Add(this.buttonListView);
            this.panelLeft.Controls.Add(this.labelView);
            this.panelLeft.Controls.Add(this.menuStrip1);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(250, 600);
            this.panelLeft.TabIndex = 0;
            // 
            // groupBoxDayAfter
            // 
            this.groupBoxDayAfter.Controls.Add(this.labelDayAfterCount);
            this.groupBoxDayAfter.Controls.Add(this.labelDayAfter);
            this.groupBoxDayAfter.Location = new System.Drawing.Point(15, 210);
            this.groupBoxDayAfter.Name = "groupBoxDayAfter";
            this.groupBoxDayAfter.Size = new System.Drawing.Size(220, 80);
            this.groupBoxDayAfter.TabIndex = 5;
            this.groupBoxDayAfter.TabStop = false;
            // 
            // labelDayAfterCount
            // 
            this.labelDayAfterCount.AutoSize = true;
            this.labelDayAfterCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDayAfterCount.ForeColor = System.Drawing.Color.Gray;
            this.labelDayAfterCount.Location = new System.Drawing.Point(10, 40);
            this.labelDayAfterCount.Name = "labelDayAfterCount";
            this.labelDayAfterCount.Size = new System.Drawing.Size(52, 15);
            this.labelDayAfterCount.TabIndex = 1;
            this.labelDayAfterCount.Text = "0 задач";
            // 
            // labelDayAfter
            // 
            this.labelDayAfter.AutoSize = true;
            this.labelDayAfter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelDayAfter.Location = new System.Drawing.Point(10, 20);
            this.labelDayAfter.Name = "labelDayAfter";
            this.labelDayAfter.Size = new System.Drawing.Size(135, 19);
            this.labelDayAfter.TabIndex = 0;
            this.labelDayAfter.Text = "Послезавтра, ...";
            // 
            // groupBoxTomorrow
            // 
            this.groupBoxTomorrow.Controls.Add(this.labelTomorrowCount);
            this.groupBoxTomorrow.Controls.Add(this.labelTomorrow);
            this.groupBoxTomorrow.Location = new System.Drawing.Point(15, 120);
            this.groupBoxTomorrow.Name = "groupBoxTomorrow";
            this.groupBoxTomorrow.Size = new System.Drawing.Size(220, 80);
            this.groupBoxTomorrow.TabIndex = 4;
            this.groupBoxTomorrow.TabStop = false;
            // 
            // labelTomorrowCount
            // 
            this.labelTomorrowCount.AutoSize = true;
            this.labelTomorrowCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelTomorrowCount.ForeColor = System.Drawing.Color.Gray;
            this.labelTomorrowCount.Location = new System.Drawing.Point(10, 40);
            this.labelTomorrowCount.Name = "labelTomorrowCount";
            this.labelTomorrowCount.Size = new System.Drawing.Size(52, 15);
            this.labelTomorrowCount.TabIndex = 1;
            this.labelTomorrowCount.Text = "0 задач";
            // 
            // labelTomorrow
            // 
            this.labelTomorrow.AutoSize = true;
            this.labelTomorrow.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTomorrow.Location = new System.Drawing.Point(10, 20);
            this.labelTomorrow.Name = "labelTomorrow";
            this.labelTomorrow.Size = new System.Drawing.Size(89, 19);
            this.labelTomorrow.TabIndex = 0;
            this.labelTomorrow.Text = "Завтра, ...";
            // 
            // buttonCardsView
            // 
            this.buttonCardsView.Location = new System.Drawing.Point(140, 60);
            this.buttonCardsView.Name = "buttonCardsView";
            this.buttonCardsView.Size = new System.Drawing.Size(80, 30);
            this.buttonCardsView.TabIndex = 3;
            this.buttonCardsView.Text = "карточки";
            this.buttonCardsView.UseVisualStyleBackColor = true;
            // 
            // buttonListView
            // 
            this.buttonListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.buttonListView.ForeColor = System.Drawing.Color.White;
            this.buttonListView.Location = new System.Drawing.Point(30, 60);
            this.buttonListView.Name = "buttonListView";
            this.buttonListView.Size = new System.Drawing.Size(80, 30);
            this.buttonListView.TabIndex = 2;
            this.buttonListView.Text = "список";
            this.buttonListView.UseVisualStyleBackColor = false;
            // 
            // labelView
            // 
            this.labelView.AutoSize = true;
            this.labelView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelView.ForeColor = System.Drawing.Color.Gray;
            this.labelView.Location = new System.Drawing.Point(15, 35);
            this.labelView.Name = "labelView";
            this.labelView.Size = new System.Drawing.Size(32, 15);
            this.labelView.TabIndex = 1;
            this.labelView.Text = "ВИД";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAccount,
            this.menuItemTheme,
            this.menuItemSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(248, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuItemAccount
            // 
            this.menuItemAccount.Name = "menuItemAccount";
            this.menuItemAccount.Size = new System.Drawing.Size(69, 20);
            this.menuItemAccount.Text = "1. аккаунт";
            // 
            // menuItemTheme
            // 
            this.menuItemTheme.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLightTheme,
            this.menuItemDarkTheme});
            this.menuItemTheme.Name = "menuItemTheme";
            this.menuItemTheme.Size = new System.Drawing.Size(135, 20);
            this.menuItemTheme.Text = "2. тема (светлая/тёмная)";
            // 
            // menuItemLightTheme
            // 
            this.menuItemLightTheme.Checked = true;
            this.menuItemLightTheme.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemLightTheme.Name = "menuItemLightTheme";
            this.menuItemLightTheme.Size = new System.Drawing.Size(126, 22);
            this.menuItemLightTheme.Text = "Светлая";
            // 
            // menuItemDarkTheme
            // 
            this.menuItemDarkTheme.Name = "menuItemDarkTheme";
            this.menuItemDarkTheme.Size = new System.Drawing.Size(126, 22);
            this.menuItemDarkTheme.Text = "Тёмная";
            // 
            // menuItemSettings
            // 
            this.menuItemSettings.Name = "menuItemSettings";
            this.menuItemSettings.Size = new System.Drawing.Size(96, 20);
            this.menuItemSettings.Text = "3. настройки";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Controls.Add(this.buttonEditTask);
            this.panelMain.Controls.Add(this.buttonComplete);
            this.panelMain.Controls.Add(this.buttonDelete);
            this.panelMain.Controls.Add(this.buttonAddTask);
            this.panelMain.Controls.Add(this.panelTaskCards);
            this.panelMain.Controls.Add(this.labelTodayDate);
            this.panelMain.Controls.Add(this.groupBoxCompleted);
            this.panelMain.Controls.Add(this.listBoxActiveTasks);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(250, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(550, 600);
            this.panelMain.TabIndex = 1;
            // 
            // buttonEditTask
            // 
            this.buttonEditTask.Location = new System.Drawing.Point(65, 570);
            this.buttonEditTask.Name = "buttonEditTask";
            this.buttonEditTask.Size = new System.Drawing.Size(100, 30);
            this.buttonEditTask.TabIndex = 12;
            this.buttonEditTask.Text = "Редактировать";
            this.buttonEditTask.UseVisualStyleBackColor = true;
            // 
            // buttonComplete
            // 
            this.buttonComplete.Location = new System.Drawing.Point(425, 570);
            this.buttonComplete.Name = "buttonComplete";
            this.buttonComplete.Size = new System.Drawing.Size(100, 30);
            this.buttonComplete.TabIndex = 11;
            this.buttonComplete.Text = "Выполнить";
            this.buttonComplete.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(305, 570);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 30);
            this.buttonDelete.TabIndex = 10;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            // 
            // buttonAddTask
            // 
            this.buttonAddTask.Location = new System.Drawing.Point(185, 570);
            this.buttonAddTask.Name = "buttonAddTask";
            this.buttonAddTask.Size = new System.Drawing.Size(100, 30);
            this.buttonAddTask.TabIndex = 9;
            this.buttonAddTask.Text = "Добавить +";
            this.buttonAddTask.UseVisualStyleBackColor = true;
            // 
            // panelTaskCards
            // 
            this.panelTaskCards.AutoScroll = true;
            this.panelTaskCards.Location = new System.Drawing.Point(23, 60);
            this.panelTaskCards.Name = "panelTaskCards";
            this.panelTaskCards.Size = new System.Drawing.Size(502, 260);
            this.panelTaskCards.TabIndex = 7;
            this.panelTaskCards.Visible = false;
            // 
            // labelTodayDate
            // 
            this.labelTodayDate.AutoSize = true;
            this.labelTodayDate.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTodayDate.Location = new System.Drawing.Point(23, 23);
            this.labelTodayDate.Name = "labelTodayDate";
            this.labelTodayDate.Size = new System.Drawing.Size(158, 25);
            this.labelTodayDate.TabIndex = 8;
            this.labelTodayDate.Text = "среда, 4 февраля";
            // 
            // groupBoxCompleted
            // 
            this.groupBoxCompleted.Controls.Add(this.listBoxCompletedTasks);
            this.groupBoxCompleted.Location = new System.Drawing.Point(23, 340);
            this.groupBoxCompleted.Name = "groupBoxCompleted";
            this.groupBoxCompleted.Size = new System.Drawing.Size(502, 220);
            this.groupBoxCompleted.TabIndex = 6;
            this.groupBoxCompleted.TabStop = false;
            this.groupBoxCompleted.Text = "Выполненные задачи";
            // 
            // listBoxCompletedTasks
            // 
            this.listBoxCompletedTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCompletedTasks.FormattingEnabled = true;
            this.listBoxCompletedTasks.ItemHeight = 15;
            this.listBoxCompletedTasks.Location = new System.Drawing.Point(3, 19);
            this.listBoxCompletedTasks.Name = "listBoxCompletedTasks";
            this.listBoxCompletedTasks.Size = new System.Drawing.Size(496, 198);
            this.listBoxCompletedTasks.TabIndex = 0;
            // 
            // listBoxActiveTasks
            // 
            this.listBoxActiveTasks.FormattingEnabled = true;
            this.listBoxActiveTasks.ItemHeight = 15;
            this.listBoxActiveTasks.Location = new System.Drawing.Point(23, 60);
            this.listBoxActiveTasks.Name = "listBoxActiveTasks";
            this.listBoxActiveTasks.Size = new System.Drawing.Size(502, 259);
            this.listBoxActiveTasks.TabIndex = 2;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRight.Controls.Add(this.panelNoTaskSelected);
            this.panelRight.Controls.Add(this.panelTaskDetails);
            this.panelRight.Controls.Add(this.labelDate23);
            this.panelRight.Controls.Add(this.labelDate22);
            this.panelRight.Controls.Add(this.labelDate21);
            this.panelRight.Controls.Add(this.labelDate20);
            this.panelRight.Controls.Add(this.labelDate19);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(800, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(20);
            this.panelRight.Size = new System.Drawing.Size(300, 600);
            this.panelRight.TabIndex = 2;
            // 
            // panelNoTaskSelected
            // 
            this.panelNoTaskSelected.Controls.Add(this.labelNoTaskSelected);
            this.panelNoTaskSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNoTaskSelected.Location = new System.Drawing.Point(20, 20);
            this.panelNoTaskSelected.Name = "panelNoTaskSelected";
            this.panelNoTaskSelected.Size = new System.Drawing.Size(258, 558);
            this.panelNoTaskSelected.TabIndex = 14;
            this.panelNoTaskSelected.Visible = true;
            // 
            // labelNoTaskSelected
            // 
            this.labelNoTaskSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNoTaskSelected.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelNoTaskSelected.ForeColor = System.Drawing.Color.Gray;
            this.labelNoTaskSelected.Location = new System.Drawing.Point(0, 0);
            this.labelNoTaskSelected.Name = "labelNoTaskSelected";
            this.labelNoTaskSelected.Size = new System.Drawing.Size(258, 558);
            this.labelNoTaskSelected.TabIndex = 0;
            this.labelNoTaskSelected.Text = "Выберите задачу";
            this.labelNoTaskSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTaskDetails
            // 
            this.panelTaskDetails.Controls.Add(this.labelTimeFull);
            this.panelTaskDetails.Controls.Add(this.labelDateFull);
            this.panelTaskDetails.Controls.Add(this.textBoxNotes);
            this.panelTaskDetails.Controls.Add(this.labelCreatedTime);
            this.panelTaskDetails.Controls.Add(this.labelDueTime);
            this.panelTaskDetails.Controls.Add(this.labelTaskDescription);
            this.panelTaskDetails.Controls.Add(this.labelTaskTitle);
            this.panelTaskDetails.Controls.Add(this.buttonImportant);
            this.panelTaskDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTaskDetails.Location = new System.Drawing.Point(20, 20);
            this.panelTaskDetails.Name = "panelTaskDetails";
            this.panelTaskDetails.Size = new System.Drawing.Size(258, 558);
            this.panelTaskDetails.TabIndex = 13;
            this.panelTaskDetails.Visible = false;
            // 
            // labelTimeFull
            // 
            this.labelTimeFull.AutoSize = true;
            this.labelTimeFull.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelTimeFull.ForeColor = System.Drawing.Color.Gray;
            this.labelTimeFull.Location = new System.Drawing.Point(3, 180);
            this.labelTimeFull.Name = "labelTimeFull";
            this.labelTimeFull.Size = new System.Drawing.Size(52, 15);
            this.labelTimeFull.TabIndex = 15;
            this.labelTimeFull.Text = "13:08:05";
            // 
            // labelDateFull
            // 
            this.labelDateFull.AutoSize = true;
            this.labelDateFull.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDateFull.ForeColor = System.Drawing.Color.Gray;
            this.labelDateFull.Location = new System.Drawing.Point(3, 160);
            this.labelDateFull.Name = "labelDateFull";
            this.labelDateFull.Size = new System.Drawing.Size(81, 15);
            this.labelDateFull.TabIndex = 14;
            this.labelDateFull.Text = "5 февраля 2022";
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNotes.BackColor = System.Drawing.Color.White;
            this.textBoxNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNotes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxNotes.Location = new System.Drawing.Point(3, 230);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxNotes.Size = new System.Drawing.Size(252, 150);
            this.textBoxNotes.TabIndex = 12;
            this.textBoxNotes.Text = "Заметки к задаче...";
            // 
            // labelCreatedTime
            // 
            this.labelCreatedTime.AutoSize = true;
            this.labelCreatedTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelCreatedTime.ForeColor = System.Drawing.Color.Gray;
            this.labelCreatedTime.Location = new System.Drawing.Point(3, 120);
            this.labelCreatedTime.Name = "labelCreatedTime";
            this.labelCreatedTime.Size = new System.Drawing.Size(90, 15);
            this.labelCreatedTime.TabIndex = 11;
            this.labelCreatedTime.Text = "СОЗДАНА В 13:08";
            // 
            // labelDueTime
            // 
            this.labelDueTime.AutoSize = true;
            this.labelDueTime.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelDueTime.ForeColor = System.Drawing.Color.Gray;
            this.labelDueTime.Location = new System.Drawing.Point(3, 80);
            this.labelDueTime.Name = "labelDueTime";
            this.labelDueTime.Size = new System.Drawing.Size(134, 19);
            this.labelDueTime.TabIndex = 10;
            this.labelDueTime.Text = "ВЫПОЛНИТЬ ДО 13:08";
            // 
            // labelTaskDescription
            // 
            this.labelTaskDescription.AutoSize = true;
            this.labelTaskDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelTaskDescription.ForeColor = System.Drawing.Color.Gray;
            this.labelTaskDescription.Location = new System.Drawing.Point(3, 45);
            this.labelTaskDescription.Name = "labelTaskDescription";
            this.labelTaskDescription.Size = new System.Drawing.Size(79, 15);
            this.labelTaskDescription.TabIndex = 9;
            this.labelTaskDescription.Text = "Описание задачи";
            // 
            // labelTaskTitle
            // 
            this.labelTaskTitle.AutoSize = true;
            this.labelTaskTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTaskTitle.Location = new System.Drawing.Point(3, 10);
            this.labelTaskTitle.MaximumSize = new System.Drawing.Size(250, 0);
            this.labelTaskTitle.Name = "labelTaskTitle";
            this.labelTaskTitle.Size = new System.Drawing.Size(98, 30);
            this.labelTaskTitle.TabIndex = 8;
            this.labelTaskTitle.Text = "ФИЛЬМЫ";
            // 
            // buttonImportant
            // 
            this.buttonImportant.Location = new System.Drawing.Point(160, 40);
            this.buttonImportant.Name = "buttonImportant";
            this.buttonImportant.Size = new System.Drawing.Size(95, 30);
            this.buttonImportant.TabIndex = 5;
            this.buttonImportant.Text = "☆ Обычная";
            this.buttonImportant.UseVisualStyleBackColor = true;
            // 
            // labelDate23
            // 
            this.labelDate23.AutoSize = true;
            this.labelDate23.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDate23.Location = new System.Drawing.Point(240, 40);
            this.labelDate23.Name = "labelDate23";
            this.labelDate23.Size = new System.Drawing.Size(50, 19);
            this.labelDate23.TabIndex = 4;
            this.labelDate23.Text = "23 янв.";
            // 
            // labelDate22
            // 
            this.labelDate22.AutoSize = true;
            this.labelDate22.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDate22.Location = new System.Drawing.Point(190, 40);
            this.labelDate22.Name = "labelDate22";
            this.labelDate22.Size = new System.Drawing.Size(50, 19);
            this.labelDate22.TabIndex = 3;
            this.labelDate22.Text = "22 янв.";
            // 
            // labelDate21
            // 
            this.labelDate21.AutoSize = true;
            this.labelDate21.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelDate21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.labelDate21.Location = new System.Drawing.Point(140, 40);
            this.labelDate21.Name = "labelDate21";
            this.labelDate21.Size = new System.Drawing.Size(50, 19);
            this.labelDate21.TabIndex = 2;
            this.labelDate21.Text = "21 янв.";
            // 
            // labelDate20
            // 
            this.labelDate20.AutoSize = true;
            this.labelDate20.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDate20.Location = new System.Drawing.Point(90, 40);
            this.labelDate20.Name = "labelDate20";
            this.labelDate20.Size = new System.Drawing.Size(50, 19);
            this.labelDate20.TabIndex = 1;
            this.labelDate20.Text = "20 янв.";
            // 
            // labelDate19
            // 
            this.labelDate19.AutoSize = true;
            this.labelDate19.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDate19.Location = new System.Drawing.Point(40, 40);
            this.labelDate19.Name = "labelDate19";
            this.labelDate19.Size = new System.Drawing.Size(50, 19);
            this.labelDate19.TabIndex = 0;
            this.labelDate19.Text = "19 янв.";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 600);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1100, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel.Text = "Готов к работе";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 622);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaskFlow - Управление задачами";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.groupBoxDayAfter.ResumeLayout(false);
            this.groupBoxDayAfter.PerformLayout();
            this.groupBoxTomorrow.ResumeLayout(false);
            this.groupBoxTomorrow.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.groupBoxCompleted.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.panelNoTaskSelected.ResumeLayout(false);
            this.panelTaskDetails.ResumeLayout(false);
            this.panelTaskDetails.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}