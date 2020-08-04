using HPB.API.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPB.API.TemplateReport
{
    public static class TemplateGenerator
    {
        private static string ToDateStringSafe(this DateTime? t)
        {
            return t.HasValue ? t.Value.ToString("yyyy/MM/dd") : String.Empty;
        }
        private static string ToNumberStringSafe(this decimal? t,string cul= "vi-VN")
        {
            NumberFormatInfo nfi = new CultureInfo(cul, false).NumberFormat;
            nfi.CurrencyDecimalDigits = 0;
            return t.HasValue ? t.Value.ToString("C", nfi) : String.Empty;
        }

        #region Employee Report
        public static string GetEmployeeVehicleHTMLString(EmployeeVehicleReportDto dto, string yearMonth)
        {
            NumberFormatInfo nfi = new CultureInfo("vi-VN", false).NumberFormat;
            nfi.CurrencyDecimalDigits = 0;
            var ParkingFee1 = dto.VehicleDetail.Select(x => x.ParkingFee1).FirstOrDefault().ToString("C", nfi);
            var ParkingFee2 = dto.VehicleDetail.Select(x => x.ParkingFee2).FirstOrDefault().ToString("C", nfi);
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 35px;
                            }
                            table {
                                width: 95%;
                                border-collapse: collapse;
                            }
                            td {
                                border: 1px solid gray;
                                padding: 5px;
                                font-size: 15px;
                            }
  th {
                                border: 1px solid gray;
                                padding: 5px;
                                font-size: 22px;
                            }
                            .center{ text-align: center;}
                            table th {
                                background-color: #008577;
                                color: white;
                            }
.Bold { font-weight: bold; }
                            </style>
                            </head>
                            <body>");
            sb.AppendFormat(@" <div class='header'><h1>DANH SÁCH ĐĂNG KÝ THẺ XE CÔNG TY HPB THÁNG {0}/{1}</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th width='5%'>Stt</th>
                                        <th width='30%'>Tên NV</th>
                                        <th width='15%'>Số Xe</th>
                                        <th width='15%'>Xe số ({2})</th>
                                        <th width='15%'>Xe ga ({3})</th>
                                        <th width='20%'>Mô tả xe</th>
                                    </tr> ",
                                  yearMonth.Substring(4, 2),
                                  yearMonth.Substring(0, 4),
                                  ParkingFee1,
                                  ParkingFee2);

            foreach (var emp in dto.VehicleDetail)
            {
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='center'>{2}</td>
                                    <td class='center'>{3}</td>
                                    <td class='center'>{4}</td>
                                    <td>{5}</td>
                                  </tr>", emp.No, emp.EmpName, emp.LicensePlate, emp.VehicleType1, emp.VehicleType2, emp.VehicleComment);
            }
            sb.AppendFormat(@"<tr>
                                    <td colspan='3' rowspan='2' class='Bold center'>TỔNG CỘNG</td>
                                    <td class='center'>{0}</td>
                                    <td class='center'>{1}</td>
                                    <td rowspan='2'  class='Bold center'>{2}</td>
                                  </tr>", dto.VehicleTotal.CountVehicleType1, dto.VehicleTotal.CountVehicleType2,
                                  dto.VehicleTotal.totalFee.ToString("C", nfi));
            sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td class='center'>{1}</td>
                                  </tr>", dto.VehicleTotal.TotalParkingFee1.ToString("C", nfi),
                                  dto.VehicleTotal.TotalParkingFee2.ToString("C", nfi));

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        public static string GetEmployeeRegularHTMLString(EmployeeReportDto dto, string yearMonth)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 10px;
                            }
                            table {
                                width: 95%;
                                border-collapse: collapse;
                            }
                            td, th {
                                border: 1px solid gray;
                                padding: 5px;
                                font-size: 12px;
                            }
                            .center{
                                text-align: center;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                                text-align: center;
                            }
                            </style>
                            </head>
                            <body>");
            sb.Append(@" <div class='header'><h2>DANH SÁCH THÔNG TIN NHÂN VIÊN</h2></div>
                                <table align='center'>
                                    <tr>
                                        <th>No.</th>
                                        <th>Tên NV</th>
                                        <th width='10%'>Ngày sinh</th>
                                        <th width='10%'>Số CMND</th>
                                        <th width='10%'>Ngày cấp</th>
                                        <th>Nơi cấp</th>
                                        <th width='10%'>Số Passport</th>
                                        <th width='10%'>Ngày cấp Passport</th>
                                        <th>Ngày hết hạn Passport</th>
                                        <th>HKTT</th>
                                        <th>Địa Chỉ hiện tại</th>
                                        <th width='10%'>Số ĐTDD</th>
                                        <th width='15%'>Email cty</th>
                                        <th width='15%'>Email cá nhân</th>
                                        <th width='10%'>Ngày vào Cty</th>
                                        <th>Ghi chú</th>
                                    </tr> ");

            foreach (var emp in dto.EmployeeRegular)
            {
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='center'>{2}</td>
                                    <td class='center'>{3}</td>
                                    <td class='center'>{4}</td>
                                    <td>{5}</td>
                                    <td class='center'>{6}</td>
                                    <td class='center'>{7}</td>
                                    <td>{8}</td>
                                    <td>{9}</td>
                                    <td>{10}</td>
                                    <td class='center'>{11}</td>
                                    <td>{12}</td>
                                    <td>{13}</td>
                                    <td class='center'>{14}</td>
                                    <td>{15}</td>
                                  </tr>", emp.No, emp.EmpName, emp.EmpBirthday.ToDateStringSafe(),
                                  emp.EmpIdentityNo, emp.EmpIdentityDate.ToDateStringSafe(), emp.EmpIdentityPlace,
                                  emp.EmpPassportNo, emp.EmpPassportDate.ToDateStringSafe(), emp.EmpPassportExpiryDate,
                                  emp.EmpAddressBirth, emp.EmpAddress, emp.EmpMobile1, emp.EmpEmail1,
                                  emp.EmpEmail2, emp.EmpStartDate.ToDateStringSafe(), emp.EmpComment);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        public static string GetEmployeeProbationHTMLString(EmployeeReportDto dto, string yearMonth)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 10px;
                            }
                            table {
                                width: 98%;
                                border-collapse: collapse;
                            }
                            td, th {
                                border: 1px solid gray;
                                padding: 15px;
                                font-size: 12px;
                                text-align: center;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                            }
                            </style>
                            </head>
                            <body>");
            sb.Append(@" <div class='header'><h2>DANH SÁCH THÔNG TIN NHÂN VIÊN THỬ VIỆC - THỰC TẬP - BÁN THỜI GIAN</h2></div>
                                <table align='center'>
                                    <tr>
                                        <th>No.</th>
                                        <th>Tên NV</th>
                                        <th>Ngày sinh</th>
                                        <th>Số CMND</th>
                                        <th>Ngày cấp</th>
                                        <th>Nơi cấp</th>
                                        <th>HKTT</th>
                                        <th>Địa Chỉ hiện tại</th>
                                        <th>Số ĐTDD</th>
                                        <th>Email cty</th>
                                        <th>Email cá nhân</th>
                                        <th>Ngày vào Cty</th>
                                        <th>Ghi chú</th>
                                    </tr> ");

            foreach (var emp in dto.EmployeeProbation)
            {
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='center'>{2}</td>
                                    <td class='center'>{3}</td>
                                    <td class='center'>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                    <td>{7}</td>
                                    <td class='center'>{8}</td>
                                    <td>{9}</td>
                                    <td>{10}</td>
                                    <td class='center'>{11}</td>
                                    <td>{12}</td>
                                  </tr>", emp.No, emp.EmpName, emp.EmpBirthday.ToDateStringSafe(),
                                  emp.EmpIdentityNo, emp.EmpIdentityDate.ToDateStringSafe(), emp.EmpIdentityPlace,
                                   emp.EmpAddressBirth, emp.EmpAddress, emp.EmpMobile1, emp.EmpEmail1,
                                  emp.EmpEmail2, emp.EmpStartDate.ToDateStringSafe(), emp.EmpComment);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        public static string GetEmployeeLeaveHTMLString(EmployeeReportDto dto, string yearMonth)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 10px;
                            }
                            table {
                                width: 98%;
                                border-collapse: collapse;
                            }
                            td, th {
                                border: 1px solid gray;
                                padding: 5px;
                                font-size: 12px;
                            }
                            .center{
                                text-align: center;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                                text-align: center;
                            }
                            </style>
                            </head>
                            <body>");
            sb.Append(@" <div class='header'><h2>DANH SÁCH THÔNG TIN NHÂN VIÊN NGHỈ VIỆC</h2></div>
                                <table align='center'>
                                    <tr>
                                        <th>No.</th>
                                        <th>Tên NV</th>
                                        <th>Ngày sinh</th>
                                        <th>Số CMND</th>
                                        <th>Ngày cấp</th>
                                        <th>Nơi cấp</th>
                                        <th>Số Passport</th>
                                        <th>Ngày cấp Passport</th>
                                        <th>Nơi cấp Passport</th>
                                        <th>HKTT</th>
                                        <th>Địa Chỉ hiện tại</th>
                                        <th>Số ĐTDD</th>
                                        <th>Email cty</th>
                                        <th>Email cá nhân</th>
                                        <th>Ngày vào Cty</th>
                                        <th>Ngày nghỉ việc</th>
                                        <th>Ghi chú</th>
                                    </tr> ");

            foreach (var emp in dto.EmployeeLeave)
            {
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='center'>{2}</td>
                                    <td class='center'>{3}</td>
                                    <td class='center'>{4}</td>
                                    <td>{5}</td>
                                    <td class='center'>{6}</td>
                                    <td class='center'>{7}</td>
                                    <td>{8}</td>
                                    <td>{9}</td>
                                    <td>{10}</td>
                                    <td class='center'>{11}</td>
                                    <td>{12}</td>
                                    <td>{13}</td>
                                    <td class='center'>{14}</td>
                                    <td class='center'>{15}</td>
                                    <td>{16}</td>
                                  </tr>", emp.No, emp.EmpName, emp.EmpBirthday.ToDateStringSafe(),
                                  emp.EmpIdentityNo, emp.EmpIdentityDate.ToDateStringSafe(), emp.EmpIdentityPlace,
                                  emp.EmpPassportNo, emp.EmpPassportDate.ToDateStringSafe(), emp.EmpPassportExpiryDate,
                                  emp.EmpAddressBirth, emp.EmpAddress, emp.EmpMobile1, emp.EmpEmail1,
                                  emp.EmpEmail2, emp.EmpStartDate.ToDateStringSafe(), emp.EmpEndDate.ToDateStringSafe(), emp.EmpComment);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        #endregion
        #region Salary Report
        public static string GetMonthlySalaryHTMLString(MonthlySalaryReportDto dto, string yearMonth)
        {

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 10px;
                            }
                            table {
                                width: 98%;
                                border-collapse: collapse;
                            }
                            td, th {
                                border: 1px solid gray;
                                padding: 2px;
                                font-size: 12px;
                            }
                            .right{
                               text-align: right;
                            }
                            .center{
                                text-align: center;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                            }
                            </style>
                            </head>
                            <body>");
            sb.AppendFormat(@" <div class='header'><h1>BẢNG LƯƠNG THÁNG {0}/{1}</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Stt</th>
                                        <th>Ngày phép năm</th>
                                        <th>Họ và Tên</th>
                                        <th>Chức vụ</th>
                                        <th rowspan='2'>Tổng lương thực nhận</th>
                                        <th colspan='3'>Trả Tiền Mặt (chuyển từ TK cá nhân)</th>
                                        <th rowspan='2'>Ghi chú</th>
                                    </tr> ",
                                  yearMonth.Substring(4, 2),
                                  yearMonth.Substring(0, 4)
                                  );
            sb.Append(@" <tr>
                                        <th>[1]</th>
                                        <th>[2]</th>
                                        <th>[3]</th>
                                        <th>[4]</th>
                                        <th>Bonus/OT</th>
                                        <th>Phụ cấp</th>
                                        <th>Trừ/Phạt</th>
                                    </tr> ");

            foreach (var emp in dto.detail)
            {
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td class='center'>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td class='right'>{4}</td>
                                    <td class='right'>{5}</td>
                                    <td class='right'>{6}</td>
                                    <td class='right'>{7}</td>
                                    <td>{8}</td>
                                  </tr>", emp.No,
                                  emp.TotalDayOff,
                                  emp.EmpName,
                                  emp.PositionName,
                                  emp.Salary.ToNumberStringSafe(),
                                  emp.BonusOt.ToNumberStringSafe(),
                                  emp.Allowance.ToNumberStringSafe(),
                                  emp.Deduction.ToNumberStringSafe(),
                                  emp.Comment
                                  );
            }
            sb.AppendFormat(@"<tr>
                                    <td colspan='4'  class='center' >TỔNG CỘNG</td>
                                    <td class='right'>{0}</td>
                                    <td class='right'>{1}</td>
                                    <td class='right'>{2}</td>
                                    <td class='right'>{3}</td>
                                    <td></td>
                                  </tr>", dto.total.TotalSalary.ToNumberStringSafe(),
                                  dto.total.TotalBonusOt.ToNumberStringSafe(),
                                   dto.total.TotalAllowance.ToNumberStringSafe(),
                                    dto.total.TotalDeduction.ToNumberStringSafe());

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        #endregion
        #region AnnualLeavePai Report
        private static string ToDayOfWeek(this DateTime? t)
        {
            var tempValue = t.Value.DayOfWeek;
            var result = "";
            switch (tempValue)
            {
                case DayOfWeek.Monday:
                    result = "T2";
                    break;
                case DayOfWeek.Tuesday:
                    result = "T3";
                    break;
                case DayOfWeek.Wednesday:
                    result = "T4";
                    break;
                case DayOfWeek.Thursday:
                    result = "T5";
                    break;
                case DayOfWeek.Friday:
                    result = "T6";
                    break;
                case DayOfWeek.Saturday:
                    result = "T7";
                    break;
                case DayOfWeek.Sunday:
                    result = "CN";
                    break;
            }
            return result;
        }
        public static string GetAnnualLeavePaidByMonthHTMLString(AnnualLeavePaidReportDto dto, string yearMonth)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
td[rowspan] {
            position: relative;
        }

        td[rowspan]:before {
            position: absolute;
            content: "";
            top: -1px;
            left: -1px;
            background-color: transparent;
            border: solid #666 1px;
            width: 100%;
            height: 100%;
        }
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 10px;
                            }
                            table {
                                width: 100%;
                                border-collapse: collapse;
                            }
                            td {
                                border: 1px solid gray;
                                padding: 2px;
                                font-size: 12px;
                            }
                            .right{
                               text-align: right;
                            }
                            .center{
                                text-align: center;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                                border: 1px solid gray;
                                padding: 2px;
                                font-size: 12px;
                            }
                            .total{
background-color: #008577;
font-weight: bold;
color: white;
}
.bold{
font-weight: bold;
}
                            .holidayWeekend{
                                background-color: #919191;
                            }
                            .leave{
                                background-color: #4f4f4f;
                            }
                            .out{
                                background-color: #4f4f4f;
                            }
                            </style>
                            </head>
                            <body>");
            sb.AppendFormat(@" <div class='header'><h1>BẢNG CHẤM CÔNG THÁNG {0}/{1}</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th  width='5%' rowspan='3' >Stt</th>
                                        <th width='20%' rowspan='3' >Họ và Tên</th>
                                        <th width='10%' rowspan='3'>Ngày phép năm trước</th>
                                        <th colspan='{2}'>Ngày trong tháng</th>
                                        <th  width='10%' rowspan='3' >Tổng ngày đã nghỉ</th>
                                        <th  width='10%' rowspan='3' >Ngày phép còn lại</th>
                                    </tr> ",
                                  yearMonth.Substring(4, 2),
                                  yearMonth.Substring(0, 4),
                                  DateTime.DaysInMonth(Convert.ToInt32(yearMonth.Substring(0, 4)), Convert.ToInt32(yearMonth.Substring(4, 2)))
                                  );
            sb.Append(@" <tr>");

            foreach (var DayOff in dto.AnnualLeavePaidDayOffDto.GroupBy(x => x.DayOff))
            {
                sb.AppendFormat(@" <th>{0}</th> ",
                                    DayOff.FirstOrDefault().DayOff.Value.Day);
            }
            sb.Append(@" </tr>");
            sb.Append(@" <tr>");
            foreach (var DayOff1 in dto.AnnualLeavePaidDayOffDto.GroupBy(x => x.DayOff))
            {
                sb.AppendFormat(@" <th>{0}</th> ",
                                    DayOff1.FirstOrDefault().DayOff.ToDayOfWeek());
            }
            sb.Append(@" </tr>");
            foreach (var emp in dto.AnnualLeavePaidResultDto)
            {
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='center'>{2}</td>
                                ", emp.No,
                                emp.EmpName,
                                emp.DayRemainLast);
                var stringTemp = "";
                foreach (var item in dto.AnnualLeavePaidDayOffDto.GroupBy(x => x.DayOff))
                {
                    var DayOff1 = item.FirstOrDefault();

                    var dayOff = item.FirstOrDefault(x => x.EmpId == emp.EmpId) != null ? item.FirstOrDefault(x => x.EmpId == emp.EmpId).CountDayOff : 0;
                    //Weekend
                    if (DayOff1.DayOff.Value.DayOfWeek == DayOfWeek.Saturday || DayOff1.DayOff.Value.DayOfWeek == DayOfWeek.Sunday)
                    {
                        stringTemp += @" <td class='center holidayWeekend'>" + dayOff + "</td>";
                    }
                    else if (DayOff1.Holiday.HasValue)//Holiday
                    {
                        stringTemp += @" <td class='center holidayWeekend'>" + dayOff + "</td>";
                    }
                    else
                    {
                        if (emp.EmpStartDate > DayOff1.DayOff.Value || emp.EmpEndDate < DayOff1.DayOff.Value) //leave
                        {
                            stringTemp += @" <td class='center leave'>" + dayOff + "</td>";
                        }
                        else
                        {
                            stringTemp += @" <td class='center'>" + dayOff + "</td>";
                        }

                    }
                }
                sb.Append(stringTemp);

                sb.AppendFormat(@"  <td class='center'>{0}</td>
                                    <td class='center'>{1}</td>
                                  </tr>", emp.CountDayOff,
                                  emp.TotalDay
                                  );
            }
            sb.Append(@"<tr>
                                    <td colspan='3'  class='center total' >TỔNG CỘNG</td>");
            var stringTemp1 = "";
            decimal totalDay = 0;
            foreach (var DayOff1 in dto.AnnualLeavePaidTotalResultDto)
            {
                stringTemp1 += @" <td class='center total'>" + DayOff1.TotalDayOff + "</td>";
                totalDay += DayOff1.TotalDayOff;
            }
            sb.Append(stringTemp1);
            sb.AppendFormat(@" <td class='center total'>{0}</td><td  class='center total'></td>  </tr>", totalDay);

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
        private static string GetLeaveHTML(int month, AnnualLeavePaidListDto dto, decimal valueShow,int year)
        {
            var stringTemp = @" <td width='3%' class='center ";
            if ((dto.EmpStartDate.HasValue && dto.EmpStartDate.Value.Month >= month && dto.EmpStartDate.Value.Year == year) || (dto.EmpEndDate.HasValue && dto.EmpEndDate.Value.Month <= month && dto.EmpEndDate.Value.Year == year))
            {
                if (dto.EmpStartDate.HasValue && dto.EmpStartDate.Value.Month == month && dto.EmpStartDate.Value.Year == year || dto.EmpEndDate.HasValue && dto.EmpEndDate.Value.Month == month && dto.EmpEndDate.Value.Year == year)
                {
                    stringTemp += @" leave ";
                }
                else
                {
                    stringTemp += @" out ";
                }
            }
            stringTemp += "' >" + valueShow + @"</td> ";
            return stringTemp;
        }
        public static string GetAnnualLeavePaidByYearHTMLString(IEnumerable<AnnualLeavePaidListDto> lstdto, int year)
        {
            lstdto = lstdto.OrderBy(x => x.No);
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 5px;
                            }
                            table {
                                width: 100%;
                                border-collapse: collapse;
                            }
                            td{
                                border: 1px solid gray;
                                padding: 2px;
                                font-size: 12px;
                            }
                            .right{
                               text-align: right;
                            }
                            .center{
                                text-align: center;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                                 border: 1px solid gray;
                                  padding: 2px;
                                font-size: 12px;
                            }
                            .holidayWeekend{
                                background-color: #919191;
                            }
                            .leave{
                                background-color: #ada8a8;
                            }
                            .out{
                                background-color: #4f4f4f;
                            }

                            </style>
                            </head>
                            <body>");
            sb.AppendFormat(@" <div class='header'><h1>BẢNG CHẤM CÔNG NĂM {0}</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th width='5%'>Stt</th>
                                        <th width='25%'>Họ và Tên</th>
                                        <th width='10%'>{1}</th>
                                        <th width='10%'>{2}</th>", year.ToString(), (year - 1).ToString(), year.ToString());
            for (int i = 0; i < 12; i++)
            {
                sb.AppendFormat(@"<th>{0}</th>", i + 1);
            }
            sb.Append(@" <th width='15%' >Ngày phép còn lại </th>  </tr> ");
          
            int index = 0;
            foreach (var emp in lstdto)
            {
                index++;
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='center'>{2}</td>
                                    <td  class='center'>{3}</td>"
                , emp.No,
                                emp.EmpName,
                                emp.DayRemainLastYear,
                                emp.DayCurrentYear);

                if ((emp.EmpStartDate.HasValue && emp.EmpStartDate.Value.Year == year) || (emp.EmpEndDate.HasValue && emp.EmpEndDate.Value.Year == year))
                {
                    var stringTemp = "";
                    stringTemp += GetLeaveHTML(1, emp, emp.Jan, year);
                    stringTemp += GetLeaveHTML(2, emp, emp.Feb, year);
                    stringTemp += GetLeaveHTML(3, emp, emp.Mar, year);
                    stringTemp += GetLeaveHTML(4, emp, emp.Apr, year);
                    stringTemp += GetLeaveHTML(5, emp, emp.May, year);
                    stringTemp += GetLeaveHTML(6, emp, emp.Jun, year);
                    stringTemp += GetLeaveHTML(7, emp, emp.Jul, year);
                    stringTemp += GetLeaveHTML(8, emp, emp.Aug, year);
                    stringTemp += GetLeaveHTML(9, emp, emp.Sep, year);
                    stringTemp += GetLeaveHTML(10, emp, emp.Oct, year);
                    stringTemp += GetLeaveHTML(11, emp, emp.Nov, year);
                    stringTemp += GetLeaveHTML(12, emp, emp.Dec, year);
                    sb.Append(stringTemp);
                }
                else
                {
                    sb.AppendFormat(@" <td class='center'>{0}</td>
                                    <td class='center'>{1}</td>
                                    <td class='center'>{2}</td>
                                    <td class='center'>{3}</td>
                                    <td class='center'>{4}</td>
                                    <td class='center'>{5}</td>
                                    <td class='center'>{6}</td>
                                    <td class='center'>{7}</td>
                                    <td class='center'>{8}</td>
                                    <td class='center'>{9}</td>
                                    <td class='center'>{10}</td>
                                    <td class='center'>{11}</td>",
                                   emp.Jan,
                                 emp.Feb,
                                  emp.Mar,
                                   emp.Apr,
                                    emp.May,
                                     emp.Jun,
                                      emp.Jul,
                                       emp.Aug,
                                        emp.Sep,
                                         emp.Oct,
                                          emp.Nov,
                                       emp.Dec);
                }

                sb.AppendFormat(@"  <td class='center'>{0}</td> </tr> 
                                ", emp.TotalDay
                                );
                //if (index == 30)
                //{
                //    sb.Append(@"      </table>");
                //    sb.AppendFormat(@" <div class='header'><h1>BẢNG CHẤM CÔNG NĂM {0}</h1></div>
                //                <table align='center'>
                //                    <tr>
                //                        <th width='5%'>Stt</th>
                //                        <th width='25%'>Họ và Tên</th>
                //                        <th width='10%'>{1}</th>
                //                        <th width='10%'>{2}</th>", year.ToString(), (year - 1).ToString(), year.ToString());
                //    for (int i = 0; i < 12; i++)
                //    {
                //        sb.AppendFormat(@"<th>{0}</th>", i + 1);
                //    }
                //    sb.Append(@" <th>Ngày phép còn lại </th>  </tr> ");
                //    index = 0;
                //}
               

            }
            sb.Append(@"
                               </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
        #endregion
        #region AnnualBonus Report
        public static string GetAnnualBonusForLeaderHTMLString(AnnualBonusReportDto dto, int year)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 10px;
                            }
                            table {
                                width: 98%;
                                border-collapse: collapse;
                            }
                            td, th {
                                border: 1px solid gray;
                                padding: 2px;
                                font-size: 12px;
                            }
                            .right{
                               text-align: right;
                            }
                            .center{
                                text-align: center;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                            }
                            .holidayWeekend{
                                background-color: #919191;
                            }
                            .leave{
                                background-color: #4f4f4f;
                            }
                            </style>
                            </head>
                            <body>");
            sb.AppendFormat(@" <div class='header'><h3>PHỤ CẤP DỰ ÁN CỦA {0} NĂM {1}</h3></div>
                                <table align='center' width='60%' >
                                    <tr> 
                                        <th>Tỷ giá YEN </th>
                                        <th>Tỷ giá USD </th>
                                        <th>Hệ số PCDA Leader</th>
                                    </tr>
                                    <tr> 
                                        <td class='center'>{2} </td>
                                        <td class='center'>{3}</td>
                                        <td class='center'>{4} </td>
                                    </tr>
                                </table>
                                <table align='center'>
                                    <tr>
                                        <th>Stt</th>
                                        <th>Tên dự án</th>
                                        <th>Số tiền báo giá</th>
                                        <th>Phụ cấp</th>
                                    </tr> ",
                                  dto.AnnualBonusLEADERTotalDto.EmpName.ToUpper(),
                                  year,
                                  dto.AnnualBonusLEADERTotalDto.RateYen,
                                  dto.AnnualBonusLEADERTotalDto.RateUSD,
                                  dto.AnnualBonusLEADERTotalDto.RatingFactorLeader
                                  );
            sb.Append(@" <tr>");
            foreach (var emp in dto.AnnualBonusLEADERDetailDto)
            {
                var estimateValue = "";
                switch (emp.EstimateCostCurrencyId)
                {
                    
                    case 1:
                        estimateValue = emp.Estimate.ToNumberStringSafe("ja-JP");
                        break;
                    case 2:
                        estimateValue = emp.Estimate.ToNumberStringSafe();
                        break;
                    case 3:
                        estimateValue = emp.Estimate.ToNumberStringSafe("en-US");
                        break;
                }
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='right'>{2}</td>
                                    <td class='right'>{3}</td>
                                </tr>
                                ", emp.No,
                                emp.ProjectName,
                                 estimateValue,
                                 emp.Bonus.ToNumberStringSafe()
                                ); 
            }
            sb.Append(@"<tr>  <td colspan='3'  class='center' >TỔNG CỘNG</td>");
            sb.AppendFormat(@" <td class='right'>{0}</td> </tr>", dto.AnnualBonusLEADERTotalDto.TotalBonus.ToNumberStringSafe());
            sb.Append(@"
                                </table>
                          ");
            if (!dto.AnnualBonusMEMBERDtoDetailDto.Any())
            {
                sb.Append(@"
                          
                            </body>
                        </html>");
            }
            return sb.ToString();
        }
        public static string GetAnnualBonusForMemberHTMLString(AnnualBonusReportDto dto, int year)
        {
            var sb = new StringBuilder();
            if (!dto.AnnualBonusLEADERDetailDto.Any())
            {
                sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 10px;
                            }
                            table {
                                width: 98%;
                                border-collapse: collapse;
                            }
                            td, th {
                                border: 1px solid gray;
                                padding: 2px;
                                font-size: 12px;
                            }
                            .right{
                               text-align: right;
                            }
                            .center{
                                text-align: center;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                            }
                            .holidayWeekend{
                                background-color: #919191;
                            }
                            .leave{
                                background-color: #4f4f4f;
                            }
                            </style>
                            </head>
                            <body>");
            
            sb.AppendFormat(@" <div class='header'><h3>PHỤ CẤP DỰ ÁN CỦA {0} NĂM {1}</h3></div>", dto.AnnualBonusMEMBERDtoTotalDto.EmpName.ToUpper(),
                                  year);
            }
            sb.AppendFormat(@"
                                <table align='center' width='60%'>
                                    <tr> 
                                        <th>Số ngày LV/Tháng </th>
                                        <th>Hệ số PCDA PG/PT </th>
                                        <th>Lương</th>
                                    </tr>
                                    <tr> 
                                        <td class='center'>{0} </td>
                                        <td class='center'>{1}</td>
                                        <td class='center'>{2} </td>
                                    </tr>
                                </table>
                                <table align='center'>
                                    <tr>
                                        <th>Stt</th>
                                        <th>Tên dự án</th>
                                        <th>Ngày tham giá dự án</th>
                                        <th>Phụ cấp</th>
                                    </tr> ",
                                  dto.AnnualBonusMEMBERDtoTotalDto.DayWorkInMonth,
                                  dto.AnnualBonusMEMBERDtoTotalDto.RatingFactor,
                                  dto.AnnualBonusMEMBERDtoTotalDto.Salary
                                  );
            sb.Append(@" <tr>");
            foreach (var emp in dto.AnnualBonusMEMBERDtoDetailDto)
            {
                sb.AppendFormat(@"<tr>
                                    <td class='center'>{0}</td>
                                    <td>{1}</td>
                                    <td class='right'>{2}</td>
                                    <td class='right'>{3}</td>
                                </tr>
                                ", emp.No,
                                emp.ProjectName,
                                emp.Day,
                                 emp.Bonus.ToNumberStringSafe()
                                );
            }
            sb.Append(@"<tr>  <td colspan='3'  class='center' >TỔNG CỘNG</td>");
            sb.AppendFormat(@" <td class='right'>{0}</td> </tr>", dto.AnnualBonusMEMBERDtoTotalDto.TotalBonus.ToNumberStringSafe());
            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }

        //public static string GetAnnualBonusForSumaryHTMLString(AnnualLeavePaidReportDto dto, string yearMonth)
        //{
        //    var sb = new StringBuilder();
        //    sb.Append(@"
        //                <html>
        //                    <head>
        //                    <style>
        //                    .header {
        //                        text-align: center;
        //                        color: #008577;
        //                        padding-bottom: 10px;
        //                    }
        //                    table {
        //                        width: 98%;
        //                        border-collapse: collapse;
        //                    }
        //                    td, th {
        //                        border: 1px solid gray;
        //                        padding: 2px;
        //                        font-size: 12px;
        //                    }
        //                    .right{
        //                       text-align: right;
        //                    }
        //                    .center{
        //                        text-align: center;
        //                    }
        //                    table th {
        //                        background-color: #008577;
        //                        color: white;
        //                    }
        //                    .holidayWeekend{
        //                        background-color: #919191;
        //                    }
        //                    .leave{
        //                        background-color: #4f4f4f;
        //                    }
        //                    </style>
        //                    </head>
        //                    <body>");
        //    sb.AppendFormat(@" <div class='header'><h1>BẢNG CHẤM CÔNG THÁNG {0}/{1}</h1></div>
        //                        <table align='center'>
        //                            <tr>
        //                                <th rowspan='3'>Stt</th>
        //                                <th rowspan='3'>Họ và Tên</th>
        //                                <th rowspan='3'>Ngày phép năm trước</th>
        //                                <th colspan='{2}'>Ngày trong tháng</th>
        //                                <th rowspan='3'>Tổng ngày đã nghỉ</th>
        //                                <th rowspan='3'>Ngày phép còn lại</th>
        //                            </tr> ",
        //                          yearMonth.Substring(4, 2),
        //                          yearMonth.Substring(0, 4),
        //                          DateTime.DaysInMonth(Convert.ToInt32(yearMonth.Substring(0, 4)), Convert.ToInt32(yearMonth.Substring(4, 2)))
        //                          );
        //    sb.Append(@" <tr>");

        //    foreach (var DayOff in dto.AnnualLeavePaidDayOffDto.GroupBy(x => x.DayOff))
        //    {
        //        sb.AppendFormat(@" <th>{0}</th> ",
        //                            DayOff.FirstOrDefault().DayOff.Value.Day);
        //    }
        //    sb.Append(@" </tr>");
        //    sb.Append(@" <tr>");
        //    foreach (var DayOff1 in dto.AnnualLeavePaidDayOffDto.GroupBy(x => x.DayOff))
        //    {
        //        sb.AppendFormat(@" <th>{0}</th> ",
        //                            DayOff1.FirstOrDefault().DayOff.ToDayOfWeek());
        //    }
        //    sb.Append(@" </tr>");
        //    foreach (var emp in dto.AnnualLeavePaidResultDto)
        //    {
        //        sb.AppendFormat(@"<tr>
        //                            <td class='center'>{0}</td>
        //                            <td>{1}</td>
        //                            <td class='center'>{2}</td>
        //                        ", emp.No,
        //                        emp.EmpName,
        //                        emp.DayRemainLast);
        //        var stringTemp = "";
        //        foreach (var item in dto.AnnualLeavePaidDayOffDto.GroupBy(x => x.DayOff))
        //        {
        //            var DayOff1 = item.FirstOrDefault();

        //            var dayOff = item.FirstOrDefault(x => x.EmpId == emp.EmpId) != null ? item.FirstOrDefault(x => x.EmpId == emp.EmpId).CountDayOff : 0;
        //            //Weekend
        //            if (DayOff1.DayOff.Value.DayOfWeek == DayOfWeek.Saturday || DayOff1.DayOff.Value.DayOfWeek == DayOfWeek.Sunday)
        //            {
        //                stringTemp += @" <td class='center holidayWeekend'>" + dayOff + "</td>";
        //            }
        //            else if (DayOff1.Holiday != "0")//Holiday
        //            {
        //                stringTemp += @" <td class='center holidayWeekend'>" + dayOff + "</td>";
        //            }
        //            else
        //            {
        //                if (emp.EmpStartDate > DayOff1.DayOff.Value || emp.EmpEndDate < DayOff1.DayOff.Value) //leave
        //                {
        //                    stringTemp += @" <td class='center leave'>" + dayOff + "</td>";
        //                }
        //                else
        //                {
        //                    stringTemp += @" <td class='center'>" + dayOff + "</td>";
        //                }

        //            }
        //        }
        //        sb.Append(stringTemp);

        //        sb.AppendFormat(@"  <td class='center'>{0}</td>
        //                            <td class='center'>{1}</td>
        //                          </tr>", emp.CountDayOff,
        //                          emp.TotalDay
        //                          );
        //    }
        //    sb.Append(@"<tr>
        //                            <td colspan='3'  class='center' >TỔNG CỘNG</td>");
        //    var stringTemp1 = "";
        //    decimal totalDay = 0;
        //    foreach (var DayOff1 in dto.AnnualLeavePaidTotalResultDto)
        //    {
        //        stringTemp1 += @" <td class='center'>" + DayOff1.TotalDayOff + "</td>";
        //        totalDay += DayOff1.TotalDayOff;
        //    }
        //    sb.Append(stringTemp1);
        //    sb.AppendFormat(@" <td class='center'>{0}</td><td></td>  </tr>", totalDay);

        //    sb.Append(@"
        //                        </table>
        //                    </body>
        //                </html>");

        //    return sb.ToString();
        //}
        #endregion
        #region KPI report
        public static string GetKPIHTMLString(KPIReportDto dto, int year,string empName, string empDeptName)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            <style>
                            .header {
                                text-align: center;
                                color: #008577;
                                padding-bottom: 10px;
                            }
                            table {
table-layout: fixed;
                                width: 98%;
                                border-collapse: collapse;
                            }
                            td, th {
                                border: 1px solid gray;
                                padding: 2px;
                                font-size: 12px;

                            }
span{white-space:pre-wrap; 
word-wrap:break-word}
                            .right{
                               text-align: right;
                            }
                            .left{
                               text-align: left;
                            }
                            .center{
                                text-align: center;
                            }
                            .title {
                                background-color: #008577;
                                color: white;
                            }
                            .red{
                                 color: red;
                                font-weight: bold;
                                background-color: #008577;
                            }
                            .kpi{
                                background-color:#a9e6e1 ;
                                font-weight: bold;
                            }
                            table th {
                                background-color: #008577;
                                color: white;
                            }
                            .holidayWeekend{
                                background-color: #919191;
                            }
                            .leave{
                                background-color: #4f4f4f;
                            }
                            </style>
                            </head>
                            <body>");
            sb.AppendFormat(@" <div class='header'><h3>KẾT QUẢ ĐÁNH GIÁ CỦA {0} - {1} NĂM {2}</h3></div>
                              ",
                                  empName.ToUpper(),
                                  empDeptName.ToUpper(),
                                  year
                                  );
            sb.Append(@"   <table align='center'>
                        <tr> 
                            <th class='left'> Điểm số:  Tối đa 100 điểm </th> ");
            foreach (var item in dto.KPIEvaluationReportDto)
            {
                sb.AppendFormat(@"  <th colspan='2'> {0} - {1} </th> ", item.EvaluationName, item.PositionName);
            }
            sb.Append(@" </tr>");

            sb.Append(@" <tr> 
                            <th  width='30%' > Đề mục </th> ");
            foreach (var item in dto.KPIEvaluationReportDto)
            {
                sb.Append(@" <th  width='5%'> Điểm  số </th>");
                sb.Append(@" <th  width='30%'> Đánh giá </th>");
            }
            sb.Append(@" </tr>");

            var _length = dto.KPIResultForEmployeeReportDto.Count();
            var _step = dto.KPIEvaluationReportDto.Count();
            for (int i = 0; i < _length; i = i + _step)
            {
                var item = dto.KPIResultForEmployeeReportDto.ToArray()[i];
                
                sb.Append(@" <tr> ");
                if (item.KPIDetailId==0)
                {
                    sb.AppendFormat(@"  <td class='kpi'> {0} </td> ", item.Kpiheading);
                }
                else {
                    sb.AppendFormat(@"  <td > {0} </td> ", item.Kpiheading);
                }
                
                for (int j = 0; j < dto.KPIEvaluationReportDto.Count(); j++)
                {
                    var item1 = dto.KPIEvaluationReportDto.ToArray()[j];
                    var item2 = dto.KPIResultForEmployeeReportDto.ToArray()[i+j];
                    if (item1.EvaluationId == item2.EvaluatorId)
                    {
                        if (item.KPIDetailId == 0)
                        {
                            sb.AppendFormat(@"  <td class='center kpi'> {0} </td> ", item2.Score);
                            sb.AppendFormat(@"  <td class='kpi'><span> {0}</span> </td> ", item2.EvaluationContent);
                        }
                        else
                        {
                            sb.AppendFormat(@"  <td class='center'> {0} </td> ", item2.Score);
                            sb.AppendFormat(@"  <td> <span>{0}</span> </td> ", item2.EvaluationContent);
                        }
                        
                    }
                }
                sb.Append(@" </tr> ");
            }
           
            sb.Append(@" <tr>  <td class='title'>XẾP LOẠI:</td>");
            foreach (var item in dto.KPIClassificationForEmployeeReportDto)
            {
                    sb.AppendFormat(@"  <td class='center red'>{0}</td> ", item.Classification);
                    sb.Append(@" <td class='title'></td> ");
            }

           
            sb.Append(@" </tr> ");

            _length = dto.EvaluationResultForEmployeeReportDto.Count();
            _step = dto.KPIEvaluationReportDto.Count();
            sb.AppendFormat(@" <tr>  <td class='title center'  colspan='{0}'>Đánh giá chung</td></tr>", (_step*2)+1);
           
            for (int i = 0; i < _length; i = i + _step)
            {
                var item = dto.EvaluationResultForEmployeeReportDto.ToArray()[i];
                sb.Append(@" <tr> ");
                sb.AppendFormat(@"  <td class='kpi'> {0} </td> ", item.EvaluationHeading);
                for (int j = 0; j < dto.KPIEvaluationReportDto.Count(); j++)
                {
                    var item1 = dto.KPIEvaluationReportDto.ToArray()[j];
                    var item2 = dto.EvaluationResultForEmployeeReportDto.ToArray()[i + j];
                    if (item1.EvaluationId == item2.EvaluationId)
                    {
                            sb.AppendFormat(@"  <td colspan='{1}'><span> {0}</span> </td> ", item2.EvaluationContent, _step );
                    }
                    else
                    {
                        sb.AppendFormat(@"  <td colspan='{0}'>  </td> ", _step);
                    }
                }
                sb.Append(@" </tr> ");
            }
            sb.AppendFormat(@" <tr>  <td class='title center' colspan='{0}'> Ý kiến nhân viên </td></tr>", (_step*2)+1);
            foreach (var item in dto.ReviewForEmployeeReportDto)
            {
                sb.Append(@" <tr> ");
                sb.AppendFormat(@"  <td class='kpi'> {0} </td> ", item.ReviewHeading);
                sb.AppendFormat(@"  <td  colspan='{1}'><span> {0} </span></td> ", item.ReviewContent, _step*2);
                sb.Append(@" </tr> ");
            }

                sb.Append(@"  </table> ");
                sb.Append(@" </body>  </html>");
            return sb.ToString();
        }
        #endregion
    }
}
