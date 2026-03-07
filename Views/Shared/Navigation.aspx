<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Shared.NavigationPage" %>
<nav class="navbar navbar-expand-sm navbar-light bg-white border-bottom mb-3">
    <div class="container-fluid">
        <a class="navbar-brand" href="<%: Url.Action("Index", "Home") %>">Akoho ASPX</a>
        <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
            <ul class="navbar-nav flex-grow-1">
                <li class="nav-item"><a class="nav-link text-dark" href="<%: Url.Action("Index", "Home") %>">Home</a></li>
                <li class="nav-item"><a class="nav-link text-dark" href="<%: Url.Action("Index", "Race") %>">Race</a></li>
                <li class="nav-item"><a class="nav-link text-dark" href="<%: Url.Action("Index", "Lot") %>">Lot</a></li>
                <li class="nav-item"><a class="nav-link text-dark" href="<%: Url.Action("Index", "DashBoard") %>">Dashboard</a></li>
            </ul>
        </div>
    </div>
</nav>
