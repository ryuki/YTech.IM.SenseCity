<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MRoom>>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false
            });


            var editDialog = {
                url: '<%= Url.Action("Update", "Room") %>'
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
                url: '<%= Url.Action("Insert", "Room") %>'
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
                url: '<%= Url.Action("Delete", "Room") %>'
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
            $.jgrid.edit.addCaption = "Tambah Ruangan Baru";
            $.jgrid.edit.editCaption = "Edit Ruangan";
            $.jgrid.del.caption = "Hapus Produk";
            $.jgrid.del.msg = "Anda yakin menghapus Ruangan yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Room") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Ruangan', 'Nama', 'No Order', 'Tipe Ruangan', 'Status', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'RoomName', index: 'RoomName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'RoomOrderNo', index: 'RoomOrderNo', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: false },
                    { name: 'RoomType', index: 'RoomType', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true} },
                    { name: 'RoomStatus', index: 'RoomStatus', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true }, hidden: false },
                    { name: 'RoomDesc', index: 'RoomDesc', width: 200, align: 'left', editable: true, edittype: 'textarea', editrules: { edithidden: true}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Ruangan',
                autowidth: true,
                loadComplete: function () {
                    $('#list').setColProp('RoomType', { editoptions: { value: types} });
                    $('#list').setColProp('RoomStatus', { editoptions: { value: status} });
                 },
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    $('#list').editGridRow(rowid, editDialog);
                }
            }).navGrid('#listPager',
                {
                    edit: true, add: true, del: true, search: false, refresh: true
                },
                editDialog,
                insertDialog,
                deleteDialog
            );
        });

                var types = $.ajax({ url: '<%= Url.Action("GetRoomTypeList","Room") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the RoomType.'); } }).responseText;

                var status = $.ajax({ url: '<%= Url.Action("GetRoomStatusList","Room") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the RoomType.'); } }).responseText;
    </script>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
</asp:Content>
