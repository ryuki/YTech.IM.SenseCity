<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<ReservationFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <% if (false)
       { %>
    <script src="../../../Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <% } %>
    <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess",
                                           OnBegin = "validateForm"

                                       }

          ))
       {%>
    <%=Html.AntiForgeryToken()%>
    Tambah Reservasi baru
    <table>
        <%--  <tr>
            <td>
                <label for="ReservationIsMember">
                    Punya kartu member</label>
            </td>
            <td>
                <%= Html.CheckBox("ReservationIsMember", Model.Reservation.ReservationIsMember)%>
            </td>
        </tr>--%>
        <tr>
            <td>
                <label for="CustomerId">
                    No Member :</label>
            </td>
            <td>
                <%=Html.TextBox("CustomerId",
                                      Model.Reservation.CustomerId != null ? Model.Reservation.CustomerId.Id : null)%>
                &nbsp;<img src='<%= Url.Content("~/Content/Images/window16.gif") %>' style='cursor: hand;'
                    id='imgCustomerId' />
            </td>
        </tr>
        <tr>
            <td>
                <label for="ReservationName">
                    Nama Pemesan :</label>
            </td>
            <td>
                <%=Html.TextBox("ReservationName", Model.Reservation.ReservationName)%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="ReservationPhoneNo">
                    No Telp :</label>
            </td>
            <td>
                <%=Html.TextBox("ReservationPhoneNo", Model.Reservation.ReservationPhoneNo)%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="ReservationDate">
                    Tanggal Reservasi :</label>
            </td>
            <td>
                <%=Html.TextBox("ReservationDate",
                                      Model.Reservation.ReservationDate.HasValue
                                          ? Model.Reservation.ReservationDate.Value.ToString(CommonHelper.DateFormat)
                                          : null)%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="ReservationAppoinmentTime">
                    Jam :</label>
            </td>
            <td>
                <%=Html.TextBox("ReservationAppoinmentTime",
                                      Model.Reservation.ReservationAppoinmentTime.HasValue
                                          ? Model.Reservation.ReservationAppoinmentTime.Value.ToString(
                                              CommonHelper.TimeFormat)
                                          : null)%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="ReservationNoOfPeople">
                    Jumlah Orang :</label>
            </td>
            <td>
                <%=Html.TextBox("ReservationNoOfPeople", Model.Reservation.ReservationNoOfPeople)%>
                <input id="btnDetail" type="button" value="Detail" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table id="tblDetail">
                </table>
            </td>
        </tr>
        <tfoot>
            <tr>
                <td colspan="2" align="center">
                    <input id="Submit1" type="submit" value="Simpan" />
                </td>
            </tr>
        </tfoot>
    </table>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <div id="error" title="Error">
        <span id="error_msg"></span>
    </div>
    <div id='popup'>
        <iframe width='100%' height='100%' id="popup_frame"></iframe>
    </div>
    <%}%>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#Submit1").button();
            $("#dialog").dialog({
                autoOpen: false
            });
            $("#popup").dialog({
                autoOpen: false,
                height: 400,
                width: '80%',
                modal: true,
                close: function (event, ui) {

                }
            });

            $("div#error").dialog({
                autoOpen: false
            });
            $('#btnDetail').click(function () {
                var noOfPeople = $('#ReservationNoOfPeople').val();
                //  alert(noOfPeople);
                var tbl = '<tr><th>Nama</th><th>Paket</th><th>Nama Terapis</th></tr>';
                //tbl += "<tbody>";
                var imgUrl = '<%= Url.Content("~/Content/Images/window16.gif") %>';
                for (var i = 0; i < noOfPeople; i++) {
                    //alert(i);
                    tbl += "<tr>";
                    tbl += "<td>";
                    tbl += "<input type='text' id='txtDetailName_" + i + "' name='txtDetailName_" + i + "'  />";
                    tbl += "</td>";
                    tbl += "<td>";
                    tbl += "<input type='text' id='txtPacketId_" + i + "' name='txtPacketId_" + i + "'  />";
                    tbl += "&nbsp;<img src='" + imgUrl + "' style='cursor: hand; id='imgPacketId_" + i + "' class='imgTriggerOpenPacket' data='" + i + "' />";
                    tbl += "<input type='text' id='txtPacketName_" + i + "' />";
                    tbl += "</td>";
                    tbl += "<td>";
                    tbl += "<input type='text' id='txtEmployeeId_" + i + "' name='txtEmployeeId_" + i + "' />";
                    tbl += "&nbsp;<img src='" + imgUrl + "' style='cursor: hand; id='imgEmployeeId_" + i + "' class='imgTriggerOpenEmployee' data='" + i + "' />";
                    tbl += "<input type='text' id='txtEmployeeName_" + i + "' />";
                    tbl += "</td>";
                    tbl += "</tr>";
                }
                //tbl += "</tbody>";
                $('#tblDetail').html(tbl);
                $('.imgTriggerOpenPacket').click(imgPacketClick);
                $('.imgTriggerOpenEmployee').click(imgEmployeeClick);
            });
            $('#imgCustomerId').click(imgCustomerClick);

        });
        var imgCustomerClick = function () {
            OpenPopupSearch('Customer', '');
        };
        var imgPacketClick = function () {
            var data = $(this).attr('data');
            //alert(data);
            OpenPopupSearch('Packet', data);
        };
        var imgEmployeeClick = function () {
            var data = $(this).attr('data');
            //alert(data);
            OpenPopupSearch('Employee', data);
        };

        function OpenPopupSearch(search, data) {
            var src = '';
            if (search == 'Customer') {
                src = '<%= ResolveUrl("~/Master/Customer/Search") %>';
            }
            else if (search == 'Packet') {
                src = '<%= ResolveUrl("~/Master/Packet/Search") %>';
            }
            else if (search == 'Employee') {
                src = '<%= ResolveUrl("~/Master/Employee/Search") %>';
            }
            src += "?src=" + data;
            $("#popup_frame").attr("src", src);
            $("#popup").dialog("open");
            return false;
        }

        function SetPacketDetail(src, packetId, packetName, price, pricevip) {
            $("#popup").dialog("close");
            $('#txtPacketId_' + src).attr('value', packetId);
            $('#txtPacketName_' + src).attr('value', packetName);
        }
        function SetEmployeeDetail(src, employeeId, employeeName) {
            $("#popup").dialog("close");
            $('#txtEmployeeId_' + src).attr('value', employeeId);
            $('#txtEmployeeName_' + src).attr('value', employeeName);
        }
        function SetCustomerDetail(customerId, customerName, customerServiceDisc) {
            $("#popup").dialog("close");
            $('#CustomerId').attr('value', customerId);
            $('#ReservationName').attr('value', customerName);
        }
        function validateForm() {
            return true;
        }

        function onSavedSuccess(e) {
            var returndata = e.get_response().get_object();
            var success = returndata.Success;
            //            alert(success);
            if (success == true) {
                $('#Submit1').attr('disabled', 'disabled');
            }
            var msg = returndata.Message;
            $('#dialog p:first').text(msg);
            $("#dialog").dialog("open");

        }
    </script>
</asp:Content>
