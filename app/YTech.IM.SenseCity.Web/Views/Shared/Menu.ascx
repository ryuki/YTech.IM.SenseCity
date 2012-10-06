<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="accordion">
    <h3>
        <a href="#">Home</a></h3>
    <div class="child-menu-container">
        <%=Html.ActionLinkForAreas<HomeController>(c => c.Index(), "Home") %>
    </div>
    <% if (Request.IsAuthenticated)
       {
    %>
    <% if (Membership.GetUser().UserName.ToLower().Equals("admin"))
       { %>
    <h3>
        <a href="#">Data Pokok</a></h3>
    <div class="child-menu-container">
       <%-- <%= Html.ActionLinkForAreas<WarehouseController>(c => c.Index(),"Master Gudang") %>--%>
        <%= Html.ActionLinkForAreas<MItemCatController>(c => c.Index(),"Master Kategori Perawatan") %>
        <%= Html.ActionLinkForAreas<BrandController>(c => c.Index(),"Master Merek") %>
        <%= Html.ActionLinkForAreas<ItemController>(c => c.Index(), "Master Produk")%>
        <%= Html.ActionLinkForAreas<PacketController>(c => c.Index(), "Master Paket")%>
        <%= Html.ActionLinkForAreas<RoomController>(c => c.Index(), "Master Ruangan")%>
        <%= Html.ActionLinkForAreas<PromoController>(c => c.Index(), "Master Promo")%>
        <%--<%= Html.ActionLinkForAreas<SupplierController>(c => c.Index(),"Master Supplier") %>--%>
        <%= Html.ActionLinkForAreas<CustomerController>(c => c.Index(),"Master Konsumen") %>
        <%= Html.ActionLinkForAreas<DepartmentController>(c => c.Index(),"Master Departemen") %>
        <%= Html.ActionLinkForAreas<EmployeeController>(c => c.Index(), "Master Karyawan")%>
        <%--<%= Html.ActionLinkForAreas<CostCenterController>(c => c.Index(),"Master Cost Center") %>
        <%= Html.ActionLinkForAreas<AccountController>(c => c.Index(),"Master Akun") %>--%>
    </div>
    <% } %>
  <%--  <h3>
        <a href="#">Inventori</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Index(), "Order Pembelian")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Purchase(), "Pembelian")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.ReturPurchase(), "Retur Pembelian")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Sales(), "Penjualan")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.ReturSales(), "Retur Penjualan")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Mutation(), "Mutasi Stok")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Adjusment(), "Penyesuaian Stok")%>
    </div>--%>
    <h3>
        <a href="#">Customer </a>
    </h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<ReservationController>(c => c.Index(), "Reservasi")%>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.Billing(), "Billing")%>
    </div>
    <% if (Membership.GetUser().UserName.ToLower().Equals("admin"))
       { %>
    <%--<h3>
        <a href="#">Pembukuan</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<AccountingController>(c => c.GeneralLedger(), "General Ledger")%>
        <%= Html.ActionLinkForAreas<AccountingController>(c => c.CashIn(), "Kas Masuk")%>
        <%= Html.ActionLinkForAreas<AccountingController>(c => c.CashOut(), "Kas Keluar")%>
        
    </div>--%>
    <% } %>
    <% if (Membership.GetUser().UserName.ToLower().Equals("admin"))
       { %>
    <%--<h3>
        <a href="#">Absensi</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<HRController>(c => c.Absent(), "Absen Karyawan")%>
    </div>--%>
    <% } %>
    <h3>
        <a href="#">Laporan</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptBrand), "Daftar Merek")%>
        <%--<%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCostCenter), "Daftar Cost Center")%>--%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptServiceOmzet), "Laporan Omzet Penjualan")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCommissionRecap), "Laporan Rekap Komisi Terapis")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCommission), "Laporan Komisi Terapis")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCustomerBirthday), "Daftar Member Ulang Tahun")%>
       <%-- <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptStockCard), "Kartu Stok")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptStockItem), "Laporan Stok Per Gudang")%>--%>
        <% if (Membership.GetUser().UserName.ToLower().Equals("admin"))
           { %>
        <%--<%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptJournal), "Lap. Jurnal")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptNeraca), "Lap. Neraca")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptLR), "Lap. Laba / Rugi")%>--%>
    </div>
    <% } %>
    <h3>
        <a href="#">Utiliti</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<ShiftController>(c => c.Closing(), "Tutup Shift")%>
        <% if (Membership.GetUser().UserName.ToLower().Equals("admin"))
           { %>
        <%= Html.ActionLinkForAreas<InventoryController>(c => c.ListBilling(), "Daftar Billing")%>
        <%= Html.ActionLinkForAreas<UserAdministrationController>(c => c.Index(null), "Daftar Pengguna")%>
        
        <%--<%= Html.ActionLinkForAreas<AccountingController>(c => c.Closing(), "Tutup Buku")%>--%>
    </div>
    <% } %>
    <%
       }
    %>
