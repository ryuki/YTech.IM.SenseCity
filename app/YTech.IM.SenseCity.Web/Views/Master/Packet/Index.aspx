<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MPacket>>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
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
        <iframe width='100%' height='340px' id="popup_frame" frameborder="0"></iframe>
    </div>
    <script type="text/javascript">

        var editDialog = {
            url: '<%= Url.Action("Update", "Packet") %>'
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
            url: '<%= Url.Action("Insert", "Packet") %>'
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
            url: '<%= Url.Action("Delete", "Packet") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
        };
        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false
            });

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Paket Baru";
            $.jgrid.edit.editCaption = "Edit Paket";
            $.jgrid.del.caption = "Hapus Produk";
            $.jgrid.del.msg = "Anda yakin menghapus Produk yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Packet") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'Kode Paket', 'Nama', 'Harga', 'Harga VIP', 'Status', 'Deskripsi'],
                colModel: [
                    { name: 'act', index: 'act', width: 75, sortable: false },
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'PacketName', index: 'PacketName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'PacketPrice', index: 'PacketPrice', width: 200, align: 'right', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: false ,
                        editoptions: {
                            dataInit: function (elem) {
                                $(elem).autoNumeric();
                                $(elem).attr("style", "text-align:right;");
                            }
                        }
                    },
                    { name: 'PacketPriceVip', index: 'ItemCatName', width: 200, align: 'right', editable: true, edittype: 'text', editrules: { edithidden: true} ,
                        editoptions: {
                            dataInit: function (elem) {
                                $(elem).autoNumeric();
                                $(elem).attr("style", "text-align:right;");
                            }
                        }
                    },
                    { name: 'PacketStatus', index: 'PacketStatus', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: false },
                    { name: 'PacketDesc', index: 'PacketDesc', width: 200, align: 'left', editable: true, edittype: 'textarea', editrules: { edithidden: true}}],

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
                    var ids = jQuery("#list").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        //var be = "<input   type='button' value='T' tooltips='Tambah Perawatan'  onclick=\"$('#list').editGridRow('" + cl + "', editDialog);\" /> ";
                        //var be = "<a rel='example1' href='<%= Url.Action("AddPacketItemCat", "PacketItemCat") %>?"+cl+"'>Tambah</a>";
                        var be = "<input type='button' value='T' tooltips='Tambah Perawatan' onClick=\"OpenPopup('"+cl+"');\" />";
                        //                        alert(be);
                        $(this).setRowData(ids[i], { act: be });
                    }
                },
                multiselect: false,
                subGrid: true,
                subGridUrl: '<%= Url.Action("ListForSubGrid", "PacketItemCat") %>',
                subGridModel: [{ name: [ 'Kategori Item', 'Kuantitas', 'Status', 'Deskripsi'],
                    width: [  55, 80, 80, 80],
                       //subrig columns aligns
                       align: ['left', 'right', 'left', 'left'],
                    params: ['Id']
                }],
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
            $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function(event, ui) {                 
                    $("#list").trigger("reloadGrid");
                 }
            });
        });

        function OpenPopup(id)
        {
            $("#popup_frame").attr("src", "<%= Url.Action("AddPacketItemCat", "PacketItemCat") %>/"+id);
            $("#popup").dialog("open");
            return false;   
        }
    </script>
</asp:Content>
