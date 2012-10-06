<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function (event, ui) {
                    $("#list").trigger("reloadGrid");
                }
            });
            $("#dialog").dialog({
                autoOpen: false
            });


            var editDialog = {
                url: '<%= Url.Action("Update", "Reservation") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true

                , onclickSubmit: function (params) {
                    var ajaxData = {};

                    var list = $("#list");
                    var selectedRow = list.getGridParam("selrow");
                    rowData = list.getRowData(selectedRow);
                    ajaxData = { Id: rowData.Id };

                    return ajaxData;
                }
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', 'disabled');
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "Reservation") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', '');

                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "Reservation") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Reservasi Baru";
            $.jgrid.edit.editCaption = "Edit Reservasi";
            $.jgrid.del.caption = "Hapus Reservasi";
            $.jgrid.del.msg = "Anda yakin menghapus Reservasi yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Reservation") %>',
                postData: {
                    ReservationStatus: function () { return $('#ReservationStatus option:selected').val(); }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Aksi', 'Kode Reservasi', 'Nama', 'No Telp', 'Tanggal', 'Jam', 'Jumlah Orang', 'Status'],
                colModel: [
                    { name: 'act', index: 'act', width: 325, sortable: false },
                                     { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: true },
                    { name: 'ReservationName', index: 'ReservationName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true} },
                    { name: 'ReservationPhoneNo', index: 'ReservationPhoneNo', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true} },
                    { name: 'ReservationDate', index: 'ReservationDate', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true} },
                    { name: 'ReservationAppoinmentTime', index: 'ReservationAppoinmentTime', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true} },
                    { name: 'ReservationNoOfPeople', index: 'ReservationNoOfPeople', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true} },
                    { name: 'ReservationStatus', index: 'ReservationStatus', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'ReservationDate',
                sortorder: "desc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Reservasi',
                autowidth: true,
                loadComplete: function () {
                    var batal = '<%= EnumReservationStatus.Batal.ToString() %>';
                    var tunda = '<%= EnumReservationStatus.Tunda.ToString() %>';
                    var jadi = '<%= EnumReservationStatus.Jadi.ToString() %>';
                    var ids = jQuery("#list").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var be = "<input type='button' value='Batal' tooltips='Batal Reservasi' onClick=\"UpdateReservationStatus('" + cl + "','" + batal + "');\" />";
                        be += "<input type='button' value='Tunda' tooltips='Tunda Reservasi' onClick=\"UpdateReservationStatus('" + cl + "','" + tunda + "');\" />";
                        be += "<input type='button' value='Jadi' tooltips='Jadi Reservasi' onClick=\"UpdateReservationStatus('" + cl + "','" + jadi + "');\" />";
                        //                        alert(be);
                        $(this).setRowData(ids[i], { act: be });
                    }
                },
                multiselect: false,
                subGrid: true,
                subGridUrl: '<%= Url.Action("ListForSubGrid", "Reservation") %>',
                subGridModel: [{ name: ['Nama', 'Paket', 'Terapis'],
                    width: ['30%', '30%', '30%'],
                    //subrig columns aligns
                    align: ['left', 'left', 'left'],
                    params: ['Id']
                }],
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    // $("#list").editGridRow(rowid, editDialog);
                }
            }).navGrid('#listPager',
                {
                    edit: false, add: false, del: true, search: false, refresh: true
                },
                editDialog,
                insertDialog,
                deleteDialog
            )
             .navButtonAdd('#listPager', {
                 caption: "Tambah",
                 buttonicon: "ui-icon-add",
                 onClickButton: function () {
                     OpenPopup(null);
                 },
                 position: "first"
             });

            $('#ReservationStatus').change(function () {
                $("#list").trigger("reloadGrid");
            });
        });
        function OpenPopup(id) {
            var url = '<%= Url.Action("AddNew", "Reservation" ) %>?';
            if (id) {
                url += 'customerId=' + id;
            }
            $("#popup_frame").attr("src", url);
            $("#popup").dialog("open");
            return false;
        }
        function UpdateReservationStatus(id, status) {
            var result = $.ajax({ url: '<%= Url.Action("UpdateStatus","Reservation") %>',
                data: "reservationId=" + id + "&status=" + status,
                async: false,
                cache: false,
                success: function (data, result) {
                    if (!result) alert('Update status reservasi tidak berhasil.');
                }
            });
            $('#dialog p:first').text(result.responseText);
            $("#dialog").dialog("open");
            $("#list").trigger("reloadGrid");
        }    
    </script>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <label for="ReservationStatus">
            Status Reservasi :</label>
        <%= Html.DropDownList("ReservationStatus", (SelectList)ViewData["ReservationStatusList"])%>
    </div>
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <div id='popup'>
        <iframe width='100%' height='420px' id="popup_frame" frameborder="0"></iframe>
    </div>
</asp:Content>
