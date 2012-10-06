<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MPacketItemCat>>" %>

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
                url: '<%= Url.Action("Update", "PacketItemCat") %>'
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
                url: '<%= Url.Action("Insert", "PacketItemCat") %>'
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
                url: '<%= Url.Action("Delete", "PacketItemCat") %>'
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
            $.jgrid.edit.addCaption = "Tambah Paket Item Kategori Baru";
            $.jgrid.edit.editCaption = "Edit Paket Item Kategori";
            $.jgrid.del.caption = "Hapus Produk";
            $.jgrid.del.msg = "Anda yakin menghapus Paket Item Kategori yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "PacketItemCat") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Paket Item Kategori', 'Kode Paket', 'Kode Kategori Item', 'Kuantitas', 'Status', 'Deskripsi'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'PacketId', index: 'PacketId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'ItemCatId', index: 'ItemCatId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'ItemCatQty', index: 'ItemCatQty', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: false },
                    { name: 'PacketItemCatStatus', index: 'PacketItemCatStatus', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true} },
                    { name: 'PacketItemCatDesc', index: 'PacketItemCatDesc', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: false}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Paket',
                autowidth: true,
                loadComplete: function () {
                    $('#list').setColProp('ItemCatId', { editoptions: { value: itemCats} });
                    $('#list').setColProp('PacketId', { editoptions: { value: packets} });
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

                var packets = $.ajax({ url: '<%= Url.Action("GetList","Packet") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Packets.'); } }).responseText;
                var itemCats = $.ajax({ url: '<%= Url.Action("GetList","MItemCat") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the ItemCats.'); } }).responseText;

//                alert(itemCats.toString());
    </script>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
</asp:Content>
