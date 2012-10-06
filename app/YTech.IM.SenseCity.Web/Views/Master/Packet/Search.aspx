<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MPacket>>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

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
                    { name: 'act', index: 'act', width: 75, sortable: false, hidden: true },
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'PacketName', index: 'PacketName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'PacketPrice', index: 'PacketPrice', width: 200, align: 'right', editable: true, edittype: 'text', editrules: { edithidden: true }, hidden: false,
                        editoptions: {
                            dataInit: function (elem) {
                                $(elem).autoNumeric();
                                $(elem).attr("style", "text-align:right;");
                            }
                        }
                    },
                    { name: 'PacketPriceVip', index: 'PacketPriceVip', width: 200, align: 'right', editable: true, edittype: 'text', editrules: { edithidden: true },
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
                height: 250,
                caption: 'Daftar Paket',
                autowidth: true,
                loadComplete: function () {

                },
                multiselect: false,
//                subGrid: true,
//                subGridUrl: '<%= Url.Action("ListForSubGrid", "PacketItemCat") %>',
//                subGridModel: [{ name: ['Kategori Item', 'Kuantitas', 'Status', 'Deskripsi'],
//                    width: [55, 80, 80, 80],
//                    //subrig columns aligns
//                    align: ['left', 'right', 'left', 'left'],
//                    params: ['Id']
//                }],
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    var list = $("#list");
                    var rowData = list.getRowData(rowid);
//                    alert(rowid);
//                    alert(iRow);
//                    alert(iCol);
//                    alert(e);
                     <% if (!string.IsNullOrEmpty(Request.QueryString["src"])) {	%>
                       window.parent.SetPacketDetail('<%= Request.QueryString["src"] %>',rowData["Id"], rowData["PacketName"], rowData["PacketPrice"], rowData["PacketPriceVip"]);
  <%} else {%>
    window.parent.SetPacketDetail(rowData["Id"], rowData["PacketName"], rowData["PacketPrice"], rowData["PacketPriceVip"]);
  <%}%>
                    return false;
                }
            }).navGrid('#listPager',
                {
                    edit: false, add: false, del: false, search: false, refresh: true
                }
            );
        });
    </script>
</asp:Content>
