using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SLADashboard.Core;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace SLADashboard.Infrastructure
{
    public static class ExportScannedData
    {
        public static void ExportReport(string path, TableReport tableReport)
        {
            ExcelPackage excel = new ExcelPackage();
            //generate summary sheet
            //data for summary sheet need to come from loop
            //below were setting only header rows
            var workSheetSummary = excel.Workbook.Worksheets.Add("Summary");
            workSheetSummary.DefaultRowHeight = 12;
            workSheetSummary.Row(1).Height = 20;
            workSheetSummary.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheetSummary.Row(1).Style.Font.Bold = true;
            workSheetSummary.Row(2).Style.Font.Bold = true;
            workSheetSummary.Cells["A1:C1"].Merge = true;
            workSheetSummary.Cells["A2:C2"].Merge = true;
            workSheetSummary.Cells[3, 1].Value = "Work Order";
            workSheetSummary.Cells[3, 2].Value = "Details";
            workSheetSummary.Cells[3, 3].Value = "SLA Result";

            int summaryRowindex = 4;
            //generate profile sheets and data for each sheet
            foreach (var profileGroup in tableReport.ReportInfo.GroupBy(_ => _.ProfileID))
            {
                //for each profile add row in summary sheet
                workSheetSummary.Cells[1, 1].Value = profileGroup.First().ClientName +" SLA Dashboard";
                workSheetSummary.Cells[2, 1].Value = tableReport.Period;
                //add records for each  profile
                workSheetSummary.Cells[summaryRowindex, 1].Value = profileGroup.First().Profile;
                workSheetSummary.Cells[summaryRowindex, 2].Value = profileGroup.First().ProfileDescription;
                workSheetSummary.Cells[summaryRowindex, 3].Value = profileGroup.All(p => p.SLAIndicator=="PASS")?"PASS":"FAIL";
                workSheetSummary.Cells[summaryRowindex, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheetSummary.Cells[summaryRowindex, 3].Style.Fill.BackgroundColor.SetColor(profileGroup.All(p => p.SLAIndicator == "PASS") ? System.Drawing.Color.Green : System.Drawing.Color.Red);
                //create sheet for each profile
                var workSheet = excel.Workbook.Worksheets.Add(profileGroup.First().Profile);
                workSheet.TabColor = profileGroup.All(p => p.SLAIndicator == "PASS") ? System.Drawing.Color.Green:System.Drawing.Color.Red;
                workSheet.DefaultRowHeight = 12;
                //Header for table in sheet  
                //  
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells["A1:E1"].Merge = true;
                workSheet.Cells[1, 1].Value = profileGroup.First().ProfileDescription;
                workSheet.Cells[2, 1].Value = "SL.No";
                workSheet.Cells[2, 2].Value = "SLA Item";
                workSheet.Cells[2, 3].Value = "Description";
                workSheet.Cells[2, 4].Value = "Target SLA(%)";
                workSheet.Cells[2, 5].Value = "Actual SLA(%)";
                int recordIndex = 1;
                int rowIndex = 3;
                foreach (var reportData in profileGroup)
                {
                    //Body of table  
                    workSheet.Cells[rowIndex, 1].Value =reportData.ProfilePrefix+"_"+ recordIndex .ToString();
                    workSheet.Cells[rowIndex, 2].Value = reportData.SlaName;
                    workSheet.Cells[rowIndex, 3].Value = reportData.SlaDescription;
                    workSheet.Cells[rowIndex, 4].Value = reportData.Target;
                    workSheet.Cells[rowIndex, 5].Value = reportData.TargetAchieved;
                    workSheet.Cells[rowIndex, 5].Style.Fill.PatternType =  ExcelFillStyle.Solid;
                    workSheet.Cells[rowIndex, 5].Style.Fill.BackgroundColor.SetColor(reportData.SLAIndicator.ToUpper()!="PASS" ? System.Drawing.Color.Red : System.Drawing.Color.White);
                   
                    recordIndex++;
                    rowIndex++;

                }
                //set border for cells
                using (ExcelRange range = workSheet.Cells[1, 1, rowIndex-1, 5])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                summaryRowindex++;
            }
            using (ExcelRange range = workSheetSummary.Cells[1, 1, summaryRowindex-1, 3])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }
            workSheetSummary.Column(1).AutoFit();
            workSheetSummary.Column(2).AutoFit();
            workSheetSummary.Column(3).AutoFit();
            using (var memoryStream = new MemoryStream())
            {
                excel.SaveAs(memoryStream);
                using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(file);
                }

            }
        }

    }
}
