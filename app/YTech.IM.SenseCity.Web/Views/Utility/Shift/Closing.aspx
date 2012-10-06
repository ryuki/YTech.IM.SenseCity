<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<ShiftFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <% if (false)
       { %>
    <script src="../../../Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <% } %>
    <% using (Html.BeginForm())
       {%>
    <%=Html.AntiForgeryToken()%>
    <table>
        <tr>
            <td>
                <label for="ShiftDate">
                    Tanggal :
                </label>
            </td>
            <td>
                <%=Html.TextBox("ShiftDate", Model.Shift.ShiftDate.Value.ToString(CommonHelper.DateFormat))%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="lblShiftNo">
                    Shift ke :
                </label>
            </td>
            <td>
                <%=Html.Hidden("ShiftNo", Model.Shift.ShiftNo.Value)%>                <label id="lblShiftNo">
                <%= Model.Shift.ShiftNo.Value%></label>
            </td>
        </tr>
        <tr>
            <td>
                <label for="ShiftDateFrom">
                    Dari Jam :
                </label>
            </td>
            <td>
                <%=Html.Hidden("ShiftDateFrom", Model.Shift.ShiftDateFrom.Value.ToString(CommonHelper.TimeFormat))%>              <label id="lblShiftDateFrom">
                <%= Model.Shift.ShiftDateFrom.Value.ToString(CommonHelper.TimeFormat) %></label>
            </td>
        </tr>
        <tr>
            <td>
                <label for="ShiftDateTo">
                    s/d
                </label>
            </td>
            <td>
                <%=Html.TextBox("ShiftDateTo", Model.Shift.ShiftDateTo.Value.ToString(CommonHelper.TimeFormat))%>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <%=Html.SubmitButton("Save", "OK")%>
            </td>
        </tr>
    </table>
     <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <% } %>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ShiftDate").datepicker({ dateFormat: "dd-M-yy" });
            $("#dialog").dialog({
                autoOpen: false
            });
            $(':submit').button();
            $('form').submit(function () {
                //get the form
                var f = $('form');
                var serializedForm = f.serialize();
                //alert(serializedForm);
                var action = f.attr('action');
                //alert(action);
                $.post(action, serializedForm,
                    function (result) {
                        // alert(result);
                        //                        var result = JSON.parse(result);
                        var success = result.Success;
                        var msg = result.Message;
                        $('#dialog p:first').text(msg);
                        $("#dialog").dialog("open");

                        $(':submit').attr('disabled', 'disabled');
                    });
                return false;
            });

            jQuery().ajaxStart(function () {
                $('form').fadeOut("slow");
            });

            jQuery().ajaxStop(function () {
                $('form').fadeIn("slow");
            });

            $("#ShiftDate").change(function () {
                //get Shift
                var s = $.ajax({ url: '<%= Url.Action("GetJSONLastClosing","Shift") %>?closingDate=' + $("#ShiftDate").val(), async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Shift.'); } }).responseText;
               // alert(s);
                var shift = JSON.parse(s);
                //alert(shift);
                //            alert('debug 4');
                //alert(shift.Shift.ShiftNo);
                $("#ShiftNo").val(shift.Shift.ShiftNo);
                $("#lblShiftNo").text(shift.Shift.ShiftNo);
                var ShiftDateFrom = new Date(parseInt(shift.Shift.ShiftDateFrom.substr(6)));
                //alert(ShiftDateFrom.format('HH:MM'));
                $("#ShiftDateFrom").val(ShiftDateFrom.format('HH:MM'));
                $("#lblShiftDateFrom").text(ShiftDateFrom.format('HH:MM'));
            });
        });    
    </script>
</asp:Content>
