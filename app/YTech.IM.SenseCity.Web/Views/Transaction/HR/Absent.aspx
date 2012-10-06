<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<YTech.IM.SenseCity.Core.Transaction.HR.TAbsent>>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <script type="text/javascript">
        function showtime(elementId) {
            var now = new Date();
            var hours = now.getHours();
            var minutes = now.getMinutes();
            var seconds = now.getSeconds();
            var timeValue = "" + ((hours > 12) ? hours - 12 : hours);
            if (timeValue == "0") timeValue = 12;
            timeValue += ((minutes < 10) ? ":0" : ":") + minutes;
            timeValue += ((seconds < 10) ? ":0" : ":") + seconds;
            timeValue += (hours >= 12) ? " PM" : " AM";
//            document.getElementById(elementId).value = timeValue;
            $('#'+elementId).val(timeValue);
        }

        $(function () {
            $("#txtWorkDate").datepicker({ dateFormat: "dd-M-yy" });
        });

        function refreshEmployeeTable() {
            var ex = $('#tableAbsent');
            var workdate = $('#txtWorkDate').val();
            var itemCats = JSON.parse($.ajax({ url: '<%= Url.Action("GetAbsentByDate","HR") %>?workDate=' + workdate, async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the ItemCats.'); } }).responseText);
            var tbl = '<tr><th>Kode Karyawan</th><th>Nama Karyawan</th><th>Status</th><th>Jam Masuk</th><th>Jam Keluar</th><th>Keterangan</th></tr>';
            ex.html(tbl);

            for (var i = 0; i < itemCats.rows.length; i++) {
                tbl += '<tr>' +
                                '<td>' + itemCats.rows[i].i + '<input type="hidden" name="id' + i + '" value="' + itemCats.rows[i].i + '"></td>' +
                                '<td>' + itemCats.rows[i].cell[1] + '<input type="hidden" name="name' + i + '" value="' + itemCats.rows[i].cell[1] + '"></td>' +
                                '<td><select id="selectstatus' + i + '" name="selectstatus' + i + '">' +
                                '<option value="Masuk">Masuk</option><option value="Izin">Izin</option><option value="Alfa">Alfa</option>' +
                                '<option value="Sakit">Sakit</option>' +
                                '</select></td>' +
                                '<td><input id="starttime' + i + '" name="starttime' + i + '" type="text" value="' + itemCats.rows[i].cell[3] + '"/><input type="button" value="waktu" onclick="showtime(\'starttime' + i + '\')"/></td>' +
                                '<td><input id="endtime' + i + '" name="endtime' + i + '" type="text" value="' + itemCats.rows[i].cell[4] + '"/><input type="button" value="waktu" onclick="showtime(\'endtime' + i + '\')"/></td>' +
                                '<td><input id="desc' + i + '" name="desc' + i + '" type="text" value="' + itemCats.rows[i].cell[5] + '"/></td>' +
                                '</tr>';
            }
            ex.html(tbl);
            for (var i = 0; i < itemCats.rows.length; i++) {
                var selectVal = itemCats.rows[i].cell[2];
                if (selectVal == '')
                    selectVal = 'Masuk';
                $('#selectstatus' + i).val(selectVal);
                var urlsave = '<%= Url.Action("SaveAbsent","HR") %>';
                $('#formTransaction').attr('action',urlsave+'?workDate=' + workdate + '&rowNum=' + itemCats.rows.length);
            }
            
        }

        $(document).ready(function () {
            $('#searchButton').button();
            $('#saveButton').button();

            var today = new Date();
//            alert(today.toGMTString().substr(5, 11).replace(/ /g, '-'));
            $('#txtWorkDate').val(today.toGMTString().substr(5, 11));
            //alert(new RegExp(' ', 'g').toString());
            refreshEmployeeTable();
        });
    </script>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <form id="formTransaction" method="post" action="<%= Url.Action("GetAbsent","HR") %>">
    <%--<% using (Html.BeginForm("GetAbsent","HR"))
       {%>--%>
<%=Html.AntiForgeryToken()%>
    <div id="control" title="">
        <input id='saveButton' type='submit' value='Save'/>
        <br />
        Tanggal Kerja : <input id='txtWorkDate' name='txtWorkDate' type='text' value=''/>
        <input id='searchButton' type='button' value='Search' onclick='refreshEmployeeTable()'/>
    </div>
    <hr />
    <div id="tableform" title="" >
        <table id="tableAbsent" style="width:100%;border-style:solid;">
        </table>
    </div>
    <%--<%
       }%>--%>
    </form>
</asp:Content>
