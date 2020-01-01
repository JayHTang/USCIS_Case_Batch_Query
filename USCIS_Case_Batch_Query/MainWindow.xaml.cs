using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace USCIS_Case_Batch_Query
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            casesDataGrid.ItemsSource = this.caseStatuses;
            ReceiptNumber.Text = ConfigurationManager.AppSettings["DefaultReceiptNumber"];
            NextCases.Text = ConfigurationManager.AppSettings["DefaultNextCases"];
            logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            NextCasesRange.Content = $"Range: 0 - {RANGE}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Validation.GetHasError(ReceiptNumber) || Validation.GetHasError(NextCases))
            {
                return;
            }

            this.caseStatuses = new List<CaseStatus>();

            Status.Content = "Running";
            casesDataGrid.AutoGeneratingColumn += CasesDataGrid_AutoGeneratingColumn;

            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(new JobParams { ReceiptNumber = ReceiptNumber.Text, NextCases = Int32.Parse(NextCases.Text) });
        }

        private async Task GetCaseStatusAsync(string receiptNumber)
        {
            // get response
            HttpResponseMessage response = await client.GetAsync($"https://egov.uscis.gov/casestatus/mycasestatus.do?appReceiptNum={receiptNumber}");
            string content = await response.Content.ReadAsStringAsync();

            // trim response string a little bit
            string[] splittedResult = content.Split(new string[] { "<label for=\"receipt_number\">Enter Another Receipt Number</label>" }, StringSplitOptions.None)[0].Split(new string[] { "<div class=\"rows text-center\">" }, StringSplitOptions.None);
            string result = splittedResult.Length > 1 ? splittedResult[1] : splittedResult[0];
            result = Regex.Replace(result, @"\n", "");
            result = Regex.Replace(result, @"\s{2}", "");

            Match regexResult = Regex.Match(result, @"<li>(.*?)<\/li>");
            string status = "";
            string detail = "";
            string type = "";
            if (regexResult.Success)
            {
                // there is an error message
                status = regexResult.Groups[1].Value;
            }
            else
            {
                // no error, get real message
                regexResult = Regex.Match(result, @"<h1>(.*?)<\/h1>");
                if (regexResult.Success)
                {
                    status = regexResult.Groups[1].Value;
                }
                regexResult = Regex.Match(result, @"<p>(.*?)<\/p>");
                if (regexResult.Success)
                {
                    detail = regexResult.Groups[1].Value;
                }
                regexResult = Regex.Match(result, @"Form\s(.*?),");
                if (regexResult.Success)
                {
                    type = regexResult.Groups[1].Value;
                }
            }
            lock (this.caseStatuses)
            {
                this.caseStatuses.Add(new CaseStatus { ReceiptNumber = receiptNumber, CaseType = type, Status = status, Detail = detail });
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            JobParams jobParams = (JobParams)e.Argument;
            int count = jobParams.NextCases + 1;
            Task[] tasks = new Task[count];
            string prefix = jobParams.ReceiptNumber.Substring(0, 3);
            int n = Int32.Parse(jobParams.ReceiptNumber.Substring(3));
            for (int i = 0; i < count; ++i)
            {
                tasks[i] = GetCaseStatusAsync($"{prefix}{n + i}");
            }
            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                logger.Error("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                    logger.Error($"   {ex.Message}");
            }
            this.caseStatuses.Sort(new CaseComparer());
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // do nothing
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            casesDataGrid.ItemsSource = this.caseStatuses;
            Status.Content = "Completed";
            LastUpdatedLabel.Visibility = Visibility.Visible;
            LastUpdated.Content = DateTime.Now;
        }

        private void CasesDataGrid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ReceiptNumber")
                e.Column.Header = "Receipt #";
            if (e.PropertyName == "CaseType")
                e.Column.Header = "Case Type";
        }

        public static readonly int RANGE = Int32.Parse(ConfigurationManager.AppSettings["MaxNextCases"].Trim());

        private List<CaseStatus> caseStatuses = new List<CaseStatus>();
        private static readonly HttpClient client = new HttpClient();
        private readonly NLog.Logger logger;
    }
}
