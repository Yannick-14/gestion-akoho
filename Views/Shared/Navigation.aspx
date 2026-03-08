<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Shared.NavigationPage" %>
<nav class="navbar navbar-expand-sm navbar-light bg-white border-bottom mb-3">
    <div class="container-fluid">
        <a class="navbar-brand" href="<%: Url.Action("Index", "Home") %>">Akoho ASPX</a>
        <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
            <ul class="navbar-nav flex-grow-1">
                <li class="nav-item"><a class="nav-link text-dark fw-semibold" href="<%: Url.Action("Index", "Dashboard") %>">Tableau de Bord</a></li>
                <li class="nav-item"><a class="nav-link text-dark fw-semibold" href="<%: Url.Action("LotOeufs", "Dashboard") %>">Lots d'Œufs</a></li>
                <li class="nav-item"><a class="nav-link text-dark" href="<%: Url.Action("Index", "Lot") %>">Gestion des Lots</a></li>
            </ul>
            <div class="d-flex align-items-center gap-2">
                <span class="navbar-text bg-light border px-2 py-1 rounded small">
                    <strong>Date Active :</strong> <%: AkohoAspx.Utils.Time.GetDateActuelle().ToString("dd/MM/yyyy") %>
                </span>
                <form action="/Dashboard/ResetDate" method="post" class="m-0">
                    <button type="submit" class="btn btn-sm btn-outline-danger" title="Revenir à aujourd'hui">Reset</button>
                </form>
            </div>
        </div>
    </div>
</nav>
