using AspNetCore.Reporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Helpers
{
    public class ReportGenerator
    {
        Dictionary<string, string> parametrat = new Dictionary<string, string>();
        private List<DataTable> dataTable;
        string mimeType = "";
        public ReportGenerator()
        {

        }

        public enum ReportType
        {
            PagaTabelare = 0,
            Punetoret = 1,
            Payslip = 2
        }

        public byte[] GenerateReport(ReportType type, string reportPathAndName, List<DataTable> dataTables = null, Dictionary<string,string> reportParameters = null)
        {
            this.dataTable = dataTables;
            LocalReport reportViewer = new LocalReport(reportPathAndName);

            parametrat = reportParameters;

            switch (type)
            {
                case ReportType.PagaTabelare:
                    reportViewer.AddDataSource("dsPagatTabelare", dataTable[0]);
                    break;
                case ReportType.Punetoret:
                    reportViewer.AddDataSource("dsPunetoret", dataTable[0]);
                    break;
                case ReportType.Payslip:
                    reportViewer.AddDataSource("dsPayslip", dataTable[0]);
                    break;

                default:
                    break;
            }

            var result = reportViewer.Execute(RenderType.Pdf, 1, parametrat, mimeType);

            return result.MainStream;
        }
    }
}
